using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loita.Components
{
    internal interface IInfusion
    {
        public string Name { get; }
        public string Description { get; }

        public void DrawTips(SpriteBatch sb, Vector2 startPos, Vector2 containerSize, out Vector2 size);
    }
}