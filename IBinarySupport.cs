using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Terraria.Enums;

namespace Loita
{
    internal interface IBinarySupport
    {
        public void WriteToBinary(BinaryWriter bw);

        public void ReadOnBinary(BinaryReader br);
    }
}