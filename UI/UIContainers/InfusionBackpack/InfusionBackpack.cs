using Loita.Components.LoitaComponents;
using Loita.Components.LoitaComponents.Prefixes;
using Loita.Components.LoitaComponents.Spells;
using Loita.QuickAssetReference;
using Loita.UI.UIContainers.WandInfusionManager.UIElements;
using Loita.UI.UIElements;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System;
using System.Collections.Generic;

using Terraria;

namespace Loita.UI.UIContainers.InfusionBackpack
{
    internal class InfusionBackpack : UIContainerElement
    {
        public static InfusionBackpack Instance =>
            (InfusionBackpack)Loita.UISystem.Elements["Loita.UI.UIContainers.InfusionBackpack.InfusionBackpack"];

        private UIContainerPanel infusionSlotContainer;
        private CInfusionSlot _infusionSlot;
        private int _infusionSlotIndex = -1;
        public CInfusionSlot CInfusionSlot => _infusionSlot;
        public List<LoitaComponent> Infusions => Main.LocalPlayer.GetModPlayer<MPlayer>().ComponentBackpack;
        public MPlayer Player => Main.LocalPlayer.GetModPlayer<MPlayer>();

        //public UIChoices Choices
        //{
        //    get => choices;
        //    set
        //    {
        //        if (choices == value)
        //            return;
        //        if (choices != null)
        //        {
        //            choices.Progression.Value = 1f;
        //            choices.IsSelected = false;
        //        }
        //        choices = value;
        //        choices.IsSelected = true;
        //        choices.Progression.Value = 0f;
        //    }
        //}

        //private UIChoices choices;
        private int lineInfusionCount = 0;

        public FilterCondition FilterCondition = new FilterCondition();

