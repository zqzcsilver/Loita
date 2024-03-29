using Microsoft.Xna.Framework.Graphics;

using ReLogic.Content;

namespace Loita.QuickAssetReference;
public static class ModAssets_Texture2D
{
    public static Asset<Texture2D> iconAsset => ModAssets_Utils.Mod.Assets.Request<Texture2D>(iconPath);
    public static Asset<Texture2D> iconImmediateAsset => ModAssets_Utils.Mod.Assets.Request<Texture2D>(iconPath, AssetRequestMode.ImmediateLoad);
    public const string iconPath = "icon";
    public static class Components
    {
        public static class LoitaComponents
        {
            public static class Prefixes
            {
                public static Asset<Texture2D> CUnlimitedPenetrationPrefixAsset => ModAssets_Utils.Mod.Assets.Request<Texture2D>(CUnlimitedPenetrationPrefixPath);
                public static Asset<Texture2D> CUnlimitedPenetrationPrefixImmediateAsset => ModAssets_Utils.Mod.Assets.Request<Texture2D>(CUnlimitedPenetrationPrefixPath, AssetRequestMode.ImmediateLoad);
                public const string CUnlimitedPenetrationPrefixPath = "Components/LoitaComponents/Prefixes/CUnlimitedPenetrationPrefix";
                public static Asset<Texture2D> DoubleDamageAsset => ModAssets_Utils.Mod.Assets.Request<Texture2D>(DoubleDamagePath);
                public static Asset<Texture2D> DoubleDamageImmediateAsset => ModAssets_Utils.Mod.Assets.Request<Texture2D>(DoubleDamagePath, AssetRequestMode.ImmediateLoad);
                public const string DoubleDamagePath = "Components/LoitaComponents/Prefixes/DoubleDamage";
                public static Asset<Texture2D> PLightAsset => ModAssets_Utils.Mod.Assets.Request<Texture2D>(PLightPath);
                public static Asset<Texture2D> PLightImmediateAsset => ModAssets_Utils.Mod.Assets.Request<Texture2D>(PLightPath, AssetRequestMode.ImmediateLoad);
                public const string PLightPath = "Components/LoitaComponents/Prefixes/PLight";
                public static Asset<Texture2D> POnePenetrationAsset => ModAssets_Utils.Mod.Assets.Request<Texture2D>(POnePenetrationPath);
                public static Asset<Texture2D> POnePenetrationImmediateAsset => ModAssets_Utils.Mod.Assets.Request<Texture2D>(POnePenetrationPath, AssetRequestMode.ImmediateLoad);
                public const string POnePenetrationPath = "Components/LoitaComponents/Prefixes/POnePenetration";
            }

            public static class Spells
            {
                public static Asset<Texture2D> CTestSpellAsset => ModAssets_Utils.Mod.Assets.Request<Texture2D>(CTestSpellPath);
                public static Asset<Texture2D> CTestSpellImmediateAsset => ModAssets_Utils.Mod.Assets.Request<Texture2D>(CTestSpellPath, AssetRequestMode.ImmediateLoad);
                public const string CTestSpellPath = "Components/LoitaComponents/Spells/CTestSpell";
            }

            public static class Triggers
            {
                public static Asset<Texture2D> CDoubleSpellAsset => ModAssets_Utils.Mod.Assets.Request<Texture2D>(CDoubleSpellPath);
                public static Asset<Texture2D> CDoubleSpellImmediateAsset => ModAssets_Utils.Mod.Assets.Request<Texture2D>(CDoubleSpellPath, AssetRequestMode.ImmediateLoad);
                public const string CDoubleSpellPath = "Components/LoitaComponents/Triggers/CDoubleSpell";
                public static Asset<Texture2D> CQuadrupleSpellRandomPositionByMouseAsset => ModAssets_Utils.Mod.Assets.Request<Texture2D>(CQuadrupleSpellRandomPositionByMousePath);
                public static Asset<Texture2D> CQuadrupleSpellRandomPositionByMouseImmediateAsset => ModAssets_Utils.Mod.Assets.Request<Texture2D>(CQuadrupleSpellRandomPositionByMousePath, AssetRequestMode.ImmediateLoad);
                public const string CQuadrupleSpellRandomPositionByMousePath = "Components/LoitaComponents/Triggers/CQuadrupleSpellRandomPositionByMouse";
            }

        }

    }

