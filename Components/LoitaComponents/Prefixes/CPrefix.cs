using System;
using System.Collections.Generic;

using Terraria.Localization;

namespace Loita.Components.LoitaComponents.Prefixes
{
    /// <summary>
    /// 附魔基类
    /// </summary>
    internal abstract class CPrefix : LoitaComponent
    {
        public override List<LoitaComponent> ActivableSpace => _activableSpace;
        private List<LoitaComponent> _activableSpace = new List<LoitaComponent>();

        public CInfusionSlot InfusionSlot => Entity.GetComponent<CInfusionSlot>();
        public override string Name => Language.GetTextValue($"Mods.Loita.Infusions.Prefixes.{GetType().Name}.Name");
        public override string Description => Language.GetTextValue($"Mods.Loita.Infusions.Prefixes.{GetType().Name}.Description");

        public CPrefix(IEntity entity) : base(entity)
        {
        }

        public override List<Type> GetDependComponents()
        {
            return new List<Type> { typeof(CInfusionSlot) };
        }

        public override void Apply(SpellInfo info)
        {
            info.Prefixes.Add(this);
            foreach (var lc in ActivableSpace)
            {
                lc.Apply(info.Clone());
            }
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
                return;
            }
        }
    }
}