using Loita.Utils;

using Microsoft.Xna.Framework;

using System;

namespace Loita.Components.LoitaComponents.Triggers
{
    internal class TDoubleSpell : CTrigger
    {
        public override int SlotCount => 2;

        public TDoubleSpell(IEntity entity) : base(entity)
        {
        }

        public override void Apply(SpellInfo info)
        {
            base.Apply(info);
            float mul = (float)Math.Pow(2, info.Triggers.FindAll(t => t is TDoubleSpell).Count);
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