﻿using FontStashSharp;

using Loita.Utils;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System;

namespace Loita.Components
{
    internal abstract class CInfusion : ComponentBase, IInfusion
    {
        protected CInfusion(IEntity entity) : base(entity)
        {
        }

        public string Name => string.Empty;

        public string Description => string.Empty;

        public virtual void DrawTips(SpriteBatch sb, Vector2 startPos, Vector2 containerSize, out Vector2 size)
        {
            size = Vector2.Zero;
            var nameFont = Loita.DefaultFontSystem.GetFont(30f);
            var desFont = Loita.DefaultFontSystem.GetFont(20f);
            var name = StringUtil.GetWordWrapString1(Name, nameFont, containerSize.X);
            var description = StringUtil.GetWordWrapString1(Description, desFont, containerSize.X);
            var nameSize = nameFont.MeasureString(name);
            var desSize = desFont.MeasureString(description);

            size.X = containerSize.X;
            size.Y = nameSize.Y + desSize.Y;

            sb.DrawString(nameFont, name, startPos, Color.White);
            startPos.Y += nameSize.Y;
            sb.DrawString(desFont, description, startPos, Color.White);
        }
    }
}