        public override void OnInitialization()
        {
            base.OnInitialization();

            UIPanel panel = new UIPanel();
            panel.Info.Width.SetValue(530f, 0f);
            panel.Info.Height.SetValue(300f, 0f);
            panel.Info.Left.SetValue(PositionStyle.Half - panel.Info.Width / 2f);
            panel.Info.Top.SetValue(PositionStyle.Half - panel.Info.Height / 2f);
            panel.CanDrag = true;
            Register(panel);

            float interval = 6f;

            UIInputBox searchBox = new UIInputBox(string.Empty, 28f, Point.Zero, Color.White);
            searchBox.Info.Width.SetValue(268f, 0f);
            searchBox.Info.Height.SetValue(34f, 0f);
            searchBox.Info.Top.SetValue(18f, 0f);
            searchBox.Info.Left.SetValue(PositionStyle.Full - searchBox.Info.Top - searchBox.Info.Width);
            searchBox.Info.Left.Pixel -= 34f + interval;
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

            UITextButtonPlus close = new UITextButtonPlus("×", Loita.DefaultFontSystem.GetFont(30f), Color.White);
            close.Info.Width.SetValue(34f, 0f);
            close.Info.Height.SetValue(34f, 0f);
            close.Info.Left.SetValue(PositionStyle.Full - close.Info.Width - searchBox.Info.Top);
            close.Info.Top.SetValue(searchBox.Info.Top);
            close.LockSize = true;
            close.LightColor = Color.Red;
            close.Events.OnLeftClick += element => Close();
            panel.Register(close);

            UIChoices filterButton = new UIChoices();
            filterButton.Info.Width.SetValue(34f, 0f);
            filterButton.Info.Height.SetValue(34f, 0f);
            filterButton.Info.Left.SetValue(searchBox.Info.Top);
            filterButton.Info.Top.SetValue(searchBox.Info.Top);
            panel.Register(filterButton);

            UIChoices filterButton1 = new UIChoices();
            filterButton1.Info.Width.SetValue(34f, 0f);
            filterButton1.Info.Height.SetValue(34f, 0f);
            filterButton1.Info.Left.SetValue(filterButton.Info.Left + filterButton.Info.Width);
            filterButton1.Info.Left.Pixel += interval;
            filterButton1.Info.Top.SetValue(searchBox.Info.Top);
            panel.Register(filterButton1);

            UIChoices filterButton2 = new UIChoices();
            filterButton2.Info.Width.SetValue(34f, 0f);
            filterButton2.Info.Height.SetValue(34f, 0f);
            filterButton2.Info.Left.SetValue(filterButton1.Info.Left + filterButton1.Info.Width);
            filterButton2.Info.Left.Pixel += interval;
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
            infusionPanel.Info.Left.SetValue(searchBox.Info.Top);
            infusionPanel.Info.Top.SetValue(searchBox.Info.Height + (searchBox.Info.Top * 2f));
            //infusionPanel.Info.Top.Pixel += 4f;
            infusionPanel.Info.Width.SetValue(PositionStyle.Full - (infusionPanel.Info.Left * 2f));
            infusionPanel.Info.Height.SetValue(PositionStyle.Full - infusionPanel.Info.Top - searchBox.Info.Top);
            int originalWidth = (int)(panel.Info.Width.Pixel - infusionPanel.Info.Left.Pixel * 2f);
            int cellWidth = 46 * 2;
            var t = originalWidth % cellWidth;
            t -= 10;
            lineInfusionCount = originalWidth / cellWidth;
            if (t < 20)
            {
                t += cellWidth / 2;
                lineInfusionCount--;
            }
            //lineInfusionCount--;
            infusionPanel.Info.Width.Pixel -= t;
            panel.Register(infusionPanel);

            infusionSlotContainer = new UIContainerPanel();
            infusionSlotContainer.Info.SetMargin(2f);
            infusionPanel.Register(infusionSlotContainer);

            UIVerticalScrollbar verticalScrollbar = new UIVerticalScrollbar();
            verticalScrollbar.Info.Left.SetValue(PositionStyle.Full - searchBox.Info.Top - verticalScrollbar.Info.Width);
            verticalScrollbar.Info.Top.SetValue(infusionPanel.Info.Top);
            verticalScrollbar.Info.Height.SetValue(infusionPanel.Info.Height);
            panel.Register(verticalScrollbar);
            infusionSlotContainer.SetVerticalScrollbar(verticalScrollbar);
            verticalScrollbar.BindElement = infusionPanel;

            UIPanel boundaryLine = new UIPanel();
            boundaryLine.Info.Width.SetValue(PositionStyle.Full - (searchBox.Info.Top * 2f));
            boundaryLine.Info.Height.SetValue(4f, 0f);
            boundaryLine.Info.Left.SetValue(searchBox.Info.Top);
            boundaryLine.Info.Top.SetValue(searchBox.Info.Top * 1.5f + searchBox.Info.Height - boundaryLine.Info.Height / 2f);
            panel.Register(boundaryLine);
        }

        public override void Show(params object[] args)
        {
            base.Show(args);
            _infusionSlot = null;
            if (args.Length > 0 && args[0] is CInfusionSlot cInfusionSlot && args[1] is int infusionSlotIndex)
            {
                _infusionSlot = cInfusionSlot;
                _infusionSlotIndex = infusionSlotIndex;
            }
            ResetInfusions();
        }

        public bool HasInfusionSlot => _infusionSlot != null;

        public void ChangeInfusionSlot(CInfusionSlot cInfusionSlot, int infusionSlotIndex)
        {
            _infusionSlot = cInfusionSlot;
            _infusionSlotIndex = infusionSlotIndex;
        }

