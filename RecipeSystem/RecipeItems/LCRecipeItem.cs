using Loita.RecipeSystem.Conditions;
using Loita.RecipeSystem.Results;

using System.Collections.Generic;

namespace Loita.RecipeSystem.RecipeItems
{
    internal class LCRecipeItem : RecipeItem<LCCondition, LCResult>
    {
        private readonly List<LCCondition> _conditions = new List<LCCondition>();
        public override List<LCCondition> Conditions => _conditions;
        private readonly List<LCResult> _results = new List<LCResult>();
        public override List<LCResult> Results => _results;
    }
}