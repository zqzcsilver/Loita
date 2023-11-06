using Loita.Components.LoitaComponents;
using Loita.Components.LoitaComponents.Prefixes;
using Loita.Components.LoitaComponents.Spells;
using Loita.KeyBindSystem;
using Loita.QuickAssetReference;
using Loita.RecipeSystem.RecipeItems;
using Loita.UI.UIContainers.InfusionBackpack;
using Loita.UI.UIContainers.WandInfusionManager.UIElements;
using Loita.UI.UIElements;
using Loita.Utils.Expands;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using System.Collections.Generic;

using Terraria;

namespace Loita.UI.UIContainers.RecipeUI
{
    internal class RecipeContainer : UIContainerElement
    {
        private class UIRecipeBlock : UIChoices
        {
            private readonly RecipeItem _recipeItem;

            public UIRecipeBlock(RecipeItem recipeItem)
            {
                _recipeItem = recipeItem;
            }

            public override void Update(GameTime gt)
            {
                base.Update(gt);
                BorderColor = _recipeItem.Permission() ? Color.Yellow : new Color(134, 161, 166);
            }

            protected override void DrawSelf(SpriteBatch sb)
            {
                base.DrawSelf(sb);
                sb.Draw(_recipeItem.Icon, Info.HitBox, Color.White);
            }
        }

        private class UIRecipeItem : UIChoices
        {
            private readonly RecipeItem _recipeItem;
            private UIContainerPanel _conditionsPanel;
            private UIContainerPanel _resultsPanel;
            private float _progress = 0f;
            private bool _pressed = false;
            private bool _active = false;

            public UIRecipeItem(RecipeItem recipeItem)
            {
                ShowBorder = (false, false, false, false);
                _recipeItem = recipeItem;
                Info.Width.SetValue(0f, 1f);
                Info.Height.SetValue((40f + 6f) * 2f + 6f + 6f * 4f, 0f);
                Info.SetMargin(6f * 2f);
                //Info.IsSensitive = false;

                UIPanel conditions = new UIPanel();
                conditions.Info.Width.SetValue((40f + 6f) * 3f + 6f, 0f);
                conditions.Info.Height.SetFull();
                Register(conditions);

                UIVerticalScrollbar scrollbar = new UIVerticalScrollbar();
                _conditionsPanel = new UIContainerPanel();
                scrollbar.BindElement = _conditionsPanel;
                _conditionsPanel.SetVerticalScrollbar(scrollbar);
                _conditionsPanel.Events.OnUpdate += (e, gt) =>
                {
                    scrollbar.Update(gt);
                };
                conditions.Register(_conditionsPanel);

                List<BaseElement> es = new List<BaseElement>();
                for (int i = 0; i < recipeItem.Conditions.Count; i++)
                {
                    var condition = recipeItem.Conditions[i];
                    UIChoices choices = new UIChoices();
                    choices.Info.Width.SetValue(40f, 0f);
                    choices.Info.Height.SetValue(40f, 0f);
                    choices.Info.Left.SetValue((40f + 6f) * (i % 4), 0f);
                    choices.Info.Top.SetValue((40f + 6f) * (i / 4), 0f);
                    choices.Info.IsSensitive = true;
                    choices.Events.OnUpdate += (e, gt) =>
                    {
                        choices.BorderColor = condition.Permission() ? Color.Yellow : Color.Black;
                    };
                    es.Add(choices);

                    UIImage image = new UIImage(condition.Icon, Color.White);
                    image.Info.Width.SetFull();
                    image.Info.Height.SetFull();
                    image.Events.OnUpdate += (e, gt) =>
                    {
                        image.ChangeColor(condition.IsEnable ? Color.White : Color.White * 0.6f);
                    };
                    choices.Register(image);
                }
                _conditionsPanel.AddElements(es);

                UIPanel results = new UIPanel();
                results.Info.Width.SetValue((40f + 6f) * 3f + 6f, 0f);
                results.Info.Height.SetFull();
                results.Info.SetToRight();
                Register(results);

                UIVerticalScrollbar vscrollbar = new UIVerticalScrollbar();
                _resultsPanel = new UIContainerPanel();
                vscrollbar.BindElement = _resultsPanel;
                _resultsPanel.SetVerticalScrollbar(vscrollbar);
                _resultsPanel.Events.OnUpdate += (e, gt) =>
                {
                    vscrollbar.Update(gt);
                };
                results.Register(_resultsPanel);

                es.Clear();
                for (int i = 0; i < recipeItem.Results.Count; i++)
                {
                    var result = recipeItem.Results[i];
                    UIChoices choices = new UIChoices();
                    choices.Info.Width.SetValue(40f, 0f);
                    choices.Info.Height.SetValue(40f, 0f);
                    choices.Info.Left.SetValue((40f + 6f) * (i % 4), 0f);
                    choices.Info.Top.SetValue((40f + 6f) * (i / 4), 0f);
                    choices.Info.IsSensitive = true;
                    choices.Events.OnUpdate += (e, gt) => choices.BorderColor = choices.PanelColor;
                    es.Add(choices);

                    UIImage image = new UIImage(result.Icon, Color.White);
                    image.Info.Width.SetFull();
                    image.Info.Height.SetFull();
                    image.Events.OnUpdate += (e, gt) =>
                    {
                        image.ChangeColor(result.IsEnable ? Color.White : Color.White * 0.6f);
                    };
                    choices.Register(image);
                }
                _resultsPanel.AddElements(es);
            }

