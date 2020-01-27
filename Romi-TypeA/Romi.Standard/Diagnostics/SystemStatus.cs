using Microsoft.Xna.Framework;
using Romi.Standard.UIControls.Texts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Romi.Standard.Diagnostics
{
    public class SystemStatus : DrawableGameComponent
    {
        private readonly StringBuilder debugStringBuilder;
        private readonly FPSCounter fpsCounter;

        public SystemStatus(Game game) : base(game)
        {
            debugStringBuilder = new StringBuilder();
            fpsCounter = new FPSCounter();
        }

        public override void Update(GameTime gameTime)
        {
            fpsCounter.UpdateFPS(gameTime);
            debugStringBuilder.Clear();
            Print("Romi Type-A " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            Print($"FPS {fpsCounter.FPS}");
            Print($"Mouse: ({App.MouseInput.X}, {App.MouseInput.Y})");
            if (App.TouchInput.IsConnected)
            {
                Print($"TouchInput: ({App.TouchInput.Count}) [{string.Join(", ", App.TouchInput.Select(t => $"({t.Position.X}, {t.Position.Y})"))}]");
            }
        }

        public override void Draw(GameTime gameTime)
        {
            new RMText(debugStringBuilder.ToString())
            {
                Color = Color.FloralWhite,
                HorizontalAlignment = HorizontalAlignment.Right,
                VerticalAlignment = VerticalAlignment.Top
            }
            .Draw(gameTime);
        }

        private void Print(string s)
        {
            if (debugStringBuilder.Length > 0)
                debugStringBuilder.AppendLine();
            debugStringBuilder.Append(s);
        }
    }

    internal class FPSCounter
    {
        private double fpsMeasure;
        private int fpsCounter;

        // Frame Per Second 업데이트
        internal void UpdateFPS(GameTime gameTime)
        {
            if (fpsMeasure >= 1)
            {
                FPS = fpsCounter;
                fpsCounter = 0;
                fpsMeasure = 0;
            }
            else
            {
                ++fpsCounter;
                fpsMeasure += gameTime.ElapsedGameTime.TotalSeconds;
            }
        }

        public int FPS { get; private set; }
    }
}
