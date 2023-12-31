﻿using FontStashSharp;

using Loita.Utils;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System;

using Terraria;

namespace Loita.Components.ProjectileComponents
{
    internal class LightComponent : LogicComponentBase, IInfusion
    {
        public string Name => "发光";

        public string Description => "此魔法将会发出光芒";

        public LightComponent(IEntity entity) : base(entity)
        {
        }

        public override void Load()
        {
            RegisterHook(HookType.AI, AI);
            RegisterHook(HookType.Okay, new OkayDelegate(Okay));
        }

        public override void Okay(Projectile projectile, ref Color color)
        {
            base.Okay(projectile, ref color);
        }

        public override void AI(Projectile projectile)
        {
            base.AI(projectile);
            Lighting.AddLight(projectile.position, 1f, 1f, 1f);
        }

        public void DrawTips(SpriteBatch sb, Vector2 startPos, Vector2 containerSize, out Vector2 size)
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