        public void ResetInfusions()
        {
            infusionSlotContainer.ClearAllElements();

            int i = 0, j = 0;
            if (_infusionSlot != null)
            {
                UIChoices takeOut = new UIChoices();
                takeOut.Info.Width.SetValue(92f, 0f);
                takeOut.Info.Height.SetValue(46f, 0f);
                takeOut.Info.Left.SetValue(92f * i, 0f);
                takeOut.Info.Top.SetValue(46f * j, 0f);
                takeOut.Events.OnLeftClick += element =>
                {
                    var nc = _infusionSlot.ActivableSpace[_infusionSlotIndex];
                    _infusionSlot.ChangeComponent(_infusionSlotIndex, null);
                    Player.GainComponent(nc);

                    int i = 0;
                    _infusionSlot.InitActivableSpace(ref i);

                    ResetInfusions();
                    //Close();
                };
                infusionSlotContainer.AddElement(takeOut);

                UIImage takeOutImage = new UIImage(ModAssets_Texture2D.Images.CBlockImmediateAsset.Value, Color.White);
                takeOutImage.Info.Width.SetValue(0f, 0.5f);
                takeOutImage.Info.Height.SetValue(0f, 1f);
                takeOut.Register(takeOutImage);

                UIImage takeOutImage1 = new UIImage(ModAssets_Texture2D.Images.TakeOutImmediateAsset.Value, Color.White);
                takeOutImage1.Info.Width.SetValue(0f, 0.5f);
                takeOutImage1.Info.Height.SetValue(0f, 1f);
                takeOut.Register(takeOutImage1);

                takeOut.Events.OnUpdate += (element, gt) =>
                {
                    takeOut.BorderColor = takeOut.PanelColor;
                };

                UIText count = new UIText("×1", Loita.DefaultFontSystem.GetFont(18f));
                count.CenterX = new PositionStyle(0f, 0.75f);
                count.CenterY = new PositionStyle(0f, 0.45f);
                takeOut.Register(count);

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
            Dictionary<(Type, string, Texture2D), List<LoitaComponent>> comps = new Dictionary<(Type, string, Texture2D), List<LoitaComponent>>();
            infusions.ForEach(c =>
            {
                var key = (c.GetType(), c.Name, c.Texture);
                if (!comps.ContainsKey(key))
                    comps.Add(key, new List<LoitaComponent>());
                comps[key].Add(c);
            });
            foreach (var cp in comps)
            {
                var comp = cp.Value[0];
                UIChoices choice = new UIChoices();
                choice.Info.Width.SetValue(92f, 0f);
                choice.Info.Height.SetValue(46f, 0f);
                choice.Info.Left.SetValue(92f * i, 0f);
                choice.Info.Top.SetValue(46f * j, 0f);
                //choice.NoSelectedPrimaryColor = Color.Black;
                choice.Events.OnLeftClick += element =>
                {
                    if (/*choice.IsSelected && */_infusionSlot != null)
                    {
                        var nc = _infusionSlot.ActivableSpace[_infusionSlotIndex];
                        Infusions.Remove(comp);
                        _infusionSlot.ChangeComponent(_infusionSlotIndex, comp);
                        Player.GainComponent(nc);

                        int i = 0;
                        _infusionSlot.InitActivableSpace(ref i);

                        ResetInfusions();
                        //Close();
                    }
                    //Choices = choice;
                };
                infusionSlotContainer.AddElement(choice);

                UIImage image = new UIImage(comp == null ? ModAssets_Texture2D.Images.CBlockImmediateAsset.Value :
                    comp.Texture, Color.White);
                image.Info.Width.SetValue(0f, 0.5f);
                image.Info.Height.SetValue(0f, 1f);
                choice.Register(image);

                UIImage take = new UIImage(comp == null ? ModAssets_Texture2D.Images.TakeOutImmediateAsset.Value :
                    ModAssets_Texture2D.Images.TakeInImmediateAsset.Value, Color.Transparent);
                take.Info.Width.SetValue(0f, 0.5f);
                take.Info.Height.SetValue(0f, 1f);
                choice.Register(take);

                choice.Events.OnUpdate += (element, gt) =>
                {
                    choice.BorderColor = choice.PanelColor;
                    if (choice.IsSelected && _infusionSlot != null)
                    {
                        take.ChangeColor(Color.White * choice.Progression.Value);
                        image.ChangeColor(Color.White * Math.Max(0.6f, 1f - choice.Progression.Value));
                    }
                    else
                    {
                        take.ChangeColor(Color.Transparent);
                        image.ChangeColor(Color.White);
                    }
                };

                UIText count = new UIText($"×{cp.Value.Count}", Loita.DefaultFontSystem.GetFont(18f));
                count.CenterX = new PositionStyle(0f, 0.75f);
                count.CenterY = new PositionStyle(0f, 0.45f);
                choice.Register(count);

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