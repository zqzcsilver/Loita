using FontStashSharp;

using Loita.Utils.Expands;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System.Collections.Generic;

namespace Loita.StringDrawerSystem.DrawerItems.TextDrawers
{
    internal class TypewriterText : TextDrawer
    {
        public float AnimationTime = Loita.FRAME_NUMBER * 0.7f;
        public float CursorBlinkingInterval = 0f;
        public float CursorBlinkingTime = Loita.FRAME_NUMBER * 0.7f;
        public bool EnableAnimation = false;
        public Color CursorColor = Color.White;
        public Vector2 CursorScale = Vector2.One;
        public float RepeatAnimationTime = -1;
        public float RepeatLinkAnimationTime = -1;

        public override void Init(StringDrawer stringDrawer, string originalText, string name, StringParameters stringParameters)
        {
            base.Init(stringDrawer, originalText, name, stringParameters);
            if (stringParameters == null)
                return;
            AnimationTime = stringParameters.GetFloat("AnimationTime", Loita.FRAME_NUMBER * 0.7f);
            CursorBlinkingTime = stringParameters.GetFloat("CursorBlinkingTime", Loita.FRAME_NUMBER * 0.7f);
            CursorBlinkingInterval = stringParameters.GetFloat("CursorBlinkingInterval");
            CursorColor = stringParameters.GetColor("CursorColor", Color.White);
            CursorScale = stringParameters.GetVector2("CursorScale", Vector2.One);
            RepeatAnimationTime = stringParameters.GetFloat("RepeatAnimationTime", -1);
            RepeatLinkAnimationTime = stringParameters.GetFloat("RepeatLinkAnimationTime", -1);
        }

        private int textIndex = 0;
        private float time = 0f;
        private float time1 = 0f;
        private float time2 = 0f;
        private float time3 = 0f;
        private float time4 = 0f;
        private bool head = false;

        public override void ResetAnimation()
        {
            base.ResetAnimation();
            head = false;
            EnableAnimation = false;
            time = 0f;
            time1 = 0f;
            time2 = 0f;
            textIndex = 0;
        }

        public override void StartAnimation()
        {
            EnableAnimation = true;
            time2 = CursorBlinkingTime;
        }

        public override TextDrawer Decomposition(string text, List<DrawerItem> drawerItems, int index)
        {
            TypewriterText typewriterText = (TypewriterText)base.Decomposition(text, drawerItems, index);
            typewriterText.AnimationTime = AnimationTime;
            typewriterText.CursorBlinkingTime = CursorBlinkingTime;
            typewriterText.CursorBlinkingInterval = CursorBlinkingInterval;
            typewriterText.CursorColor = CursorColor;
            return typewriterText;
        }

        public override void Draw(SpriteBatch sb, GameTime gt)
        {
            if (RepeatLinkAnimationTime >= 0f)
            {
                if (time4 >= RepeatLinkAnimationTime)
                {
                    time4 = 0f;
                    ResetLinkAnimation();
                }
                time4 += gt.GetRefreshFactor();
            }
            if (RepeatAnimationTime >= 0f)
            {
                if (time3 >= RepeatAnimationTime)
                {
                    time3 = 0f;
                    ResetAnimation();
                    StartAnimation();
                }
                time3 += gt.GetRefreshFactor();
            }
            if (!head && HeadLinkItems.Count == 0)
            {
                head = true;
                StartAnimation();
            }
            var pos = Position;
            var text = Text[..textIndex];
            sb.DrawString(Font, text, pos + Offset,
                Color, Scale, Rotation,
                Origin, 0, CharacterSpacing, 0, TextStyle,
                FontSystemEffect, EffectAmount);
            pos.X += GetTextSize(text).X;
            if (!EnableAnimation || textIndex >= Text.Length)
                return;
            //if (AnimationTime == 0f)
            //{
            //    textIndex = Text.Length;
            //    foreach (var i in TailLinkItems)
            //    {
            //        i.StartAnimation();
            //    }
            //    EnableAnimation = false;
            //    return;
            //}
            if (time1 >= CursorBlinkingInterval)
            {
                time1 = 0f;
                time2 = CursorBlinkingTime;
            }
            if (time2 > 0f)
            {
                time2 -= gt.GetRefreshFactor();
                sb.DrawString(Font, "_", pos + Offset,
                    CursorColor, CursorScale, Rotation,
                    Origin, 0, CharacterSpacing, 0);
            }
            if (time >= AnimationTime)
            {
                time = 0f;
                textIndex++;
                if (textIndex >= Text.Length)
                {
                    foreach (var i in TailLinkItems)
                    {
                        i.StartAnimation();
                    }
                    EnableAnimation = false;
                }
            }
            time += gt.GetRefreshFactor();
            if (time2 <= 0f)
                time1 += gt.GetRefreshFactor();
        }
    }
}