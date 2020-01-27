using Microsoft.Xna.Framework.Content;
using Romi.Standard;
using Romi.Standard.Resources;

namespace Romi.GameA.Resources
{
    public class FontLoader : IContentLoader
    {
        public void Load(RMGame game, ContentManager contentManager)
        {
            FontMan.RegisterFont(FontType.NanumSquareRound24.ToString(), 24, "Fonts/NanumSquareRoundR.ttf");
            //FontMan.RegisterFont(FontType.NanumBarunGothic16.ToString(), 16, "Fonts/NanumBarunGothic.ttf");
            FontMan.RegisterFont(FontType.NanumGothic16.ToString(), 16, "Fonts/NanumGothic.ttf");
            //FontMan.RegisterFont(FontType.NanumSquareR16.ToString(), 16, "Fonts/NanumSquareR.ttf");
            FontMan.RegisterFont(FontType.NanumGothic64.ToString(), 64, "Fonts/NanumGothic.ttf");
            FontMan.DefaultFont = FontMan.GetSpriteFont(FontType.NanumGothic16.ToString());
        }

        public void Unload(RMGame game, ContentManager contentManager)
        {
            FontMan.UnregisterAll();
        }
    }

    public enum FontType
    {
        NanumGothic64,
        NanumSquareRound24,
        //NanumBarunGothic16,
        NanumGothic16,
        //NanumSquareR16
    }
}
