using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Romi.Standard.Graphics;
using Romi.Standard.Objects;
using Romi.Standard.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Romi.Standard.UIControls.Texts
{
    public enum HorizontalAlignment { Left, Center, Right }

    public enum VerticalAlignment { Top, Middle, Bottom }

    public class RMText : RMObj
    {
        private static readonly char[] SplitConstants = new char[] { '\r', '\n' };

        public RMText(string content)
            : this((string)null, content, Point.Zero)
        {
        }

        public RMText(string content, Point pt)
            : this((string)null, content, pt)
        {
        }

        public RMText(Enum enumValue, string content)
            : this(enumValue.ToString(), content, Point.Zero)
        {
        }

        public RMText(string fontName, string content) : this(fontName, content, Point.Zero, Rectangle.Empty)
        {
        }

        public RMText(Enum enumValue, string content, Point pt)
            : this(enumValue.ToString(), content, pt)
        {
        }

        public RMText(string fontName, string content, Point pt) : this(fontName, content, pt, Rectangle.Empty)
        {
        }

        public RMText(string fontName, string content, Point pt, Rectangle rect) : base(pt)
        {
            this.Font = FontMan.GetSpriteFont(fontName);
            this.Content = content;
            this.Rect = rect;
        }

        public Rectangle Rect { get; set; }

        public Color Color { get; set; }

        public SpriteFont Font { get; set; }

        public string Content { get; set; }

        public Vector2 Origin { get; set; } = Vector2.Zero;

        public SpriteEffects SpriteEffects { get; set; } = SpriteEffects.None;

        public float Scale { get; set; } = 1.0f;

        public float Rotation { get; set; }

        public float LayerDepth { get; set; } = 0.5f;

        /// <summary>
        /// 텍스트 사이즈
        /// </summary>
        public Vector2 Size => Font.MeasureString(Content);

        /// <summary>
        /// 수평 위치
        /// </summary>
        public HorizontalAlignment HorizontalAlignment { get; set; } = HorizontalAlignment.Left;

        /// <summary>
        /// 수직 위치
        /// </summary>
        public VerticalAlignment VerticalAlignment { get; set; } = VerticalAlignment.Top;

        /// <summary>
        /// 줄이 바뀌면 새로 정렬을 시도할지 or 줄이 바뀌어도 새로 정렬하지 않고 첫줄의 왼쪽에 아랫줄까지 모두 맞춰 출력할지
        /// </summary>
        public bool AutoRepositionLineChanged { get; set; } = true;

        public override void Draw(GameTime gameTime)
        {
            var rect = Rect;
            
            // 범위 자동 생성
            if (rect == Rectangle.Empty)
            {
                var vp = App.Game.GraphicsDevice.Viewport;
                App.Game.GetScaleXY(out var scaleX, out var scaleY);
                rect = new Rectangle(0, 0, (int) (vp.Width / scaleX), (int) (vp.Height / scaleY));
            }

            var offset = Position.ToVector2();
            if (AutoRepositionLineChanged)
            {
                foreach (var content in Content.Split(SplitConstants, StringSplitOptions.RemoveEmptyEntries))
                {
                    Draw0(rect, offset, content, out var size);
                    offset = new Vector2(offset.X, offset.Y + size.Y);
                }
            }
            else
            {
                Draw0(rect, offset, Content, out _);
            }
        }

        private void Draw0(Rectangle rect, Vector2 offset, string content, out Vector2 size)
        {
            size = Font.MeasureString(content);
            switch (HorizontalAlignment)
            {
                case HorizontalAlignment.Left:
                    Origin = new Vector2(0, Origin.Y);
                    offset = new Vector2(offset.X + rect.X, offset.Y);
                    break;
                case HorizontalAlignment.Right:
                    Origin = new Vector2(size.X, Origin.Y);
                    offset = new Vector2(offset.X + rect.X + rect.Width, offset.Y);
                    break;
                case HorizontalAlignment.Center:
                    Origin = new Vector2(size.X / 2, Origin.Y);
                    offset = new Vector2(offset.X + rect.X + rect.Width / 2, offset.Y);
                    break;
            }
            switch (VerticalAlignment)
            {
                case VerticalAlignment.Top:
                    Origin = new Vector2(Origin.X, 0);
                    offset = new Vector2(offset.X, offset.Y + rect.Y);
                    break;
                case VerticalAlignment.Middle:
                    Origin = new Vector2(Origin.X, size.Y / 2);
                    offset = new Vector2(offset.X, offset.Y + rect.Height / 2 + rect.Y);
                    break;
                case VerticalAlignment.Bottom:
                    Origin = new Vector2(Origin.X, size.Y);
                    offset = new Vector2(offset.X, offset.Y + rect.Height + rect.Y);
                    break;
            }
            App.SpriteBatch.DrawString(Font, content, offset, Color, Rotation, Origin, Scale, SpriteEffects, LayerDepth);
        }

        public override void Update(GameTime gameTime)
        {
        }
    }
}
