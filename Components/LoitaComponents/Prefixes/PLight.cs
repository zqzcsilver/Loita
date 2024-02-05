using Loita.Components.ProjectileComponents;

namespace Loita.Components.LoitaComponents.Prefixes
{
    internal class PLight : CPrefix
    {
        public PLight(IEntity entity) : base(entity)
        {
        }

        public override void Apply(SpellInfo info)
        {
            info.PostShoot += Info_PostShoot;
            base.Apply(info);
        }

        private void Info_PostShoot(Terraria.Projectile projectile, IEntity entity)
        {
            entity.AddComponent<CPLight>(entity);
        }
    }
}