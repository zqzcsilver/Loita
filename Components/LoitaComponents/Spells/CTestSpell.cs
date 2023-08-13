using Loita.QuickAssetReference;

using Microsoft.Xna.Framework.Graphics;

namespace Loita.Components.LoitaComponents.Spells
{
    internal class CTestSpell : CSpell
    {
        public override Texture2D Texture => ModAssets_Texture2D.Components.LoitaComponents.Spells.CTestSpellImmediateAsset.Value;

        public override string Name => "箭矢";

        public override string Description => "释放一支箭矢造成伤害";

        public CTestSpell(IEntity entity) : base(entity)
        {
        }

        public override void Apply(SpellInfo info)
        {
            base.Apply(info);
            var proj = info.NewProjectile(1);
            info.Shoot(proj);
        }
    }
}