using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Terraria;
using Terraria.GameContent;

namespace Loita.UI.UIElements
{
    internal class UIHorizontalScrollbar : UIPanel, IScrollbar
    {
        private const int LEFT_HEIGHT = 1;
        private const int RIGHT_HEIGHT = 1;

        public Texture2D UIScrollbarInnerTexture { get; protected set; }
        private UIImage inner;
        public float MaxAlpha = 1f;
        public float MinAlpha = 0.4f;
        private float alpha;
        public bool AlwaysOnLight = false;
        public bool UseScrollWheel = false;
        private float mouseX;
        private bool isMouseHover = false;
        private bool isMouseDown = false;
        public float Scale => 1f;
        public float LeftMin => LEFT_HEIGHT * Scale;
        public float LeftMax => RIGHT_HEIGHT * Scale;

        public float InnerHeight = 0.75f;
        public float InnerWidth = 30f;
        public float LightSpeed = 0.04f;
        public Color InnerColor = Color.White;

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

        private float wheelValue;
        private float waitToWheelValue = 0f;

        private float _wheelValueMult = 1f;
        public float WheelValueMult { get => _wheelValueMult; set => _wheelValueMult = value; }
        private int wheel = 0;

        public UIHorizontalScrollbar(float heightPixel = 20f, float wheelValue = 0f)
        {
            alpha = MinAlpha;
            Info.Height = new PositionStyle(heightPixel, 0f);
            Info.Top = new PositionStyle(-heightPixel, 1f);
            Info.Width = new PositionStyle(-20f, 1f);
            Info.LeftMargin.Pixel = 5f;
            Info.RightMargin.Pixel = 5f;
            Info.IsSensitive = true;
            UIScrollbarInnerTexture = TextureAssets.MagicPixel.Value;
            WheelValue = wheelValue;

            inner = new UIImage(TextureAssets.MagicPixel.Value, Color.White);
            inner.Info.IsSensitive = true;
            inner.Info.Width.Pixel = InnerWidth;
            inner.Info.Height.SetValue(0f, InnerHeight);
            inner.Events.OnCalculation += Events_OnCalculation;
            inner.Events.OnMouseHover += Events_OnMouseHover;
            Register(inner);
        }

        private void Events_OnMouseHover(BaseElement baseElement)
        {
            InnerColor = Color.White;
        }

        private bool Events_OnCalculation(BaseElement baseElement)
        {
            inner.Info.Top.SetValue(-(inner.Info.Size.Y - Info.Size.Y) / 2f, 0f);
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
            Events.OnMouseHover += Events_OnMouseHover1;
        }

        private void Events_OnMouseHover1(BaseElement baseElement)
        {
            isMouseHover = true;
        }

        public override void Update(GameTime gt)
        {
            base.Update(gt);
            isMouseHover |= BindElement.GetCanHitBox().Contains(Main.MouseScreen.ToPoint());
            if (AlwaysOnLight)
                alpha = 1f;
            else
            {
                if (isMouseHover && alpha < MaxAlpha)
                    alpha += LightSpeed;
                if (!isMouseHover && alpha > MinAlpha)
                    alpha -= LightSpeed;
            }

            inner.ChangeColor(InnerColor * alpha);

            MouseState state = Mouse.GetState();
            float width = Info.Size.X - LeftMin - LeftMax - inner.Info.Size.X;
            if (!isMouseHover)
                wheel = state.ScrollWheelValue;

            if (UseScrollWheel && isMouseHover && wheel != state.ScrollWheelValue)
            {
                WheelValue -= (float)(state.ScrollWheelValue - wheel) / 6f / width * WheelValueMult;
                wheel = state.ScrollWheelValue;
            }
            if (isMouseDown && mouseX != Main.mouseX)
            {
                WheelValue = ((float)Main.mouseX - Info.Location.X - LeftMin - inner.Info.Size.X / 2f) / width;
                mouseX = Main.mouseX;
            }

            inner.Info.Left.Pixel = LeftMin + width * WheelValue;
            wheelValue += (waitToWheelValue - wheelValue) / 6f;

            if (waitToWheelValue != wheelValue)
                Calculation();
            isMouseHover = false;
        }
    }
}