using Loita.Components;
using Loita.Components.LoitaComponents;
using Loita.Components.LoitaComponents.Prefixes;
using Loita.Globals;
using Loita.Items;
using Loita.UI.UIContainers.InfusionBackpack;

using System;
using System.Collections.Generic;
using System.IO;

using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace Loita.ModPlayers
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
        private Dictionary<Type, int> _componentCounter = new Dictionary<Type, int>();

        public override void OnEnterWorld()
        {
            //LCRecipeItem recipeItem = new LCRecipeItem();
            //recipeItem.AddCondition(new LCCondition<CDoubleSpell>());
            //recipeItem.AddResult(new LCResult<CTestSpell>());
            //Loita.RecipeSystem.Register(recipeItem);

            //Loita.RecipeSystem.ForEach(r => r.Apply());
            Item.NewItem(Player.GetSource_FromThis(),
                Player.Center - new Microsoft.Xna.Framework.Vector2(200f, 0f),
                ComponentAcquirer.GetItemID<PDoubleDamage>(), 10);
            //var item = Player.QuickSpawnItemDirect(Player.GetSource_FromThis(), ModContent.ItemType<ComponentAcquirer>(), 10);
            base.OnEnterWorld();
        }

        public void GainComponent(LoitaComponent component)
        {
            if (component == null)
                return;

            if (InfusionBackpack.Instance.IsVisible)
                InfusionBackpack.Instance.ResetInfusions();

            var funcName = nameof(GainComponent);
            IAmEntity.Call(funcName, component);

            var type = component.GetType();

            if (ComponentBackpack.Count == 0)
            {
                ComponentBackpack.Add(component);
                if (_componentCounter.ContainsKey(type))
                    _componentCounter[type]++;
                else
                    _componentCounter.Add(type, 1);
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
                    if (_componentCounter.ContainsKey(type))
                        _componentCounter[type]++;
                    else
                        _componentCounter.Add(type, 1);
                    return;
                }
                else if (i > 0)
                    startIndex = middle + 1;
                else
                    endIndex = middle - 1;
                if (startIndex > endIndex)
                {
                    ComponentBackpack.Insert(startIndex, component);
                    if (_componentCounter.ContainsKey(type))
                        _componentCounter[type]++;
                    else
                        _componentCounter.Add(type, 1);
                    return;
                }
            }
        }

        public void GainComponents<T>(int count = 1) where T : LoitaComponent => GainComponents(typeof(T), count);

        public void GainComponents(Type t, int count = 1)
        {
            if (InfusionBackpack.Instance.IsVisible)
                InfusionBackpack.Instance.ResetInfusions();

            var component = (LoitaComponent)Activator.CreateInstance(t, new object[] { null });

            var funcName = nameof(GainComponents);
            IAmEntity.Call(funcName, t, count);

            if (ComponentBackpack.Count == 0)
            {
                do
                {
                    ComponentBackpack.Add(component);
                    count--;
                    if (_componentCounter.ContainsKey(t))
                        _componentCounter[t]++;
                    else
                        _componentCounter.Add(t, 1);
                    component = (LoitaComponent)Activator.CreateInstance(t, new object[] { null });
                }
                while (count > 0);
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
                    do
                    {
                        ComponentBackpack.Insert(middle, component);
                        count--;
                        if (_componentCounter.ContainsKey(t))
                            _componentCounter[t]++;
                        else
                            _componentCounter.Add(t, 1);
                        component = (LoitaComponent)Activator.CreateInstance(t, new object[] { null });
                    }
                    while (count > 0);
                    return;
                }
                else if (i > 0)
                    startIndex = middle + 1;
                else
                    endIndex = middle - 1;
                if (startIndex > endIndex)
                {
                    do
                    {
                        ComponentBackpack.Insert(middle, component);
                        count--;
                        if (_componentCounter.ContainsKey(t))
                            _componentCounter[t]++;
                        else
                            _componentCounter.Add(t, 1);
                        component = (LoitaComponent)Activator.CreateInstance(t, new object[] { null });
                    }
                    while (count > 0);
                    return;
                }
            }
        }

        public int ComponentCount<T>() where T : LoitaComponent
        {
            return ComponentCount(typeof(T));
        }

        public int ComponentCount(Type type)
        {
            if (_componentCounter.ContainsKey(type))
                return _componentCounter[type];
            return 0;
        }

        public void RecalculationComponentCounter()
        {
            foreach (var component in ComponentBackpack)
            {
                var type = component.GetType();
                if (_componentCounter.ContainsKey(type))
                    _componentCounter[type]++;
                else
                    _componentCounter.Add(type, 1);
            }
            if (InfusionBackpack.Instance.IsVisible)
                InfusionBackpack.Instance.ResetInfusions();
        }

        public void RemoveComponents<T>(int count = 1) => RemoveComponents(typeof(T), count);

        public void RemoveComponents(Type type, int count = 1)
        {
            if (InfusionBackpack.Instance.IsVisible)
                InfusionBackpack.Instance.ResetInfusions();

            var funcName = nameof(RemoveComponents);
            IAmEntity.Call(funcName, type, count);

            var comps = new List<LoitaComponent>();
            ComponentBackpack.ForEach(comp =>
            {
                if (count > 0 && (comp.GetType() == type || comp.GetType().IsSubclassOf(type)))
                {
                    comps.Add(comp);
                    count--;
                }
            });

            comps.ForEach(comp =>
            {
                ComponentBackpack.Remove(comp);
                var type = comp.GetType();
                if (_componentCounter.ContainsKey(type))
                    _componentCounter[type]--;
                else
                    _componentCounter.Add(type, -1);
            });
        }

        public override void SaveData(TagCompound tag)
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

            WriteToBinary(writer);

            tag.Add("Loita:Components", stream.GetBuffer());
            base.SaveData(tag);
        }

        public override void LoadData(TagCompound tag)
        {
            base.LoadData(tag);
            if (tag.TryGet<byte[]>("Loita:Components", out var bytes))
            {
                using MemoryStream stream = new MemoryStream(bytes);
                using BinaryReader reader = new BinaryReader(stream);
                int count = reader.ReadInt32();
                for (int i = 0; i < count; i++)
                {
                    try
                    {
                        var type = Type.GetType(reader.ReadString());
                        if (!IAmEntity.HasComponent(type))
                        {
                            var component = (IComponent)Activator.CreateInstance(type, this);
                            IAmEntity.AddComponent(component);
                        }
                        else
                        {
                            if (IAmEntity.GetComponent(type) is IBinarySupport binary)
                                binary.ReadOnBinary(reader);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"读取物玩家组件时发生异常。\n玩家：{Player.name}\n异常：{ex}");
                        break;
                    }
                }

                ReadOnBinary(reader);
            }
            RecalculationComponentCounter();
        }

        public void WriteToBinary(BinaryWriter bw)
        {
            bw.Write(ComponentBackpack.Count);
            foreach (var c in ComponentBackpack)
            {
                bw.Write(c.GetType().FullName);
                c.WriteToBinary(bw);
            }
        }

        public void ReadOnBinary(BinaryReader br)
        {
            ComponentBackpack.Clear();
            int count = br.ReadInt32();
            for (int i = 0; i < count; i++)
            {
                try
                {
                    var comp = (LoitaComponent)Activator.CreateInstance(Type.GetType(br.ReadString()), IAmEntity);
                    comp.ReadOnBinary(br);
                    ComponentBackpack.Add(comp);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"读取物玩家组件时发生异常。\n玩家：{Player.name}\n异常：{ex}");
                    break;
                }
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