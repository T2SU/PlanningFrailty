using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Romi.Standard.Inputs
{
    public class KeyboardStateEx
    {
        private readonly Dictionary<Keys, KeyboardKeyDownData> keyDownData = new Dictionary<Keys, KeyboardKeyDownData>();
        private KeyboardState previousKeyState;
        private KeyboardState keyState;

        internal KeyboardStateEx()
        {
            // keyDownData 멤버 변수의 내용에 Keys의 값 미리 모두 넣어두기
            foreach (Keys key in Enum.GetValues(typeof(Keys)))
            {
                keyDownData.Add(key, new KeyboardKeyDownData());
            }
        }

        /// <summary>
        /// 키보드 데이터 갱신
        /// </summary>
        /// <param name="gameTime">현재 GameTime</param>
        public void UpdateData(GameTime gameTime)
        {
            previousKeyState = keyState;
            keyState = Keyboard.GetState();
            foreach (var k in keyDownData)
                k.Value.UpdateData(gameTime, keyState[k.Key]);
        }

        /// <summary>
        /// Returns the state of a specified key.
        /// </summary>
        /// <param name="key">The key to query.</param>
        /// <returns>The state of the key.</returns>
        public KeyState this[Keys key] => keyState[key];

        /// <summary>
        /// Gets the current state of the Caps Lock key.
        /// </summary>
        public bool CapsLock => keyState.CapsLock;

        /// <summary>
        /// Gets the current state of the Num Lock key.
        /// </summary>
        public bool NumLock => keyState.NumLock;

        /// <summary>
        /// Returns an array of values holding keys that are currently being pressed.
        /// </summary>
        /// <returns>The keys that are currently being pressed.</returns>
        public Keys[] GetPressedKeys() => keyState.GetPressedKeys();

        /// <summary>
        /// Gets whether given key is currently being pressed.
        /// </summary>
        /// <param name="key">The key to query.</param>
        /// <returns>true if the key is pressed; false otherwise.</returns>
        public bool IsKeyDown(Keys key) => keyState.IsKeyDown(key);
        
        /// <summary>
        /// Gets whether given key is currently being not pressed.
        /// </summary>
        /// <param name="key">The key to query.</param>
        /// <returns>true if the key is not pressed; false otherwise.</returns>
        public bool IsKeyUp(Keys key) => keyState.IsKeyUp(key);

        /// <summary>
        /// 키가 한 번만 눌린 여부를 반환
        /// </summary>
        /// <param name="key">눌린 여부를 얻을 키</param>
        /// <returns></returns>
        public bool IsKeyTapped(Keys key) => previousKeyState != null && keyState != null && 
            previousKeyState[key] == KeyState.Down && keyState[key] == KeyState.Up;
    }

    internal class KeyboardKeyDownData
    {
        internal GameTime keydownTime;
        internal GameTime keyupTime;

        internal void UpdateData(GameTime gameTime, KeyState state)
        {
            switch (state)
            {
                case KeyState.Down:
                    keyupTime = default;
                    keydownTime = gameTime;
                    break;
                case KeyState.Up:
                    if (keydownTime != default)
                    {
                        // 키 다운 되었다가, 키 업 된 적이 없다면 키 업 설정
                        if (keyupTime == default)
                        {
                            keyupTime = gameTime;
                        }
                        // 키 다운 되었다가, 키 업 된 적이 없다면 키 업 이벤트 삭제 
                        else
                        {
                            keyupTime = default;
                            keydownTime = default;
                        }
                    }
                    break;
            }
        }
    }
}
