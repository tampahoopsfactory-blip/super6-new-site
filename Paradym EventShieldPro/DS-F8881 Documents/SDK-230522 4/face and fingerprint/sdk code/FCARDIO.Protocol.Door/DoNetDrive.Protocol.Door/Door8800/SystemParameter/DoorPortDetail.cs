using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoNetDrive.Protocol.Door.Door8800.SystemParameter
{
    /// <summary>
    /// 互锁参数信息
    /// </summary>
    public class DoorPortDetail
    {
        /// <summary>
        /// 最大门数
        /// </summary>
        public int DoorMax;

        /// <summary>
        /// 门的端口
        /// </summary>
        public byte[] DoorPort;

        /// <summary>
        /// 初始化一个4端口集合
        /// </summary>
        public DoorPortDetail() : this((int)4) { }

        /// <summary>
        /// 设置几个门的端口
        /// </summary>
        /// <param name="_DoorMax">最大门数</param>
        public DoorPortDetail(int _DoorMax)
        {
            DoorMax = _DoorMax;
            DoorPort = new byte[_DoorMax];
        }

        /// <summary>
        /// 设置值
        /// </summary>
        /// <param name="iPort">1-4</param>
        /// <param name="value"></param>
        public void SetValue(int iPort, byte value)
        {
            DoorPort[iPort - 1] = value;
        }
        /// <summary>
        /// 获取值
        /// </summary>
        /// <param name="iPort">1-4</param>
        /// <returns></returns>
        public byte GetValue(int iPort)
        {
            return DoorPort[iPort - 1];
        }
    }
}