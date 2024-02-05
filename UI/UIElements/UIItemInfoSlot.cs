using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using ReLogic.Content;

using Terraria;
using Terraria.GameContent;

namespace Loita.UI.UIElements
{
    internal class UIItemInfoSlot : UIPanel
    {
        public bool IsLightUp;
        private int _type;
        private float _scale;

        public UIItemInfoSlot(int type, float scale = 1f, bool isLightUp = false)
        {
            IsLightUp = isLightUp;
            _type = type;
            _scale = scale;
            Info.Width.SetValue(50f, 0f);
            Info.Height.SetValue(50f, 0f);
            CanDrag = false;
        }

        public int GetItemType() => _type;

        protected override void DrawSelf(SpriteBatch sb)
        {
            base.DrawSelf(sb);
            try
            {
                if (TextureAssets.Item[_type].State == AssetState.NotLoaded)
                    Main.Assets.Request<Texture2D>(TextureAssets.Item[_type].Name);

                var DrawRectangle = Info.TotalHitBox;
                var frame = Main.itemAnimations[_type] != null ? Main.itemAnimations[_type].GetFrame(TextureAssets.Item[_type].Value) : Item.GetDrawHitbox(_type, null);
                //绘制物品贴图
                sb.Draw(TextureAssets.Item[_type].Value, new Vector2(DrawRectangle.X + DrawRectangle.Width / 2,
                    DrawRectangle.Y + DrawRectangle.Height / 2) - (new Vector2(frame.Width, frame.Height) / 2f * _scale),
                    new Rectangle?(frame), Color.White * (IsLightUp ? 1f : 0.4f), 0f, Vector2.Zero, _scale, 0, 0);
            }
            catch
            {
            }
        }
    }
}