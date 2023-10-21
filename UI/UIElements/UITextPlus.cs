using Loita.StringDrawerSystem;

using Microsoft.Xna.Framework.Graphics;

using Terraria;

namespace Loita.UI.UIElements
{
    internal class UITextPlus : BaseElement
    {
        private StringDrawer _stringDrawer;
        private string text;

        public string Text
        {
            get { return text; }
            set
            {
                if (text != value)
                {
                    text = value;
                    _stringDrawer.Init(text);
                }
            }
        }

        /// <summary>
        /// 绘制大小，不改变部件碰撞箱，不改变绘制中心
        /// </summary>
        public float Scale;

        public bool CalculateSize = true;
        public PositionStyle? CenterX;
        public PositionStyle? CenterY;

        public UITextPlus(string t)
        {
            _stringDrawer = new StringDrawer();
            Text = t;

            Info.Width.Pixel = _stringDrawer.Size.X;
            Info.Height.Pixel = _stringDrawer.Size.Y;
        }

        public void WordWrap(float wrapWidth)
        {
            _stringDrawer.WordWrap(wrapWidth);
            Info.Width.Pixel = _stringDrawer.Size.X;
            Info.Height.Pixel = _stringDrawer.Size.Y;
        }

        public override void Calculation()
        {
            if (CalculateSize)
            {
                Info.Width.Pixel = _stringDrawer.Size.X;
                Info.Height.Pixel = _stringDrawer.Size.Y;
                Info.Width.Percent = 0f;
                Info.Height.Percent = 0f;
            }
            if (CenterX != null && CenterY != null)
            {
                var x = CenterX.Value;
                var y = CenterY.Value;
                Info.Left.Percent = x.Percent;
                Info.Top.Percent = y.Percent;
                Info.Left.Pixel = x.Pixel - Info.Width.Pixel / 2f;
                Info.Top.Pixel = y.Pixel - Info.Height.Pixel / 2f;
            }
            base.Calculation();
            _stringDrawer.SetPosition(Info.Location);
        }

        protected override void DrawSelf(SpriteBatch sb)
        {
            base.DrawSelf(sb);
            _stringDrawer.Draw(sb, Main.gameTimeCache);
        }
    }
}