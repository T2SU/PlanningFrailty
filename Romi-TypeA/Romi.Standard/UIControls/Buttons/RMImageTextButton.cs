using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Romi.Standard.UIControls.Texts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Romi.Standard.UIControls.Buttons
{
    public class RMImageTextButton : RMButton
    {
        private Texture2D clicked;
        private Texture2D disabled;
        private Texture2D hover;
        private Texture2D normal;
        private RMText text;

        public RMImageTextButton(string btnImage, Point pt) : base(pt)
        {
            ButtonImageName = btnImage;
            LoadTextures();
            Area = new Rectangle(pt, new Point(normal.Width, normal.Height));
        }

        public RMImageTextButton(string btnImage, Point pt, Point size) : base(pt, size)
        {
            ButtonImageName = btnImage;
            LoadTextures();
        }

        public RMImageTextButton(string btnImage, Point pt, Rectangle rect) : base(pt, rect)
        {
            ButtonImageName = btnImage;
            LoadTextures();
        }

        public RMImageTextButton(string btnImage, Point pt, int Width, int Height) : base(pt, Width, Height)
        {
            ButtonImageName = btnImage;
            LoadTextures();
        }

        public RMImageTextButton(string btnImage, int X, int Y, int Width, int Height) : base(X, Y, Width, Height)
        {
            ButtonImageName = btnImage;
            LoadTextures();
        }

        private void LoadTextures()
        {
            clicked = App.Game.Content.Load<Texture2D>($"Sprites/{ButtonImageName}/Clicked");
            disabled = App.Game.Content.Load<Texture2D>($"Sprites/{ButtonImageName}/Disabled");
            hover = App.Game.Content.Load<Texture2D>($"Sprites/{ButtonImageName}/Hover");
            normal = App.Game.Content.Load<Texture2D>($"Sprites/{ButtonImageName}/Normal");
        }

        public string ButtonImageName { get; }

        public RMText Text
        {
            get => text;
            set
            {
                text = value;
                text.Rect = Area;
            }
        }

        public Action<RMImageTextButton> Clicked { get; set; }

        protected override void OnDrawClicked(GameTime gameTime)
        {
            App.SpriteBatch.Draw(clicked, Area, Color.White);
            DrawText(gameTime);
        }

        protected override void OnDrawDisabled(GameTime gameTime)
        {
            App.SpriteBatch.Draw(disabled, Area, Color.White);
            DrawText(gameTime);
        }

        protected override void OnDrawHover(GameTime gameTime)
        {
            App.SpriteBatch.Draw(hover, Area, Color.White);
            DrawText(gameTime);
        }

        protected override void OnDrawNormal(GameTime gameTime)
        {
            App.SpriteBatch.Draw(normal, Area, Color.White);
            DrawText(gameTime);
        }

        private void DrawText(GameTime gameTime)
        {
            if (Text != null)
            {
                Text.Draw(gameTime);
            }
        }

        public override void OnClick()
        {
            Clicked?.Invoke(this);
        }
    }
}
