using Microsoft.Xna.Framework;

using Terraria;

namespace Loita.Components.ProjectileComponents
{
    internal class LightComponent : LogicComponentBase
    {
        public LightComponent(IEntity entity) : base(entity)
        {
        }

        public override void Load()
        {
            RegisterHook(HookType.AI, AI);
            RegisterHook(HookType.Okay, new OkayDelegate(Okay));
        }

        public override void Okay(Projectile projectile, ref Color color)
        {
            base.Okay(projectile, ref color);
        }

        public override void AI(Projectile projectile)
        {
            base.AI(projectile);
            Lighting.AddLight(projectile.position, 1f, 1f, 1f);
        }
    }
}