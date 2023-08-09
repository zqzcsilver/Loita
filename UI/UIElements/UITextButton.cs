using FontStashSharp;

using Microsoft.Xna.Framework;

namespace Loita.UI.UIElements
{
    internal class UITextButton : UIPanel
    {
        public UIText UIText;
        public bool LockSize = false;

        public UITextButton(string text, SpriteFontBase font, Color color)
        {
            Info.IsSensitive = true;
            CanDrag = false;
            Info.SetMargin(0f);
            UIText = new UIText(text, font, color);
            UIText.CenterX = new PositionStyle(0f, 0.5f);
            UIText.CenterY = new PositionStyle(-4f, 0.5f);
            Register(UIText);
            Info.Width.SetValue(UIText.Info.Width.Pixel + 4f, UIText.Info.Width.Percent);
            Info.Height.SetValue(UIText.Info.Height.Pixel + 4f, UIText.Info.Height.Percent);

            Events.OnCalculation += Events_OnCalculation;
        }

        private bool Events_OnCalculation(BaseElement baseElement)
        {
            if (LockSize)
                return false;
            Info.Width.SetValue(UIText.Info.Width.Pixel + 4f, UIText.Info.Width.Percent);
            Info.Height.SetValue(UIText.Info.Height.Pixel + 4f, UIText.Info.Height.Percent);
            return true;
        }
    }
}