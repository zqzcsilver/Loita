using Microsoft.Xna.Framework.Graphics;

namespace Loita.RecipeSystem.Results
{
    internal abstract class RecipeResult
    {
        public abstract Texture2D Icon { get; }
        public abstract string Name { get; }
        public abstract string Description { get; }
        public abstract bool IsEnable { get; }

        public abstract void Apply();
    }
}