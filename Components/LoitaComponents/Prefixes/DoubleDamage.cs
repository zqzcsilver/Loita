namespace Loita.Components.LoitaComponents.Prefixes
{
    internal class DoubleDamage : CPrefix
    {
        public override string Name => "双倍伤害";

        public override string Description => "使你的法术在发射时伤害翻倍";

        public DoubleDamage(IEntity entity) : base(entity)
        {
        }

        public override void Apply(SpellInfo info)
        {
            info.Damage *= 2;
            base.Apply(info);
        }
    }
}