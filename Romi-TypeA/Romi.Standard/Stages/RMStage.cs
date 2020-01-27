using Microsoft.Xna.Framework;
using Romi.Standard.Diagnostics;
using Romi.Standard.Objects.Sprites;
using Romi.Standard.Stages.Configurations;
using System;

namespace Romi.Standard.Stages
{
    public abstract class RMStage : Stage
    {
        private static StageConfiguration TryLoadConfiguration(int id, StageConfiguration conf)
        {
            if (conf == null)
            {
                try
                {
                    conf = StageConfiguration.Load(id);
                }
                catch (Exception ex)
                {
                    Log.Error($"Failed to load stage configuration {id}");
                    Log.Exception(ex);
                }
            }
            return conf;
        }

        // 업데이트 및 그리기할 요소들
        protected readonly GameComponentCollection gameComponents;

        // 그리기만 할 요소들 (gameComponents 보다 먼저 그려짐)
        protected readonly GameComponentCollection sprites;

        protected RMStage(int id, StageConfiguration conf = null) : base(id, TryLoadConfiguration(id, conf))
        {
            gameComponents = new GameComponentCollection();
            sprites = new GameComponentCollection();
        }

        internal override void LoadStageConfiguration(StageConfiguration conf)
        {
            base.LoadStageConfiguration(conf);
            if (conf != null)
            {
                foreach (var image in conf.Back)
                {
                    sprites.Add(new Sprite(image.Path, image.Position)
                    {
                        Scale = new Vector2(image.ScaleX, image.ScaleY)
                    });
                }
            }
        }

        public override void Draw(GameTime gameTime)
        {
            foreach (GameComponent gc in sprites)
                if (gc is DrawableGameComponent dgc)
                    dgc.Draw(gameTime);
            foreach (GameComponent gc in gameComponents)
                if (gc is DrawableGameComponent dgc)
                    dgc.Draw(gameTime);
        }

        public override void Update(GameTime gameTime)
        {
            foreach (GameComponent gc in gameComponents)
                gc.Update(gameTime);
        }
    }
}
