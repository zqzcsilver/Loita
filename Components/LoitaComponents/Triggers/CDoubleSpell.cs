using Loita.QuickAssetReference;
using Loita.Utils;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System;

namespace Loita.Components.LoitaComponents.Triggers
{
    internal class CDoubleSpell : CTrigger
    {
        public override Texture2D Texture => ModAssets_Texture2D.Components.LoitaComponents.Triggers.CDoubleSpellImmediateAsset.Value;

        public override string Name => "双重释放";

        public override string Description => "能够以夹角为10°的角度(调用链上每有一个双重释放法术则角度/2)同时释放两个法术";

        public CDoubleSpell(IEntity entity) : base(entity)
        {
        }

        public override void Apply(SpellInfo info)
        {
            float mul = (float)Math.Pow(2, info.Triggers.FindAll(t => t is CDoubleSpell).Count);
            base.Apply(info);
            var ovelRotation = info.Velocity.ToRotation() - MathHelper.ToRadians(10f / mul);
            var length = info.Velocity.Length();
            info.Velocity = Vector2Expand.GetVector2ByRotation(ovelRotation, length, Vector2.Zero);
            foreach (var lc in ActivableSpace)
            {
                var ic = info.Clone();
                lc.Apply(ic);
                ovelRotation += MathHelper.ToRadians(20f / mul);
                info.Velocity = Vector2Expand.GetVector2ByRotation(ovelRotation, length, Vector2.Zero);
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