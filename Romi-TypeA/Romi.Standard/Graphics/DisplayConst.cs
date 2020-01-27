using Microsoft.Xna.Framework;

namespace Romi.Standard.Graphics
{
    public static class DisplayConst_PC
    {
        public const int Width = 1024;

        public const int Height = 768;
    }

    public static class DisplayConst_Mobile
    {
        public const int Width = 800;

        public const int Height = 480;
    }

    public static class DisplayConst
    {
        public static int Width => App.Game.IsMobile ? DisplayConst_Mobile.Width : DisplayConst_PC.Width;

        public static int Height => App.Game.IsMobile ? DisplayConst_Mobile.Height : DisplayConst_PC.Height;
    }
}
