using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loita.Components
{
    /// <summary>
    /// 可以被玩家看到的组件接口
    /// </summary>
    internal interface IInfusion
    {
        /// <summary>
        /// 组件名字
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// 组件介绍
        /// </summary>
        public string Description { get; }

        /// <summary>
        /// 绘制组件说明
        /// </summary>
        /// <param name="sb">画笔</param>
        /// <param name="startPos">位置</param>
        /// <param name="containerSize">容器大小</param>
        /// <param name="size">预计需要调整的大小</param>
        public void DrawTips(SpriteBatch sb, Vector2 startPos, Vector2 containerSize, out Vector2 size);
    }
}