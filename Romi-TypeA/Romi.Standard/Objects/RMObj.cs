using Microsoft.Xna.Framework;
using System;

namespace Romi.Standard.Objects
{
    public abstract class RMObj : DrawableGameComponent
    {
        protected RMObj(int x, int y) : base(App.Game)
        {
            Position = new Point(x, y);
        }

        protected RMObj(Point pt) : base(App.Game)
        {
            Position = pt;
        }

        public Point Position { get; set; }
    }
}
