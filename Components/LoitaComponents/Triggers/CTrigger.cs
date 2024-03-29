﻿using System;
using System.Collections.Generic;

using Terraria.Localization;

namespace Loita.Components.LoitaComponents.Triggers
{
    /// <summary>
    /// 触发器基类
    /// </summary>
    internal abstract class CTrigger : LoitaComponent
    {
        public override List<LoitaComponent> ActivableSpace => _activableSpace;
        private List<LoitaComponent> _activableSpace = new List<LoitaComponent>();

        public CInfusionSlot InfusionSlot => Entity.GetComponent<CInfusionSlot>();

        public abstract int SlotCount
        {
            get;
        }

        public override string Name => Language.GetTextValue($"Mods.Loita.Infusions.Triggers.{GetType().Name}.Name");
        public override string Description => Language.GetTextValue($"Mods.Loita.Infusions.Triggers.{GetType().Name}.Description");

        public CTrigger(IEntity entity) : base(entity)
        {
        }

        public override List<Type> GetDependComponents()
        {
            return new List<Type> { typeof(CInfusionSlot) };
        }

        public override void Apply(SpellInfo info)
        {
            info.Triggers.Add(this);
        }

        public override void InitActivableSpace(ref int index)
        {
            base.InitActivableSpace(ref index);
            ActivableSpace.Clear();
            index++;

            var activableSpace = InfusionSlot.ActivableSpace;
            while (index < activableSpace.Count)
            {
                var comp = activableSpace[index];
                if (comp == null)
                {
                    index++;
                    continue;
                }
                ActivableSpace.Add(comp);
                comp.Parent = this;
                comp.InitActivableSpace(ref index);
                if (ActivableSpace.Count == SlotCount)
                    return;
            }
        }
    }
}