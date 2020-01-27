using Romi.GameX.Resources;
using Romi.GameX.Stages;
using Romi.Standard;
using Romi.Standard.Graphics;
using Romi.Standard.Resources;
using System;

namespace Romi.GameX
{
#if WINDOWS || LINUX
    /// <summary>
    /// The main class.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            ContentLoaderMan.RegisterType<FontLoader>();

            using (var game = new RMGame(false, new TitleStage()))
            {
                App.GraphicsDevice.PreferredBackBufferWidth = DisplayConst_PC.Width;
                App.GraphicsDevice.PreferredBackBufferHeight = DisplayConst_PC.Height;
                game.Run();
            }
        }
    }
#endif
}
