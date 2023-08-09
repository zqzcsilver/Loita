using FontStashSharp;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System;

using Terraria;
using Terraria.GameContent;
using Terraria.ID;

namespace Loita.UI.UIElements
{
    internal delegate bool CheckPutSlotCondition(Item mouseItem);

    internal delegate void ExchangeItemHandler(BaseElement target);

    internal class UIItemSlot : UIPanel
    {
        /// <summary>
        /// 框贴图
        /// </summary>
        public Texture2D SlotBackTexture { get; set; }

        /// <summary>
        /// 是否可以放置物品
        /// </summary>
        public CheckPutSlotCondition CanPutInSlot { get; set; }

        /// <summary>
        /// 是否可以拿去物品
        /// </summary>
        public CheckPutSlotCondition CanTakeOutSlot { get; set; }

        /// <summary>
        /// 框内物品
        /// </summary>
        public Item ContainedItem { get; set; }

        /// <summary>
        /// 框的绘制的拐角尺寸
        /// </summary>
        public Vector2 CornerSize { get; set; }

        /// <summary>
        /// 绘制颜色
        /// </summary>
        public Color DrawColor { get; set; }

        /// <summary>
        /// 介绍
        /// </summary>
        public string Tooltip { get; set; }

        /// <summary>
        /// 更改物品时调用
        /// </summary>
        public event ExchangeItemHandler PostExchangeItem;

        /// <summary>
        /// 玩家拿取物品时调用
        /// </summary>
        public event ExchangeItemHandler OnPickItem;

        /// <summary>
        /// 玩家放入物品时调用
        /// </summary>
        public event ExchangeItemHandler OnPutItem;

        /// <summary>
        /// 在部件更新时调用
        /// </summary>
        public event ExchangeItemHandler PostUpdate;

        /// <summary>
        /// 透明度
        /// </summary>
        public float Opacity { get; set; }

        public UIItemSlot(Texture2D texture = default(Texture2D)) : base()
        {
            Opacity = 1f;
            ContainedItem = new Item();
            CanPutInSlot = null;
            SlotBackTexture = texture == default(Texture2D) ? TextureAssets.InventoryBack.Value : texture;
            DrawColor = Color.White;
            CornerSize = new Vector2(10, 10);
            Tooltip = "";
            Info.Width.SetValue(50f, 0f);
            Info.Height.SetValue(50f, 0f);
        }

