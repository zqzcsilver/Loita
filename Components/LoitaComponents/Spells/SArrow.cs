namespace Loita.Components.LoitaComponents.Spells
{
    internal class SArrow : CSpell
    {
        public SArrow(IEntity entity) : base(entity)
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