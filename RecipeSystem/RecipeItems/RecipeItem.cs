using Loita.RecipeSystem.Conditions;
using Loita.RecipeSystem.Results;

using Microsoft.Xna.Framework.Graphics;

using System.Collections.Generic;

namespace Loita.RecipeSystem.RecipeItems
{
    internal abstract class RecipeItem
    {
        public abstract List<RecipeCondition> Conditions { get; }
        public abstract List<RecipeResult> Results { get; }
        public abstract Texture2D Icon { get; }

        public abstract bool Permission();

        public abstract void Apply();
    }

    internal abstract class RecipeItem<TCondition, TResult> : RecipeItem
        where TCondition : RecipeCondition
        where TResult : RecipeResult
    {
        public List<TCondition> TConditions { get => Conditions.ConvertAll(r => (TCondition)r); }
        public List<TResult> TResults { get => Results.ConvertAll(t => (TResult)t); }

        public virtual void AddCondition(TCondition condition)
        {
            if (!Conditions.Contains(condition))
                Conditions.Add(condition);
        }

        public virtual void RemoveCondition(TCondition condition)
        {
            Conditions.Remove(condition);
        }

        public virtual void AddResult(TResult result)
        {
            Results.Add(result);
        }

        public virtual void RemoveResult(TResult result)
        {
            Results.Remove(result);
        }

        public override bool Permission()
        {
            foreach (var condition in Conditions)
            {
                if (condition == null || !(condition.IsEnable && condition.Permission()))
                    return false;
            }
            return true;
        }

        public override void Apply()
        {
            if (Permission())
            {
                foreach (var condition in Conditions)
                {
                    if (condition != null && condition.IsEnable)
                        condition.Apply();
                }
                foreach (var result in Results)
                {
                    if (result != null && result.IsEnable)
                        result.Apply();
                }
            }
        }
    }
}