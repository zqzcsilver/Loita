using Loita.RecipeSystem.Conditions;
using Loita.RecipeSystem.Results;

using System.Collections.Generic;

namespace Loita.RecipeSystem.RecipeItems
{
    internal abstract class RecipeItem
    {
        public abstract bool Permission();

        public abstract void Apply();
    }

    internal abstract class RecipeItem<TCondition, TResult> : RecipeItem
        where TCondition : RecipeCondition
        where TResult : RecipeResult
    {
        public abstract List<TCondition> Conditions { get; }
        public abstract List<TResult> Results { get; }

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