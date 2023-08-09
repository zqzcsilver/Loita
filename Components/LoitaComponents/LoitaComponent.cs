using Loita.QuickAssetReference;

using Microsoft.Xna.Framework.Graphics;

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

        public virtual void Apply(SpellInfo info)
        {
        }

        public override void Load()
        {
            Entity.RegisterHook(this, "Apply", Apply);
        }

        public virtual void InitActivableSpace(ref int index)
        {
            Index = index;
        }

        public virtual void WriteToBinary(BinaryWriter bw)
        {
        }

        public virtual void ReadOnBinary(BinaryReader br)
        {
        }
    }
}