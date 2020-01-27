using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Romi.Standard.Inputs;

namespace Romi.Standard
{
    public static class App
    {
        public static GraphicsDeviceManager GraphicsDevice { get; internal set; }

        public static SpriteBatch SpriteBatch { get; internal set; }

        public static RMGame Game { get; internal set; }

        public static MouseStateEx MouseInput { get; } = new MouseStateEx();

        public static KeyboardStateEx KeyboardInput { get; } = new KeyboardStateEx();

        public static TouchPanelEx TouchInput { get; } = new TouchPanelEx();
    }
}
