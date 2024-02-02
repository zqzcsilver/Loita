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
        /// <summary>
        /// 将数据以二进制形式写入
        /// </summary>
        /// <param name="bw"></param>
        public void WriteToBinary(BinaryWriter bw);

        /// <summary>
        /// 将数据从二进制中读取
        /// </summary>
        /// <param name="br"></param>
        public void ReadOnBinary(BinaryReader br);
    }
}