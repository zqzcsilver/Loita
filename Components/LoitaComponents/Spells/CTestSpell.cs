using Loita.QuickAssetReference;
using Microsoft.Xna.Framework.Graphics;

namespace Loita.Components.LoitaComponents.Spells
{
    internal class CTestSpell : CSpell
    {
        public override Texture2D Texture => ModAssets_Texture2D.Components.LoitaComponents.Spells.CTestSpellImmediateAsset.Value;
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