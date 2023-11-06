using Loita.Components.LoitaComponents;
using Loita.ModPlayers;
using Loita.Utils;

using Microsoft.Xna.Framework;

using System;
using System.Linq;

using Terraria;
using Terraria.DataStructures;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Loita.Items
{
    internal class ComponentAcquirer : ModItem
    {
        private Type _componentType = null;

        public Type ComponentType
        {
            get => _componentType;
            set
            {
                _componentType = value;
                var instance = InstanceManager<LoitaComponent>.GetTypeInstance(value);
                _texturePath = instance?.TexturePath ?? "Loita/Images/CBlock";
                _name = _componentType?.Name ?? "Component Acquirer";
                _displayName = Language.GetText(instance?.Name ?? "Component Acquirer");
            }
        }

        private string _name = "Component Acquirer";
        public override string Name => _name;

        private string _texturePath = "Loita/Images/CBlock";
        public override string Texture => _texturePath;
        private LocalizedText _displayName = null;
        public override LocalizedText DisplayName => _displayName ?? base.DisplayName;

        public override bool IsLoadingEnabled(Mod mod)
        {
            return ComponentType != null;
        }

        public override void UpdateInventory(Player player)
        {
            base.UpdateInventory(player);
            if (ComponentType != null && player.TryGetModPlayer<MPlayer>(out var modPlayer))
            {
                modPlayer.GainComponents(ComponentType, Item.stack);
                CombatText.NewText(player.Hitbox, Color.White, $"Gain {DisplayName.Value}({Item.stack})");
                Item.SetDefaults();
                return;
            }
            CombatText.NewText(player.Hitbox, Color.White, $"Gain {DisplayName.Value}({Item.stack})");
            Item.SetDefaults();
        }

        public override bool CanPickup(Player player)
        {
            return true;
        }

        public override bool OnPickup(Player player)
        {
            if (ComponentType != null && player.TryGetModPlayer<MPlayer>(out var modPlayer))
            {
                modPlayer.GainComponents(ComponentType, Item.stack);
                CombatText.NewText(player.Hitbox, Color.White, $"Gain {DisplayName.Value}({Item.stack})");
                Item.SetDefaults();
            }
            return false;
        }

        public override void OnCreated(ItemCreationContext context)
        {
            base.OnCreated(context);
        }

        public static int GetItemID<T>() where T : LoitaComponent
        {
            return Loita.Instance.GetContent<ComponentAcquirer>().ToList().Find(c => c.ComponentType == typeof(T)).Type;
        }

        public override ModItem Clone(Item newEntity)
        {
            var item = (ComponentAcquirer)base.Clone(newEntity);
            item.ComponentType = ComponentType;
            return item;
        }

        public override ModItem NewInstance(Item entity)
        {
            var item = (ComponentAcquirer)base.NewInstance(entity);
            item.ComponentType = ComponentType;
            return item;
        }
    }
}