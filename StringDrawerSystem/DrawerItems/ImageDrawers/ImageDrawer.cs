using Loita.Utils.Expands;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using ReLogic.Content;

using System;
using System.Collections.Generic;

using Terraria.GameContent;
using Terraria.ModLoader;

namespace Loita.StringDrawerSystem.DrawerItems.ImageDrawers
{
    internal class ImageDrawer : DrawerItem
    {
        public Texture2D Texture = TextureAssets.MagicPixel.Value;
        public Vector2 Size = Vector2.Zero;
        public Vector2 Offset = Vector2.Zero;
        public Color Color = Color.White;
        public Rectangle SourceRectangle = Rectangle.Empty;
        public Vector2 Origin = Vector2.Zero;
        public float Rotation = 0f;
        public SpriteEffects Effects = SpriteEffects.None;
        public float LayerDepth = 0f;
        public Vector2 Scale = Vector2.One;

        public override string ToString()
        {
            return $"{Line} {Texture.Name}";
        }

        public override void Draw(SpriteBatch sb, GameTime gt)
        {
            var pos = Position + Offset;
            var size = Size * Scale;
            sb.Draw(Texture, new Rectangle((int)pos.X, (int)pos.Y, (int)size.X, (int)size.Y),
                SourceRectangle == Rectangle.Empty ? null : SourceRectangle,
                Color, Rotation, Origin, Effects, LayerDepth);
        }

        public override DrawerItem GetInstance(StringDrawer stringDrawer, string originalText, string name, StringParameters stringParameters)
        {
            ImageDrawer drawerItem = (ImageDrawer)Activator.CreateInstance(GetType());
            drawerItem.Init(stringDrawer, originalText, name, stringParameters);
            return drawerItem;
        }

        public override Vector2 GetSize()
        {
            return Size * Scale;
        }

        public override void Init(StringDrawer stringDrawer, string originalText, string name, StringParameters stringParameters)
        {
            base.Init(stringDrawer, originalText, name, stringParameters);
            if (!string.IsNullOrEmpty(stringParameters["Image"]))
            {
                Texture = ModContent.Request<Texture2D>(stringParameters["Image"], AssetRequestMode.ImmediateLoad).Value;
            }
            Color = stringParameters.GetColor("Color", Color.White);
            Scale = stringParameters.GetVector2("Scale", Vector2.One);
            Origin = stringParameters.GetVector2("Origin");
            Offset = stringParameters.GetVector2("Offset");
            Size = stringParameters.GetVector2("Size", Texture.GetSize());
            Rotation = stringParameters.GetFloat("Rotation");
            LayerDepth = stringParameters.GetFloat("LayerDepth", 1f);
            Effects = (SpriteEffects)stringParameters.GetInt("SpriteEffects");
            SourceRectangle = stringParameters.GetRectangle("SourceRectangle");
        }

        public override float WordWrap(ref int index, List<DrawerItem> drawerItems, ref int line, float width, float originWidth)
        {
            Line++;
            line++;
            return originWidth - GetSize().X;
        }
    }
}