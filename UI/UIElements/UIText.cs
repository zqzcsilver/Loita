using FontStashSharp;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria.Localization;

namespace Loita.UI.UIElements
{
    internal class UIText : BaseElement
    {
        private string text;

        public string Text
        {
            get { return text; }
            set
            {
                text = value;
            }
        }

        public string DisplayText => Language.GetTextValue(text);

        private SpriteFontBase font;
        public SpriteFontBase Font { get => font; set => font = value; }
        public Color Color;

        public bool CalculateSize = true;
        public PositionStyle? CenterX;
        public PositionStyle? CenterY;
        public bool CalculationCenter = true;

        public UIText(string t, SpriteFontBase spriteFont)
        {
            text = t;
            font = spriteFont;
            Color = Color.White;

            Vector2 size = font.MeasureString(DisplayText);
            Info.Width.Pixel = size.X;
            Info.Height.Pixel = size.Y;

            Events.OnCalculation += Events_OnCalculation;
        }

        public UIText(string t, SpriteFontBase spriteFont, Color textColor)
        {
            text = t;
            font = spriteFont;
            Color = textColor;

            Vector2 size = font.MeasureString(DisplayText);
            Info.Width.Pixel = size.X;
            Info.Height.Pixel = size.Y;

            Events.OnCalculation += Events_OnCalculation;
        }

        private bool Events_OnCalculation(BaseElement baseElement)
        {
            bool op = false;
            if (CalculateSize)
            {
                Vector2 size = font.MeasureString(DisplayText);
                Info.Width.Pixel = size.X;
                Info.Height.Pixel = size.Y;
                Info.Width.Percent = 0f;
                Info.Height.Percent = 0f;
                op = true;
            }
            if (CalculationCenter && CenterX != null && CenterY != null)
            {
                var x = CenterX.Value;
                var y = CenterY.Value;
                Info.Left.Percent = x.Percent;
                Info.Top.Percent = y.Percent;
                Info.Left.Pixel = x.Pixel - Info.Width.Pixel / 2f;
                Info.Top.Pixel = y.Pixel - Info.Height.Pixel / 2f;
                op = true;
            }
            return op;
        }

        public void ChangeFont(SpriteFontBase spriteFont)
        {
            font = spriteFont;
        }

        protected override void DrawSelf(SpriteBatch sb)
        {
            base.DrawSelf(sb);
            sb.DrawString(font, DisplayText,
                Info.Location, Color);
        }
    }
}