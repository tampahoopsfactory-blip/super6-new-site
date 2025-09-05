using DotNetty.Buffers;

namespace DoNetDrive.Protocol.Elevator.FC8864.Door.UnlockingTime
{
    /// <summary>
    /// 设置开锁时输出时长
    /// </summary>
    public class WriteUnlockingTime_Parameter : AbstractParameter
    {
        /// <summary>
        /// 开锁时输出时长
        /// </summary>
        public ushort[] TimeList;

        /// <summary>
        /// 构建一个空的实例
        /// </summary>
        public WriteUnlockingTime_Parameter()
        {
        }

        /// <summary>
        /// 参数初始化实例
        /// </summary>
        /// <param name="_TimeErrorCorrection">误差自修正参数</param>
        public WriteUnlockingTime_Parameter(ushort[] timeList)
        {
            TimeList = timeList;

        }

        /// <summary>
        /// 检查参数
        /// </summary>
        /// <returns></returns>
        public override bool checkedParameter()
        {
            if (TimeList == null || TimeList.Length != 65)
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
            return;
        }

        /// <summary>
        /// 对误差自修正参数进行编码
        /// </summary>
        /// <param name="databuf"></param>
        /// <returns></returns>
        public override IByteBuffer GetBytes(IByteBuffer databuf)
        {
            foreach (var item in TimeList)
            {
                databuf.WriteUnsignedShort(item);
            }
            return databuf;
        }

        /// <summary>
        /// 获取数据长度
        /// </summary>
        /// <returns></returns>
        public override int GetDataLen()
        {
            return 0x82;
        }

        /// <summary>
        /// 对误差自修正参数进行解码
        /// </summary>
        /// <param name="databuf"></param>
        public override void SetBytes(IByteBuffer databuf)
        {
            TimeList = new ushort[65];
            for (int i = 0; i < 65; i++)
            {
                TimeList[i] = databuf.ReadUnsignedShort();
            }
        }
    }
}
