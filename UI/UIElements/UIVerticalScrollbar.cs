using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Terraria;
using Terraria.GameContent;

namespace Loita.UI.UIElements
{
    internal class UIVerticalScrollbar : UIPanel, IScrollbar
    {
        private const int TOP_HEIGHT = 1;
        private const int BUTTOM_HEIGHT = 1;

        public Texture2D UIScrollbarInnerTexture { get; protected set; }
        private UIImage inner;
        private bool isMouseDown = false;
        private bool isMouseHover = false;
        private float mouseY;
        private int whell = 0;
        private float wheelValue;
        private float waitToWheelValue;
        public float MaxAlpha = 1f;
        public float MinAlpha = 0.4f;
        private float alpha;
        public bool AlwaysOnLight = false;
        public float LightSpeed = 0.04f;
        public float InnerHeight = 30f;
        public float InnerWidth = 0.75f;
        public float Scale => 1f;
        public float TopMin => TOP_HEIGHT * Scale;
        public float TopMax => BUTTOM_HEIGHT * Scale;
        public BaseElement BindElement;

        public float WheelValue
        {
            get { return wheelValue; }
            set
            {
                if (value > 1f)
                    waitToWheelValue = 1f;
                else if (value < 0f)
                    waitToWheelValue = 0f;
                else
                    waitToWheelValue = value;
            }
        }

        private float _wheelValueMult = 1f;
        public float WheelValueMult { get => _wheelValueMult; set => _wheelValueMult = value; }
        public bool UseScrollWheel = true;

        public UIVerticalScrollbar(float heightPixel = 20f, float wheelValue = 0f)
        {
            alpha = MinAlpha;
            Info.Width = new PositionStyle(heightPixel, 0f);
            Info.Left = new PositionStyle(-heightPixel, 1f);
            Info.Height = new PositionStyle(0f, 1f);
            Info.TopMargin.Pixel = 5f;
            Info.ButtomMargin.Pixel = 5f;
            Info.IsSensitive = true;
            UIScrollbarInnerTexture = TextureAssets.MagicPixel.Value;
            WheelValue = wheelValue;

            inner = new UIImage(TextureAssets.MagicPixel.Value, Color.White);
            inner.Info.Width.SetValue(0f, InnerWidth);
            inner.Info.Height.Pixel = InnerHeight;
            inner.Events.OnCalculation += Events_OnCalculation;
            Register(inner);
        }

        private bool Events_OnCalculation(BaseElement baseElement)
        {
            baseElement.Info.Left.SetValue(-(inner.Info.Size.X - Info.Size.X) / 2f, 0f);
            return true;
        }

        public override void LoadEvents()
        {
            base.LoadEvents();
            Events.OnLeftDown += element =>
            {
                if (!isMouseDown)
                {
                    isMouseDown = true;
                }
            };
            Events.OnLeftUp += element =>
            {
                isMouseDown = false;
            };
            Events.OnMouseHover += Events_OnMouseHover;
        }

        private void Events_OnMouseHover(BaseElement baseElement)
        {
            isMouseHover = true;
        }

        public override void Update(GameTime gt)
        {
            base.Update(gt);
            if (BindElement != null)
                isMouseHover |= BindElement.GetCanHitBox().Contains(Main.MouseScreen.ToPoint());
            if (AlwaysOnLight)
                alpha = 1f;
            else
            {
                if ((isMouseHover) && alpha < MaxAlpha)
                    alpha += LightSpeed;
                if ((!(isMouseHover)) && alpha > MinAlpha)
                    alpha -= LightSpeed;
            }

            inner.ChangeColor(Color.White * alpha * MaxAlpha);

            MouseState state = Mouse.GetState();
            float height = Info.Size.Y - TopMin - TopMax - inner.Info.Size.Y;
            if (!isMouseHover)
                whell = state.ScrollWheelValue;

            if (UseScrollWheel && isMouseHover && whell != state.ScrollWheelValue)
            {
                WheelValue -= (float)(state.ScrollWheelValue - whell) / 6f / height * WheelValueMult;
                whell = state.ScrollWheelValue;
            }
            if (isMouseDown && mouseY != Main.mouseY)
            {
                WheelValue = ((float)Main.mouseY - Info.Location.Y - TopMin - inner.Info.Size.Y / 2f) / height;
                mouseY = Main.mouseY;
            }

            inner.Info.Top.Pixel = TopMin + WheelValue * height;
            wheelValue += (waitToWheelValue - wheelValue) / 6f;

            if (waitToWheelValue != wheelValue)
                Calculation();
            isMouseHover = false;
        }
    }
}