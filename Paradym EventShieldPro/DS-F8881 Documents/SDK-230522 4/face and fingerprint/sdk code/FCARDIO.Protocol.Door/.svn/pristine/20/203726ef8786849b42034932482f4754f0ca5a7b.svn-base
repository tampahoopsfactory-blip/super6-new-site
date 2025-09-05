using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoNetDrive.Protocol.USB.OfflinePatrol.Test.Model
{
    public class PatrolEmplUI 
    {
        public Data.PatrolEmpl PatrolEmpl;

        private static StringBuilder mStrBuf = new StringBuilder(1024);
        /// <summary>
        /// 
        /// </summary>
        public bool Selected { get; set; }

        public string Index { get; set; }
        /// <summary>
        /// 工号
        /// </summary>
        public ushort PCode { get; set; }

        /// <summary>
        /// 包装一个卡信息
        /// </summary>
        /// <param name="card"></param>
        public PatrolEmplUI(Data.PatrolEmpl empl)
        {
            PatrolEmpl = empl;
        }

        /// <summary>
        /// 卡号
        /// 1 - 16777215
        /// </summary>
        public string CardData {
            get
            {
                mStrBuf.Clear();
                mStrBuf.Append(PatrolEmpl.CardData.ToString("d10")).Append("(0x").Append(PatrolEmpl.CardData.ToString("X16")).Append(")");
                return mStrBuf.ToString();
            }
           
        }

        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }
    }
}
