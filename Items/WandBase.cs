using Loita.Components.LoitaComponents;
using Loita.Globals;

using Microsoft.Xna.Framework;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;

namespace Loita.Items
{
    /// <summary>
    /// 法杖基类
    /// </summary>
    internal abstract class WandBase : ModItem
    {
        /// <summary>
        /// 可装载组件的实体
        /// </summary>
        public new IEntity Entity => Item.GetGlobalItem<GItem>();

        /// <summary>
        /// 组件槽
        /// </summary>
        public CInfusionSlot InfusionSlot => Entity.GetComponent<CInfusionSlot>();

        /// <summary>
        /// 组件槽大小
        /// </summary>
        public virtual int SlotCount => 1;

        public override void SetDefaults()
        {
            base.SetDefaults();
            Entity.AddComponent<CInfusionSlot>(Entity, SlotCount);
            SetDefaultInfusion();
            int i = 0;
            InfusionSlot.InitActivableSpace(ref i);
        }

        /// <summary>
        /// 设置默认组件（初始组件）
        /// </summary>
        public virtual void SetDefaultInfusion()
        {
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            base.ModifyTooltips(tooltips);
            var count = InfusionSlot.ActivableSpace.FindAll(c => c != null).Count;
            if (count > 0)
                tooltips.Add(new TooltipLine(Loita.Instance, "Loita:Infusions", $"已装载组件({count}/{SlotCount})："));
        }

        public override void PostDrawTooltip(ReadOnlyCollection<DrawableTooltipLine> lines)
        {
            base.PostDrawTooltip(lines);
            var asp = InfusionSlot.ActivableSpace;
            if (asp.FindAll(c => c != null).Count == 0)
                return;
            var f = lines.First();
            Vector2 pos = new Vector2(f.X, f.Y);
            foreach (var l in lines)
            {
                pos.X = Math.Min(pos.X, l.X);
                pos.Y = Math.Max(pos.Y, l.Y + FontAssets.ItemStack.Value.MeasureString(l.Text).Y);
            }
            int i = 0;
            Vector2 changePos = pos;
            foreach (var c in asp)
            {
                if (c != null)
                {
                    Main.spriteBatch.Draw(c.Texture, changePos, Color.White);
                    changePos.X += c.Texture.Width + 6f;
                    i++;
                    if (i == 5)
                    {
                        changePos.Y += c.Texture.Height + 6f;
                        changePos.X = pos.X;
                        i = 0;
                    }
                }
            }
        }
    }
}