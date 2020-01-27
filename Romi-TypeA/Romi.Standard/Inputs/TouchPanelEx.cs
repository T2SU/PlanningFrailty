using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input.Touch;
using Romi.Standard.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Romi.Standard.Inputs
{
    public class TouchPanelEx : IEnumerable<TouchLocationEx>
    {
        private TouchCollection o_touches;
        
        public void UpdateData(GameTime gameTime)
        {
            o_touches = TouchPanel.GetState();
        }

        /// <summary>
        /// TouchLocationEx 열거형 반환. 모바일일 경우 좌표를 스케일업해서 반환함.
        /// </summary>
        /// <returns>모바일일 경우 스케일업된 좌표가 포함된 터치 좌표들</returns>
        public IEnumerator<TouchLocationEx> GetEnumerator()
        {
            return o_touches.Select(t => new TouchLocationEx(t.Position)).GetEnumerator();
        }

        /// <summary>
        /// TouchLocationEx 열거형 반환. 모바일일 경우 좌표를 스케일업해서 반환함.
        /// </summary>
        /// <returns>모바일일 경우 스케일업된 좌표가 포함된 터치 좌표들</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return o_touches.Select(t => new TouchLocationEx(t.Position)).GetEnumerator();
        }

        /// <summary>
        /// States if a touch screen is available.
        /// </summary>
        public bool IsConnected => o_touches.IsConnected;
        /// <summary>
        /// States if touch collection is read only.
        /// </summary>
        public bool IsReadOnly => o_touches.IsReadOnly;
        /// <summary>
        /// Returns the number of Microsoft.Xna.Framework.Input.Touch.TouchLocation items
        /// that exist in the collection.
        /// </summary>
        public int Count => o_touches.Count;
    }

    public class TouchLocationEx
    {
        public TouchLocationEx(Vector2 position)
        {
            if (App.Game.IsMobile)
            {
                Position = App.Game.GetScaledVector2(position);
                //Log.Debug($"TouchPosition = ({Position.X}, {Position.Y})");
            }
            else
            {
                Position = position;
            }
        }

        public Vector2 Position { get; }
    }
}
