using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Romi.Standard.Inputs
{
    public class MouseStateEx
    {
        private MouseState mouseState;

        public void UpdateData(GameTime gameTime)
        {
            mouseState = Mouse.GetState();
        }

        /// <summary>
        /// Gets horizontal position of the cursor in relation to the window.
        /// </summary>
        public int X => mouseState.X;

        /// <summary>
        /// Gets vertical position of the cursor in relation to the window.
        /// </summary>
        public int Y => mouseState.Y;

        /// <summary>
        /// Gets cursor position.
        /// </summary>
        public Point Position => new Point(X, Y);

        /// <summary>
        /// Gets state of the left mouse button.
        /// </summary>
        public ButtonState LeftButton => mouseState.LeftButton;

        /// <summary>
        /// Gets state of the middle mouse button.
        /// </summary>
        public ButtonState MiddleButton => mouseState.MiddleButton;

        /// <summary>
        /// Gets state of the right mouse button.
        /// </summary>
        public ButtonState RightButton => mouseState.RightButton;

        /// <summary>
        /// Returns cumulative scroll wheel value since the game start.
        /// </summary>
        public int ScrollWheelValue => mouseState.ScrollWheelValue;

        /// <summary>
        /// Returns the cumulative horizontal scroll wheel value since the game start
        /// </summary>
        public int HorizontalScrollWheelValue => mouseState.HorizontalScrollWheelValue;

        /// <summary>
        /// Gets state of the XButton1.
        /// </summary>
        public ButtonState XButton1 => mouseState.XButton1;

        /// <summary>
        /// Gets state of the XButton2.
        /// </summary>
        public ButtonState XButton2 => mouseState.XButton2;
    }
}
