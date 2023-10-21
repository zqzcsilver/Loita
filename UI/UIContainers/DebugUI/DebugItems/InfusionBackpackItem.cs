using Loita.Components.LoitaComponents;
using Loita.Components.LoitaComponents.Prefixes;
using Loita.Components.LoitaComponents.Spells;
using Loita.ModPlayers;
using Loita.UI.UIElements;
using Loita.Utils;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System;
using System.Collections.Generic;

using Terraria;

using static Loita.UI.UIElements.BaseElement;

namespace Loita.UI.UIContainers.DebugUI.DebugItems
{
    internal class InfusionBackpackItem : DebugItemBase
    {
        private class InfusionItem : UIPanel
        {
            private LoitaComponent _component;

            public Color LightColor = new Color(134, 161, 166);
            public Color PrimaryColor;
            public SleekProgression Progression = new SleekProgression();

            public InfusionItem(LoitaComponent component)
            {
                _component = component;
                Type type = _component.GetType();

                ShowBorder.LeftBorder = ShowBorder.RightBorder = ShowBorder.TopBorder = ShowBorder.BottomBorder = false;
                Info.Width.SetValue(0f, 0.5f);
                Info.Height.SetValue(46f, 0f);
                Info.IsSensitive = true;
                Events.OnMouseOver += element =>
                {
                    Progression.WaitToValue = 1f;
                };
                Events.OnMouseOut += element =>
                {
                    Progression.WaitToValue = 0f;
                };

                UIImage icon = new UIImage(_component.Texture, Color.White);
                icon.Style = UIImage.CalculationStyle.LockedAspectRatioMainHeight;
                icon.Info.Height.SetValue(0f, 1f);
                icon.Events.OnMouseHover += e =>
                {
                    TipsElement.DrawTips = _component;
                };
                Register(icon);

                BaseElement element = new BaseElement();
                element.Info.Width.SetValue(-46f, 1f);
                element.Info.Height.SetValue(0f, 1f);
                element.Info.Left.SetValue(46f, 0f);
                Register(element);

                var font = Loita.DefaultFontSystem.GetFont(34f);
                UIText add = new UIText(">", font);
                add.Info.Top.SetValue(PositionStyle.Half - add.Info.Height / 2f);
                add.Info.Left.SetValue(new PositionStyle(0f, 0.8f) - add.Info.Width / 2f);
                int addTimeMax = 30;
                int addTime = 0;
                int addCount = 0;
                bool addMouseDown = false;
                add.Events.OnLeftDown += element =>
                {
                    addMouseDown = true;
                };
                add.Events.OnUpdate += (e, gt) =>
                {
                    if (!addMouseDown)
                        return;

                    if (addTime > 0)
                        addTime--;
                    else
                    {
                        MPlayer.Instance.GainComponents(type);
                        addCount++;
                        addTime = addTimeMax;
                        if (addCount > 2)
                        {
                            addCount = 0;
                            addTimeMax = Math.Max(6, addTimeMax / 2);
                        }
                    }
                };
                add.Events.OnLeftUp += element =>
                {
                    addMouseDown = false;
                    addTimeMax = 60;
                    addTime = 0;
                    addCount = 0;
                };
                element.Register(add);

                UIText decrease = new UIText("<", font);
                decrease.Info.Top.SetValue(PositionStyle.Half - decrease.Info.Height / 2f);
                decrease.Info.Left.SetValue(new PositionStyle(0f, 0.2f) - decrease.Info.Width / 2f);
                int decreaseTimeMax = 30;
                int decreaseTime = 0;
                int decreaseCount = 0;
                bool decreaseMouseDown = false;
                decrease.Events.OnLeftDown += element =>
                {
                    decreaseMouseDown = true;
                };
                decrease.Events.OnUpdate += (e, gt) =>
                {
                    if (!decreaseMouseDown)
                        return;

                    if (decreaseTime > 0)
                        decreaseTime--;
                    else
                    {
                        MPlayer.Instance.RemoveComponents(type);
                        decreaseCount++;
                        decreaseTime = decreaseTimeMax;
                        if (decreaseCount > 2)
                        {
                            decreaseCount = 0;
                            decreaseTimeMax = Math.Max(6, decreaseTimeMax / 2);
                        }
                    }
                };
                decrease.Events.OnLeftUp += element =>
                {
                    decreaseMouseDown = false;
                    decreaseTimeMax = 60;
                    decreaseTime = 0;
                    decreaseCount = 0;
                };
                element.Register(decrease);

                UIText count = new UIText("0", font);
                count.Info.Top.SetValue(PositionStyle.Half - decrease.Info.Height / 2f);
                count.Info.Left.SetValue(PositionStyle.Half - decrease.Info.Width / 2f);
                count.Events.OnUpdate += (e, gt) =>
                {
                    var half = new PositionStyle(0f, 0.4f);
                    count.Text = MPlayer.Instance.ComponentCount(type).ToString();
                    count.Info.Top.SetValue(half - count.Info.Height / 2f);
                    count.Info.Left.SetValue(PositionStyle.Half - count.Info.Width / 2f);
                    count.Calculation();

                    add.Info.Top.SetValue(half - add.Info.Height / 2f);
                    add.Info.Left.SetValue(new PositionStyle(0f, 0.8f) - add.Info.Width / 2f);
                    add.Calculation();

                    decrease.Info.Top.SetValue(half - decrease.Info.Height / 2f);
                    decrease.Info.Left.SetValue(new PositionStyle(0f, 0.2f) - decrease.Info.Width / 2f);
                    decrease.Calculation();
                };
                element.Register(count);
            }

