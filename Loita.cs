using FontStashSharp;

using Loita.UI;
using Loita.Utils;

using System.Collections.Generic;

using Terraria.ID;
using Terraria;
using Terraria.IO;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;

namespace Loita
{
    internal class Loita : Mod
    {
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
                    Instance._fontManager = new FontManager();
                return Instance._fontManager;
            }
        }

        private FontManager _fontManager;

        internal static BinaryProcessed BinaryProcessed
        {
            get
            {
                if (Instance._binaryProcessed == null)
                {
                    Instance._binaryProcessed = new BinaryProcessed();
                    Instance._binaryProcessed.LoadProcessed();
                }
                return Instance._binaryProcessed;
            }
        }

        private BinaryProcessed _binaryProcessed;

        /// <summary>
        /// 默认字体大小
        /// </summary>
        public const float DEFAULT_FONT_SIZE = 40f;

        /// <summary>
        /// 默认字体系统
        /// </summary>
        public static FontSystem DefaultFontSystem => FontManager["Fonts/fusion-pixel-12px-proportional-zh_hans.ttf"];

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
        }

        public override void Load()
        {
            base.Load();

            if (Main.netMode != NetmodeID.Server)
            {
                system.Load();
            }
        }
    }
}