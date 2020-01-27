using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Romi.Standard.Stages
{
    /// <summary>
    /// 기본 빈 스테이지.. 없으면 심심하니까
    /// </summary>
    public sealed class RMEmptyStage : Stage
    {
        public RMEmptyStage() : base(-1)
        {
        }

        public override void Draw(GameTime gameTime)
        {
        }

        public override void Update(GameTime gameTime)
        {
        }
    }
}
