using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.GameContent;

namespace Loita.UI.UIElements
{
    internal class UIPanel : BaseElement
    {
        public Color PanelColor = new Color(97, 97, 97);
        public Color BorderColor = Color.Black;
        public bool CanDrag = false;
        private bool dragging = false;
        private Vector2 startPoint = Vector2.Zero;
        public (bool LeftBorder, bool TopBorder, bool RightBorder, bool BottomBorder) ShowBorder = (true, true, true, true);
        public int BorderWidth = 2;

        public override void LoadEvents()
        {
            base.LoadEvents();
            Events.OnLeftDown += element =>
            {
                if (CanDrag && !dragging)
                {
                    dragging = true;
                    startPoint = Main.MouseScreen;
                }
            };
            Events.OnLeftClick += element =>
            {
                if (CanDrag)
                    dragging = false;
            };
        }

        public override void OnInitialization()
        {
            base.OnInitialization();
            Info.SetMargin(2f);
        }

        public override void Calculation()
        {
            base.Calculation();
            if (Info.TotalSize.X < 1)
            {
                Info.TotalSize.X = 1;
                Info.TotalHitBox.Width = (int)Info.TotalSize.X;
            }
            if (Info.TotalSize.Y < 1)
            {
                Info.TotalSize.Y = 1;
                Info.TotalHitBox.Height = (int)Info.TotalSize.Y;
            }
        }

        public override void Update(GameTime gt)
        {
            base.Update(gt);
            if (CanDrag && startPoint != Main.MouseScreen && dragging)
            {
                var offsetValue = Main.MouseScreen - startPoint;
                Info.Left.Pixel += offsetValue.X;
                Info.Top.Pixel += offsetValue.Y;
                startPoint = Main.MouseScreen;

                Calculation();
            }
        }

        protected override void DrawSelf(SpriteBatch sb)
        {
            base.DrawSelf(sb);
            Texture2D texture = TextureAssets.MagicPixel.Value;
            sb.Draw(texture, Info.TotalHitBox, PanelColor);
            if (ShowBorder.LeftBorder)
            {
                if (ShowBorder.BottomBorder)
                    sb.Draw(texture,
                        new Rectangle(Info.TotalHitBox.X, Info.TotalHitBox.Y, BorderWidth, Info.TotalHitBox.Height - BorderWidth),
                        BorderColor);
                else
                    sb.Draw(texture,
                    new Rectangle(Info.TotalHitBox.X, Info.TotalHitBox.Y, BorderWidth, Info.TotalHitBox.Height),
                    BorderColor);
            }
            if (ShowBorder.TopBorder)
            {
                if (ShowBorder.LeftBorder)
                    sb.Draw(texture,
                        new Rectangle(Info.TotalHitBox.X + BorderWidth, Info.TotalHitBox.Y, Info.TotalHitBox.Width - BorderWidth, BorderWidth),
                        BorderColor);
                else
                    sb.Draw(texture,
                    new Rectangle(Info.TotalHitBox.X, Info.TotalHitBox.Y, Info.TotalHitBox.Width, BorderWidth),
                    BorderColor);
            }
            if (ShowBorder.RightBorder)
            {
                if (ShowBorder.TopBorder)
                    sb.Draw(texture,
                        new Rectangle(Info.TotalHitBox.Right - BorderWidth, Info.TotalHitBox.Y + BorderWidth, BorderWidth, Info.TotalHitBox.Height - BorderWidth),
                        BorderColor);
                else
                    sb.Draw(texture,
                    new Rectangle(Info.TotalHitBox.Right - BorderWidth, Info.TotalHitBox.Y, BorderWidth, Info.TotalHitBox.Height),
                    BorderColor);
            }
            if (ShowBorder.BottomBorder)
            {
                if (ShowBorder.RightBorder)
                    sb.Draw(texture,
                        new Rectangle(Info.TotalHitBox.X, Info.TotalHitBox.Bottom - BorderWidth, Info.TotalHitBox.Width - BorderWidth, BorderWidth),
                        BorderColor);
                else
                    sb.Draw(texture,
                    new Rectangle(Info.TotalHitBox.X, Info.TotalHitBox.Bottom - BorderWidth, Info.TotalHitBox.Width, BorderWidth),
                    BorderColor);
            }
            //Texture2D texture = Assets_Png.Images.UI.Panel;
            //Point textureSize = new Point(texture.Width, texture.Height);
            //Rectangle rectangle = Info.TotalHitBox;
            ////绘制四个角
            //sb.Draw(texture, new Vector2(rectangle.X, rectangle.Y), new Rectangle(0, 0, 6, 6), PanelColor);
            //sb.Draw(texture, new Vector2(rectangle.X + rectangle.Width - 6, rectangle.Y), new Rectangle(textureSize.X - 6, 0, 6, 6), PanelColor);
            //sb.Draw(texture, new Vector2(rectangle.X, rectangle.Y + rectangle.Height - 6), new Rectangle(0, textureSize.Y - 6, 6, 6), PanelColor);
            //sb.Draw(texture, new Vector2(rectangle.X + rectangle.Width - 6, rectangle.Y + rectangle.Height - 6), new Rectangle(textureSize.X - 6, textureSize.Y - 6, 6, 6), PanelColor);
            ////绘制本体
            //sb.Draw(texture, new Rectangle(rectangle.X + 6, rectangle.Y, rectangle.Width - 12, 6), new Rectangle(6, 0, textureSize.X - 12, 6), PanelColor);
            //sb.Draw(texture, new Rectangle(rectangle.X + 6, rectangle.Y + rectangle.Height - 6, rectangle.Width - 12, 6), new Rectangle(6, textureSize.Y - 6, textureSize.X - 12, 6), PanelColor);
            //sb.Draw(texture, new Rectangle(rectangle.X, rectangle.Y + 6, 6, rectangle.Height - 12), new Rectangle(0, 6, 6, textureSize.Y - 12), PanelColor);
            //sb.Draw(texture, new Rectangle(rectangle.X + rectangle.Width - 6, rectangle.Y + 6, 6, rectangle.Height - 12), new Rectangle(textureSize.X - 6, 6, 6, textureSize.Y - 12), PanelColor);
            //sb.Draw(texture, new Rectangle(rectangle.X + 6, rectangle.Y + 6, rectangle.Width - 12, rectangle.Height - 12), new Rectangle(6, 6, textureSize.X - 12, textureSize.Y - 12), PanelColor);
        }
    }
}