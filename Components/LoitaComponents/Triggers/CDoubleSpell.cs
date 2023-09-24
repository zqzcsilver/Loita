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

        public override int SlotCount => 2;

        public CDoubleSpell(IEntity entity) : base(entity)
        {
        }

        public override void Apply(SpellInfo info)
        {
            base.Apply(info);
            float mul = (float)Math.Pow(2, info.Triggers.FindAll(t => t is CDoubleSpell).Count);
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
    }
}