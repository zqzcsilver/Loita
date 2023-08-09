using System;
using System.Collections.Generic;

namespace Loita.Components.LoitaComponents.Spells
{
    internal abstract class CSpell : LoitaComponent
    {
        public CInfusionSlot InfusionSlot => Entity.GetComponent<CInfusionSlot>();

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