    public static class DesignDraft
    {
        public static Asset<Texture2D> BackpackAsset => ModAssets_Utils.Mod.Assets.Request<Texture2D>(BackpackPath);
        public static Asset<Texture2D> BackpackImmediateAsset => ModAssets_Utils.Mod.Assets.Request<Texture2D>(BackpackPath, AssetRequestMode.ImmediateLoad);
        public const string BackpackPath = "DesignDraft/Backpack";
        public static Asset<Texture2D> CreateAsset => ModAssets_Utils.Mod.Assets.Request<Texture2D>(CreatePath);
        public static Asset<Texture2D> CreateImmediateAsset => ModAssets_Utils.Mod.Assets.Request<Texture2D>(CreatePath, AssetRequestMode.ImmediateLoad);
        public const string CreatePath = "DesignDraft/Create";
        public static Asset<Texture2D> RecipeAsset => ModAssets_Utils.Mod.Assets.Request<Texture2D>(RecipePath);
        public static Asset<Texture2D> RecipeImmediateAsset => ModAssets_Utils.Mod.Assets.Request<Texture2D>(RecipePath, AssetRequestMode.ImmediateLoad);
        public const string RecipePath = "DesignDraft/Recipe";
        public static Asset<Texture2D> StaffUIAsset => ModAssets_Utils.Mod.Assets.Request<Texture2D>(StaffUIPath);
        public static Asset<Texture2D> StaffUIImmediateAsset => ModAssets_Utils.Mod.Assets.Request<Texture2D>(StaffUIPath, AssetRequestMode.ImmediateLoad);
        public const string StaffUIPath = "DesignDraft/StaffUI";
    }

    public static class Images
    {
        public static Asset<Texture2D> CBlockAsset => ModAssets_Utils.Mod.Assets.Request<Texture2D>(CBlockPath);
        public static Asset<Texture2D> CBlockImmediateAsset => ModAssets_Utils.Mod.Assets.Request<Texture2D>(CBlockPath, AssetRequestMode.ImmediateLoad);
        public const string CBlockPath = "Images/CBlock";
        public static Asset<Texture2D> ChangeAsset => ModAssets_Utils.Mod.Assets.Request<Texture2D>(ChangePath);
        public static Asset<Texture2D> ChangeImmediateAsset => ModAssets_Utils.Mod.Assets.Request<Texture2D>(ChangePath, AssetRequestMode.ImmediateLoad);
        public const string ChangePath = "Images/Change";
        public static Asset<Texture2D> TakeInAsset => ModAssets_Utils.Mod.Assets.Request<Texture2D>(TakeInPath);
        public static Asset<Texture2D> TakeInImmediateAsset => ModAssets_Utils.Mod.Assets.Request<Texture2D>(TakeInPath, AssetRequestMode.ImmediateLoad);
        public const string TakeInPath = "Images/TakeIn";
        public static Asset<Texture2D> TakeOutAsset => ModAssets_Utils.Mod.Assets.Request<Texture2D>(TakeOutPath);
        public static Asset<Texture2D> TakeOutImmediateAsset => ModAssets_Utils.Mod.Assets.Request<Texture2D>(TakeOutPath, AssetRequestMode.ImmediateLoad);
        public const string TakeOutPath = "Images/TakeOut";
    }

    public static class Items
    {
        public static Asset<Texture2D> TestWandAsset => ModAssets_Utils.Mod.Assets.Request<Texture2D>(TestWandPath);
        public static Asset<Texture2D> TestWandImmediateAsset => ModAssets_Utils.Mod.Assets.Request<Texture2D>(TestWandPath, AssetRequestMode.ImmediateLoad);
        public const string TestWandPath = "Items/TestWand";
    }

