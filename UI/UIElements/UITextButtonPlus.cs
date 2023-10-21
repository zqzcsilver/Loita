using FontStashSharp;

using Loita.Utils;

using Microsoft.Xna.Framework;

namespace Loita.UI.UIElements
{
    internal class UITextButtonPlus : UITextButton
    {
        public Color LightColor = new Color(134, 161, 166);
        public Color PrimaryColor;
        public SleekProgression Progression = new SleekProgression();

        public UITextButtonPlus(string text, SpriteFontBase font, Color color) : base(text, font, color)
        {
            PrimaryColor = PanelColor;
        }

        public override void LoadEvents()
        {
            base.LoadEvents();
            Events.OnMouseOver += element =>
            {
                Progression.WaitToValue = 1f;
            };
            Events.OnMouseOut += element =>
            {
                Progression.WaitToValue = 0f;
            };
        }

        public override void Update(GameTime gt)
        {
            base.Update(gt);
            Progression.Update(gt);
            PanelColor = Color.Lerp(PrimaryColor, LightColor, Progression.Value);
        }
    }
}