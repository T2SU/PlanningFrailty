using Microsoft.Xna.Framework.Content;

namespace Romi.Standard.Resources
{
    /// <summary>
    /// 컨텐츠 로드를 위한 인터페이스
    /// </summary>
    public interface IContentLoader
    {
        void Load(RMGame game, ContentManager contentManager);
        void Unload(RMGame game, ContentManager contentManager);
    }
}
