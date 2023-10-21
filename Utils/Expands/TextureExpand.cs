using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Loita.Utils.Expands
{
    internal static class TextureExpand
    {
        /// <summary>
        /// 获取图片向量大小
        /// </summary>
        /// <param name="texture"></param>
        /// <returns></returns>
        public static Vector2 GetSize(this Texture2D texture)
        {
            return new Vector2(texture.Width, texture.Height);
        }

        /// <summary>
        /// 获取图片大小
        /// </summary>
        /// <param name="texture"></param>
        /// <returns></returns>
        public static Point GetIntSize(this Texture2D texture)
        {
            return new Point(texture.Width, texture.Height);
        }
    }
}