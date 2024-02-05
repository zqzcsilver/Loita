using FontStashSharp;

using Loita.Components.LoitaComponents;
using Loita.Components.LoitaComponents.Spells;
using Loita.Components.LoitaComponents.Triggers;
using Loita.Items;
using Loita.KeyBindSystem;
using Loita.RecipeSystem.Conditions;
using Loita.RecipeSystem.RecipeItems;
using Loita.RecipeSystem.Results;
using Loita.UI;
using Loita.UI.UIContainers.DebugUI.DebugItems;
using Loita.Utils;

using System;
using System.Linq;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Loita
{
    internal class Loita : Mod
    {
        public const float FRAME_NUMBER = 60f;

        public static LoitaUISystem UISystem
        {
            get => Instance.system;
        }

        public static Loita Instance
        {
            get => instance;
        }

        internal static FontManager FontManager
        {
            get
            {
                if (Instance._fontManager == null)
                {
                    Instance._fontManager = new FontManager();
                    LoggerItem.WriteLine($"[Loita:Font Manager]服务注册完毕");
                }
                return Instance._fontManager;
            }
        }

        private FontManager _fontManager;

        internal static KeyGroupManager KeyGroupManager => Instance._keyGroupManager;
        private KeyGroupManager _keyGroupManager;

        internal static BinaryProcessed BinaryProcessed
        {
            get
            {
                if (Instance._binaryProcessed == null)
                {
                    Instance._binaryProcessed = new BinaryProcessed();
                    LoggerItem.WriteLine($"[Loita:Binary Processed]服务注册完毕");
                    Instance._binaryProcessed.LoadProcessed();
                }
                return Instance._binaryProcessed;
            }
        }

        private BinaryProcessed _binaryProcessed;

        public static RecipeSystem.RecipeSystem RecipeSystem => Instance._recipeSystem;
        private RecipeSystem.RecipeSystem _recipeSystem;

        /// <summary>
        /// 默认字体大小
        /// </summary>
        public const float DEFAULT_FONT_SIZE = 40f;

        /// <summary>
        /// 默认字体系统
        /// </summary>
        public static FontSystem DefaultFontSystem => FontManager["Fonts/FusionPixel12.ttf"];

        /// <summary>
        /// 默认字体
        /// </summary>
        public static DynamicSpriteFont DefaultFont => DefaultFontSystem.GetFont(DEFAULT_FONT_SIZE);

        private LoitaUISystem system;
        private static Loita instance;

        public Loita()
        {
            instance = this;

            system = new LoitaUISystem();
            LoggerItem.WriteLine($"[Loita:UI System]服务注册完毕");
            _recipeSystem = new RecipeSystem.RecipeSystem();
            LoggerItem.WriteLine($"[Loita:Recipe System]服务注册完毕");
            _keyGroupManager = new KeyGroupManager();
            LoggerItem.WriteLine($"[Loita:Key Group Manager]服务注册完毕");

            for (int i = 0; i < 100; i++)
            {
                LCRecipeItem recipeItem = new LCRecipeItem();
                recipeItem.AddCondition(new LCCondition<TDoubleSpell>());
                recipeItem.AddResult(new LCResult<SArrow>());
                RecipeSystem.Register(recipeItem);
            }
        }

        public override void Load()
        {
            base.Load();

            if (Main.netMode != NetmodeID.Server)
            {
                system.Load();
                loadComponentInstance();
            }
        }

        private void loadComponentInstance()
        {
            LoggerItem.WriteLine($"[Loita:Instance Manager]正在注册 Loita Component 组件实例...");
            var cTypes = from c in GetType().Assembly.GetTypes()
                         where !c.IsAbstract && c.IsSubclassOf(typeof(LoitaComponent)) && c != typeof(CInfusionSlot)
                         select c;
            foreach (var ctype in cTypes)
            {
                var instance = (LoitaComponent)Activator.CreateInstance(ctype, new object[] { null });
                InstanceManager<LoitaComponent>.RegisterInstance(instance);
                LoggerItem.WriteLine($"[Loita:Instance Manager]组件 {instance.Name} 注册完毕");

                ComponentAcquirer ca = new ComponentAcquirer();
                ca.ComponentType = ctype;
                AddContent(ca);
            }
            LoggerItem.WriteLine($"[Loita:Instance Manager]Loita Component 组件实例注册完毕");
        }
    }
}