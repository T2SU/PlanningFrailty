using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Romi.Standard.Diagnostics;
using Romi.Standard.Graphics;
using Romi.Standard.Resources;
using Romi.Standard.Stages;
using System.Collections.Generic;

namespace Romi.Standard
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class RMGame : Game
    {
        readonly GraphicsDeviceManager graphics;
        readonly Queue<Stage> stageQueue;
        SpriteBatch spriteBatch;
        Stage stage;
        SystemStatus systemStatus;
        

        public RMGame(bool mobile, Stage defaultStage = null, Stage loadingStage = null)
        {
            App.GraphicsDevice = graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = ContentLoaderMan.RootDirectory;
            stageQueue = new Queue<Stage>();
            if (defaultStage != null)
            {
                Stage = defaultStage;
            }
            IsMouseVisible = true;
            IsMobile = mobile;
        }

        /// <summary>
        /// 현재 게임이 표시 중인 스테이지 클래스
        /// </summary>
        public Stage Stage 
        {
            get => stage;
            set
            {
                stageQueue.Enqueue(value);
            }
        }

        /// <summary>
        /// 모바일 모드 게임인가?
        /// </summary>
        public bool IsMobile { get; }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            Log.Debug($"ROMI Type-A Initializing...");
            // 고정 인스턴스에 현재 Game 인스턴스 설정
            App.Game = this;

            // 기본 빈 스테이지 설정
            stage = new RMEmptyStage();

            base.Initialize();
            Log.Debug($"ROMI Type-A Initialized.");
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            Log.Debug($"ROMI Type-A Loading Contents..");
            // Create a new SpriteBatch, which can be used to draw textures.
            App.SpriteBatch = spriteBatch = new SpriteBatch(GraphicsDevice);

            foreach (var cl in ContentLoaderMan.PickLoaders())
            {
                Log.Debug($"ROMI Type-A ContentLoader [{cl.GetType().Name}] Loading Processing...");
                cl.Load(this, this.Content);
                Log.Debug($"ROMI Type-A ContentLoader [{cl.GetType().Name}] Loading Proceeed.");
            }

            Log.Debug($"ROMI Type-A Loaded Contents.");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            Log.Debug($"ROMI Type-A Unloading Contents..");
            foreach (var cl in ContentLoaderMan.PickLoaders())
            {
                Log.Debug($"ROMI Type-A ContentLoader [{cl.GetType().Name}] Unloading Processing...");
                cl.Unload(this, this.Content);
                Log.Debug($"ROMI Type-A ContentLoader [{cl.GetType().Name}] Unloading Proceeed.");
            }
            Log.Debug($"ROMI Type-A Unloaded Contents.");
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            App.KeyboardInput.UpdateData(gameTime);
            App.MouseInput.UpdateData(gameTime);
            App.TouchInput.UpdateData(gameTime);

            // 디버그 정보 출력
            if (App.KeyboardInput.IsKeyTapped(Keys.F12))
            {
                if (systemStatus == null) 
                    systemStatus = new SystemStatus(this);
                else 
                    systemStatus = null;
            }
            systemStatus?.Update(gameTime);

            // 스테이지 전환 시도
            if (!(stage is TransferStage) && stage != null && stageQueue.Count > 0)
            {
                stage = new TransferStage(stage, stageQueue.Dequeue(), s => stage = s);
            }

            // 스테이지 로드가 완료되었을 경우 스테이지 업데이트
            if (stage.State == StageState.Normal)
            {
                stage.Update(gameTime);
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            if (IsMobile)
            {
                GetScaleXY(out var scaleX, out var scaleY);
                var matrix = Matrix.CreateScale(scaleX, scaleY, 1.0f);
                spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, transformMatrix: matrix);
            }
            else
            {
                spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            }
            systemStatus?.Draw(gameTime);
            if (stage.State == StageState.Normal)
            {
                stage.Draw(gameTime);
            }
            spriteBatch.End();

            base.Draw(gameTime);
        }

        public Rectangle CreateScaledRect(int x, int y, int w, int h)
        {
            return CreateScaledRect(new Rectangle(x, y, w, h));
        }

        public Rectangle CreateScaledRect(Point pt, Point size)
        {
            return CreateScaledRect(new Rectangle(pt, size));
        }

        public Rectangle CreateScaledRect(Rectangle rect)
        {
            return new Rectangle(GetScaledPoint(rect.Location), GetScaledPoint(rect.Size));
        }

        public Point GetScaledPoint(Point pt)
        {
            GetScaleXY(out var scaleX, out var scaleY);
            return new Point((int)(pt.X / scaleX), (int)(pt.Y / scaleY));
        }

        public Vector2 GetScaledVector2(Vector2 vector2)
        {
            GetScaleXY(out var scaleX, out var scaleY);
            return new Vector2((int)(vector2.X / scaleX), (int)(vector2.Y / scaleY));
        }

        internal void GetScaleXY(out float scaleX, out float scaleY)
        {
            if (IsMobile)
            {
                var actualWidth = (float)GraphicsDevice.Viewport.Width;
                var actualHeight = (float)GraphicsDevice.Viewport.Height;
                scaleX = actualWidth / DisplayConst_Mobile.Width;
                scaleY = actualHeight / DisplayConst_Mobile.Height;
            }
            else
            {
                scaleX = 1.0f;
                scaleY = 1.0f;
            }
        }
    }
}
