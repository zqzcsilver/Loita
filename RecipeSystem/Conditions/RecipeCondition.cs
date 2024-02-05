using Microsoft.Xna.Framework.Graphics;

namespace Loita.RecipeSystem.Conditions
{
    internal abstract class RecipeCondition
    {
        public abstract Texture2D Icon { get; }
        public abstract string Name { get; }
        public abstract string Description { get; }
        public abstract bool IsEnable { get; }

        public abstract bool Permission();

        public abstract void Apply();
    }
}