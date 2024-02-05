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
                    LoggerItem.WriteLine($"[Loita:Font Manager]����ע�����");
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
                    LoggerItem.WriteLine($"[Loita:Binary Processed]����ע�����");
                    Instance._binaryProcessed.LoadProcessed();
                }
                return Instance._binaryProcessed;
            }
        }

        private BinaryProcessed _binaryProcessed;

        public static RecipeSystem.RecipeSystem RecipeSystem => Instance._recipeSystem;
        private RecipeSystem.RecipeSystem _recipeSystem;

        /// <summary>
        /// Ĭ�������С
        /// </summary>
        public const float DEFAULT_FONT_SIZE = 40f;

        /// <summary>
        /// Ĭ������ϵͳ
        /// </summary>
        public static FontSystem DefaultFontSystem => FontManager["Fonts/FusionPixel12.ttf"];

        /// <summary>
        /// Ĭ������
        /// </summary>
        public static DynamicSpriteFont DefaultFont => DefaultFontSystem.GetFont(DEFAULT_FONT_SIZE);

        private LoitaUISystem system;
        private static Loita instance;

        public Loita()
        {
            instance = this;

            system = new LoitaUISystem();
            LoggerItem.WriteLine($"[Loita:UI System]����ע�����");
            _recipeSystem = new RecipeSystem.RecipeSystem();
            LoggerItem.WriteLine($"[Loita:Recipe System]����ע�����");
            _keyGroupManager = new KeyGroupManager();
            LoggerItem.WriteLine($"[Loita:Key Group Manager]����ע�����");

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
            LoggerItem.WriteLine($"[Loita:Instance Manager]����ע�� Loita Component ���ʵ��...");
            var cTypes = from c in GetType().Assembly.GetTypes()
                         where !c.IsAbstract && c.IsSubclassOf(typeof(LoitaComponent)) && c != typeof(CInfusionSlot)
                         select c;
            foreach (var ctype in cTypes)
            {
                var instance = (LoitaComponent)Activator.CreateInstance(ctype, new object[] { null });
                InstanceManager<LoitaComponent>.RegisterInstance(instance);
                LoggerItem.WriteLine($"[Loita:Instance Manager]��� {instance.Name} ע�����");

                ComponentAcquirer ca = new ComponentAcquirer();
                ca.ComponentType = ctype;
                AddContent(ca);
            }
            LoggerItem.WriteLine($"[Loita:Instance Manager]Loita Component ���ʵ��ע�����");
        }
    }
}