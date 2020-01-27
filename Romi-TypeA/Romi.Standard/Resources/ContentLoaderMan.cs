using Romi.Standard.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Romi.Standard.Resources
{
    /// <summary>
    /// 컨텐츠 로드를 위한 인터페이스들을 관리하는 클래스.
    /// </summary>
    public static class ContentLoaderMan
    {
        public const string RootDirectory = "Content";

        private static readonly HashSet<Type> contentLoaders;

        static ContentLoaderMan()
        {
            contentLoaders = new HashSet<Type>();
        }

        /// <summary>
        /// 컨텐츠 로더 클래스 등록. 가져다 쓸때 컨텐츠 로더 인스턴스를 생성하므로 타입만 등록시켜둔다.
        /// </summary>
        /// <typeparam name="T">IContentLoader를 상속한 타입</typeparam>
        public static void RegisterType<T>() where T : IContentLoader
        {
            contentLoaders.Add(typeof(T));
            Log.Debug($"Registered ContentLoader: ({typeof(T).FullName})");
        }

        internal static IEnumerable<IContentLoader> PickLoaders()
        {
            return contentLoaders
                .Select(t => Activator.CreateInstance(t))
                .OfType<IContentLoader>();
        }
    }
}
