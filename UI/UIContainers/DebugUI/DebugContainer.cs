using Loita.KeyBindSystem;
using Loita.UI.UIContainers.DebugUI.DebugItems;
using Loita.UI.UIElements;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using System;
using System.Collections.Generic;
using System.Linq;

using Terraria;

namespace Loita.UI.UIContainers.DebugUI
{
    internal class DebugContainer : UIContainerElement
    {
        public const string OPEN_HOT_KEY = "Open Debug Panel";

        public override void Load()
        {
            Loita.KeyGroupManager.RegisterKeyGroup(new KeyGroup(OPEN_HOT_KEY, new List<Keys>() { Keys.U }));
            base.Load();
        }

        public override void OnInitialization()
        {
            base.OnInitialization();

            BaseElement element = new BaseElement();
            element.Info.Width.SetValue(652f, 0f);
            element.Info.Height.SetValue(300f, 0f);
            element.Info.Left.SetValue(PositionStyle.Half - element.Info.Width / 2f);
            element.Info.Top.SetValue(PositionStyle.Half - element.Info.Height / 2f);
            Register(element);

            UIPanel panel = new UIPanel();
            panel.Info.Width.SetValue(450f, 0f);
            panel.Info.Height.SetValue(0f, 1f);
            panel.Info.Left.SetValue(PositionStyle.Full - panel.Info.Width);
            element.Register(panel);

            UIPanel tabsPanel = new UIPanel();
            tabsPanel.Info.Width.SetValue(panel.Info.Left);
            tabsPanel.Info.Height.SetValue(PositionStyle.Full);
            element.Register(tabsPanel);

            UIVerticalScrollbar scrollbar = new UIVerticalScrollbar();
            UIContainerPanel tabs = new UIContainerPanel();
            tabs.Info.Width.SetValue(PositionStyle.Full);
            tabs.Info.Height.SetValue(PositionStyle.Full);
            scrollbar.BindElement = tabs;
            tabs.SetVerticalScrollbar(scrollbar);
            tabs.Events.OnUpdate += (e, gt) =>
            {
                scrollbar.Update(gt);
            };
            tabsPanel.Register(tabs);

            var itemTypes = from c in GetType().Assembly.GetTypes()
                            where !c.IsAbstract && c.IsSubclassOf(typeof(DebugItemBase))
                            select c;
            List<BaseElement> debugItemTabs = new List<BaseElement>();
            List<DebugItemBase> debugItems = new List<DebugItemBase>();
            DebugItemBase selectedItem = null;
            PositionStyle top = PositionStyle.Empty;
            foreach (var type in itemTypes)
            {
                var item = (DebugItemBase)Activator.CreateInstance(type);
                debugItems.Add(item);
            }
            debugItems.Sort((i1, i2) => i1.Level.CompareTo(i2.Level));
            bool initDefaultPage = false;
            foreach (var i in debugItems)
            {
                var item = i;
                var tab = item.GetTab();
                tab.Info.Top.SetValue(top);
                tab.Events.OnLeftClick += e =>
                {
                    if (item.BeSelected)
                        return;
                    if (selectedItem != null)
                    {
                        selectedItem.BeSelected = false;
                        panel.RemoveAll();
                    }
                    selectedItem = item;
                    selectedItem.BeSelected = true;
                    panel.Register(selectedItem.GetPage());
                };
                debugItemTabs.Add(tab);
                top += tab.Info.Height;
                if (!initDefaultPage)
                {
                    initDefaultPage = true;
                    selectedItem = item;
                    selectedItem.BeSelected = true;
                    panel.Register(selectedItem.GetPage());
                }
            }

            tabs.AddElements(debugItemTabs);
        }

        public override void PreUpdate(GameTime gt)
        {
            base.PreUpdate(gt);
            if (KeyGroupManager.Instance.GetKeyGroup(OPEN_HOT_KEY).IsClick)
            {
                if (IsVisible)
                    Close();
                else
                    Show();
            }
        }
    }
}