using Loita.Components.LoitaComponents.Prefixes;
using Loita.Globals;

using Microsoft.Xna.Framework;

using System.Collections.Generic;

using Terraria;
using Terraria.DataStructures;

namespace Loita.Components.LoitaComponents
{
    internal delegate void ShootEvent(Projectile projectile, IEntity entity);

    internal struct SpellInfo
    {
        public event ShootEvent PostShoot;

        public Vector2 Position;
        public Vector2 Velocity;
        public List<CPrefix> Prefixes = new List<CPrefix>();
        public List<CTrigger> Triggers = new List<CTrigger>();
        public int Owner = -1;
        public int Damage;
        public float KnockbBack;
        public float[] AI = new float[3];
        public IEntitySource ProjectileSource;
        public string CallSource;

        public SpellInfo()
        {
        }

        public void Shoot(Projectile projectile) => PostShoot?.Invoke(projectile, projectile.GetGlobalProjectile<GProjectile>());

        public static SpellInfo FromItem(Item item)
        {
            SpellInfo info = new SpellInfo();
            info.Owner = Main.LocalPlayer.whoAmI;
            info.Position = Main.LocalPlayer.Center;
            info.KnockbBack = item.knockBack;
            info.Damage = item.damage;
            var unit = Main.MouseScreen - Main.LocalPlayer.Center;
            unit.SafeNormalize(Vector2.Zero);
            info.Velocity = unit * item.shootSpeed;
            return info;
        }

        public static SpellInfo FromProjectile(Projectile projectile)
        {
            SpellInfo info = new SpellInfo();
            info.Owner = projectile.owner;
            info.Position = projectile.Center;
            info.KnockbBack = projectile.knockBack;
            info.Damage = projectile.damage;
            info.Velocity = projectile.velocity;
            return info;
        }

        public static SpellInfo FromPlayer(Player player)
        {
            SpellInfo info = new SpellInfo();
            info.Owner = player.whoAmI;
            info.Position = player.Center;
            var unit = Main.MouseScreen - player.Center;
            unit.SafeNormalize(Vector2.Zero);
            info.Velocity = unit;
            return info;
        }

        public SpellInfo Clone()
        {
            SpellInfo info = new SpellInfo();
            info.Position = Position;
            info.Velocity = Velocity;
            info.Prefixes = new List<CPrefix>(Prefixes);
            info.Triggers = new List<CTrigger>(Triggers);
            info.Owner = Owner;
            info.Damage = Damage;
            info.KnockbBack = KnockbBack;
            info.AI = (float[])AI.Clone();
            info.ProjectileSource = ProjectileSource;
            info.CallSource = CallSource;
            info.PostShoot = PostShoot;
            return info;
        }

        public Projectile NewProjectile(int type)
        {
            return Projectile.NewProjectileDirect(ProjectileSource, Position, Velocity, type,
                Damage, KnockbBack, Owner, AI[0], AI[1], AI[2]);
        }
    }
}