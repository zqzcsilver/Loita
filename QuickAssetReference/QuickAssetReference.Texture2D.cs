using ReLogic.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Loita.QuickAssetReference;
public static class ModAssets_Texture2D
{
    public static Asset<Texture2D> BackpackAsset => ModAssets_Utils.Mod.Assets.Request<Texture2D>(BackpackPath);
    public static Asset<Texture2D> BackpackImmediateAsset => ModAssets_Utils.Mod.Assets.Request<Texture2D>(BackpackPath, AssetRequestMode.ImmediateLoad);
    public const string BackpackPath = "Backpack";
    public static Asset<Texture2D> iconAsset => ModAssets_Utils.Mod.Assets.Request<Texture2D>(iconPath);
    public static Asset<Texture2D> iconImmediateAsset => ModAssets_Utils.Mod.Assets.Request<Texture2D>(iconPath, AssetRequestMode.ImmediateLoad);
    public const string iconPath = "icon";
    public static Asset<Texture2D> StaffUIAsset => ModAssets_Utils.Mod.Assets.Request<Texture2D>(StaffUIPath);
    public static Asset<Texture2D> StaffUIImmediateAsset => ModAssets_Utils.Mod.Assets.Request<Texture2D>(StaffUIPath, AssetRequestMode.ImmediateLoad);
    public const string StaffUIPath = "StaffUI";
    public static class Components
    {
        public static class LoitaComponents
        {
            public static class Prefixes
            {
                public static Asset<Texture2D> CLightPrefixAsset => ModAssets_Utils.Mod.Assets.Request<Texture2D>(CLightPrefixPath);
                public static Asset<Texture2D> CLightPrefixImmediateAsset => ModAssets_Utils.Mod.Assets.Request<Texture2D>(CLightPrefixPath, AssetRequestMode.ImmediateLoad);
                public const string CLightPrefixPath = "Components/LoitaComponents/Prefixes/CLightPrefix";
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
            }

        }

    }

    public static class Images
    {
        public static Asset<Texture2D> CBlockAsset => ModAssets_Utils.Mod.Assets.Request<Texture2D>(CBlockPath);
        public static Asset<Texture2D> CBlockImmediateAsset => ModAssets_Utils.Mod.Assets.Request<Texture2D>(CBlockPath, AssetRequestMode.ImmediateLoad);
        public const string CBlockPath = "Images/CBlock";
        public static Asset<Texture2D> TakeInAsset => ModAssets_Utils.Mod.Assets.Request<Texture2D>(TakeInPath);
        public static Asset<Texture2D> TakeInImmediateAsset => ModAssets_Utils.Mod.Assets.Request<Texture2D>(TakeInPath, AssetRequestMode.ImmediateLoad);
        public const string TakeInPath = "Images/TakeIn";
        public static Asset<Texture2D> TakeOutAsset => ModAssets_Utils.Mod.Assets.Request<Texture2D>(TakeOutPath);
        public static Asset<Texture2D> TakeOutImmediateAsset => ModAssets_Utils.Mod.Assets.Request<Texture2D>(TakeOutPath, AssetRequestMode.ImmediateLoad);
        public const string TakeOutPath = "Images/TakeOut";
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

        }

    }

}

