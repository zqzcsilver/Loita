using Loita.Components;
using Loita.Components.LoitaComponents;
using Loita.Globals;
using Loita.KeyBindSystem;
using Loita.QuickAssetReference;
using Loita.UI.UIContainers.UIElements;
using Loita.UI.UIContainers.WandInfusionManager.UIElements;
using Loita.UI.UIElements;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using System;
using System.Collections.Generic;

using Terraria;
using Terraria.GameContent;
using Terraria.ID;

namespace Loita.UI.UIContainers.WandInfusionManager
{
    internal class WandInfusionManager : UIContainerElement
    {
        public const string OPEN_HOT_KEY = "Open Wand Infusion Manager";

        public static WandInfusionManager Instance =>
            (WandInfusionManager)Loita.UISystem.Elements["Loita.UI.UIContainers.WandInfusionManager.WandInfusionManager"];

        private UIContainerPanel componentsContainer;
        private UIContainerPanel componentInfoContainer;
        private UIContainerPanel infusionSlotContainer;
        private UIChoices choices;
        private CInfusionSlot _infusionSlot;
        private int _infusionSlotIndex = -1;

        public UIChoices Choices
        {
            get => choices;
            set
            {
                if (value == null || choices == value)
                    return;
                if (choices != null)
                {
                    choices.Progression.Value = 1f;
                    choices.IsSelected = false;
                }
                choices = value;
                choices.IsSelected = true;
                choices.Progression.Value = 0f;
            }
        }

        private UIChoices defaultChoices;
        private UITips _tips;

        public override void Load()
        {
            base.Load();
            Loita.KeyGroupManager.RegisterKeyGroup(new KeyGroup(OPEN_HOT_KEY, new List<Keys>() { Keys.I }));
        }

        public override void OnInitialization()
        {
            base.OnInitialization();
            UIPanel panel = new UIPanel();
            panel.Info.Width.SetValue(716f, 0f);
            panel.Info.Height.SetValue(324f, 0f);
            panel.Info.Left.SetValue(PositionStyle.Half - panel.Info.Width / 2f);
            panel.Info.Top.SetValue(PositionStyle.Half - panel.Info.Height / 2f);
            panel.CanDrag = true;
            Register(panel);

            UIPanel infusionSlots = new UIPanel();
            infusionSlots.Info.Width.SetValue(-18f * 2f, 1f);
            infusionSlots.Info.Height.SetValue(50f, 0f);
            infusionSlots.Info.Left.SetValue(18f, 0f);
            infusionSlots.Info.Top.SetValue(18f, 0f);
            panel.Register(infusionSlots);

            infusionSlotContainer = new UIContainerPanel();
            infusionSlotContainer.Info.SetMargin(0f);
            infusionSlots.Register(infusionSlotContainer);

            UIHorizontalScrollbar horizontalScrollbar = new UIHorizontalScrollbar();
            horizontalScrollbar.Info.Width.SetValue(infusionSlots.Info.Width);
            horizontalScrollbar.Info.Left.SetValue(infusionSlots.Info.Left);
            horizontalScrollbar.Info.Top.SetValue(infusionSlots.Info.Top + infusionSlots.Info.Height);
            horizontalScrollbar.Info.Top.Pixel += 4f;
            horizontalScrollbar.UseScrollWheel = true;
            horizontalScrollbar.BindElement = infusionSlots;
            panel.Register(horizontalScrollbar);
            infusionSlotContainer.SetHorizontalScrollbar(horizontalScrollbar);

            UIPanel components = new UIPanel();
            components.Info.Width.SetValue(268f, 0f);
            components.Info.Height.SetValue(PositionStyle.Full - horizontalScrollbar.Info.Top -
                horizontalScrollbar.Info.Height - infusionSlots.Info.Left - infusionSlots.Info.Left);
            components.Info.Left.SetValue(infusionSlots.Info.Left);
            components.Info.Top.SetValue(horizontalScrollbar.Info.Top + horizontalScrollbar.Info.Height + horizontalScrollbar.Info.Left);
            panel.Register(components);

            componentsContainer = new UIContainerPanel();
            components.Register(componentsContainer);

            UIVerticalScrollbar verticalScrollbar = new UIVerticalScrollbar();
            verticalScrollbar.Info.Height.SetValue(components.Info.Height);
            verticalScrollbar.Info.Left.SetValue(components.Info.Width + components.Info.Left);
            verticalScrollbar.Info.Left.Pixel += 4f;
            verticalScrollbar.Info.Top.SetValue(components.Info.Top);
            verticalScrollbar.BindElement = components;
            panel.Register(verticalScrollbar);
            componentsContainer.SetVerticalScrollbar(verticalScrollbar);

            UIPanel componentInfo = new UIPanel();
            componentInfo.Info.Width.SetValue(244f, 0f);
            componentInfo.Info.Height.SetValue(verticalScrollbar.Info.Height);
            componentInfo.Info.Left.SetValue(verticalScrollbar.Info.Left + verticalScrollbar.Info.Width + infusionSlots.Info.Left);
            componentInfo.Info.Top.SetValue(verticalScrollbar.Info.Top);
            panel.Register(componentInfo);

            componentInfoContainer = new UIContainerPanel();
            componentInfo.Register(componentInfoContainer);

            _tips = new UITips((LoitaComponent)null);
            _tips.Info.SamplerState = SamplerState.PointClamp;
            componentInfoContainer.AddElement(_tips);

            verticalScrollbar = new UIVerticalScrollbar();
            verticalScrollbar.Info.Height.SetValue(componentInfo.Info.Height);
            verticalScrollbar.Info.Left.SetValue(componentInfo.Info.Width + componentInfo.Info.Left);
            verticalScrollbar.Info.Left.Pixel += 4f;
            verticalScrollbar.Info.Top.SetValue(componentInfo.Info.Top);
            verticalScrollbar.BindElement = componentInfo;
            panel.Register(verticalScrollbar);
            componentInfoContainer.SetVerticalScrollbar(verticalScrollbar);

            UIChoices staffImage = new UIChoices();
            staffImage.Info.Width.SetValue(PositionStyle.Full - verticalScrollbar.Info.Left -
                verticalScrollbar.Info.Width - infusionSlots.Info.Left - infusionSlots.Info.Left);
            staffImage.Info.Height.SetValue(componentInfo.Info.Height);
            staffImage.Info.Left.SetValue(verticalScrollbar.Info.Left + verticalScrollbar.Info.Width + infusionSlots.Info.Left);
            staffImage.Info.Top.SetValue(verticalScrollbar.Info.Top);
            staffImage.Events.OnLeftClick += element =>
            {
                Choices = (UIChoices)element;
            };
            defaultChoices = staffImage;
            panel.Register(staffImage);
        }

