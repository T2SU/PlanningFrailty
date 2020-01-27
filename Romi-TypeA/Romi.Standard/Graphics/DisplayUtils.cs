using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Romi.Standard.Graphics
{
    public static class DisplayUtils
    {
        public static float ScaleX { get; set; }

        public static float ScaleY { get; set; }

        private static float CalcRatio(Vector2 sizes)
        {
            return sizes.Y / sizes.X;
        }

        public static Vector2 CalcPos(Vector2 refPos, Vector2 refScreenSize, Vector2 currentScreenSize)
        {
            return new Vector2(
                refPos.X / refScreenSize.X * currentScreenSize.X,
                refPos.Y / refScreenSize.Y * currentScreenSize.Y);
        }

        public static Vector2 CalculateSize(Vector2 refSize, Vector2 refScreenSize,
                                       Vector2 currentScreenSize)
        {
            float origRatio = CalcRatio(refSize);
            float perW = refSize.X * 100f / refScreenSize.X;
            float newW = perW / 100f * currentScreenSize.X;
            float newH = newW * origRatio;
            return new Vector2(newW, newH);
        }
    }
}
