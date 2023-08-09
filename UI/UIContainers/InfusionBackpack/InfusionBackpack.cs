using Loita.Components.LoitaComponents;
using Loita.Components.LoitaComponents.Prefixes;
using Loita.Components.LoitaComponents.Spells;
using Loita.QuickAssetReference;
using Loita.UI.UIContainers.StaffInfusionManager.UIElements;
using Loita.UI.UIElements;

using Microsoft.Xna.Framework;

using System;
using System.Collections.Generic;

using Terraria;

namespace Loita.UI.UIContainers.InfusionBackpack
{
    internal class InfusionBackpack : UIContainerElement
    {
        public static InfusionBackpack Instance =>
            (InfusionBackpack)Loita.UISystem.Elements["Loita.UI.UIContainers.StaffInfusionManager.InfusionBackpack"];

        private UIContainerPanel infusionSlotContainer;
        private CInfusionSlot _infusionSlot;
        private int _infusionSlotIndex = -1;
        public List<LoitaComponent> Infusions => Main.LocalPlayer.GetModPlayer<MPlayer>().ComponentBackpack;

        public UIChoices Choices
        {
            get => choices;
            set
            {
                if (choices == value)
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

        private UIChoices choices;
        private int lineInfusionCount = 0;
        public FilterCondition FilterCondition = new FilterCondition();

        public override void OnInitialization()
        {
            base.OnInitialization();
            UIPanel panel = new UIPanel();
            panel.Info.Width.SetValue(490f, 0f);
            panel.Info.Height.SetValue(300f, 0f);
            panel.Info.Left.SetValue(PositionStyle.Half - panel.Info.Width / 2f);
            panel.Info.Top.SetValue(PositionStyle.Half - panel.Info.Height / 2f);
            panel.CanDrag = true;
            Register(panel);

            UIInputBox searchBox = new UIInputBox(string.Empty, 28f, Point.Zero, Color.White);
            searchBox.Info.Width.SetValue(268f, 0f);
            searchBox.Info.Height.SetValue(34f, 0f);
            searchBox.Info.Left.SetValue(18f, 0f);
            searchBox.Info.Top.SetValue(18f, 0f);
            searchBox.PanelColor = new Color(163, 163, 163);
            searchBox.Info.IsSensitive = true;
            panel.Register(searchBox);

            UIText text = new UIText("Search", Loita.DefaultFontSystem.GetFont(28f), new Color(217, 217, 217));
            text.Info.Left.SetValue(6f, 0f);
            searchBox.Register(text);
            searchBox.Events.OnUpdate += (e, gt) =>
            {
                text.Info.IsVisible = string.IsNullOrEmpty(searchBox.Text);
            };

            float interval = 6f;
            UIChoices filterButton = new UIChoices();
            filterButton.Info.Width.SetValue(34f, 0f);
            filterButton.Info.Height.SetValue(34f, 0f);
            filterButton.Info.Left.SetValue(PositionStyle.Full - filterButton.Info.Width - searchBox.Info.Left);
            filterButton.Info.Top.SetValue(searchBox.Info.Top);
            panel.Register(filterButton);

            UIChoices filterButton1 = new UIChoices();
            filterButton1.Info.Width.SetValue(34f, 0f);
            filterButton1.Info.Height.SetValue(34f, 0f);
            filterButton1.Info.Left.SetValue(filterButton.Info.Left - filterButton1.Info.Width);
            filterButton1.Info.Left.Pixel -= interval;
            filterButton1.Info.Top.SetValue(searchBox.Info.Top);
            panel.Register(filterButton1);

            UIChoices filterButton2 = new UIChoices();
            filterButton2.Info.Width.SetValue(34f, 0f);
            filterButton2.Info.Height.SetValue(34f, 0f);
            filterButton2.Info.Left.SetValue(filterButton1.Info.Left - filterButton2.Info.Width);
            filterButton2.Info.Left.Pixel -= interval;
            filterButton2.Info.Top.SetValue(searchBox.Info.Top);
            panel.Register(filterButton2);

            filterButton.NoSelectedPrimaryColor = filterButton1.NoSelectedPrimaryColor = filterButton2.NoSelectedPrimaryColor = searchBox.PanelColor;
            FilterCondition["Prefix"] = FilterCondition["Trigger"] = FilterCondition["Spell"] =
            filterButton.IsSelected = filterButton1.IsSelected = filterButton2.IsSelected = true;

            UIImage prefix = new UIImage(ModAssets_Texture2D.UI.UIContainers.InfusionBackpack.Images.PrefixImmediateAsset.Value, Color.White);
            prefix.Info.Width.SetValue(0f, 1f);
            prefix.Info.Height.SetValue(0f, 1f);
            filterButton.Register(prefix);
            filterButton.Events.OnLeftClick += element =>
            {
                FilterCondition["Prefix"] = !FilterCondition["Prefix"];
                filterButton.IsSelected = FilterCondition["Prefix"];
                ResetInfusions();
            };

            UIImage trigger = new UIImage(ModAssets_Texture2D.UI.UIContainers.InfusionBackpack.Images.TriggerImmediateAsset.Value, Color.White);
            trigger.Info.Width.SetValue(0f, 1f);
            trigger.Info.Height.SetValue(0f, 1f);
            filterButton1.Register(trigger);
            filterButton1.Events.OnLeftClick += element =>
            {
                FilterCondition["Trigger"] = !FilterCondition["Trigger"];
                filterButton1.IsSelected = FilterCondition["Trigger"];
                ResetInfusions();
            };

            UIImage spell = new UIImage(ModAssets_Texture2D.UI.UIContainers.InfusionBackpack.Images.SpellImmediateAsset.Value, Color.White);
            spell.Info.Width.SetValue(0f, 1f);
            spell.Info.Height.SetValue(0f, 1f);
            filterButton2.Register(spell);
            filterButton2.Events.OnLeftClick += element =>
            {
                FilterCondition["Spell"] = !FilterCondition["Spell"];
                filterButton2.IsSelected = FilterCondition["Spell"];
                ResetInfusions();
            };

            UIPanel infusionPanel = new UIPanel();
            infusionPanel.Info.Left.SetValue(searchBox.Info.Left);
            infusionPanel.Info.Top.SetValue(searchBox.Info.Height + (searchBox.Info.Top * 2f));
            //infusionPanel.Info.Top.Pixel += 4f;
            infusionPanel.Info.Width.SetValue(PositionStyle.Full - (infusionPanel.Info.Left * 2f));
            infusionPanel.Info.Height.SetValue(PositionStyle.Full - infusionPanel.Info.Top - searchBox.Info.Top);
            var t = (int)panel.Info.Width.Pixel % 46;
            t -= 2;
            lineInfusionCount = (int)panel.Info.Width.Pixel / 46;
            if (t < 20)
            {
                t += 46;
                lineInfusionCount--;
            }
            lineInfusionCount--;
            infusionPanel.Info.Width.Pixel -= t;
            panel.Register(infusionPanel);

            infusionSlotContainer = new UIContainerPanel();
            infusionSlotContainer.Info.SetMargin(2f);
            infusionPanel.Register(infusionSlotContainer);

            UIVerticalScrollbar verticalScrollbar = new UIVerticalScrollbar();
            verticalScrollbar.Info.Left.SetValue(PositionStyle.Full - searchBox.Info.Left - verticalScrollbar.Info.Width);
            verticalScrollbar.Info.Top.SetValue(infusionPanel.Info.Top);
            verticalScrollbar.Info.Height.SetValue(infusionPanel.Info.Height);
            panel.Register(verticalScrollbar);
            infusionSlotContainer.SetVerticalScrollbar(verticalScrollbar);
            verticalScrollbar.BindElement = infusionPanel;

            UIPanel boundaryLine = new UIPanel();
            boundaryLine.Info.Width.SetValue(PositionStyle.Full - (infusionPanel.Info.Left * 2f));
            boundaryLine.Info.Height.SetValue(4f, 0f);
            boundaryLine.Info.Left.SetValue(searchBox.Info.Left);
            boundaryLine.Info.Top.SetValue(searchBox.Info.Top * 1.5f + searchBox.Info.Height);
            boundaryLine.Info.Top.Pixel -= 2f;
            panel.Register(boundaryLine);
        }

        public override void PreUpdate(GameTime gt)
        {
            base.PreUpdate(gt);
            if (Main.playerInventory && !IsVisible)
            {
                Show();
            }
            if (!Main.playerInventory && IsVisible)
                Close();
        }

        public override void Show(params object[] args)
        {
            base.Show(args);
            _infusionSlot = null;
            if (args.Length > 0 && args[0] is CInfusionSlot cInfusionSlot)
            {
                _infusionSlot = cInfusionSlot;
            }
            ResetInfusions();
        }

        public void ResetInfusions()
        {
            infusionSlotContainer.ClearAllElements();

            int i = 0, j = 0;
            if (_infusionSlot != null)
            {
                UIChoices takeOut = new UIChoices();
                takeOut.Info.Width.SetValue(46f, 0f);
                takeOut.Info.Height.SetValue(46f, 0f);
                takeOut.Info.Left.SetValue(46f * i, 0f);
                takeOut.Info.Top.SetValue(46f * j, 0f);
                takeOut.Events.OnLeftClick += element =>
                {
                };
                infusionSlotContainer.AddElement(takeOut);

                UIImage takeOutImage = new UIImage(ModAssets_Texture2D.Images.CBlockImmediateAsset.Value, Color.White);
                takeOutImage.Info.Width.SetValue(0f, 1f);
                takeOutImage.Info.Height.SetValue(0f, 1f);
                takeOut.Register(takeOutImage);

                UIImage takeOutImage1 = new UIImage(ModAssets_Texture2D.Images.TakeOutImmediateAsset.Value, Color.White);
                takeOutImage1.Info.Width.SetValue(0f, 1f);
                takeOutImage1.Info.Height.SetValue(0f, 1f);
                takeOut.Register(takeOutImage1);

                takeOut.Events.OnUpdate += (element, gt) =>
                {
                    takeOut.BorderColor = takeOut.PanelColor;
                };

                i++;
                if (i >= lineInfusionCount)
                {
                    i = 0;
                    j++;
                }
            }

            var infusions = Infusions.FindAll(c =>
            {
                if (FilterCondition["Trigger"] && c is CTrigger)
                    return true;
                if (FilterCondition["Prefix"] && c is CPrefix)
                    return true;
                if (FilterCondition["Spell"] && c is CSpell)
                    return true;
                return false;
            });
            foreach (var comp in infusions)
            {
                UIChoices choice = new UIChoices();
                choice.Info.Width.SetValue(46f, 0f);
                choice.Info.Height.SetValue(46f, 0f);
                choice.Info.Left.SetValue(46f * i, 0f);
                choice.Info.Top.SetValue(46f * j, 0f);
                //choice.NoSelectedPrimaryColor = Color.Black;
                choice.Events.OnLeftClick += element =>
                {
                    if (choice.IsSelected && _infusionSlot != null)
                    {
                        var nc = _infusionSlot.ActivableSpace[_infusionSlotIndex];
                        Infusions.Remove(comp);
                        _infusionSlot.ChangeComponent(_infusionSlotIndex, comp);
                        Infusions.Add(nc);
                    }
                    Choices = choice;
                };
                infusionSlotContainer.AddElement(choice);

                UIImage image = new UIImage(comp == null ? ModAssets_Texture2D.Images.CBlockImmediateAsset.Value :
                    comp.Texture, Color.White);
                image.Info.Width.SetValue(0f, 1f);
                image.Info.Height.SetValue(0f, 1f);
                choice.Register(image);

                UIImage take = new UIImage(comp == null ? ModAssets_Texture2D.Images.TakeOutImmediateAsset.Value :
                    ModAssets_Texture2D.Images.TakeInImmediateAsset.Value, Color.Transparent);
                take.Info.Width.SetValue(0f, 1f);
                take.Info.Height.SetValue(0f, 1f);
                choice.Register(take);

                choice.Events.OnUpdate += (element, gt) =>
                {
                    choice.BorderColor = choice.PanelColor;
                    if (choice.IsSelected && _infusionSlot != null)
                    {
                        take.ChangeColor(Color.White * choice.Progression.Value);
                        image.ChangeColor(Color.White * Math.Max(0.6f, (1f - choice.Progression.Value)));
                    }
                    else
                    {
                        take.ChangeColor(Color.Transparent);
                        image.ChangeColor(Color.White);
                    }
                };

                i++;
                if (i >= lineInfusionCount)
                {
                    i = 0;
                    j++;
                }
            }
        }
    }
}