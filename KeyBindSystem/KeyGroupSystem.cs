using Microsoft.Xna.Framework;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Terraria.ModLoader;

namespace Loita.KeyBindSystem
{
    internal class KeyGroupSystem : ModSystem
    {
        public override void UpdateUI(GameTime gameTime)
        {
            base.UpdateUI(gameTime);
            Loita.KeyGroupManager.Update(gameTime);
        }
    }
}