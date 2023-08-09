using Loita.UI.UIElements;
using Loita.Utils;

using Microsoft.Xna.Framework;

namespace Loita.UI.UIContainers.StaffInfusionManager.UIElements
{
    internal class UIChoices : UIPanel
    {
        public Color LightColor = new Color(134, 161, 166);
        public Color PrimaryColor;
        public SleekProgression Progression = new SleekProgression();
        public bool IsSelected = false;
        public Color NoSelectedPrimaryColor = new Color(97, 97, 97);
        public UIChoices()
        {
            PrimaryColor = PanelColor;
            Info.IsSensitive = true;
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
            PrimaryColor = IsSelected ? LightColor : NoSelectedPrimaryColor;
            PanelColor = Color.Lerp(PrimaryColor, LightColor, Progression.Value);
        }
    }
}