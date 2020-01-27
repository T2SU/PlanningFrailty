using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Romi.Standard.Diagnostics;
using SpriteFontPlus;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Romi.Standard.Resources
{
    public static class FontMan
    {
        public const int FontBitmapWidth = 32;
        public const int FontBitmapHeight = 64;

        private static readonly Dictionary<string, SpriteFont> _fonts
            = new Dictionary<string, SpriteFont>();
        private static SpriteFont defaultFont;

        public static CharacterRange Korean1 { get; } = new CharacterRange((char)0xAC00, (char)0xD7AF);

        public static CharacterRange Korean2 { get; } = new CharacterRange((char)0x3130, (char)0x318F); // 호환용 한글 자모

        public static CharacterRange Korean3 { get; } = new CharacterRange((char)0x1100, (char)0x11FF); // 한글 자모

        public static CharacterRange CJKSymbolsPunctuation { get; } = new CharacterRange((char)0x3000, (char)0x303F); // 한중일 기호 및 구두점

        public static CharacterRange Symbols1 { get; } = new CharacterRange((char)0x2600, (char)0x26FF); // 특수문자

        public static SpriteFont DefaultFont
        {
            get
            {
                if (defaultFont == null)
                    return _fonts.Values.Last();
                return defaultFont;
            }
            set => defaultFont = value;
        }

        public static SpriteFont GetSpriteFont(string fontName)
        {
            if (fontName == null)
                return DefaultFont;
            return _fonts[fontName];
        }

        public static void RegisterFont(string fontName, float fontSize, string contentPath)
        {
            using (var s = TitleContainer.OpenStream(Path.Combine(App.Game.Content.RootDirectory, contentPath)))
            using (var ms = new MemoryStream())
            {
                s.CopyTo(ms);
                RegisterFont(fontName, fontSize, ms.ToArray());
            }
        }

        public static void RegisterFont(string fontName, float fontSize, byte[] fontData)
        {
            Log.Debug($"ROMI Type-A Loading Font [{fontName}].");
            var fontResult = TtfFontBaker.Bake(
                fontData, 
                fontSize, 
                (int) Math.Pow(2, Math.Ceiling(Math.Log(FontBitmapWidth * fontSize) / Math.Log(2))),
                (int) Math.Pow(2, Math.Ceiling(Math.Log(FontBitmapHeight * fontSize) / Math.Log(2))),
                new[] { 
                    CharacterRange.BasicLatin,
                    CharacterRange.Latin1Supplement,
                    CharacterRange.LatinExtendedA,
                    CharacterRange.Cyrillic,
                    Korean1,
                    Korean2,
                    Korean3,
                    CJKSymbolsPunctuation,
                    Symbols1
                });
            _fonts.Add(fontName, fontResult.CreateSpriteFont(App.Game.GraphicsDevice));
            Log.Debug($"ROMI Type-A Loaded Font [{fontName}].");
        }

        public static void UnregisterAll()
        {
            _fonts.Clear();
        }
    }
}
