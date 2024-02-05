using Microsoft.Xna.Framework;

using System;

namespace Loita.Utils
{
    internal class SleekProgression
    {
        internal delegate void ValueChangeEvent(float value);

        public event ValueChangeEvent OnValueChange;

        public float Value;
        public float WaitToValue;
        public float Speed = 4f;
        public float JitterBufferLength = 0.01f;

        public void Update(GameTime gt)
        {
            if (Math.Abs(Value - WaitToValue) >= JitterBufferLength)
            {
                Value += (WaitToValue - Value) / Speed;
                OnValueChange?.Invoke(Value);
            }
        }
    }
}