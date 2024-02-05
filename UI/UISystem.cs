using FontStashSharp;

using Microsoft.Xna.Framework;

using ReLogic.Graphics;

using System.Collections.Generic;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;

namespace Loita.UI
{
    internal class UISystem : ModSystem
    {
        public string MouseText = string.Empty;
        private Point ScreenSize;
        public bool DrawIME = false;
        public Vector2 IMEPosition = Vector2.Zero;

        public override void UpdateUI(GameTime gameTime)
        {
            base.UpdateUI(gameTime);

            if (ScreenSize != Main.ScreenSize)
            {
                ScreenSize = Main.ScreenSize;
                Loita.UISystem.Calculation();
            }
            Loita.UISystem.Update(gameTime);
        }

        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            base.ModifyInterfaceLayers(layers);
            int mouseTextIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Cursor"));
            if (mouseTextIndex != -1)
            {
                layers.Insert(mouseTextIndex, new LegacyGameInterfaceLayer(
                    "Loita: UI System",
                    delegate
                    {
                        if (Main.netMode != NetmodeID.Server)
                        {
                            var sb = Main.spriteBatch;
                            Loita.UISystem.Draw(sb);
                            if (DrawIME)
                            {
                                Main.instance.DrawWindowsIMEPanel(IMEPosition, 0f);
                                DrawIME = false;
                            }
                            if (!string.IsNullOrEmpty(MouseText))
                            {
                                var pos = Main.MouseScreen + new Vector2(10f, 18f);
                                var font = Loita.DefaultFontSystem.GetFont(30f);
                                var textSize = font.MeasureString(MouseText);

                                if (pos.X + textSize.X > Main.screenWidth)
                                    pos.X = Main.screenWidth - textSize.X;
                                if (pos.Y + textSize.Y > Main.screenHeight)
                                    pos.Y = Main.screenHeight - textSize.Y;
                                if (pos.X < 0)
                                    pos.X = 0;
                                if (pos.Y < 0)
                                    pos.Y = 0;

                                UIElements.UIPanel panel = new UI.UIElements.UIPanel();
                                panel.Info.TotalHitBox = new Rectangle((int)pos.X, (int)pos.Y, (int)textSize.X, (int)textSize.Y);
                                panel.Draw(sb);
                                sb.DrawString(font, MouseText, pos, Color.White);

                                MouseText = string.Empty;
                            }
                        }
                        return true;
                    },
                    InterfaceScaleType.UI)
                );
            }
        }
    }
}