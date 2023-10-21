using System.Collections.Generic;

namespace Loita.Utils.Expands
{
    internal static class ListExpands
    {
        public static bool Contains<T>(this List<T> me, List<T> list)
        {
            if (list == null || list.Count == 0) return true;
            foreach (var t in list)
            {
                if (!me.Contains(t)) return false;
            }
            return true;
        }

        public static bool Intersect<T>(this List<T> me, List<T> list)
        {
            if (list == null || list.Count == 0) return false;
            foreach (var t in list)
            {
                if (me.Contains(t)) return true;
            }
            return false;
        }

        /// <summary>
        /// 获取两个队列不同的部分
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="older">原队列</param>
        /// <param name="newer">作为对比的队列</param>
        /// <returns>additional为新增部分，decrease为减少部分</returns>
        public static (List<T> additional, List<T> decrease) GetDifferenceSection<T>(this List<T> older, List<T> newer)
        {
            List<T> additional = new List<T>();
            List<T> decrease = new List<T>();
            foreach (var item in older)
            {
                if (!newer.Contains(item))
                    decrease.Add(item);
            }
            foreach (var item in newer)
            {
                if (!older.Contains(item))
                    additional.Add(item);
            }
            return (additional, decrease);
        }
    }
}