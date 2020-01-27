using Microsoft.Xna.Framework;
using Romi.Standard;
using Romi.Standard.Objects;
using Romi.Standard.UIControls.Buttons;
using System;

namespace Romi.GameA.Objects.Fullmoon
{
    public enum IconButtonType
    {
        None,
        Izumi,
        Jonathan,
        Meroko,
        Mitsuki,
        Fullmoon,
        Takuto
    }

    public class IconButton : RMSingleImageTextButton, IExpirableObj
    {
        public IconButton(IconButtonType btnType, Point pt, float duration, GameTime gameTime) : base($"Fullmoon/{(int)btnType:D03}", pt)
        {
            Type = btnType;
            Created = gameTime.TotalGameTime;
            End = Created.Add(TimeSpan.FromSeconds(duration));
            Duration = duration;
        }

        public float Duration { get; }

        public TimeSpan End { get; }

        public IconButtonType Type { get; private set; }

        public TimeSpan Created { get; private set; }

        public float Opacity { get; private set; }

        public bool IsGoodButton
        {
            get
            {
                switch (Type)
                {
                    case IconButtonType.Meroko:
                    case IconButtonType.Takuto:
                    case IconButtonType.Mitsuki:
                    case IconButtonType.Fullmoon:
                        return true;
                    default:
                        return false;
                }
            }
        }

        // 불투명도가 0.9 이상일 경우 100점으로 간주
        public int Score => (int) (100 * (Opacity >= 0.9f ? 1 : Opacity));

        public bool IsExpired(GameTime gameTime)
        {
            return gameTime.TotalGameTime >= End;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            // 지속시간동안 불투명도가 올라갔다가 다시 내려감
            var d = (gameTime.TotalGameTime - Created).TotalSeconds / Duration;
            Opacity = (float) (Math.Sin(d * Math.PI));
        }

        protected override void OnDrawClicked(GameTime gameTime)
        {
            App.SpriteBatch.Draw(normal, Position.ToVector2(), Color.White * Opacity);
        }

        protected override void OnDrawDisabled(GameTime gameTime)
        {
        }

        protected override void OnDrawHover(GameTime gameTime)
        {
            App.SpriteBatch.Draw(normal, Position.ToVector2(), Color.White * Opacity);
        }

        protected override void OnDrawNormal(GameTime gameTime)
        {
            App.SpriteBatch.Draw(normal, Position.ToVector2(), Color.White * Opacity);
        }
    }
}
