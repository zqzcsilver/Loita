using FontStashSharp;

using Microsoft.Xna.Framework;

namespace Loita.UI.UIElements
{
    internal class UITextPanel : UIPanel
    {
        public UIText UIText;

        public UITextPanel(string text, SpriteFontBase font, Color color)
        {
            UIText = new UIText(text, font, color);
            UIText.Color = Color.Black;
            Info.Width.Pixel = UIText.Info.Width.Pixel + 4f;
            Info.Height.Pixel = UIText.Info.Height.Pixel + 4f;
            Register(UIText);

            CanDrag = false;
        }

        public override void Calculation()
        {
            base.Calculation();
            Info.Width.Pixel = UIText.Info.Width.Pixel + 4f;
            Info.Height.Pixel = UIText.Info.Height.Pixel + 4f;
            base.Calculation();
        }
    }
}