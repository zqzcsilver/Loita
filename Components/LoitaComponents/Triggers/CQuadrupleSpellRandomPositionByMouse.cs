using Loita.QuickAssetReference;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;

namespace Loita.Components.LoitaComponents.Triggers
{
    internal class CQuadrupleSpellRandomPositionByMouse : CTrigger
    {
        public override Texture2D Texture => ModAssets_Texture2D.Components.LoitaComponents.Triggers.CQuadrupleSpellRandomPositionByMouseImmediateAsset.Value;

        public override string Name => "四重释放-鼠标随机";

        public override string Description => "以鼠标位置作为原点，随机取原点周围位置作为法术发射位置，并以原点方向作为法术发射方向。重复四次。";

        public override int SlotCount => 4;

        public CQuadrupleSpellRandomPositionByMouse(IEntity entity) : base(entity)
        {
        }

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