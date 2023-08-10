using System;
using System.Collections.Generic;
using System.IO;

namespace Loita.Components.LoitaComponents
{
    internal class CInfusionSlot : LoitaComponent
    {
        public override List<LoitaComponent> ActivableSpace => _activableSpace;
        private List<LoitaComponent> _activableSpace = new List<LoitaComponent>();

        public override LoitaComponent Parent
        {
            get => null;
            set { }
        }

        public int SlotSize => _slotSize;
        private int _slotSize;

        public CInfusionSlot(IEntity entity, int slotSize) : base(entity)
        {
            if (slotSize < 0)
                throw new ArgumentException();
            _slotSize = slotSize;
            for (int i = 0; i < slotSize; i++)
            {
                _activableSpace.Add(null);
            }
        }

        public int GetLoitaComponentCount()
        {
            int count = 0;
            for (int i = 0; i < SlotSize; i++)
            {
                var comp = _activableSpace[i];
                if (comp == null)
                    continue;
                count++;
            }
            return count;
        }

        public override void Load()
        {
            Entity.RegisterHook(this, "Apply", Apply);
            Entity.RegisterHook(this, "InitActivableSpace", InitActivableSpace);
        }

        public override void Apply(SpellInfo info)
        {
            foreach (var comp in _activableSpace)
            {
                if (comp != null && (comp.Parent == this || comp.Parent == null))
                {
                    comp.Apply(info.Clone());
                }
            }
        }

        public override void InitActivableSpace(ref int index)
        {
            while (index < _activableSpace.Count)
            {
                var comp = _activableSpace[index];
                if (comp == null)
                {
                    index++;
                    continue;
                }
                comp.InitActivableSpace(ref index);
                comp.Parent = this;
            }
        }

        public void ChangeComponent(int index, LoitaComponent component)
        {
            if (index < 0 || index >= SlotSize)
                return;
            var ocomp = _activableSpace[index];
            if (ocomp == component)
                return;
            var ocomps = _activableSpace.FindAll(c => c == ocomp);
            var comp = _activableSpace.Find(c => c == component);
            if (ocomps.Count == 1)
                Entity.RemoveComponent(ocomp);
            if (comp == null && component != null)
                Entity.AddComponent(component);
            _activableSpace[index] = component;
        }

        public override void WriteToBinary(BinaryWriter bw)
        {
            bw.Write(SlotSize);
            bw.Write(GetLoitaComponentCount());
            for (int i = 0; i < SlotSize; i++)
            {
                var comp = _activableSpace[i];
                if (comp == null)
                    continue;
                bw.Write(i);
                bw.Write(comp.GetType().FullName);
                comp.WriteToBinary(bw);
            }
        }

        public override void ReadOnBinary(BinaryReader br)
        {
            _slotSize = br.ReadInt32();
            int count = GetLoitaComponentCount();
            for (int i = 0; i < count; i++)
            {
                int index = br.ReadInt32();
                var comp = (LoitaComponent)Activator.CreateInstance(Type.GetType(br.ReadString()), Entity);
                comp.ReadOnBinary(br);
                ChangeComponent(index, comp);
            }
        }
    }
}