            public override void LoadEvents()
            {
                base.LoadEvents();
                Events.OnLeftDown += Events_OnLeftDown;
                Events.OnLeftUp += Events_OnLeftUp;
            }

            private void Events_OnLeftUp(BaseElement baseElement)
            {
                _pressed = false;
            }

            private void Events_OnLeftDown(BaseElement baseElement)
            {
                _pressed = true;
            }

            public override void Update(GameTime gt)
            {
                base.Update(gt);
                _active = _recipeItem.Permission();
                if (!_pressed || !_active)
                {
                    if (_progress > 0f)
                        _progress -= 0.2f;
                    else
                        _progress = 0f;
                    return;
                }
                if (_progress < 1f)
                {
                    _progress += 0.01f;
                }
                else
                {
                    _progress = 0f;
                    _recipeItem.Apply();
                    _pressed = false;
                }
            }

            protected override void DrawChildren(SpriteBatch sb)
            {
                base.DrawChildren(sb);
                var texture = ModAssets_Texture2D.UI.UIContainers.RecipeUI.Images.ToImmediateAsset.Value;
                var size = texture.GetSize();
                var pos = Info.HitBox.Center.ToVector2() - size / 2f;
                sb.Draw(texture, pos, Color.White);
                if (_active)
                    sb.Draw(ModAssets_Texture2D.UI.UIContainers.RecipeUI.Images.ToFullImmediateAsset.Value,
                        pos, new Rectangle(0, 0, (int)(size.X * _progress), (int)size.Y), Color.White);
                else
                    sb.Draw(ModAssets_Texture2D.UI.UIContainers.RecipeUI.Images.ToFullImmediateAsset.Value,
                        pos, Color.White * 0.6f);
            }
        }

        public const string OPEN_HOT_KEY = "Open Recipe Panel";
        private UIContainerPanel _recipePanel;
        private List<RecipeItem> _recipeItems = new List<RecipeItem>();
        public FilterCondition FilterCondition = new FilterCondition();
        private bool _isRecipeListPage = true;

        private RecipeItem selectedItem = null;
        private UIInputBox _searchBox;

        public override void Load()
        {
            base.Load();
            Loita.KeyGroupManager.RegisterKeyGroup(new KeyGroup(OPEN_HOT_KEY, new List<Keys>() { Keys.P }));
        }