            public override void Update(GameTime gt)
            {
                base.Update(gt);
                Progression.Update(gt);
                PanelColor = Color.Lerp(PrimaryColor, LightColor, Progression.Value);
            }
        }

        private class TipsElement : BaseElement
        {
            public static LoitaComponent DrawTips = null;
            private UIPanel panel = new UIPanel();

            protected override void DrawChildren(SpriteBatch sb)
            {
                base.DrawChildren(sb);
                if (DrawTips != null)
                {
                    var mousePos = Main.MouseScreen + new Vector2(20f);
                    panel.Draw(sb);
                    DrawTips.DrawTips(sb, mousePos,
                        new Vector2(Math.Min(450f, Main.screenWidth - mousePos.X)), out Vector2 size);
                    mousePos.Y += 6f;
                    panel.Info.TotalHitBox = new Rectangle((int)mousePos.X, (int)mousePos.Y, (int)size.X, (int)size.Y);
                    DrawTips = null;
                }
            }
        }

        public override string Name => "Infusion Backpack";
        private BaseElement _page;

        public override BaseElement GetPage()
        {
            if (_page != null)
                return _page;

            _page = new TipsElement();
            _page.Info.Width.SetValue(0f, 1f);
            _page.Info.Height.SetValue(0f, 1f);

            UIVerticalScrollbar verticalScrollbar = new UIVerticalScrollbar();
            verticalScrollbar.Info.Height.SetValue(4f, 1f);
            verticalScrollbar.Info.Left.SetValue(-18f, 1f);
            verticalScrollbar.Info.Top.SetValue(-2f, 0f);
            _page.Register(verticalScrollbar);

            UIContainerPanel infusions = new UIContainerPanel();
            verticalScrollbar.BindElement = infusions;
            infusions.SetVerticalScrollbar(verticalScrollbar);
            infusions.Info.Width.SetValue(PositionStyle.Full - verticalScrollbar.Info.Width);
            _page.Register(infusions);

            List<BaseElement> elements = new List<BaseElement>();
            var components = InstanceManager<LoitaComponent>.GetInstances();
            PositionStyle top = PositionStyle.Empty;

            UIText prefix = new UIText("Prefixes", Loita.DefaultFontSystem.GetFont(50f));
            prefix.Info.Top.SetValue(top);
            elements.Add(prefix);
            top += prefix.Info.Height;

            var prefixes = components.FindAll(c => c is CPrefix);
            for (int i = 0; i < prefixes.Count; i++)
            {
                InfusionItem infusionItem = new InfusionItem(prefixes[i]);
                infusionItem.Info.Top.SetValue(top);
                infusionItem.Info.Left.SetValue(0f, i % 2 == 0 ? 0f : 0.5f);
                elements.Add(infusionItem);
                if ((i + 1) % 2 == 0)
                {
                    top += infusionItem.Info.Height;
                }
            }
            if (prefixes.Count % 2 != 0)
                top.Pixel += 46f;

            UIText trigger = new UIText("Triggers", Loita.DefaultFontSystem.GetFont(50f));
            trigger.Info.Top.SetValue(top);
            elements.Add(trigger);
            top += trigger.Info.Height;

            var triggers = components.FindAll(c => c is CTrigger);
            for (int i = 0; i < triggers.Count; i++)
            {
                InfusionItem infusionItem = new InfusionItem(triggers[i]);
                infusionItem.Info.Top.SetValue(top);
                infusionItem.Info.Left.SetValue(0f, i % 2 == 0 ? 0f : 0.5f);
                elements.Add(infusionItem);
                if ((i + 1) % 2 == 0)
                {
                    top += infusionItem.Info.Height;
                }
            }
            if (triggers.Count % 2 != 0)
                top.Pixel += 46f;

            UIText spell = new UIText("Spells", Loita.DefaultFontSystem.GetFont(50f));
            spell.Info.Top.SetValue(top);
            elements.Add(spell);
            top += spell.Info.Height;

            var spells = components.FindAll(c => c is CSpell);
            for (int i = 0; i < spells.Count; i++)
            {
                InfusionItem infusionItem = new InfusionItem(spells[i]);
                infusionItem.Info.Top.SetValue(top);
                infusionItem.Info.Left.SetValue(0f, i % 2 == 0 ? 0f : 0.5f);
                elements.Add(infusionItem);
                if ((i + 1) % 2 == 0)
                {
                    top += infusionItem.Info.Height;
                }
            }

            infusions.AddElements(elements);
            return _page;
        }
    }
}