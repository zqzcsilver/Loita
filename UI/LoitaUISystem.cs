﻿using System;
using System.Collections.Generic;
using System.Linq;

using Loita.UI.UIContainers.DebugUI.DebugItems;
using Loita.UI.UIElements;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;

namespace Loita.UI
{
    internal class LoitaUISystem
    {
        public static LoitaUISystem Instance => Loita.UISystem;

        /// <summary>
        /// 存放着所有<see cref="UIContainerElement"/>实例的字典
        /// </summary>
        public Dictionary<string, UIContainerElement> Elements { get; private set; }

        private Dictionary<string, UIContainerElement> ElementsCache = new Dictionary<string, UIContainerElement>();

        /// <summary>
        /// 访问顺序
        /// </summary>
        public List<string> CallOrder { get; private set; }

        private List<string> CallOrderCache = new List<string>();

        /// <summary>
        /// 交互部件缓存
        /// </summary>
        private List<BaseElement> interactContainerElementsBuffer;

        /// <summary>
        /// 记录需要触发MouseLeftUp事件的部件
        /// </summary>
        private List<BaseElement> needCallMouseLeftUpElements;

        /// <summary>
        /// 记录需要触发MouseRightUp事件的部件
        /// </summary>
        private List<BaseElement> needCallMouseRightUpElements;

        /// <summary>
        /// 缓存鼠标左键状态
        /// </summary>
        private bool mouseLeftDown = false;

        /// <summary>
        /// 缓存鼠标右键状态
        /// </summary>
        private bool mouseRightDown = false;

        /// <summary>
        /// 鼠标左键冷却
        /// </summary>
        private KeyCooldown mouseLeftCooldown;

        /// <summary>
        /// 鼠标右键冷却
        /// </summary>
        private KeyCooldown mouseRightCooldown;

        public LoitaUISystem()
        {
            Elements = new Dictionary<string, UIContainerElement>();
            CallOrder = new List<string>();
            interactContainerElementsBuffer = new List<BaseElement>();
            needCallMouseLeftUpElements = new List<BaseElement>();
            needCallMouseRightUpElements = new List<BaseElement>();
            mouseLeftCooldown = new KeyCooldown(() =>
            {
                return Main.mouseLeft;
            });
            mouseRightCooldown = new KeyCooldown(() =>
            {
                return Main.mouseRight;
            });
        }

        /// <summary>
        /// 反射加载所有UIContainerElement
        /// </summary>
        public void Load()
        {
            LoggerItem.WriteLine($"[Loita:UI System]加载中...");
            var containers = from c in GetType().Assembly.GetTypes()
                             where !c.IsAbstract && c.IsSubclassOf(typeof(UIContainerElement))
                             select c;
            UIContainerElement element;
            foreach (var c in containers)
            {
                element = (UIContainerElement)Activator.CreateInstance(c);
                if (element.AutoLoad)
                    Register(element);
            }
            LoggerItem.WriteLine($"[Loita:UI System]加载完毕");
        }

        /// <summary>
        /// 执行逻辑
        /// </summary>
        /// <param name="gt"></param>
        public void Update(GameTime gt)
        {
            ElementsCache = new Dictionary<string, UIContainerElement>(Elements);
            CallOrderCache = new List<string>(CallOrder);
            if (CallOrderCache.Count == 0 || ElementsCache.Count == 0)
                return;

            List<BaseElement> interact = new List<BaseElement>();
            UIContainerElement child;
            Point mousePos = Main.MouseScreen.ToPoint();
            foreach (var key in CallOrderCache)
            {
                child = ElementsCache[key];
                child?.PreUpdate(gt);
                if (child != null && child.IsVisible)
                {
                    child.Update(gt);
                    if (interact.Count == 0)
                        interact = child.GetElementsContainsPoint(mousePos);
                }
            }

            foreach (var ce in interact)
                if (!interactContainerElementsBuffer.Contains(ce))
                    ce.Events.MouseOver(ce);
            foreach (var ce in interactContainerElementsBuffer)
                if (!interact.Contains(ce))
                    ce.Events.MouseOut(ce);
            interactContainerElementsBuffer = interact;

            interact.ForEach(x => x.Events.MouseHover(x));

            if (interact.Count > 0)
                Main.LocalPlayer.mouseInterface = true;

            if (mouseLeftDown != Main.mouseLeft)
            {
                if (Main.mouseLeft)
                {
                    interact.ForEach(x => x.Events.LeftDown(x));
                    needCallMouseLeftUpElements.AddRange(interact);
                }
                else
                {
                    if (mouseLeftCooldown.IsCoolDown())
                    {
                        interact.ForEach(x => x.Events.LeftClick(x));
                        mouseLeftCooldown.ResetCoolDown();
                    }
                    else
                    {
                        interact.ForEach(x => x.Events.LeftDoubleClick(x));
                        mouseLeftCooldown.CoolDown();
                    }
                    needCallMouseLeftUpElements.ForEach(x => x.Events.LeftUp(x));
                    needCallMouseLeftUpElements.Clear();
                }

                mouseLeftDown = Main.mouseLeft;
            }

            if (mouseRightDown != Main.mouseRight)
            {
                if (Main.mouseRight)
                {
                    interact.ForEach(x => x.Events.RightDown(x));
                    needCallMouseRightUpElements.AddRange(interact);
                }
                else
                {
                    if (mouseRightCooldown.IsCoolDown())
                    {
                        interact.ForEach(x => x.Events.RightClick(x));
                        mouseRightCooldown.ResetCoolDown();
                    }
                    else
                    {
                        interact.ForEach(x => x.Events.RightDoubleClick(x));
                        mouseRightCooldown.CoolDown();
                    }
                    needCallMouseRightUpElements.ForEach(x => x.Events.RightUp(x));
                    needCallMouseRightUpElements.Clear();
                }
                mouseRightDown = Main.mouseRight;
            }
            mouseLeftCooldown.Update();
            mouseRightCooldown.Update();
        }

