using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoNetDrive.Protocol.POS.SystemParameter.ReceiptPrint
{
    public class PrintContent
    {
        /// <summary>
        /// 序号
        /// </summary>
        public byte Index { get; set; }

        /// <summary>
        /// 启用开关
        /// </summary>
        public byte IsOpen { get; set; }

        /// <summary>
        /// 自定义内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 打印位置
        /// </summary>
        public byte Location { get; set; }

        public bool ShowIsOpen
        {
            get
            {
                return IsOpen == 1;
            }
        }

        public string ShowLocation
        {
            get
            { 
                    return Location == 1  ?"页头" :"页尾";
            }
        }
    }
}
