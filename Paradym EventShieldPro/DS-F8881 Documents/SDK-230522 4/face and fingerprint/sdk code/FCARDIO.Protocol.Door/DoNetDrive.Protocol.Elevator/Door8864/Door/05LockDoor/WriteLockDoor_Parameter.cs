using DotNetty.Buffers;

namespace DoNetDrive.Protocol.Elevator.FC8864.Door.LockDoor
{
    /// <summary>
    /// 设置门锁定参数
    /// </summary>
    public class WriteLockDoor_Parameter : AbstractParameter
    {
        /// <summary>
        /// 端口集合 1-64
        /// </summary>
        public byte[] DoorNumList;

        /// <summary>
        /// 初始化实例
        /// </summary>
        /// <param name="doorNumList">端口集合</param>
        public WriteLockDoor_Parameter(byte[] doorNumList)
        {
            DoorNumList = doorNumList;
        }

        /// <summary>
        /// 检查参数
        /// </summary>
        /// <returns></returns>
        public override bool checkedParameter()
        {
            if (DoorNumList == null || DoorNumList.Length != 64)
            {
                return false;
            }
            foreach (var item in DoorNumList)
            {
                if (item > 1)
                {
                    return false;
                }
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
           
            for (int i = 0; i < 8; i++)
            {
                byte[] list = new byte[8];
                for (int j = 0; j < 8; j++)
                {
                    list[j] = DoorNumList[i * 8 + j];
                }
               
                byte type = DoNetDrive.Common.NumUtil.BitToByte(list);
                databuf.WriteByte(type);
            }
            

            return databuf;
        }

        /// <summary>
        /// 获取数据长度
        /// </summary>
        /// <returns></returns>
        public override int GetDataLen()
        {

            return 0x08;
        }

        /// <summary>
        /// 对误差自修正参数进行解码
        /// </summary>
        /// <param name="databuf"></param>
        public override void SetBytes(IByteBuffer databuf)
        {
          
        }
    }
}
