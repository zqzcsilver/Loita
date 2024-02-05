using System;
using System.Collections.Generic;

using Terraria.Localization;

namespace Loita.Components.LoitaComponents.Spells
{
    /// <summary>
    /// 法术基类
    /// </summary>
    internal abstract class CSpell : LoitaComponent
    {
        public CInfusionSlot InfusionSlot => Entity.GetComponent<CInfusionSlot>();

        public override string Name => Language.GetTextValue($"Mods.Loita.Infusions.Spells.{GetType().Name}.Name");
        public override string Description => Language.GetTextValue($"Mods.Loita.Infusions.Spells.{GetType().Name}.Description");

        public CSpell(IEntity entity) : base(entity)
        {
        }

        public override List<Type> GetDependComponents()
        {
            return new List<Type> { typeof(CInfusionSlot) };
        }

        public override void InitActivableSpace(ref int index)
        {
            base.InitActivableSpace(ref index);
            index++;
        }
    }
}