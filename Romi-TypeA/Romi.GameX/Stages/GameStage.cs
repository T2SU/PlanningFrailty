using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Romi.GameX.Objects;
using Romi.GameX.Objects.Fullmoon;
using Romi.GameX.Resources;
using Romi.GameX.Users;
using Romi.Standard;
using Romi.Standard.Diagnostics;
using Romi.Standard.Graphics;
using Romi.Standard.Objects;
using Romi.Standard.Stages;
using Romi.Standard.UIControls.Buttons;
using Romi.Standard.UIControls.Texts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Romi.GameX.Stages
{
    public enum GameState
    {
        Ready,
        Play,
        End
    }

    public sealed class GameStage : RMStage
    {
        private readonly Random random;
        private readonly Dictionary<Point, IconButton> iconButtons;

        private RMText secondsText;
        private RMText scoreText;
        private RMExpirableText gainScoreText;
        private TimeSpan begin;
        private TimeSpan end;
        private TimeSpan nextGen;
        private GameTime current;
        private GameState GameState;
        private int seconds;
        private ScoreData score;

        public GameStage(int id) : base(id)
        {
            seconds = 5;
            GameState = GameState.Ready;
            iconButtons = new Dictionary<Point, IconButton>();
            random = new Random();
        }

        private int Progress => (int) ((current.TotalGameTime - begin).TotalSeconds / (end - begin).TotalSeconds * 100);

        public override void Init(GameTime gameTime)
        {
            begin = gameTime.TotalGameTime;
            secondsText = new RMText(FontType.NanumGothic64, "")
            {
                Color = Color.Black,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Middle
            };
            scoreText = new RMText(FontType.NanumSquareRound24, "", new Point(12, 12))
            {
                Color = Color.Black,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top
            };
        }

        public override void UnInit()
        {
            gameComponents.Clear();
            sprites.Clear();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            current = gameTime;
            if (App.KeyboardInput.IsKeyDown(Keys.Escape))
            {
                App.Game.Stage = new TitleStage();
                RM<User>.Instance = null;
            }
            switch (GameState)
            {
                case GameState.Ready:
                    UpdateGame_Ready();
                    break;
                case GameState.Play:
                    UpdateGame_Play();
                    break;
                case GameState.End:
                    UpdateGame_End();
                    break;
            }
        }

        // Ready State 업데이트
        private void UpdateGame_Ready()
        {
            if ((current.TotalGameTime - begin).TotalSeconds >= 1)
            {
                seconds--;
                begin = current.TotalGameTime;
                switch (seconds)
                {
                    case -1:
                        BeginGame();
                        break;
                    case 0:
                        secondsText.Content = "START!!";
                        break;
                    default:
                        secondsText.Content = seconds.ToString();
                        break;
                }
            }
        }

        // Play State 업데이트
        private void UpdateGame_Play()
        {
            if (Progress >= 100)
            {
                EndGame();
            }
            else
            {
                GenerateIconButton();
                foreach (var ib in iconButtons.Values.ToList())
                    ib.Update(current);
                gainScoreText?.Update(current);
                Expire();
            }
        }

        private void GenerateIconButton()
        {
            if (nextGen == default) return;
            if (nextGen > current.TotalGameTime) return;

            var rect = new Rectangle(64, 64, DisplayConst.Width - 164, DisplayConst.Height - 164);

            // max 30 tries
            for (int i = 0; i < 30; ++i)
            {
                int x = random.Next(rect.X, rect.Width);
                int y = random.Next(rect.Y, rect.Height);
                var box = new Rectangle(x - 99, y - 99, 298, 298); // 이미지 사이즈 100x100
                if (!iconButtons.Keys.Any(pos => box.Contains(pos)))
                {
                    var pt = new Point(x, y);
                    iconButtons[pt] = new IconButton(MakeType(), pt, random.Next(2, 5), current) 
                    { 
                        Clicked = OnClicked 
                    };
                    break;
                }
            }

            MakeNextGenTime();
        }

        private IconButtonType MakeType()
        {
            int r = random.Next(100);
            if (r < 7)
                return IconButtonType.Fullmoon;
            else if (r < 20)
                return IconButtonType.Mitsuki;
            else if (r < 50)
                return IconButtonType.Takuto;
            else if (r < 85)
                return IconButtonType.Meroko;
            else if (r < 95)
                return IconButtonType.Jonathan;
            else
                return IconButtonType.Izumi;
        }

        private void MakeNextGenTime()
        {
            int elapsed = Progress;
            if (elapsed < 10)
            {
                nextGen = current.TotalGameTime.Add(TimeSpan.FromMilliseconds(random.Next(500, 1000)));
            }
            else if (elapsed < 30)
            {
                nextGen = current.TotalGameTime.Add(TimeSpan.FromMilliseconds(random.Next(350, 750)));
            }
            else if (elapsed < 50)
            {
                nextGen = current.TotalGameTime.Add(TimeSpan.FromMilliseconds(random.Next(250, 750)));
            }
            else if (elapsed < 65)
            {
                nextGen = current.TotalGameTime.Add(TimeSpan.FromMilliseconds(random.Next(150, 350)));
            }
            else if (elapsed < 80)
            {
                nextGen = current.TotalGameTime.Add(TimeSpan.FromMilliseconds(random.Next(100, 350)));
            }
            else 
            {
                nextGen = current.TotalGameTime.Add(TimeSpan.FromMilliseconds(random.Next(300, 1200)));
            }
        }

        private void Expire()
        {
            foreach (var btn in iconButtons.Values.ToList())
            {
                if (btn.IsExpired(current))
                {
                    iconButtons.Remove(btn.Position);
                    if (btn.IsGoodButton)
                    {
                        IncScore(0, 0);
                        score.Missed++;
                    }
                }
            }
            if (gainScoreText != null)
            {
                if (gainScoreText.IsExpired(current))
                {
                    gainScoreText = null;
                }
            }
        }

        // End State 업데이트
        private void UpdateGame_End()
        {
            if (end < current.TotalGameTime)
            {
                App.Game.Stage = new TitleStage();
            }
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            switch (GameState)
            {
                case GameState.Ready:
                    secondsText.Draw(gameTime);
                    break;
                case GameState.Play:
                    foreach (var ib in iconButtons.Values)
                        ib.Draw(gameTime);
                    DrawPlayScore();
                    break;
                case GameState.End:
                    break;
            }
        }

        private void BeginGame()
        {
            BGM = App.Game.Content.Load<Song>("Sounds/BGM/Myself");
            MediaPlayer.IsRepeating = false;
            MediaPlayer.Play(BGM, TimeSpan.FromSeconds(4.5));
            GameState = GameState.Play;
            begin = current.TotalGameTime;
            end = begin.Add(BGM.Duration - TimeSpan.FromSeconds(4.5));
            nextGen = current.TotalGameTime.Add(TimeSpan.FromMilliseconds(1000));
        }

        private void DrawPlayScore()
        {
            scoreText.Content = $"점수: {this.score.Score}\n진행: {Progress}%";
            scoreText.Draw(current);
            if (gainScoreText != null)
            {
                gainScoreText.Position = new Point((int) (scoreText.Position.X + scoreText.Size.X + 12), scoreText.Position.Y);
                gainScoreText.Draw(current);
            }
        }

        private void EndGame()
        {
            GameState = GameState.End;
            gameComponents.Clear();
            begin = current.TotalGameTime;
            end = current.TotalGameTime.Add(TimeSpan.FromSeconds(10));

            float accurancy = score.AccurancyAccumalated * 1.0f / score.AccurancyTotal;
            var text = $" - 획득 점수 : {score.Score}\n - 정확도 : {accurancy:0.##}%\n - 풀문 : {score.Fullmoon}\n - 루나 : {score.Mitsuki}\n - 대파라면 사신콤비 : {score.Sinigami}\n - 놓침 : {score.Missed}\n - 잡탕전골 사신콤비 : {score.Broken}";
            gameComponents.Add(new RMText(FontType.NanumSquareRound24, text, Point.Zero)
            {
                Color = Color.Black,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Middle,
                AutoRepositionLineChanged = false
            });
        }

        private void OnClicked(RMSingleImageTextButton btn)
        {
            if (btn is IconButton iconButton)
            {
                switch (iconButton.Type)
                {
                    case IconButtonType.Izumi:
                        IncScore(-1, 500);
                        score.Broken++;
                        break;
                    case IconButtonType.Jonathan:
                        IncScore(-1, 200);
                        score.Broken++;
                        break;
                    case IconButtonType.Meroko:
                        IncScore(3, iconButton.Score);
                        AccumalateAccurancy(iconButton.Score);
                        score.Sinigami++;
                        break;
                    case IconButtonType.Takuto:
                        IncScore(4, iconButton.Score);
                        AccumalateAccurancy(iconButton.Score);
                        score.Sinigami++;
                        break;
                    case IconButtonType.Mitsuki:
                        IncScore(5, iconButton.Score);
                        AccumalateAccurancy(iconButton.Score);
                        score.Mitsuki++;
                        break;
                    case IconButtonType.Fullmoon:
                        IncScore(10, iconButton.Score);
                        AccumalateAccurancy(iconButton.Score);
                        score.Fullmoon++;
                        break;
                }
                iconButtons.Remove(btn.Position);
            }
        }
        
        private void IncScore(int multiplier, int accurancy)
        {
            int score = multiplier * accurancy;
            if (score != 0)
            {
                Log.Debug($"[IncScore] Score: {score}  Total: {this.score.Score}");
                this.score.Score += score;
            }
            SetGainScoreText(score, accurancy);
            AccumalateAccurancy(accurancy);
        }

        private void AccumalateAccurancy(int accurancy)
        {
            score.AccurancyTotal++;
            score.AccurancyAccumalated += accurancy;
        }

        private void SetGainScoreText(int score, int accurancy)
        {
            var content = $"({(accurancy == 0 ? "MISS" : score.ToString("+#;-#;0"))})";
            if (score > 0)
            {
                content += $" ({accurancy}%)";
            }
            this.gainScoreText = new RMExpirableText(
                   FontType.NanumSquareRound24,
                   content,
                   Point.Zero,
                   current,
                   0.5f)
            {
                Color = score > 0 ? Color.Green : Color.Red
            };
        }
    }

    public struct ScoreData
    {
        public int Score;
        public int AccurancyTotal;
        public int AccurancyAccumalated;
        public int Missed;
        public int Broken;
        public int Sinigami;
        public int Fullmoon;
        public int Mitsuki;
    }
}
