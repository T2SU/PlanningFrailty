using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Romi.Standard.Diagnostics;
using Romi.Standard.Events;
using Romi.Standard.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Romi.Standard.UIControls.Buttons
{
    public enum ButtonDrawState { Normal, Hover, Clicked, Disabled }

    public abstract class RMButton : RMObj, IInteractible
    {
        private InputSignal clickSignal;
        private ButtonDrawState buttonDrawState = ButtonDrawState.Normal;

        protected RMButton(int X, int Y, int Width, int Height) : base(X, Y)
        {
            this.Area = new Rectangle(X, Y, Width, Height);
            this.EnabledChanged += Button_EnabledChanged;
        }

        protected RMButton(Point pt, int Width, int Height) : base(pt)
        {
            this.Area = new Rectangle(pt.X, pt.Y, Width, Height);
            this.EnabledChanged += Button_EnabledChanged;
        }

        protected RMButton(Point pt, Point size) : base(pt)
        {
            this.Area = new Rectangle(pt, size);
            this.EnabledChanged += Button_EnabledChanged;
        }

        protected RMButton(Point pt, Rectangle rect) : base(pt)
        {
            this.Area = rect;
            this.EnabledChanged += Button_EnabledChanged;
        }

        protected RMButton(Point pt) : base(pt)
        {
            this.EnabledChanged += Button_EnabledChanged;
        }

        /// <summary>
        /// 버튼의 영역
        /// </summary>
        public Rectangle Area { get; set; }

        /// <summary>
        /// 버튼의 상태
        /// </summary>
        public ButtonDrawState ButtonDrawState
        {
            get => buttonDrawState;
            set
            {
                switch (buttonDrawState = value)
                {
                    case ButtonDrawState.Normal:
                    case ButtonDrawState.Hover:
                    case ButtonDrawState.Disabled:
                        clickSignal.Clear();
                        break;
                    case ButtonDrawState.Clicked:
                        clickSignal.Triggered = true;
                        break;
                }
            }
        }

        /// <summary>
        /// 버튼 상태를 갱신하는 함수
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            if (!Enabled) return;
            //var area = App.Game.CreateScaledRect(Area);
            var area = Area;
            if (App.TouchInput.IsConnected)
            {
                //Log.Debug($"TouchArea = [{area.X}, {area.Y}, {area.X + area.Width}, {area.Y + area.Height}]");
                if (App.TouchInput.Any(ti => area.Contains(ti.Position)))
                {
                    ButtonDrawState = ButtonDrawState.Clicked;
                }
                else
                {
                    if (clickSignal.Triggered)
                    {
                        ButtonDrawState = ButtonDrawState.Normal;
                        if (App.TouchInput.Count == 0)
                        {
                            OnClick();
                        }
                    }
                }
            }
            else if (area.Contains(App.MouseInput.Position))
            {
                if (clickSignal.Triggered)
                {
                    if (App.MouseInput.LeftButton == ButtonState.Released)
                    {
                        ButtonDrawState = ButtonDrawState.Normal;
                        OnClick();
                    }
                }
                else
                {
                    if (App.MouseInput.LeftButton == ButtonState.Pressed)
                    {
                        ButtonDrawState = ButtonDrawState.Clicked;
                    }
                    else
                    {
                        ButtonDrawState = ButtonDrawState.Hover;
                    }
                }
            }
            else
            {
                ButtonDrawState = ButtonDrawState.Normal;
            }
        }

        /// <summary>
        /// 버튼을 렌더링하는 메인 함수
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Draw(GameTime gameTime)
        {
            switch (ButtonDrawState)
            {
                case ButtonDrawState.Hover:
                    OnDrawHover(gameTime);
                    break;
                case ButtonDrawState.Normal:
                    OnDrawNormal(gameTime);
                    break;
                case ButtonDrawState.Clicked:
                    OnDrawClicked(gameTime);
                    break;
                case ButtonDrawState.Disabled:
                    OnDrawDisabled(gameTime);
                    break;
            }
        }

        private void Button_EnabledChanged(object sender, EventArgs e)
        {
            ButtonDrawState = Enabled ? ButtonDrawState.Normal : ButtonDrawState.Disabled;
        }

        /// <summary>
        /// Normal 상태일 때 버튼을 그리는 함수
        /// </summary>
        /// <param name="gameTime"></param>
        protected abstract void OnDrawNormal(GameTime gameTime);

        /// <summary>
        /// Hover 상태일 때 버튼을 그리는 함수
        /// </summary>
        /// <param name="gameTime"></param>
        protected abstract void OnDrawHover(GameTime gameTime);

        /// <summary>
        /// Clicked 상태일 때 버튼을 그리는 함수
        /// </summary>
        /// <param name="gameTime"></param>
        protected abstract void OnDrawClicked(GameTime gameTime);

        /// <summary>
        /// Disabled 상태일 때 버튼을 그리는 함수
        /// </summary>
        /// <param name="gameTime"></param>
        protected abstract void OnDrawDisabled(GameTime gameTime);

        /// <summary>
        /// 클릭 시에 실행될 함수
        /// </summary>
        public abstract void OnClick();
    }
}
