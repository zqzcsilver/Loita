using Loita.Components.LoitaComponents;
using Loita.ModPlayers;

using Microsoft.Xna.Framework.Graphics;

using System;

using Terraria;

namespace Loita.RecipeSystem.Conditions
{
    internal abstract class LCCondition : RecipeCondition
    {
    }

    internal class LCCondition<T> : LCCondition where T : LoitaComponent
    {
        public override Texture2D Icon => _component.Texture;

        public override string Name => _component.Name;

        public override string Description => _component.Description;

        public override bool IsEnable => Enable;
        public bool Enable = true;

        public int Count
        {
            get => count;
            set => count = Math.Max(0, value);
        }

        private int count = 1;
        private T _component;

        public LCCondition()
        {
            _component = (T)Activator.CreateInstance(typeof(T), new object[] { null });
        }

        public override void Apply()
        {
            MPlayer.Instance.RemoveComponents<T>(Count);
        }

        public override bool Permission()
        {
            return MPlayer.Instance.ComponentCount<T>() >= Count;
        }
    }
}