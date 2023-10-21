using Loita.UI.UIElements;

using System;
using System.Collections.Generic;

using static Loita.UI.UIElements.BaseElement;

namespace Loita.UI.UIContainers.DebugUI.DebugItems
{
    internal class LoggerItem : DebugItemBase
    {
        public static LoggerItem Instance { get; private set; }

        public LoggerItem()
        {
            Level = 0;
            Instance = this;
        }

        public override string Name => "Logger";

        private BaseElement _page;
        private UIContainerPanel _logsContianer;
        private PositionStyle _top;
        private static List<string> _logs = new List<string>();
        private static bool _saveInCache = true;

        public override BaseElement GetPage()
        {
            if (_page != null)
                return _page;

            _page = new BaseElement();
            _page.Info.Width.SetValue(0f, 1f);
            _page.Info.Height.SetValue(0f, 1f);

            UIVerticalScrollbar verticalScrollbar = new UIVerticalScrollbar();
            verticalScrollbar.Info.Height.SetValue(4f, 1f);
            verticalScrollbar.Info.Left.SetValue(-18f, 1f);
            verticalScrollbar.Info.Top.SetValue(-2f, 0f);
            _page.Register(verticalScrollbar);

            _logsContianer = new UIContainerPanel();
            verticalScrollbar.BindElement = _logsContianer;
            _logsContianer.SetVerticalScrollbar(verticalScrollbar);
            _logsContianer.Info.Width.SetValue(PositionStyle.Full - verticalScrollbar.Info.Width);
            _page.Register(_logsContianer);
            _logsContianer.Events.OnUpdate += (e, gt) =>
            {
                if (_saveInCache)
                {
                    _saveInCache = false;
                    if (e.Info.InitDone && _logs.Count > 0)
                    {
                        _logs.ForEach(log => WriteLine(log, false));
                        _logs.Clear();
                    }
                }
            };
            return _page;
        }

        public static void WriteLine(string text, bool showTime = true)
        {
            if (showTime)
            {
                text = $"[{DateTime.Now:yyyy-MM-ddTHH:mm:sszzz}]" + text;
            }
            if (Instance == null || Instance._logsContianer == null || _saveInCache)
            {
                _logs.Add(text);
                return;
            }
            UITextPlus textPlus = new UITextPlus(text);
            textPlus.WordWrap(Instance._logsContianer.Info.Size.X);
            textPlus.Info.Top.SetValue(Instance._top);
            Instance._logsContianer.AddElement(textPlus);
            Instance._top += textPlus.Info.Height;
        }
    }
}