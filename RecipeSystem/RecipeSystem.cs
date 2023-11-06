using Loita.RecipeSystem.RecipeItems;

using System;
using System.Collections.Generic;

namespace Loita.RecipeSystem
{
    internal class RecipeSystem
    {
        public static RecipeSystem Instance => Loita.RecipeSystem;
        private readonly List<RecipeItem> _recipeItems = new List<RecipeItem>();
        public List<RecipeItem> RecipeItems => _recipeItems;

        public event Action<RecipeItem> OnRecipeAdd;

        public event Action<RecipeItem> OnRecipeRemove;

        public void Register(RecipeItem item)
        {
            if (_recipeItems.Contains(item))
                return;
            _recipeItems.Add(item);
            OnRecipeAdd?.Invoke(item);
        }

        public void Remove(RecipeItem item)
        {
            _recipeItems.Remove(item);
            OnRecipeRemove?.Invoke(item);
        }

        public RecipeItem Find(Predicate<RecipeItem> match)
        {
            return _recipeItems.Find(match);
        }

        public List<RecipeItem> FindAll(Predicate<RecipeItem> match)
        {
            return _recipeItems.FindAll(match);
        }

        public void ForEach(Action<RecipeItem> action)
        {
            _recipeItems.ForEach(action);
        }

        public List<RecipeItem> GetAvailableRecipes() => FindAll(r => r.Permission());
    }
}