        public override void PreUpdate(GameTime gt)
        {
            base.PreUpdate(gt);
            bool keyPressed = KeyGroupManager.Instance.GetKeyGroup("Open Wand Infusion Manager").IsClick;
            if (keyPressed && IsVisible)
            {
                Close();
                return;
            }
            if (keyPressed && Main.HoverItem != null && Main.HoverItem.type != ItemID.None &&
                Main.HoverItem.TryGetGlobalItem(out GItem gitem) && gitem.Entity.HasComponent<CInfusionSlot>() && !IsVisible)
            {
                if (gitem.OriginalItem == null)
                {
                    Show(gitem, TextureAssets.Item[Main.HoverItem.type].Value);
                }
                else if (gitem.OriginalItem.TryGetGlobalItem(out GItem ngitem))
                {
                    if (ngitem.Entity.HasComponent<CInfusionSlot>())
                        Show(ngitem, TextureAssets.Item[gitem.OriginalItem.type].Value);
                }
            }
        }

        public void ResetInfusions(int index = -1)
        {
            infusionSlotContainer.ClearAllElements();
            if (_infusionSlot != null)
            {
                int i = 0;
                foreach (var comp in _infusionSlot.ActivableSpace)
                {
                    int isr = i;
                    UIChoices choice = new UIChoices();
                    choice.Info.Width.SetValue(46f, 0f);
                    choice.Info.Height.SetValue(46f, 0f);
                    choice.Info.Left.SetValue(46f * i, 0f);
                    choice.Events.OnLeftClick += element =>
                    {
                        var ibp = InfusionBackpack.InfusionBackpack.Instance;
                        if (choice.IsSelected && (!ibp.IsVisible || !ibp.HasInfusionSlot))
                        {
                            ibp.Close();
                            ibp.Show(_infusionSlot, isr);
                        }
                        Choices = choice;
                        if (ibp.HasInfusionSlot)
                            ibp.ChangeInfusionSlot(_infusionSlot, isr);
                    };
                    infusionSlotContainer.AddElement(choice);

                    if (i == index)
                        Choices = choice;

                    UIImage image = new UIImage(comp == null ? ModAssets_Texture2D.Images.CBlockImmediateAsset.Value :
                        comp.Texture, Color.White);
                    image.Info.Width.SetValue(0f, 1f);
                    image.Info.Height.SetValue(0f, 1f);
                    image.Events.OnUpdate += (e, gt) =>
                    {
                        var cp = _infusionSlot.ActivableSpace[isr];
                        image.ChangeImage(cp == null ? ModAssets_Texture2D.Images.CBlockImmediateAsset.Value : cp.Texture);
                    };
                    choice.Register(image);

                    UIImage take = new UIImage(comp == null ? ModAssets_Texture2D.Images.TakeInImmediateAsset.Value :
                        ModAssets_Texture2D.Images.TakeOutImmediateAsset.Value, Color.White);
                    take.Info.Width.SetValue(0f, 1f);
                    take.Info.Height.SetValue(0f, 1f);
                    choice.Register(take);

                    choice.Events.OnMouseHover += element =>
                    {
                        var cp = _infusionSlot.ActivableSpace[isr];
                        if (cp == null || cp.ActivableSpace == null)
                            return;
                        foreach (var comp2 in cp.ActivableSpace)
                        {
                            var c = (UIChoices)infusionSlotContainer.Elements[comp2.Index];
                            //if (!c.IsSelected)
                            c.PanelColor = Color.Yellow * choice.Progression.Value;
                        }
                    };
                    choice.Events.OnUpdate += (element, gt) =>
                    {
                        if (choice.IsSelected && _tips.LoitaComponent != _infusionSlot.ActivableSpace[isr])
                        {
                            _tips.Reset();
                            _tips.LoitaComponent = _infusionSlot.ActivableSpace[isr];
                        }
                        choice.BorderColor = choice.PanelColor;
                        if (choice.IsSelected)
                        {
                            take.ChangeColor(Color.White * choice.Progression.Value);
                            image.ChangeColor(Color.White * Math.Max(0.6f, 1f - choice.Progression.Value));
                            var cp = _infusionSlot.ActivableSpace[isr];
                            if (cp == null || cp.ActivableSpace == null)
                                return;
                            foreach (var comp2 in cp.ActivableSpace)
                            {
                                var c = (UIChoices)infusionSlotContainer.Elements[comp2.Index];
                                //if (!c.IsSelected)
                                c.PanelColor = Color.Yellow;
                            }
                        }
                        else
                        {
                            take.ChangeColor(Color.Transparent);
                            image.ChangeColor(Color.White);
                        }
                    };
                    i++;
                }
            }
        }

