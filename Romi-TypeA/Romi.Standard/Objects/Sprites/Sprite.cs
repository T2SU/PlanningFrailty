using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Romi.Standard.Objects.Sprites
{
    /// <summary>
    /// 2D 텍스쳐를 표시하기 위한 스프라이트 클래스
    /// </summary>
    public class Sprite : RMObj
    {
        private string name;
        private Texture2D texture;

        public Sprite(string spriteName, Point pt) : base(pt)
        {
            Name = spriteName;
        }

        public Sprite(string spriteName, int x, int y) : base(x, y)
        {
            Name = spriteName;
        }

        /// <summary>
        /// 스프라이트 이름. 이름이 설정될 때 자동으로 텍스쳐도 함께 로드한다.
        /// </summary>
        public string Name
        {
            get => name;
            set
            {
                texture = App.Game.Content.Load<Texture2D>($"Sprites/{name = value}");
                if (Area == Rectangle.Empty)
                {
                    Area = new Rectangle(Position, new Point(texture.Width, texture.Height));
                }
            }
        }

        /// <summary>
        /// 스프라이트가 그려질 영역. 기본 값은 비어있으나, 이름이 설정되어 텍스쳐가 로드될 때 영역이 비어있으면 자동으로 위치 및 텍스쳐 사이즈로 영역 생성.
        /// </summary>
        public Rectangle Area { get; set; } = Rectangle.Empty;

        public float Rotation { get; set; }

        public Vector2 Origin { get; set; } = Vector2.Zero;

        public Vector2 Scale { get; set; } = new Vector2(1.0f, 1.0f);

        public SpriteEffects Effects { get; set; }

        public float LayerDepth { get; set; }

        public float Opacity { get; set; } = 1.0f;

        public override void Draw(GameTime gameTime)
        {
            App.SpriteBatch.Draw(texture, Position.ToVector2(), null, Color.White * Opacity, Rotation, Origin, Scale, Effects, LayerDepth);
        }
    }
}
