namespace Loita.Components.LoitaComponents.Prefixes
{
    internal class PUnlimitedPenetration : CPrefix
    {
        public PUnlimitedPenetration(IEntity entity) : base(entity)
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