        /// <summary>
        /// 绘制
        /// </summary>
        /// <param name="sb">画笔</param>
        public void Draw(SpriteBatch sb)
        {
            if (CallOrderCache.Count == 0 || ElementsCache.Count == 0)
                return;
            UIContainerElement child;
            for (int i = CallOrderCache.Count - 1; i >= 0; i--)
            {
                child = ElementsCache[CallOrderCache[i]];
                if (child != null && child.IsVisible) child.Draw(sb);
            }
            sb.End();
            sb.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.UIScaleMatrix);
        }

        /// <summary>
        /// 添加子元素
        /// </summary>
        /// <param name="element">需要添加的子元素</param>
        /// <returns>成功时返回true，否则返回false</returns>
        public bool Register(UIContainerElement element)
        {
            return Register(element.Name, element);
        }

        /// <summary>
        /// 添加子元素
        /// </summary>
        /// <param name="name">需要添加的子元素的Name</param>
        /// <param name="element">需要添加的子元素</param>
        /// <returns>成功时返回true，否则返回false</returns>
        public bool Register(string name, UIContainerElement element)
        {
            if (element == null || Elements.ContainsKey(name) || CallOrder.Contains(name))
                return false;

            Elements.Add(name, element);
            CallOrder.Add(element.Name);
            element.OnInitialization();
            element.Calculation();

            LoggerItem.WriteLine($"[Loita:UI System]UI页 {name} 注册完毕");
            return true;
        }

        /// <summary>
        /// 移除子元素
        /// </summary>
        /// <param name="name">需要移除的子元素的Key</param>
        /// <returns>成功时返回true，否则返回false</returns>
        public bool Remove(string name)
        {
            if (CallOrder.Count == 0 || Elements.Count == 0 || !(Elements.ContainsKey(name) || CallOrder.Contains(name)))
                return false;
            Elements.Remove(name);
            CallOrder.Remove(name);
            LoggerItem.WriteLine($"[Loita:UI System]UI页 {name} 被移除");
            return true;
        }

        /// <summary>
        /// 将所有容器相对坐标计算为具体坐标
        /// </summary>
        public void Calculation()
        {
            foreach (var child in ElementsCache.Values)
                child?.Calculation();
        }

        /// <summary>
        /// 将容器置顶
        /// </summary>
        /// <param name="name">需要置顶的容器Name</param>
        /// <returns>成功返回true，否则返回false</returns>
        public bool SetContainerTop(string name)
        {
            if (CallOrder.Count == 0 || Elements.Count == 0 || !(Elements.ContainsKey(name) || CallOrder.Contains(name)))
                return false;
            if (CallOrder[0] == name)
                return true;
            CallOrder.Remove(name);
            CallOrder.Insert(0, name);
            LoggerItem.WriteLine($"[Loita:UI System]UI页 {name} 被置顶");
            return true;
        }

        /// <summary>
        /// 交换两个容器的顺序
        /// </summary>
        /// <param name="name1">容器1的Name</param>
        /// <param name="name2">容器2的Name</param>
        /// <returns>是否交换成功。成功则返回true，否则返回false</returns>
        public bool ExchangeContainer(string name1, string name2)
        {
            if (CallOrder.Count == 0 || Elements.Count == 0 || !(Elements.ContainsKey(name1) || CallOrder.Contains(name1)) ||
                !(Elements.ContainsKey(name2) || CallOrder.Contains(name2)))
                return false;
            int index1 = CallOrder.FindIndex(x => x == name1);
            int index2 = CallOrder.FindIndex(x => x == name2);
            CallOrder.Remove(name1);
            CallOrder.Remove(name2);
            CallOrder.Insert(index1, name2);
            CallOrder.Insert(index2, name1);
            LoggerItem.WriteLine($"[Loita:UI System]UI页 {name1} 与 {name2} 交换UI层");
            return true;
        }

        /// <summary>
        /// 寻找开启的顶部容器索引
        /// </summary>
        /// <returns>开启的顶部容器索引</returns>
        public int FindTopContainer()
        {
            return CallOrder.FindIndex(x => Elements[x].IsVisible);
        }

        /// <summary>
        /// 关闭所有容器
        /// </summary>
        public void CloseAllContainers()
        {
            foreach (var c in CallOrderCache)
            {
                ElementsCache[c].Info.IsVisible = false;
            }
            LoggerItem.WriteLine($"[Loita:UI System]关闭所有UI页");
        }
    }
}