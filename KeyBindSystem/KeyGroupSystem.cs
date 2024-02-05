using Microsoft.Xna.Framework;

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