using Microsoft.Xna.Framework;

namespace Loita.Utils.Expands
{
    internal static class GameTimeExpand
    {
        /// <summary>
        /// 获取刷新因数，为防止出现帧率不同导致出现计算结果不符，请在所有每帧增量上乘上此数
        /// </summary>
        /// <param name="gt"></param>
        /// <returns></returns>
        public static float GetRefreshFactor(this GameTime gt) => Loita.FRAME_NUMBER * (float)gt.ElapsedGameTime.TotalSeconds;
    }
}