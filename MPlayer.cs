using Loita.Components;
using Loita.Components.LoitaComponents;
using Loita.Components.LoitaComponents.Prefixes;
using Loita.Components.LoitaComponents.Spells;
using Loita.Components.LoitaComponents.Triggers;
using Loita.Globals;

using System;
using System.Collections.Generic;
using System.IO;

using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace Loita
{
    internal class MPlayer : ModPlayer, IBinarySupport, IEntity
    {
        public object Source { get => _source; set => _source = value; }
        private object _source;

        public Dictionary<string, List<IComponent>> CallOrder => _callOrder;

        private Dictionary<string, List<IComponent>> _callOrder = new Dictionary<string, List<IComponent>>();
        public Dictionary<string, Dictionary<IComponent, Delegate>> Hooks => _hooks;
        private Dictionary<string, Dictionary<IComponent, Delegate>> _hooks = new Dictionary<string, Dictionary<IComponent, Delegate>>();
        public IEntity IAmEntity => this;

        public List<LoitaComponent> ComponentBackpack = new List<LoitaComponent>();
        public static MPlayer Instance => Main.LocalPlayer.GetModPlayer<MPlayer>();

        public override void OnEnterWorld()
        {
            //var random = new Random();
            //for (int i = 0; i < 100; i++)
            //{
            //    var r = random.Next(4);
            //    if (r == 0)
            //        GainComponent(new CDoubleSpell(null));
            //    else if (r == 1)
            //        GainComponent(new CTestSpell(null));
            //    else if (r == 2)
            //        GainComponent(new CLightPrefix(null));
            //    else if (r == 3)
            //        GainComponent(new CFirePrefix(null));
            //}
            for (int i = 0; i < 20; i++)
            {
                GainComponent(new CDoubleSpell(null));
                GainComponent(new CLightPrefix(null));
                GainComponent(new DoubleDamage(null));
                GainComponent(new CTestSpell(null));
            }
            base.OnEnterWorld();
        }

        public void GainComponent(LoitaComponent component)
        {
            if (component == null)
                return;
            if (ComponentBackpack.Count == 0)
            {
                ComponentBackpack.Add(component);
                return;
            }
            int endIndex = ComponentBackpack.Count - 1;
            int middle;
            int startIndex = 0;
            LoitaComponent comp;
            var originalName = component.Name;
            while (true)
            {
                middle = (startIndex + endIndex) / 2;
                comp = ComponentBackpack[middle];
                int i = originalName.CompareTo(comp.Name);
                if (i == 0)
                {
                    var ct = component.GetType();
                    var comt = comp.GetType();
                    i = ct.Name.CompareTo(comt.Name);
                    if (i == 0)
                    {
                        i = ct.FullName.CompareTo(comt.FullName);
                    }
                }
                if (i == 0)
                {
                    ComponentBackpack.Insert(middle, component);
                    return;
                }
                else if (i > 0)
                    startIndex = middle + 1;
                else
                    endIndex = middle - 1;
                if (startIndex > endIndex)
                {
                    ComponentBackpack.Insert(startIndex, component);
                    return;
                }
            }
        }

        public override void SaveData(TagCompound tag)
        {
            base.SaveData(tag);
            //tag.Add("Loita:ComponentBackpack", ComponentBackpack);
        }

        public override void LoadData(TagCompound tag)
        {
            //tag.GetByteArray("Loita:ComponentBackpack");
        }

        public void WriteToBinary(BinaryWriter bw)
        {
            bw.Write(ComponentBackpack.Count);
            foreach (var c in ComponentBackpack)
            {
                bw.Write(c.GetType().FullName);
                c.WriteToBinary(bw);
            }

            //var comps = IAmEntity.GetComponents();
            //bw.Write(comps.Count);
            //comps.ForEach(c =>
            //{
            //    bw.Write(c.GetType().FullName);
            //    if (c is IBinarySupport binarySupport)
            //    {
            //        binarySupport.WriteToBinary(bw);
            //    }
            //});
        }

        public void ReadOnBinary(BinaryReader br)
        {
            ComponentBackpack.Clear();
            int count = br.ReadInt32();
            for (int i = 0; i < count; i++)
            {
                var comp = (LoitaComponent)Activator.CreateInstance(Type.GetType(br.ReadString()), Entity);
                comp.ReadOnBinary(br);
                ComponentBackpack.Add(comp);
            }
        }

        public IEntity Clone(object source)
        {
            return (IEntity)MemberwiseClone();
        }

        public IEntity PrimitiveClone(object source)
        {
            return new GProjectile();
        }

        public IEntity TotallyClone(object source)
        {
            return Clone(source);
        }
    }
}