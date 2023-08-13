using Loita.Components;
using Loita.Components.LoitaComponents;
using Loita.UI.UIElements;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Loita.UI.UIContainers.UIElements
{
    internal class UITips : BaseElement
    {
        public IInfusion Infusion;
        public LoitaComponent LoitaComponent;
        private Vector2 _size = Vector2.Zero;

        public UITips(IInfusion infusion)
        {
            Infusion = infusion;
            Info.Width.SetValue(0f, 1f);
            Info.Height.SetValue(0f, 1f);
        }

        public UITips(LoitaComponent loitaComponent)
        {
            LoitaComponent = loitaComponent;
            Info.Width.SetValue(0f, 1f);
            Info.Height.SetValue(0f, 1f);
        }

        public void Reset()
        {
            Infusion = null;
            LoitaComponent = null;
        }

        protected override void DrawSelf(SpriteBatch sb)
        {
            base.DrawSelf(sb);
            if (LoitaComponent != null)
            {
                LoitaComponent.DrawTips(sb, Info.Location, Info.Size, out Vector2 size);
                if (size != _size)
                {
                    _size = size;
                    Info.Width.SetValue(_size.X, 0f);
                    Info.Height.SetValue(_size.Y, 0f);
                    Calculation();
                }
            }
            else if (Infusion != null)
            {
                Infusion.DrawTips(sb, Info.Location, Info.Size, out Vector2 size);
                if (size != _size)
                {
                    _size = size;
                    Info.Width.SetValue(_size.X, 0f);
                    Info.Height.SetValue(_size.Y, 0f);
                    Calculation();
                }
            }
        }
    }
}