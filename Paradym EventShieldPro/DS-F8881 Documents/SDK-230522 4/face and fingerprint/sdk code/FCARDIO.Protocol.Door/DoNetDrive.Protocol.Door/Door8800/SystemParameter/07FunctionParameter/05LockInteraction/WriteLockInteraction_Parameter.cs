using DotNetty.Buffers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoNetDrive.Protocol.Door.Door8800.SystemParameter.FunctionParameter
{
    /// <summary>
    /// 设置互锁参数_参数
    /// </summary>
    public class WriteLockInteraction_Parameter : AbstractParameter
    {
        /// <summary>
        /// 互锁参数信息
        /// </summary>
        public DoorPortDetail DoorPort;

        /// <summary>
        /// 构建一个空的实例
        /// </summary>
        public WriteLockInteraction_Parameter() { }

        /// <summary>
        /// 使用互锁参数信息初始化实例
        /// </summary>
        /// <param name="_DoorPort">互锁参数信息</param>
        public WriteLockInteraction_Parameter(DoorPortDetail _DoorPort)
        {
            DoorPort = _DoorPort;
            if (!checkedParameter())
            {
                throw new ArgumentException("TCP Error");
            }
        }

        /// <summary>
        /// 检查参数
        /// </summary>
        /// <returns></returns>
        public override bool checkedParameter()
        {
            if (DoorPort == null || DoorPort.DoorMax != 4)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public override void Dispose()
        {
            DoorPort = null;

            return;
        }

        /// <summary>
        /// 对互锁参数进行编码
        /// </summary>
        /// <param name="databuf"></param>
        /// <returns></returns>
        public override IByteBuffer GetBytes(IByteBuffer databuf)
        {
            for (int i = 0; i < DoorPort.DoorMax; i++)
            {
                databuf.WriteByte(DoorPort.DoorPort[i]);
            }
            return databuf;
        }

        /// <summary>
        /// 获取数据长度
        /// </summary>
        /// <returns></returns>
        public override int GetDataLen()
        {
            return 0x04;
        }

        /// <summary>
        /// 对互锁参数进行解码
        /// </summary>
        /// <param name="databuf"></param>
        public override void SetBytes(IByteBuffer databuf)
        {
            if (DoorPort == null)
            {
                DoorPort = new DoorPortDetail(4);
            }
            for (int i = 0; i < DoorPort.DoorMax; i++)
            {
                DoorPort.DoorPort[i] = databuf.ReadByte();
            }
        }
    }
}