        public override void OnInitialization()
        {
            FilterCondition.OnConditionChanged += key =>
            {
                RefreshActiveRecipe();
                RefreshRecipePanel();
            };
            base.OnInitialization();
            const float ELEMENT_INTERVAL = 6f;
            UIPanel panel = new UIPanel();
            panel.Info.Width.SetValue(652f, 0f);
            panel.Info.Height.SetValue(360f, 0f);
            panel.Info.SetToCenter();
            panel.Info.SetMargin(18f);
            panel.CanDrag = true;
            Register(panel);

            BaseElement topTab = new BaseElement();
            topTab.Info.Width.SetFull();
            topTab.Info.Height.SetValue(40f, 0f);
            topTab.Info.SetMargin(0f);
            panel.Register(topTab);

            UIChoices recipeList = new UIChoices();
            recipeList.Info.Width.SetValue(40f, 0f);
            recipeList.Info.Height.SetValue(40f, 0f);
            recipeList.Events.OnLeftClick += e =>
            {
                if (!_isRecipeListPage)
                {
                    _isRecipeListPage = true;
                    RefreshPage();
                }
            };
            recipeList.Events.OnUpdate += (e, gt) => recipeList.IsSelected = _isRecipeListPage;
            topTab.Register(recipeList);

            UIImage recipeIcon = new UIImage(ModAssets_Texture2D.UI.UIContainers.RecipeUI.Images.RecipeIconImmediateAsset.Value, Color.White);
            recipeIcon.Info.Width.SetFull();
            recipeIcon.Info.Height.SetFull();
            recipeList.Register(recipeIcon);

            UIChoices creater = new UIChoices();
            creater.Info.Width.SetValue(40f, 0f);
            creater.Info.Height.SetValue(40f, 0f);
            creater.Info.Left.SetValue(recipeList.Info.Width);
            creater.Info.Left.Pixel += ELEMENT_INTERVAL;
            creater.Info.IsSensitive = true;
            creater.Events.OnUpdate += (e, gt) => creater.IsSelected = !_isRecipeListPage;
            creater.Events.OnLeftClick += e =>
            {
                if (_isRecipeListPage)
                {
                    _isRecipeListPage = false;
                    RefreshPage();
                }
            };
            topTab.Register(creater);

            UIImage createIcon = new UIImage(ModAssets_Texture2D.UI.UIContainers.RecipeUI.Images.CreateIconImmediateAsset.Value, Color.White);
            createIcon.Info.Width.SetFull();
            createIcon.Info.Height.SetFull();
            creater.Register(createIcon);

            UIChoices close = new UIChoices();
            close.Info.Width.SetValue(40f, 0f);
            close.Info.Height.SetValue(40f, 0f);
            close.Info.SetToRight();
            close.LightColor = Color.Red;
            close.Events.OnLeftClick += element => Close();
            close.Info.IsSensitive = true;
            topTab.Register(close);

            UIImage closeIcon = new UIImage(ModAssets_Texture2D.UI.UIContainers.RecipeUI.Images.CloseIconImmediateAsset.Value, Color.White);
            closeIcon.Info.Width.SetFull();
            closeIcon.Info.Height.SetFull();
            close.Register(closeIcon);

            _searchBox = new UIInputBox(string.Empty, 36f, Point.Zero, Color.White);
            _searchBox.Info.Height.SetValue(40f, 0f);
            _searchBox.Info.Left.SetValue(creater.Info.Left + creater.Info.Width);
            _searchBox.Info.Left.Pixel += ELEMENT_INTERVAL * 2f;
            _searchBox.Info.Width.SetValue(PositionStyle.Full - _searchBox.Info.Left - close.Info.Width);
            _searchBox.Info.Width.Pixel -= ELEMENT_INTERVAL * 2f;
            _searchBox.PanelColor = new Color(163, 163, 163);
            _searchBox.Info.IsSensitive = true;
            panel.Register(_searchBox);

            UIText text = new UIText("Search", Loita.DefaultFontSystem.GetFont(36f), new Color(217, 217, 217));
            text.Info.Left.SetValue(6f, 0f);
            _searchBox.Register(text);
            _searchBox.Events.OnUpdate += (e, gt) =>
            {
                text.Info.IsVisible = string.IsNullOrEmpty(_searchBox.Text);
            };

            UIPanel filterPanel = new UIPanel();
            filterPanel.Info.Width.SetValue((40f + ELEMENT_INTERVAL) * 3f + ELEMENT_INTERVAL, 0f);
            filterPanel.Info.Height.SetValue(PositionStyle.Full - topTab.Info.Height);
            filterPanel.Info.Height.Pixel -= ELEMENT_INTERVAL;
            filterPanel.Info.SetToBottom();
            filterPanel.Info.SetMargin(ELEMENT_INTERVAL);
            filterPanel.ShowBorder.RightBorder = false;
            panel.Register(filterPanel);

            UIPanel content = new UIPanel();
            content.Info.Width.SetValue(PositionStyle.Full - filterPanel.Info.Width);
            content.Info.Height.SetValue(filterPanel.Info.Height);
            content.Info.SetToBottom();
            content.Info.SetToRight();
            content.Info.SetMargin(ELEMENT_INTERVAL);
            panel.Register(content);

            UIVerticalScrollbar scrollbar = new UIVerticalScrollbar();
            content.Register(scrollbar);

            _recipePanel = new UIContainerPanel();
            _recipePanel.Info.Width.SetValue(PositionStyle.Full - scrollbar.Info.Width);
            scrollbar.BindElement = _recipePanel;
            _recipePanel.SetVerticalScrollbar(scrollbar);
            content.Register(_recipePanel);

            recipeList.NoSelectedPrimaryColor = close.NoSelectedPrimaryColor = creater.NoSelectedPrimaryColor = _searchBox.PanelColor;
        }

