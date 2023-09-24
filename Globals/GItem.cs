using Loita.Components;

using System;
using System.Collections.Generic;
using System.IO;

using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace Loita.Globals
{
    internal class GItem : GlobalItem, IEntity
    {
        public override bool InstancePerEntity => true;
        public object Source { get => _source; set => _source = value; }
        private object _source;

        public Dictionary<string, List<IComponent>> CallOrder => _callOrder;

        private Dictionary<string, List<IComponent>> _callOrder = new Dictionary<string, List<IComponent>>();
        public Dictionary<string, Dictionary<IComponent, Delegate>> Hooks => _hooks;
        private Dictionary<string, Dictionary<IComponent, Delegate>> _hooks = new Dictionary<string, Dictionary<IComponent, Delegate>>();
        public IEntity Entity => this;

        public IEntity Clone(object source)
        {
            return (IEntity)MemberwiseClone();
        }

        public IEntity PrimitiveClone(object source)
        {
            return new GProjectile();
        }

        public override void SaveData(Item item, TagCompound tag)
        {
            using MemoryStream stream = new MemoryStream();
            using BinaryWriter writer = new BinaryWriter(stream);
            var components = ((IEntity)this).GetComponents();
            writer.Write(components.Count);
            foreach (var component in components)
            {
                writer.Write(component.GetType().FullName);
                if (component is IBinarySupport binary)
                    binary.WriteToBinary(writer);
            }
            tag.Add("Loita:Components", stream.GetBuffer());
            base.SaveData(item, tag);
        }

        public override void LoadData(Item item, TagCompound tag)
        {
            base.LoadData(item, tag);
            if (tag.TryGet<byte[]>("Loita:Components", out var bytes))
            {
                using MemoryStream stream = new MemoryStream(bytes);
                using BinaryReader reader = new BinaryReader(stream);
                int count = reader.ReadInt32();
                for (int i = 0; i < count; i++)
                {
                    var type = Type.GetType(reader.ReadString());
                    if (!Entity.HasComponent(type))
                    {
                        var component = (IComponent)Activator.CreateInstance(type, this);
                        Entity.AddComponent(component);
                    }
                    else
                    {
                        if (Entity.GetComponent(type) is IBinarySupport binary)
                            binary.ReadOnBinary(reader);
                    }
                }
            }
        }

        public IEntity TotallyClone(object source)
        {
            return Clone(source);
        }

        public Item OriginalItem = null;

        public override GlobalItem Clone(Item from, Item to)
        {
            if (to.TryGetGlobalItem(out GItem gitem))
                gitem.OriginalItem = from;
            var op = base.Clone(from, to);
            if (op is GItem ngitem && ngitem != this)
            {
                ngitem.Source = this;
                ngitem._callOrder = new Dictionary<string, List<IComponent>>();
                ngitem._hooks = new Dictionary<string, Dictionary<IComponent, Delegate>>();
                var components = ((IEntity)this).GetComponents();
                foreach (var component in components)
                {
                    ((IEntity)ngitem).AddComponent(component.Clone(ngitem));
                }
            }
            return op;
        }
    }
}