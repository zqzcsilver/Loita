using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;

namespace Loita.Items
{
    internal class ExampleTurret : ModProjectile
    {
        //因为我没贴图，我也不想用啥贴图，所以就随便选了个原版的贴图塞给TML做安检
        public override string Texture => "Terraria/Images/Projectile_724";

        public override void SetDefaults()
        {
            base.SetDefaults();
            Projectile.width = 64;
            Projectile.height = 114;
            Projectile.timeLeft = 10;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
        }

        //目标位置
        private Vector2 targetPos = Vector2.Zero;

        public override void AI()
        {
            base.AI();
            //这里可以插入寻敌AI，为了便于测试，我直接将目标位置设置成了鼠标
            targetPos = Main.MouseWorld;
            //让弹幕速度衰减，避免一直移动
            Projectile.velocity /= 4f;
            //使弹幕不随时间消失
            Projectile.timeLeft = 10;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            SpriteBatch sb = Main.spriteBatch;

            //获取弹幕屏幕坐标
            var pos = Projectile.position - Main.screenPosition;
            //绘制底座
            sb.Draw(TextureAssets.MagicPixel.Value,
                new Rectangle((int)pos.X,
                (int)(pos.Y + Projectile.height - 20), Projectile.width, 20), Color.White);
            sb.Draw(TextureAssets.MagicPixel.Value,
                new Rectangle((int)(pos.X + Projectile.width / 2f - 6),
                (int)(pos.Y + 20), 12, Projectile.height - 20), Color.White);

            //相对向量
            var vec = targetPos - Projectile.Center;

            //加载id为157的物品的贴图
            Main.instance.LoadItem(157);
            var texture = TextureAssets.Item[157].Value;
            //绘制炮台
            sb.Draw(texture,
                pos + Projectile.Size / 2f, null, Color.White,
                vec.ToRotation(), texture.Size() / 2f, 2f,
                SpriteEffects.None, 0f);

            //这里是禁用了原来的弹幕绘制
            return false;
        }
    }
}