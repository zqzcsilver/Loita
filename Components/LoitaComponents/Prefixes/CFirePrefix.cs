using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Terraria;

namespace Loita.Components.LoitaComponents.Prefixes
{
    internal class CFirePrefix : CPrefix
    {
        public CFirePrefix(IEntity entity) : base(entity)
        {
        }

        public override void Apply(SpellInfo info)
        {
            base.Apply(info);
        }
    }
}