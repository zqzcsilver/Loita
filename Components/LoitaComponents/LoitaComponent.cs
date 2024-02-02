using FontStashSharp;

using Loita.Utils;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using ReLogic.Content;

using System;
using System.Collections.Generic;
using System.IO;

using Terraria.ModLoader;

namespace Loita.Components.LoitaComponents
{
    /// <summary>
    /// 可拆卸组件的基类
    /// </summary>
    internal abstract class LoitaComponent : ComponentBase, IBinarySupport, IInfusion
    {
        protected LoitaComponent(IEntity entity) : base(entity)
        {
        }

        /// <summary>
        /// 组件的子组件
        /// </summary>
        public virtual List<LoitaComponent> ActivableSpace { get; }

        /// <summary>
        /// 组件在组件槽里的索引
        /// </summary>
        public virtual int Index { get; set; }

        /// <summary>
        /// 组件的父组件
        /// </summary>
        public virtual LoitaComponent Parent { get; set; }

        /// <summary>
        /// 组件的贴图
        /// </summary>
        public virtual Texture2D Texture => ModContent.Request<Texture2D>(TexturePath, AssetRequestMode.ImmediateLoad).Value;

        /// <summary>
        /// 组件的贴图路径
        /// </summary>
        public virtual string TexturePath => GetType().FullName.Replace('.', '/');

        /// <summary>
        /// 组件名字
        /// </summary>
        public virtual string Name => "Loita Component";

        /// <summary>
        /// 组件介绍
        /// </summary>
        public virtual string Description => "This is a Loita Component";

        /// <summary>
        /// 可以被扩展（尚未实装）
        /// </summary>
        public virtual bool CanExpanded => false;

        /// <summary>
        /// 触发其与其子组件
        /// </summary>
        /// <param name="info">法术信息</param>
        public virtual void Apply(SpellInfo info)
        {
        }

        /// <summary>
        /// 加载组件
        /// </summary>
        public override void Load()
        {
        }

        /// <summary>
        /// 初始化子组件
        /// </summary>
        /// <param name="index">组件槽内的索引</param>
        public virtual void InitActivableSpace(ref int index)
        {
            Index = index;
        }

        /// <summary>
        /// 绘制组件说明
        /// </summary>
        /// <param name="sb">画笔</param>
        /// <param name="startPos">位置</param>
        /// <param name="containerSize">容器大小</param>
        /// <param name="size">预计需要调整的大小</param>
        public virtual void DrawTips(SpriteBatch sb, Vector2 startPos, Vector2 containerSize, out Vector2 size)
        {
            size = Vector2.Zero;
            var nameFont = Loita.DefaultFontSystem.GetFont(38f);
            var desFont = Loita.DefaultFontSystem.GetFont(24f);
            var name = StringUtil.GetWordWrapString1(Name, nameFont, containerSize.X);
            var description = StringUtil.GetWordWrapString1(Description, desFont, containerSize.X);
            var nameSize = nameFont.MeasureString(name);
            var desSize = desFont.MeasureString(description);

            if (nameSize.X >= containerSize.X || desSize.X >= containerSize.X)
                size.X = containerSize.X;
            else
                size.X = Math.Max(nameSize.X, desSize.X);
            size.Y = nameSize.Y + desSize.Y;

            sb.DrawString(nameFont, name, startPos, Color.White, null, 0f,
                default, 0f, 0f, 0f, TextStyle.None,
                FontSystemEffect.Stroked, 2);
            startPos.Y += nameSize.Y;
            sb.DrawString(desFont, description, startPos, Color.White, null, 0f,
                default, 0f, 0f, 0f, TextStyle.None,
                FontSystemEffect.Stroked, 2);
        }

        public virtual void WriteToBinary(BinaryWriter bw)
        {
        }

        public virtual void ReadOnBinary(BinaryReader br)
        {
        }
    }
}