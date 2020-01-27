using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using Microsoft.Xna.Framework;
using Romi.GameA.Resources;
using Romi.GameA.Stages;
using Romi.Standard;
using Romi.Standard.Graphics;
using Romi.Standard.Resources;

namespace Romi.GameA
{
    [Activity(Label = "Romi.GameA"
        , MainLauncher = true
        , Icon = "@drawable/icon"
        , Theme = "@style/Theme.Splash"
        , AlwaysRetainTaskState = true
        , LaunchMode = Android.Content.PM.LaunchMode.SingleInstance
        , ScreenOrientation = ScreenOrientation.Landscape
        , ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.Keyboard | ConfigChanges.KeyboardHidden | ConfigChanges.ScreenSize | ConfigChanges.ScreenLayout)]
    public class Activity1 : Microsoft.Xna.Framework.AndroidGameActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            ContentLoaderMan.RegisterType<FontLoader>();
            var g = new RMGame(true, new TitleStage());
            App.GraphicsDevice.PreferredBackBufferWidth = DisplayConst_Mobile.Width;
            App.GraphicsDevice.PreferredBackBufferHeight = DisplayConst_Mobile.Height;
            App.GraphicsDevice.SupportedOrientations = DisplayOrientation.LandscapeLeft | DisplayOrientation.LandscapeRight;
            SetContentView((View)g.Services.GetService(typeof(View)));
            g.Run();
        }
    }
}