        public override void LoadEvents()
        {
            base.LoadEvents();
            Events.OnLeftClick += element =>
            {
                //开启背包
                //Main.playerInventory = true;

                //当鼠标没物品，框里有物品的时候
                if (Main.mouseItem.type == 0 && ContainedItem != null && ContainedItem.type != 0)
                {
                    //如果可以拿起物品
                    if (CanTakeOutSlot == null || CanTakeOutSlot(ContainedItem))
                    {
                        //拿出物品
                        Main.mouseItem = ContainedItem.Clone();
                        ContainedItem = new Item();
                        ContainedItem.SetDefaults(0, true);

                        //调用委托
                        OnPickItem?.Invoke(this);

                        //触发放物品声音
                        //SoundEngine.PlaySound(7, -1, -1, 1, 1f, 0.0f);
                    }
                }
                //当鼠标有物品，框里没物品的时候
                else if (Main.mouseItem.type != 0 && (ContainedItem == null || ContainedItem.type == 0))
                {
                    //如果可以放入物品
                    if (CanPutInSlot == null || CanPutInSlot(Main.mouseItem))
                    {
                        //放入物品
                        ContainedItem = Main.mouseItem.Clone();
                        Main.mouseItem = new Item();
                        Main.mouseItem.SetDefaults(0, true);

                        //调用委托
                        OnPutItem?.Invoke(this);

                        //触发放物品声音
                        //SoundEngine.PlaySound(7, -1, -1, 1, 1f, 0.0f);
                    }
                }
                //当鼠标和框都有物品时
                else if (Main.mouseItem.type != 0 && ContainedItem != null && ContainedItem.type != 0)
                {
                    //如果不能放入物品
                    if (!(CanPutInSlot == null || CanPutInSlot(Main.mouseItem)))
                    {
                        //中断函数
                        return;
                    }

                    //如果框里的物品和鼠标的相同
                    if (Main.mouseItem.type == ContainedItem.type)
                    {
                        //框里的物品数量加上鼠标物品数量
                        ContainedItem.stack += Main.mouseItem.stack;
                        //如果框里物品数量大于数量上限
                        if (ContainedItem.stack > ContainedItem.maxStack)
                        {
                            //计算鼠标物品数量，并将框内物品数量修改为数量上限
                            var exceed = ContainedItem.stack - ContainedItem.maxStack;
                            ContainedItem.stack = ContainedItem.maxStack;
                            Main.mouseItem.stack = exceed;
                        }
                        //反之
                        else
                        {
                            //清空鼠标物品
                            Main.mouseItem = new Item();
                        }
                    }
                    //如果可以放入物品也能拿出物品
                    else if ((CanPutInSlot == null || CanPutInSlot(Main.mouseItem))
                        && (CanTakeOutSlot == null || CanTakeOutSlot(ContainedItem)))
                    {
                        //交换框内物品和鼠标物品
                        var tmp = Main.mouseItem.Clone();
                        Main.mouseItem = ContainedItem;
                        ContainedItem = tmp;
                    }

                    //触发放物品声音
                    //SoundEngine.PlaySound(7, -1, -1, 1, 1f, 0.0f);
                }
                //反之
                else
                {
                    //中断函数
                    return;
                }

                //调用委托
                PostExchangeItem?.Invoke(this);
            };
        }

        public override void Update(GameTime gameTime)
        {
            PostUpdate?.Invoke(this);
            base.Update(gameTime);
        }

        protected override void DrawSelf(SpriteBatch sb)
        {
            base.DrawSelf(sb);

            //调用原版的介绍绘制
            if (ContainedItem != null && ContainsPoint(Main.MouseScreen) && ContainedItem.type != ItemID.None)
            {
                Main.hoverItemName = ContainedItem.Name;
                Main.HoverItem = ContainedItem.Clone();
            }
            //获取当前UI部件的信息
            var DrawRectangle = Info.HitBox;
            ////绘制物品框
            //DrawAdvBox(sb, (int)DrawRectangle.X, (int)DrawRectangle.Y,
            //    (int)DrawRectangle.Width, (int)DrawRectangle.Height,
            //    DrawColor * Opacity, SlotBackTexture, CornerSize, 1f);
            if (ContainedItem != null && ContainedItem.type != ItemID.None)
            {
                var frame = Main.itemAnimations[ContainedItem.type] != null ? Main.itemAnimations[ContainedItem.type].GetFrame(TextureAssets.Item[ContainedItem.type].Value) : Item.GetDrawHitbox(ContainedItem.type, null);

                float scale = Info.Size.X / Math.Max(frame.Width, frame.Height) * 0.7f;

                var pos = new Vector2(DrawRectangle.X + DrawRectangle.Width / 2,
                    DrawRectangle.Y + DrawRectangle.Height / 2) - (new Vector2(frame.Width, frame.Height) / 2f * scale);
                //绘制物品贴图
                sb.Draw(TextureAssets.Item[ContainedItem.type].Value, pos,
                    new Rectangle?(frame), Color.White * Opacity, 0f, Vector2.Zero,
                    scale, 0, 0);

                //绘制物品左下角那个代表数量的数字
                if (ContainedItem.stack > 1)
                {
                    var font = Loita.DefaultFontSystem.GetFont(Info.Size.Y * 0.32f);
                    var text = ContainedItem.stack.ToString();
                    var textSize = font.MeasureString(text);
                    sb.DrawString(font, text, new Vector2(DrawRectangle.Center.X - textSize.X / 2f, DrawRectangle.Bottom - textSize.Y),
                        Color.White * Opacity, Vector2.One);
                }
            }
        }
    }
}