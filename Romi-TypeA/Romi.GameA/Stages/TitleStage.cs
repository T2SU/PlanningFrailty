using Microsoft.Xna.Framework;
using Romi.GameA.Resources;
using Romi.GameA.Users;
using Romi.Standard;
using Romi.Standard.Objects;
using Romi.Standard.Stages;
using Romi.Standard.UIControls.Buttons;
using Romi.Standard.UIControls.Texts;

namespace Romi.GameA.Stages
{
    public sealed class TitleStage : RMStage
    {
        public TitleStage() : base(999999999)
        {
        }

        public override void Init(GameTime gameTime)
        {
            sprites.Add(
                new RMText(FontType.NanumSquareRound24, "Romi Type-A (Android)", Point.Zero)
                {
                    Color = Color.FloralWhite,
                    HorizontalAlignment = HorizontalAlignment.Right,
                    VerticalAlignment = VerticalAlignment.Bottom
                }
            );
            gameComponents.Add(
                new RMImageTextButton("ButtonA", new Point(24, 24))
                {
                    Text = new RMText(FontType.NanumSquareRound24, "게임 시작")
                    {
                        Color = Color.FloralWhite,
                        HorizontalAlignment = HorizontalAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Middle
                    },
                    Clicked = btn =>
                    {
                        RM<User>.Instance = new User(new Point(0, 0));
                        App.Game.Stage = new GameStage(100000000);
                    }
                }
            );
        }

        public override void UnInit()
        {
            gameComponents.Clear();
            sprites.Clear();
        }
    }
}
