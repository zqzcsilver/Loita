using Loita.QuickAssetReference;
using Loita.RecipeSystem.Conditions;
using Loita.RecipeSystem.Results;

using Microsoft.Xna.Framework.Graphics;

using System.Collections.Generic;

namespace Loita.RecipeSystem.RecipeItems
{
    internal class LCRecipeItem : RecipeItem<LCCondition, LCResult>
    {
        private readonly List<RecipeCondition> _conditions = new List<RecipeCondition>();
        public override List<RecipeCondition> Conditions => _conditions;
        private readonly List<RecipeResult> _results = new List<RecipeResult>();
        public override List<RecipeResult> Results => _results;

        public override Texture2D Icon => Conditions.Count > 0 ? Conditions[0].Icon : ModAssets_Texture2D.Images.CBlockImmediateAsset.Value;
    }
}