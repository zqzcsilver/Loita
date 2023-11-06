using Loita.Components.LoitaComponents;
using Loita.ModPlayers;

using Microsoft.Xna.Framework.Graphics;

using System;

using Terraria;

namespace Loita.RecipeSystem.Results
{
    internal abstract class LCResult : RecipeResult
    {
        public abstract Type LCType { get; }
    }

    internal class LCResult<T> : LCResult where T : LoitaComponent
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

        public override Type LCType => typeof(T);

        private int count = 1;
        private T _component;

        public LCResult()
        {
            _component = (T)Activator.CreateInstance(typeof(T), new object[] { null });
        }

        public override void Apply()
        {
            MPlayer.Instance.GainComponents<T>(Count);
        }
    }
}