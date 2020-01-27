using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Romi.Standard.Objects
{
    public interface IExpirableObj
    {
        TimeSpan End { get; }

        bool IsExpired(GameTime gameTime);
    }
}
