using Loita.UI.UIElements;

using Microsoft.Xna.Framework;

using static Loita.UI.UIElements.BaseElement;

namespace Loita.UI.UIContainers.DebugUI.DebugItems
{
    internal abstract class DebugItemBase
    {
        public abstract string Name { get; }
        public bool BeSelected = false;
        public uint Level = 1;
        public virtual BaseElement GetTab()
        {
            UITextButtonPlus button = new UITextButtonPlus(Name, Loita.DefaultFontSystem.GetFont(30f), Color.White);
            button.Info.Width.SetValue(0f, 1f);
            button.Info.Height.SetValue(36f, 0f);
            button.LockSize = true;
            var defColor = button.PanelColor;
            button.Events.OnUpdate += (e, gt) =>
            {
                button.UIText.Text = Name;
                button.PrimaryColor = BeSelected ? button.LightColor : defColor;
            };
            return button;
        }

        public virtual BaseElement GetPage()
        {
            BaseElement baseElement = new BaseElement();
            baseElement.Info.Width.SetValue(PositionStyle.Full);
            baseElement.Info.Height.SetValue(PositionStyle.Full);
            return baseElement;
        }
    }
}