    public static class UI
    {
        public static class UIContainers
        {
            public static class InfusionBackpack
            {
                public static class Images
                {
                    public static Asset<Texture2D> PrefixAsset => ModAssets_Utils.Mod.Assets.Request<Texture2D>(PrefixPath);
                    public static Asset<Texture2D> PrefixImmediateAsset => ModAssets_Utils.Mod.Assets.Request<Texture2D>(PrefixPath, AssetRequestMode.ImmediateLoad);
                    public const string PrefixPath = "UI/UIContainers/InfusionBackpack/Images/Prefix";
                    public static Asset<Texture2D> SpellAsset => ModAssets_Utils.Mod.Assets.Request<Texture2D>(SpellPath);
                    public static Asset<Texture2D> SpellImmediateAsset => ModAssets_Utils.Mod.Assets.Request<Texture2D>(SpellPath, AssetRequestMode.ImmediateLoad);
                    public const string SpellPath = "UI/UIContainers/InfusionBackpack/Images/Spell";
                    public static Asset<Texture2D> TriggerAsset => ModAssets_Utils.Mod.Assets.Request<Texture2D>(TriggerPath);
                    public static Asset<Texture2D> TriggerImmediateAsset => ModAssets_Utils.Mod.Assets.Request<Texture2D>(TriggerPath, AssetRequestMode.ImmediateLoad);
                    public const string TriggerPath = "UI/UIContainers/InfusionBackpack/Images/Trigger";
                }

            }

            public static class RecipeUI
            {
                public static class Images
                {
                    public static Asset<Texture2D> CloseIconAsset => ModAssets_Utils.Mod.Assets.Request<Texture2D>(CloseIconPath);
                    public static Asset<Texture2D> CloseIconImmediateAsset => ModAssets_Utils.Mod.Assets.Request<Texture2D>(CloseIconPath, AssetRequestMode.ImmediateLoad);
                    public const string CloseIconPath = "UI/UIContainers/RecipeUI/Images/CloseIcon";
                    public static Asset<Texture2D> CreateIconAsset => ModAssets_Utils.Mod.Assets.Request<Texture2D>(CreateIconPath);
                    public static Asset<Texture2D> CreateIconImmediateAsset => ModAssets_Utils.Mod.Assets.Request<Texture2D>(CreateIconPath, AssetRequestMode.ImmediateLoad);
                    public const string CreateIconPath = "UI/UIContainers/RecipeUI/Images/CreateIcon";
                    public static Asset<Texture2D> RecipeIconAsset => ModAssets_Utils.Mod.Assets.Request<Texture2D>(RecipeIconPath);
                    public static Asset<Texture2D> RecipeIconImmediateAsset => ModAssets_Utils.Mod.Assets.Request<Texture2D>(RecipeIconPath, AssetRequestMode.ImmediateLoad);
                    public const string RecipeIconPath = "UI/UIContainers/RecipeUI/Images/RecipeIcon";
                    public static Asset<Texture2D> ToAsset => ModAssets_Utils.Mod.Assets.Request<Texture2D>(ToPath);
                    public static Asset<Texture2D> ToImmediateAsset => ModAssets_Utils.Mod.Assets.Request<Texture2D>(ToPath, AssetRequestMode.ImmediateLoad);
                    public const string ToPath = "UI/UIContainers/RecipeUI/Images/To";
                    public static Asset<Texture2D> ToFullAsset => ModAssets_Utils.Mod.Assets.Request<Texture2D>(ToFullPath);
                    public static Asset<Texture2D> ToFullImmediateAsset => ModAssets_Utils.Mod.Assets.Request<Texture2D>(ToFullPath, AssetRequestMode.ImmediateLoad);
                    public const string ToFullPath = "UI/UIContainers/RecipeUI/Images/ToFull";
                }

            }

        }

    }

}

