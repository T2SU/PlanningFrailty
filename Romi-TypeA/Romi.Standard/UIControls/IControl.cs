using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Romi.Standard.UIControls
{
    public interface IControl
    {
        Vector2 Origin { get; set; }

        Vector2 Size { get; set; }

        Vector2 Rect { get; set; }
    }
}