        public override void Show(params object[] args)
        {
            base.Show(args);
            componentsContainer.ClearAllElements();
            _infusionSlot = null;
            _tips.Reset();
            if (args.Length > 0 && args[0] is IEntity entity)
            {
                var comps = entity.GetComponents();
                int i = 0;
                foreach (var comp in comps)
                {
                    if (comp is IInfusion infusion)
                    {
                        UIChoices choice = new UIChoices();
                        choice.Info.Width.SetValue(0f, 1f);
                        choice.Info.Height.SetValue(28f, 0f);
                        choice.Info.Top.SetValue(32f * i, 0f);
                        choice.Events.OnLeftClick += element =>
                        {
                            Choices = (UIChoices)element;
                            _infusionSlotIndex = i;

                            _tips.Reset();
                            _tips.Infusion = infusion;
                        };
                        componentsContainer.AddElement(choice);

                        UIText text = new UIText(infusion.Name, Loita.DefaultFontSystem.GetFont(22f));
                        text.Info.Left.SetValue(2f, 0f);
                        choice.Register(text);
                        i++;
                    }
                    if (comp is CInfusionSlot infusionSlot)
                    {
                        _infusionSlot = infusionSlot;
                    }
                }

                if (args[1] is Texture2D texture)
                {
                    defaultChoices.RemoveAll();

                    UIImage image = new UIImage(texture, Color.White);
                    var scale = Math.Max(texture.Width / defaultChoices.Info.Size.X, texture.Height / defaultChoices.Info.Size.Y) / 0.7f;
                    image.Info.Width.Pixel /= scale;
                    image.Info.Height.Pixel /= scale;
                    image.Info.Left.SetValue(PositionStyle.Half - image.Info.Width / 2f);
                    image.Info.Top.SetValue(PositionStyle.Half - image.Info.Height / 2f);
                    image.Info.SamplerState = SamplerState.PointClamp;
                    defaultChoices.Register(image);
                }
            }
            Choices = defaultChoices;
            ResetInfusions();
        }

        public override void Close(params object[] args)
        {
            base.Close(args);
            var ibp = InfusionBackpack.InfusionBackpack.Instance;
            if (ibp.CInfusionSlot == _infusionSlot)
            {
                ibp.Close();
            }
        }
    }
}