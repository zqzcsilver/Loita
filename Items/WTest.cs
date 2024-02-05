using Loita.Components.LoitaComponents;
using Loita.Components.LoitaComponents.Spells;

using Microsoft.Xna.Framework;

using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader.IO;

namespace Loita.Items
{
    internal class WTest : WandBase
    {
        public override int SlotCount => 40;

        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.width = 40;
            Item.height = 20;
            Item.damage = 30;
            Item.useTime = 10;
            Item.useAnimation = 10;
            Item.shoot = ProjectileID.UnholyArrow;
            Item.shootSpeed = 10f;
            Item.useStyle = ItemUseStyleID.Shoot;
        }

        public override void SetDefaultInfusion()
        {
            base.SetDefaultInfusion();
            //var random = new Random();
            //var r = random.Next(4);
            //if (r == 0)
            //    InfusionSlot.ChangeComponent(0, new CDoubleSpell(Entity));
            //else if (r == 1)
            //    InfusionSlot.ChangeComponent(0, new CTestSpell(Entity));
            //else if (r == 2)
            //    InfusionSlot.ChangeComponent(0, new CLightPrefix(Entity));
            //else if (r == 3)
            //    InfusionSlot.ChangeComponent(0, new CFirePrefix(Entity));
            InfusionSlot.ChangeComponent(0, new SArrow(Entity));
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            SpellInfo spellInfo = new SpellInfo();
            spellInfo.ProjectileSource = source;
            spellInfo.Damage = damage;
            spellInfo.Owner = Main.myPlayer;
            spellInfo.Velocity = velocity;
            spellInfo.Position = position;
            spellInfo.KnockbBack = knockback;
            InfusionSlot.Apply(spellInfo);
            return false;
        }

        public override void LoadData(TagCompound tag)
        {
            base.LoadData(tag);
        }
    }
}