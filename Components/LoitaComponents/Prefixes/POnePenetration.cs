namespace Loita.Components.LoitaComponents.Prefixes
{
    internal class POnePenetration : CPrefix
    {
        public POnePenetration(IEntity entity) : base(entity)
        {
        }

        public override void Apply(SpellInfo info)
        {
            info.PostShoot += Info_PostShoot;
            base.Apply(info);
        }

        private void Info_PostShoot(Terraria.Projectile projectile, IEntity entity)
        {
            if (projectile.penetrate >= 0)
                projectile.penetrate++;
        }
    }
}