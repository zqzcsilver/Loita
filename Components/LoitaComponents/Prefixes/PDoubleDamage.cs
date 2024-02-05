namespace Loita.Components.LoitaComponents.Prefixes
{
    internal class PDoubleDamage : CPrefix
    {
        public PDoubleDamage(IEntity entity) : base(entity)
        {
        }

        public override void Apply(SpellInfo info)
        {
            info.Damage *= 2;
            base.Apply(info);
        }
    }
}