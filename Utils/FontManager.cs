using System.Collections.Generic;

using FontStashSharp;

namespace Loita.Utils
{
    internal class FontManager
    {
        private Dictionary<string, FontSystem> _fontSystemCache;
        public FontSystem this[string path] => GetFontSystem(path);

        public FontManager()
        {
            _fontSystemCache = new Dictionary<string, FontSystem>();
        }

        public FontSystem GetFontSystem(string path)
        {
            if (!_fontSystemCache.ContainsKey(path))
            {
                FontSystem fontSystem = new FontSystem();
                fontSystem.AddFont(Loita.Instance.GetFileBytes(path));
                _fontSystemCache.Add(path, fontSystem);
            }
            return _fontSystemCache[path];
        }

        public void ClearCache() => _fontSystemCache.Clear();
    }
}