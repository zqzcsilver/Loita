using FontStashSharp;

using Loita.QuickAssetReference;
using Loita.Utils;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System;
using System.Collections.Generic;
using System.IO;

namespace Loita.Components.LoitaComponents
{
    internal abstract class LoitaComponent : ComponentBase, IBinarySupport
    {
        protected LoitaComponent(IEntity entity) : base(entity)
        {
        }

        public virtual List<LoitaComponent> ActivableSpace { get; }
        public virtual int Index { get; set; }
        public virtual LoitaComponent Parent { get; set; }

        public virtual Texture2D Texture => ModAssets_Texture2D.Images.CBlockImmediateAsset.Value;

        public virtual string Name => "Loita Component";

        public virtual string Description => "This is a Loita Component";

        public virtual bool CanExpanded => false;

        public virtual void Apply(SpellInfo info)
        {
        }

        public override void Load()
        {
        }

        public virtual void InitActivableSpace(ref int index)
        {
            Index = index;
        }

        public virtual void DrawTips(SpriteBatch sb, Vector2 startPos, Vector2 containerSize, out Vector2 size)
        {
            size = Vector2.Zero;
            var nameFont = Loita.DefaultFontSystem.GetFont(38f);
            var desFont = Loita.DefaultFontSystem.GetFont(24f);
            var name = StringUtil.GetWordWrapString1(Name, nameFont, containerSize.X);
            var description = StringUtil.GetWordWrapString1(Description, desFont, containerSize.X);
            var nameSize = nameFont.MeasureString(name);
            var desSize = desFont.MeasureString(description);

            if (nameSize.X >= containerSize.X || desSize.X >= containerSize.X)
                size.X = containerSize.X;
            else
                size.X = Math.Max(nameSize.X, desSize.X);
            size.Y = nameSize.Y + desSize.Y;

            sb.DrawString(nameFont, name, startPos, Color.White, null, 0f,
                default, 0f, 0f, 0f, TextStyle.None,
                FontSystemEffect.Stroked, 2);
            startPos.Y += nameSize.Y;
            sb.DrawString(desFont, description, startPos, Color.White, null, 0f,
                default, 0f, 0f, 0f, TextStyle.None,
                FontSystemEffect.Stroked, 2);
        }

        public virtual void WriteToBinary(BinaryWriter bw)
        {
        }

        public virtual void ReadOnBinary(BinaryReader br)
        {
        }
    }
}