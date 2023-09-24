using Loita.QuickAssetReference;

using Microsoft.Xna.Framework.Graphics;

namespace Loita.Components.LoitaComponents.Prefixes
{
    internal class COnePenetrationPrefix : CPrefix
    {
        public override Texture2D Texture => ModAssets_Texture2D.Components.LoitaComponents.Prefixes.COnePenetrationPrefixImmediateAsset.Value;
        public override string Name => "初级穿透";

        public override string Description => "使你的法术在发射时穿透数量+1\n注：对无限穿透法术无效";

        public COnePenetrationPrefix(IEntity entity) : base(entity)
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