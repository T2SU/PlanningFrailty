using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Romi.Standard.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Romi.Standard.Stages
{
    internal class TransferStage : Stage
    {
        readonly Action<Stage> changeStage;

        TransferState state;
        double blackFade;
        Stage renderingStage;

        internal TransferStage(Stage current, Stage next, Action<Stage> change) : base(-1)
        {
            CurrentStage = current ?? throw new ArgumentNullException(nameof(current));
            NextStage = next ?? throw new ArgumentNullException(nameof(next));
            changeStage = change ?? throw new ArgumentNullException(nameof(change));
            renderingStage = CurrentStage;
            state = TransferState.None;
            this.State = StageState.Normal;
        }

        public Stage CurrentStage { get; }

        public Stage NextStage { get; }

        // 장면 전환
        //  1. 스테이지 인큐
        //  2. TransferStage 아닐 경우, 등록
        //  3. 장면 전환

        public override void Update(GameTime gameTime)
        {
            switch (state)
            {
                case TransferState.Transferring:
                    Log.Debug($"[Transferring Stage] Unload Previous Stage");
                    CurrentStage.Unload(gameTime);
                    Log.Debug($"[Transferring Stage] Loading Next Stage");
                    NextStage.Load(gameTime);
                    renderingStage = null;
                    state = TransferState.WaitingLoad;
                    break;
                case TransferState.WaitingLoad:
                    if (NextStage.State == StageState.Normal)
                    {
                        Log.Debug($"[Transferring Stage] Loaded Next Stage");
                        renderingStage = NextStage;
                        state = TransferState.FadeIn;
                        Log.Debug($"[Transferring Stage] Starting Fade-In.");
                    }
                    break;
            }
        }

        public override void Draw(GameTime gameTime)
        {
            if (renderingStage == null) return;
            RenderCurrentStage(gameTime);

            double fade = 765 * gameTime.ElapsedGameTime.TotalSeconds;

            switch (state)
            {
                case TransferState.None:
                    state = TransferState.FadeOut;
                    Log.Debug($"[Transferring Stage] Starting Fade-Out.");
                    break;
                case TransferState.FadeOut:
                    if (blackFade >= 255)
                    {
                        Log.Debug($"[Transferring Stage] Entering Transfer Level.");
                        state = TransferState.Transferring;
                    }
                    else
                    {
                        blackFade += fade;
                    }
                    break;
                case TransferState.FadeIn:
                    if (blackFade <= 0)
                    {
                        changeStage(NextStage);
                        blackFade = 0.0f;
                        Log.Debug($"[Transferring Stage] Replaced with Next Stage.");
                    }
                    else
                    {
                        blackFade -= fade;
                    }
                    break;
            }

            DrawBlackMask();
        }

        private void RenderCurrentStage(GameTime gameTime)
        {
            renderingStage.Draw(gameTime);
        }

        private void DrawBlackMask()
        {
            // draw black mask
            var gd = App.Game.GraphicsDevice;
            var mask = new Texture2D(gd, 1, 1);
            mask.SetData(new[] { Color.Black });
            App.SpriteBatch.Draw(mask, new Rectangle(0, 0, gd.Viewport.Width, gd.Viewport.Height), Color.Black * (float)(blackFade / 255));
        }

        public enum TransferState
        {
            None,
            FadeOut,
            Transferring,
            WaitingLoad,
            FadeIn
        }
    }
}
