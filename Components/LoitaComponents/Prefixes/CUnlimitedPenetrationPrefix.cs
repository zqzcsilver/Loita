namespace Loita.Components.LoitaComponents.Prefixes
{
    internal class CUnlimitedPenetrationPrefix : CPrefix
    {
        public override string Name => "无限穿透";

        public override string Description => "使你的法术变为无限穿透";

        public CUnlimitedPenetrationPrefix(IEntity entity) : base(entity)
        {
        }

        public override void Apply(SpellInfo info)
        {
            info.PostShoot += Info_PostShoot;
            base.Apply(info);
        }

        private void Info_PostShoot(Terraria.Projectile projectile, IEntity entity)
        {
            projectile.penetrate = -1;
        }
    }
}
