using Loita.Components;

using System;
using System.Collections.Generic;

using Terraria;
using Terraria.ModLoader;

namespace Loita.Globals
{
    internal class GProjectile : GlobalProjectile, IEntity
    {
        public override bool InstancePerEntity => true;
        public object Source { get => _source; set => _source = value; }
        private object _source;

        public Dictionary<string, List<IComponent>> CallOrder => _callOrder;

        private Dictionary<string, List<IComponent>> _callOrder = new Dictionary<string, List<IComponent>>();
        public Dictionary<string, Dictionary<IComponent, Delegate>> Hooks => _hooks;
        private Dictionary<string, Dictionary<IComponent, Delegate>> _hooks = new Dictionary<string, Dictionary<IComponent, Delegate>>();
        public IEntity Entity => this;

        public override bool PreAI(Projectile projectile)
        {
            bool op = true;
            var funcName = nameof(PreAI);
            var ops = Entity.Call(funcName, projectile);
            if (ops != null)
                Array.ForEach(ops, o => op &= (bool)o);
            return op;
        }

        public override void AI(Projectile projectile)
        {
            base.AI(projectile);
            var funcName = nameof(AI);
            Entity.Call(funcName, projectile);
        }

        public override void PostAI(Projectile projectile)
        {
            base.PostAI(projectile);
            var funcName = nameof(PostAI);
            Entity.Call(funcName, projectile);
        }

        public override void OnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone)
        {
            base.OnHitNPC(projectile, target, hit, damageDone);
            var funcName = nameof(OnHitNPC);
            Entity.Call(funcName, projectile, target, hit, damageDone);
        }

        public IEntity Clone(object source)
        {
            return (IEntity)MemberwiseClone();
        }

        public IEntity PrimitiveClone(object source)
        {
            return new GProjectile();
        }

        public IEntity TotallyClone(object source)
        {
            return Clone(source);
        }
    }
}