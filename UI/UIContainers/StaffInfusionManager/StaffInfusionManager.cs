using Loita.Components;
using Loita.Components.LoitaComponents;
using Loita.Components.LoitaComponents.Prefixes;
using Loita.Components.LoitaComponents.Spells;
using Loita.Components.LoitaComponents.Triggers;
using Loita.Globals;
using Loita.QuickAssetReference;
using Loita.UI.UIContainers.StaffInfusionManager.UIElements;
using Loita.UI.UIElements;

using Microsoft.Xna.Framework;

using System;

using Terraria;

namespace Loita.UI.UIContainers.StaffInfusionManager
{
    internal class StaffInfusionManager : UIContainerElement
    {
        public static StaffInfusionManager Instance =>
            (StaffInfusionManager)Loita.UISystem.Elements["Loita.UI.UIContainers.StaffInfusionManager.StaffInfusionManager"];

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

            UIText uIText = new UIText("Test Component", Loita.DefaultFontSystem.GetFont(Loita.DEFAULT_FONT_SIZE - 10f), Color.White);
            componentInfoContainer.AddElement(uIText);

            UIText tip = new UIText("Tips:\nThis is a Test Component",
                Loita.DefaultFontSystem.GetFont(Loita.DEFAULT_FONT_SIZE - 20f), Color.White);
            tip.Info.Top.SetValue(uIText.Info.Height);
            componentInfoContainer.AddElement(tip);

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
            if (Main.playerInventory && !IsVisible)
            {
                var player = Main.LocalPlayer;
                var projectile = Projectile.NewProjectileDirect(player.GetSource_FromThis(),
                    player.Center, player.velocity, 10, 10, 10f, player.whoAmI);
                IEntity entity = projectile.GetGlobalProjectile<GProjectile>();
                entity.AddComponent<CInfusionSlot>(entity, 20);
                var cis = entity.GetComponent<CInfusionSlot>();
                int index = 0;
                cis.ChangeComponent(index++, new CDoubleSpell(entity));
                cis.ChangeComponent(index++, new CLightPrefix(entity));
                cis.ChangeComponent(index++, new CDoubleSpell(entity));
                cis.ChangeComponent(index++, new CTestSpell(entity));
                cis.ChangeComponent(index++, new CTestSpell(entity));
                cis.ChangeComponent(index++, new CTestSpell(entity));
                index = 0;
                cis.InitActivableSpace(ref index);
                var si = SpellInfo.FromPlayer(player);
                si.Velocity = new Vector2(0f, -10f);
                cis.Apply(si);

                Show(entity);
            }
            if (!Main.playerInventory && IsVisible)
                Close();
        }

        public override void Show(params object[] args)
        {
            base.Show(args);
            componentsContainer.ClearAllElements();
            infusionSlotContainer.ClearAllElements();
            _infusionSlot = null;
            if (args.Length > 0 && args[0] is IEntity entity && entity.HasComponent<CInfusionSlot>())
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
                        };
                        componentsContainer.AddElement(choice);
                        i++;
                    }
                    if (comp is CInfusionSlot infusionSlot)
                    {
                        _infusionSlot = infusionSlot;
                    }
                }
                if (_infusionSlot != null)
                {
                    i = 0;
                    foreach (var comp in _infusionSlot.ActivableSpace)
                    {
                        UIChoices choice = new UIChoices();
                        choice.Info.Width.SetValue(46f, 0f);
                        choice.Info.Height.SetValue(46f, 0f);
                        choice.Info.Left.SetValue(46f * i, 0f);
                        //choice.NoSelectedPrimaryColor = Color.Black;
                        choice.Events.OnLeftClick += element =>
                        {
                            Choices = (UIChoices)element;
                        };
                        infusionSlotContainer.AddElement(choice);

                        UIImage image = new UIImage(comp == null ? ModAssets_Texture2D.Images.CBlockImmediateAsset.Value :
                            comp.Texture, Color.White);
                        image.Info.Width.SetValue(0f, 1f);
                        image.Info.Height.SetValue(0f, 1f);
                        choice.Register(image);

                        UIImage take = new UIImage(comp == null ? ModAssets_Texture2D.Images.TakeInImmediateAsset.Value :
                            ModAssets_Texture2D.Images.TakeOutImmediateAsset.Value, Color.White);
                        take.Info.Width.SetValue(0f, 1f);
                        take.Info.Height.SetValue(0f, 1f);
                        choice.Register(take);

                        choice.Events.OnMouseHover += element =>
                        {
                            if (comp == null || comp.ActivableSpace == null)
                                return;
                            foreach (var comp2 in comp.ActivableSpace)
                            {
                                var c = (UIChoices)infusionSlotContainer.Elements[comp2.Index];
                                //if (!c.IsSelected)
                                c.PanelColor = Color.Yellow * choice.Progression.Value;
                            }
                        };
                        choice.Events.OnUpdate += (element, gt) =>
                        {
                            choice.BorderColor = choice.PanelColor;
                            if (choice.IsSelected)
                            {
                                take.ChangeColor(Color.White * choice.Progression.Value);
                                image.ChangeColor(Color.White * Math.Max(0.6f, (1f - choice.Progression.Value)));
                                if (comp == null || comp.ActivableSpace == null)
                                    return;
                                foreach (var comp2 in comp.ActivableSpace)
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
            Choices = defaultChoices;
        }
    }
}