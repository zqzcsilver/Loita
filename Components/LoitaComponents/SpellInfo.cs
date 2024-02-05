using Loita.Components.LoitaComponents.Prefixes;
using Loita.Components.LoitaComponents.Triggers;
using Loita.Globals;

using Microsoft.Xna.Framework;

using System.Collections.Generic;

using Terraria;
using Terraria.DataStructures;

namespace Loita.Components.LoitaComponents
{
    /// <summary>
    /// 发射时的委托类型
    /// </summary>
    /// <param name="projectile">发射出去的弹幕</param>
    /// <param name="entity">实体</param>
    internal delegate void ShootEvent(Projectile projectile, IEntity entity);

    /// <summary>
    /// 法术信息
    /// </summary>
    internal struct SpellInfo
    {
        /// <summary>
        /// 发射弹幕后的事件
        /// </summary>
        public event ShootEvent PostShoot;

        /// <summary>
        /// 弹幕发射位置
        /// </summary>
        public Vector2 Position;

        /// <summary>
        /// 弹幕发射速度
        /// </summary>
        public Vector2 Velocity;

        /// <summary>
        /// 附魔
        /// </summary>
        public List<CPrefix> Prefixes = new List<CPrefix>();

        /// <summary>
        /// 触发器
        /// </summary>
        public List<CTrigger> Triggers = new List<CTrigger>();

        /// <summary>
        /// 弹幕持有者
        /// </summary>
        public int Owner = -1;

        /// <summary>
        /// 弹幕伤害
        /// </summary>
        public int Damage;

        /// <summary>
        /// 弹幕击退
        /// </summary>
        public float KnockbBack;

        /// <summary>
        /// 弹幕AI的预设参数项
        /// </summary>
        public float[] AI = new float[3];

        /// <summary>
        /// 弹幕的发射来源
        /// </summary>
        public IEntitySource ProjectileSource;

        /// <summary>
        /// 调用来源
        /// </summary>
        public string CallSource;

        public SpellInfo()
        {
        }

        /// <summary>
        /// 触发发射后事件
        /// </summary>
        /// <param name="projectile"></param>
        public void Shoot(Projectile projectile) => PostShoot?.Invoke(projectile, projectile.GetGlobalProjectile<GProjectile>());

        /// <summary>
        /// 从物品构建法术信息
        /// </summary>
        /// <param name="item">物品实例</param>
        /// <returns></returns>
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

        /// <summary>
        /// 从弹幕构建法术信息
        /// </summary>
        /// <param name="projectile">弹幕实例</param>
        /// <returns></returns>
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

        /// <summary>
        /// 从玩家构建法术信息
        /// </summary>
        /// <param name="player">玩家实例</param>
        /// <returns></returns>
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

        /// <summary>
        /// 克隆法术信息
        /// </summary>
        /// <returns>被克隆的法术信息</returns>
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

        /// <summary>
        /// 根据法术信息发射弹幕
        /// </summary>
        /// <param name="type">弹幕ID</param>
        /// <returns></returns>
        public Projectile NewProjectile(int type)
        {
            return Projectile.NewProjectileDirect(ProjectileSource, Position, Velocity, type,
                Damage, KnockbBack, Owner, AI[0], AI[1], AI[2]);
        }
    }
}