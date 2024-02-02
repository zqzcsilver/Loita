using Loita.KeyBindSystem;
using Loita.UI.UIElements;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using Terraria;

namespace Loita.UI.UIContainers.MagicTrailEditor
{
    internal class MagicTrailContainer : UIContainerElement
    {
        public static MagicTrailContainer Instance =>
            (MagicTrailContainer)Loita.UISystem.Elements["Loita.UI.UIContainers.MagicTrailEditor.MagicTrailContainer"];

        public const string OPEN_HOT_KEY = "Open Magic Trail Editor";
        private bool _needOpenInventory = false;

        public override void Load()
        {
            Loita.KeyGroupManager.RegisterKeyGroup(new KeyGroup(OPEN_HOT_KEY, [Keys.Y]));
            On_Main.CanPauseGame += On_Main_CanPauseGame;
            base.Load();
        }

        private bool On_Main_CanPauseGame(On_Main.orig_CanPauseGame orig)
        {
            return orig() | IsVisible;
        }

        public override void OnInitialization()
        {
            base.OnInitialization();
            UIPanel panel = new UIPanel();
            panel.Info.Width.SetFull();
            panel.Info.Height.SetFull();
            panel.Info.SetToCenter();
            Register(panel);

            UIPanel tabBox = new UIPanel();
            tabBox.Info.Width.SetValue(422f, 0f);
            tabBox.Info.Height.SetValue(94f, 0f);
            tabBox.Info.SetToCenter();
            tabBox.Info.Top.SetValue(0f, 0.02f);
            tabBox.Info.SetMargin(12f);
            panel.Register(tabBox);
            for (int i = 0; i < 5; i++)
            {
                UIPanel tab = new UIPanel();
                tab.Info.Width.SetValue(70f, 0f);
                tab.Info.Height.SetValue(70f, 0f);
                tab.Info.Left.SetValue((PositionStyle.Full - tab.Info.Width * 5f) / 4f * i);
                tab.Info.Left += tab.Info.Width * i;
                tabBox.Register(tab);
            }

            UIPanel toolBox = new UIPanel();
            toolBox.Info.Width.SetValue(0f, 0.1f);
            toolBox.Info.Height.SetValue(PositionStyle.Full - (tabBox.Info.Top + tabBox.Info.Height) * 2f);
            toolBox.Info.SetToCenter();
            toolBox.Info.Left.SetValue(0f, 0.01f);
            panel.Register(toolBox);

            UIPanel infoBox = new UIPanel();
            infoBox.Info.Width.SetValue(0f, 0.1f);
            infoBox.Info.Height.SetValue(toolBox.Info.Height);
            infoBox.Info.SetToCenter();
            infoBox.Info.Left.SetValue(0f, 0.89f);
            panel.Register(infoBox);

            //UIPanel tabBoxOpen = new UIPanel();
            //tabBoxOpen.Info.Width.SetValue(105f, 0f);
            //tabBoxOpen.Info.Height.SetValue(50f, 0f);
            //tabBoxOpen.Info.SetCenter(tabBox.Info.Width / 2f + tabBox.Info.Left, tabBox.Info.Height / 2f + tabBox.Info.Top);
            //panel.Register(tabBoxOpen);
        }

        public override void Update(GameTime gt)
        {
            base.Update(gt);
        }

        public override void PreUpdate(GameTime gt)
        {
            base.PreUpdate(gt);
            if (KeyGroupManager.Instance.GetKeyGroup(OPEN_HOT_KEY).IsClick)
            {
                if (IsVisible)
                {
                    Close();
                    if (_needOpenInventory)
                        Main.playerInventory = true;
                }
                else
                {
                    Show();
                    _needOpenInventory = Main.playerInventory;
                    Main.playerInventory = false;
                }
            }
        }
    }
}