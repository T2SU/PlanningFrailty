using Microsoft.Xna.Framework;
using Romi.Standard.Objects;
using Romi.Standard.UIControls.Texts;
using System;

namespace Romi.GameA.Objects
{
    public sealed class RMExpirableText : RMText, IExpirableObj
    {
        public RMExpirableText(Enum enumValue, string content, Point pt, GameTime gameTime, float duration) : base(enumValue, content, pt)
        {
            Created = gameTime.TotalGameTime;
            End = Created.Add(TimeSpan.FromSeconds(duration));
        }

        public TimeSpan Created { get; }

        public TimeSpan End { get; }

        public bool IsExpired(GameTime gameTime)
        {
            return End < gameTime.TotalGameTime;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            var elapsed = (gameTime.TotalGameTime - Created).TotalSeconds;
            var total = (End - Created).TotalSeconds;
            if (elapsed >= total / 2.0)
            {
                Color *= (float)Math.Sin(Math.PI * elapsed / total);
            }
        }
    }
}
