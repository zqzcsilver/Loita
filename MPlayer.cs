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
    internal class MPlayer : ModPlayer, IBinarySupport
    {
        public List<LoitaComponent> ComponentBackpack = new List<LoitaComponent>();
        public static MPlayer Instance => Main.LocalPlayer.GetModPlayer<MPlayer>();

        public override void OnEnterWorld()
        {
            var player = Player;
            var projectile = Projectile.NewProjectileDirect(player.GetSource_FromThis(),
                player.Center, player.velocity, 10, 10, 10f, player.whoAmI);
            IEntity entity = projectile.GetGlobalProjectile<GProjectile>();
            var random = new Random();
            for (int i = 0; i < 100; i++)
            {
                var r = random.Next(4);
                if (r == 0)
                    GainComponent(new CDoubleSpell(entity));
                else if (r == 1)
                    GainComponent(new CTestSpell(entity));
                else if(r == 2)
                    GainComponent(new CLightPrefix(entity));
                else if (r == 3)
                    GainComponent(new CFirePrefix(entity));
            }
            base.OnEnterWorld();
        }

        public void GainComponent(LoitaComponent component)
        {
            if (ComponentBackpack.Count == 0)
            {
                ComponentBackpack.Add(component);
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
    }
}