using Microsoft.Xna.Framework;

using Terraria;

namespace Loita.Components.LoitaComponents.Triggers
{
    /// <summary>
    /// Quadruple Spell Random Position By Mouse
    /// </summary>
    internal class TQSRPBM(IEntity entity) : CTrigger(entity)
    {
        public override int SlotCount => 4;

        public override void Apply(SpellInfo info)
        {
            base.Apply(info);
            var length = info.Velocity.Length();
            info.Position = Main.MouseWorld;
            foreach (var lc in ActivableSpace)
            {
                var ic = info.Clone();
                ic.Position += new Vector2(240f * (Main.rand.NextFloat() - 0.5f), 240f * (Main.rand.NextFloat() - 0.5f));
                ic.Velocity = Utils.Vector2Expand.GetVector2ByRotation((info.Position - ic.Position).ToRotation(), length, Vector2.Zero);
                lc.Apply(ic);
            }
        }
    }
}