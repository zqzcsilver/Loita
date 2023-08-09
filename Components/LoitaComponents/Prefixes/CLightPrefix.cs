﻿using Loita.Components.ProjectileComponents;
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
        public override Texture2D Texture => ModAssets_Texture2D.Components.LoitaComponents.Prefixes.CLightPrefixImmediateAsset.Value;
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