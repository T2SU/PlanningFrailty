using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Romi.Standard.UIControls.Buttons
{
    public class RMSingleImageTextButton : RMButton
    {
        protected Texture2D normal;

        public RMSingleImageTextButton(string btnImage, Point pt) : base(pt)
        {
            ButtonImageName = btnImage;
            LoadTextures();
            Area = new Rectangle(pt, new Point(normal.Width, normal.Height));
        }

        public RMSingleImageTextButton(string btnImage, Point pt, Point size) : base(pt, size)
        {
            ButtonImageName = btnImage;
            LoadTextures();
        }

        public RMSingleImageTextButton(string btnImage, Point pt, Rectangle rect) : base(pt, rect)
        {
            ButtonImageName = btnImage;
            LoadTextures();
        }

        public RMSingleImageTextButton(string btnImage, Point pt, int Width, int Height) : base(pt, Width, Height)
        {
            ButtonImageName = btnImage;
            LoadTextures();
        }

        public RMSingleImageTextButton(string btnImage, int X, int Y, int Width, int Height) : base(X, Y, Width, Height)
        {
            ButtonImageName = btnImage;
            LoadTextures();
        }

        private void LoadTextures()
        {
            normal = App.Game.Content.Load<Texture2D>($"Sprites/{ButtonImageName}");
        }

        public string ButtonImageName { get; }

        public Action<RMSingleImageTextButton> Clicked { get; set; }

        protected override void OnDrawClicked(GameTime gameTime)
        {
            App.SpriteBatch.Draw(normal, Area, Color.White * 0.75f);
        }

        protected override void OnDrawDisabled(GameTime gameTime)
        {
            App.SpriteBatch.Draw(normal, Area, Color.White * 0.25f);
        }

        protected override void OnDrawHover(GameTime gameTime)
        {
            App.SpriteBatch.Draw(normal, Area, Color.White * 0.75f);
        }

        protected override void OnDrawNormal(GameTime gameTime)
        {
            App.SpriteBatch.Draw(normal, Area, Color.White);
        }

        public override void OnClick()
        {
            Clicked?.Invoke(this);
        }
    }
}
