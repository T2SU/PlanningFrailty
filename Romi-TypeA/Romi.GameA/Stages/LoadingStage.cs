using Microsoft.Xna.Framework;
using Romi.GameA.Resources;
using Romi.Standard.Stages;
using Romi.Standard.UIControls.Texts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Romi.GameA.Stages
{
    internal sealed class LoadingStage : RMStage
    {
        public LoadingStage() : base(-1)
        {
        }

        public override void Init(GameTime gameTime)
        {
            gameComponents.Add(new RMText(FontType.NanumSquareRound24, "Loading...", new Point(90, 42)) { Color = Color.LightYellow });
        }

        public override void UnInit()
        {
            gameComponents.Clear();
        }
    }
}
