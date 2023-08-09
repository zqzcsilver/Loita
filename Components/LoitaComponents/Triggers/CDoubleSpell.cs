using Loita.QuickAssetReference;
using Loita.Utils;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Loita.Components.LoitaComponents.Triggers
{
    internal class CDoubleSpell : CTrigger
    {
        public override Texture2D Texture => ModAssets_Texture2D.Components.LoitaComponents.Triggers.CDoubleSpellImmediateAsset.Value;
        public CDoubleSpell(IEntity entity) : base(entity)
        {
        }

        public override void Apply(SpellInfo info)
        {
            base.Apply(info);
            var ovelRotation = info.Velocity.ToRotation() - MathHelper.ToRadians(10f);
            info.Velocity = Vector2Expand.GetVector2ByRotation(ovelRotation, 10f, Vector2.Zero);
            foreach (var lc in ActivableSpace)
            {
                var ic = info.Clone();
                lc.Apply(ic);
                ovelRotation += MathHelper.ToRadians(20f);
                info.Velocity = Vector2Expand.GetVector2ByRotation(ovelRotation, 10f, Vector2.Zero);
            }
        }

        public override void InitActivableSpace(ref int index)
        {
            base.InitActivableSpace(ref index);

            var activableSpace = InfusionSlot.ActivableSpace;
            while (index < activableSpace.Count)
            {
                var comp = activableSpace[index];
                if (comp == null)
                {
                    index++;
                    continue;
                }
                ActivableSpace.Add(comp);
                comp.Parent = this;
                comp.InitActivableSpace(ref index);
                if (ActivableSpace.Count == 2)
                    return;
            }
        }
    }
}