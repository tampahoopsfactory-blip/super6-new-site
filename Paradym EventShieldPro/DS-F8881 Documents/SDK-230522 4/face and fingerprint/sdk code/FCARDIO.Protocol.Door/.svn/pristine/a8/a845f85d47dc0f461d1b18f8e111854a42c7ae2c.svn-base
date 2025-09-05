using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoNetDrive.Protocol.Door.Door8800.Door.MultiCard
{
    /// <summary>
    /// 读取多卡组合的命令参数
    /// </summary>
    public class ReadMultiCard_Parameter : DoorPort_Parameter
    {
        /// <summary>
        /// 指示仅支持固定组多卡，不支持AB组多卡
        /// </summary>
        public readonly bool IsOnlyGroupFix;

        /// <summary>
        /// 门号参数初始化实例
        /// </summary>
        /// <param name="iDoor">门号</param>
        /// <param name="bOnlyeGroupFix">是否仅支持固定组多卡</param>
        public ReadMultiCard_Parameter(int iDoor,bool bOnlyeGroupFix):base(iDoor)
        {
            IsOnlyGroupFix = bOnlyeGroupFix;
        }
    }
}
