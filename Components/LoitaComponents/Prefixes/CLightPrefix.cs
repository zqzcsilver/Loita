using Loita.Components.ProjectileComponents;
using Loita.QuickAssetReference;

using Microsoft.Xna.Framework.Graphics;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loita.Components.LoitaComponents.Prefixes
{
    internal class CLightPrefix : CPrefix
    {
        public override string Name => "发光";

        public override string Description => "为魔法附加发光效果";

        public CLightPrefix(IEntity entity) : base(entity)
        {
        }

        public override void Apply(SpellInfo info)
        {
            info.PostShoot += Info_PostShoot;
            base.Apply(info);
        }

        private void Info_PostShoot(Terraria.Projectile projectile, IEntity entity)
        {
            entity.AddComponent<LightComponent>(entity);
        }
    }
}