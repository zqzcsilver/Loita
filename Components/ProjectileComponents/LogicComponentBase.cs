using System;

using Terraria;

namespace Loita.Components.ProjectileComponents
{
    internal abstract class LogicComponentBase : ComponentBase
    {
        public enum HookType
        {
            PreAI,
            AI,
            PostAI,
            OnHitNPC,
            Okay
        }

        public LogicComponentBase(IEntity entity) : base(entity)
        {
        }

        public void RegisterHook(HookType hookType, Delegate hook)
        {
            Entity?.RegisterHook(this, hookType.ToString(), hook);
        }

        public virtual void PreAI(Projectile projectile)
        {
        }

        public virtual void AI(Projectile projectile)
        {
        }

        public virtual void PostAI(Projectile projectile)
        {
        }

        public virtual void OnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone)
        {
        }
    }
}