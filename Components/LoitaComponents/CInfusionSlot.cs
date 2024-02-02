using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Loita.Components.LoitaComponents
{
    /// <summary>
    /// 组件槽
    /// </summary>
    internal class CInfusionSlot : LoitaComponent
    {
        public override List<LoitaComponent> ActivableSpace => _activableSpace;
        private List<LoitaComponent> _activableSpace = new List<LoitaComponent>();

        public override LoitaComponent Parent
        {
            get => null;
            set { }
        }

        /// <summary>
        /// 槽位数量
        /// </summary>
        public int SlotSize => _slotSize;

        private int _slotSize;

        public CInfusionSlot(IEntity entity) : this(entity, 20)
        {
        }

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
            //往实体注册 触发 与 初始化子组件 的钩子
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

        public override IComponent Clone(IEntity cloneEntity)
        {
            var op = (CInfusionSlot)base.Clone(cloneEntity);
            for (int i = 0; i < _activableSpace.Count; i++)
                op.ChangeComponent(i, (LoitaComponent)_activableSpace[i]?.Clone(cloneEntity));
            return op;
        }

        public override IComponent PrimitiveClone(IEntity cloneEntity)
        {
            return new CInfusionSlot(cloneEntity, SlotSize);
        }

        /// <summary>
        /// 更改组件槽内的组件
        /// </summary>
        /// <param name="index">索引</param>
        /// <param name="component">组件实例</param>
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
            if (_activableSpace[index] != null)
                _activableSpace[index].Entity = Entity;
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
            _activableSpace.Clear();
            int i;
            for (i = 0; i < _slotSize; i++)
            {
                _activableSpace.Add(null);
            }
            int count = br.ReadInt32();
            for (i = 0; i < count; i++)
            {
                int index = br.ReadInt32();
                var comp = (LoitaComponent)Activator.CreateInstance(Type.GetType(br.ReadString()), Entity);
                comp.ReadOnBinary(br);
                ChangeComponent(index, comp);
            }
            i = 0;
            InitActivableSpace(ref i);
        }
    }
}