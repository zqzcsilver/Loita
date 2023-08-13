using Loita.Components.LoitaComponents;
using Loita.Globals;

using Terraria.ModLoader;

namespace Loita.Items
{
    internal abstract class WandBase : ModItem
    {
        public new IEntity Entity => Item.GetGlobalItem<GItem>();
        public CInfusionSlot InfusionSlot => Entity.GetComponent<CInfusionSlot>();
        public virtual int SlotCount => 1;

        public override void SetDefaults()
        {
            base.SetDefaults();
            Entity.AddComponent<CInfusionSlot>(Entity, SlotCount);
            SetDefaultInfusion();
            int i = 0;
            InfusionSlot.InitActivableSpace(ref i);
        }

        public virtual void SetDefaultInfusion()
        {
        }
    }
}