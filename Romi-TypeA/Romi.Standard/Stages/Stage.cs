using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;
using Romi.Standard.Diagnostics;
using Romi.Standard.Stages.Configurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Romi.Standard.Stages
{
    public abstract class Stage : IDrawable, IUpdateable
    {
        internal readonly StageConfiguration stageConfiguration;

        protected Stage(int id, StageConfiguration conf = null)
        {
            ID = id;
            State = StageState.None;
            stageConfiguration = conf;
        }

        public int ID { get; }

        internal StageState State { get; set; }

        protected Song BGM { get; set; }

        // IDrawable 인터페이스 구현
        public event EventHandler<EventArgs> VisibleChanged;
        public event EventHandler<EventArgs> DrawOrderChanged;
        public int DrawOrder => 0;
        public bool Visible => true;

        // IUpdateable 인터페이스 구현
        public event EventHandler<EventArgs> EnabledChanged;
        public event EventHandler<EventArgs> UpdateOrderChanged;
        public bool Enabled => true;
        public int UpdateOrder => 0;

        public virtual void UnInit() { }
        public virtual void Init(GameTime gameTime) { }

        public virtual void Draw(GameTime gameTime) { }
        public virtual void Update(GameTime gameTime) { }

        internal virtual void LoadStageConfiguration(StageConfiguration conf)
        {
            if (!string.IsNullOrEmpty(conf.Info.BGM))
            {
                BGM = App.Game.Content.Load<Song>(conf.Info.BGM);
            }
        }

        internal async void Load(GameTime gameTime)
        {
            switch (State)
            {
                case StageState.None:
                    State = StageState.Loading;

                    // 스테이지 로딩에 관련된 함수들 호출
                    try
                    {
                        // 스테이지 설정 XML 파일 로드
                        if (stageConfiguration != null)
                        {
                            await Task.Run(() => LoadStageConfiguration(stageConfiguration));
                        }

                        // 스테이지 초기화 함수 실행
                        await Task.Run(() => Init(gameTime));
                    }
                    catch (Exception ex)
                    {
                        Log.Exception(ex);
                    }

                    // 로딩 완료.
                    State = StageState.Normal;

                    // BGM 재생
                    if (BGM != null)
                    {
                        MediaPlayer.IsRepeating = true;
                        MediaPlayer.Play(BGM);
                        Log.Debug($"[Stage] PlayBGM ({BGM.Name})");
                    }
                    break;
            }
        }

        internal async void Unload(GameTime gameTime)
        {
            if (BGM != null)
            {
                MediaPlayer.Stop();
                Log.Debug($"[Stage] StopBGM");
            }
            // 스테이지 언로딩에 관련된 함수들 호출
            try
            {
                // 스테이지 UnInit 함수 실행
                await Task.Run(() => UnInit());
            }
            catch (Exception ex)
            {
                Log.Exception(ex);
            }
            State = StageState.Unloaded;
        }
    }
}