        private void RefreshPage()
        {
            RefreshRecipePanel();
        }

        private void RefreshRecipePanel()
        {
            const float ELEMENT_INTERVAL = 6f;
            _recipePanel.ClearAllElements();
            List<BaseElement> es = new List<BaseElement>();
            if (_isRecipeListPage)
            {
                var margin = ((int)_recipePanel.Info.Size.X % (int)(40f + ELEMENT_INTERVAL) + ELEMENT_INTERVAL) / 2f;
                _recipePanel.Info.LeftMargin = _recipePanel.Info.RightMargin = new PositionStyle(margin, 0f);
                int widthCount = (int)(_recipePanel.Info.Size.X / (40f + ELEMENT_INTERVAL));
                for (int i = 0; i < _recipeItems.Count; i++)
                {
                    var recipeItem = _recipeItems[i];
                    UIRecipeBlock recipeBlock = new UIRecipeBlock(recipeItem);
                    recipeBlock.Info.Width.SetValue(40f, 0f);
                    recipeBlock.Info.Height.SetValue(40f, 0f);
                    recipeBlock.Info.Left.SetValue((40f + ELEMENT_INTERVAL) * (i % widthCount), 0f);
                    recipeBlock.Info.Top.SetValue((40f + ELEMENT_INTERVAL) * (i / widthCount), 0f);
                    recipeBlock.Events.OnLeftClick += e =>
                    {
                        selectedItem = recipeItem;
                        _isRecipeListPage = false;
                        RefreshRecipePanel();
                    };
                    es.Add(recipeBlock);
                }
                _recipePanel.AddElements(es);
            }
            else
            {
                _recipePanel.Info.SetMargin(ELEMENT_INTERVAL);
                PositionStyle top = PositionStyle.Empty;
                float p = 0f;
                for (int i = 0; i < _recipeItems.Count; i++)
                {
                    if (_recipeItems[i] == selectedItem)
                    {
                        p = top.Pixel;
                        selectedItem = null;
                    }
                    UIRecipeItem recipeItem = new UIRecipeItem(_recipeItems[i]);
                    recipeItem.Info.Top.SetValue(top);
                    es.Add(recipeItem);
                    top += recipeItem.Info.Height;
                    top.Pixel += ELEMENT_INTERVAL;
                    if (i != _recipeItems.Count - 1)
                    {
                        UIPanel line = new UIPanel();
                        line.Info.Width.SetFull();
                        line.Info.Height.SetValue(4f, 0f);
                        line.Info.Top.SetValue(top);
                        line.Info.Top.Pixel -= 5f;
                        es.Add(line);
                    }
                }
                _recipePanel.AddElements(es);
                _recipePanel.SetVerticalScrollbarToPixel(p);
            }
        }

        private void RefreshActiveRecipe()
        {
            _recipeItems = new List<RecipeItem>(Loita.RecipeSystem.RecipeItems);
            _recipeItems.RemoveAll(r =>
            {
                if (!FilterCondition["LCRecipe"] && r is not LCRecipeItem)
                    return false;
                if (!FilterCondition["SingleCondition"] && r.Conditions.Count > 1)
                    return false;
                if (!FilterCondition["SingleResult"] && r.Results.Count > 1)
                    return false;
                if (r is LCRecipeItem lcr)
                {
                    if (!FilterCondition["Trigger"] &&
                        lcr.TResults.Find(result => result.LCType.IsSubclassOf(typeof(CTrigger))) == null)
                        return false;
                    if (!FilterCondition["Prefix"] &&
                        lcr.TResults.Find(result => result.LCType.IsSubclassOf(typeof(CPrefix))) == null)
                        return false;
                    if (!FilterCondition["Spell"] &&
                        lcr.TResults.Find(result => result.LCType.IsSubclassOf(typeof(CSpell))) == null)
                        return false;
                }
                if (FilterCondition["All"])
                    return false;
                return true;
            });
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

        public override void Show(params object[] args)
        {
            base.Show(args);
            RefreshActiveRecipe();
            RefreshRecipePanel();
        }
    }
}