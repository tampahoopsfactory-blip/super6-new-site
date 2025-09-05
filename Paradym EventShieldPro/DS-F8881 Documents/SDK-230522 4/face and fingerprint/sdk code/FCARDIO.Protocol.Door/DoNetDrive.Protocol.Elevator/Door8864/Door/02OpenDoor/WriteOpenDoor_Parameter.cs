using DotNetty.Buffers;

namespace DoNetDrive.Protocol.Elevator.FC8864.Door.OpenDoor
{
    /// <summary>
    /// 普通开门
    /// </summary>
    public class WriteOpenDoor_Parameter : AbstractParameter
    {
        /// <summary>
        /// 端口集合 1-72
        /// </summary>
        public byte[] DoorNumList;

        /// <summary>
        /// 验证码
        /// 硬件加电时复位为FF，取值：1-254，作用，验证此命令是否重复发送
        /// </summary>
        public byte Code;

        /// <summary>
        /// 初始化实例
        /// </summary>
        /// <param name="doorNumList">端口集合</param>
        public WriteOpenDoor_Parameter(byte[] doorNumList)
        {
            DoorNumList = doorNumList;
        }

        /// <summary>
        /// 初始化实例
        /// </summary>
        /// <param name="doorNumList">端口集合</param>
        /// <param name="code">验证码</param>
        public WriteOpenDoor_Parameter(byte[] doorNumList, byte code)
        {
            DoorNumList = doorNumList;
            Code = code;
        }

        /// <summary>
        /// 检查参数
        /// </summary>
        /// <returns></returns>
        public override bool checkedParameter()
        {
            if (DoorNumList == null || DoorNumList.Length != 72)
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
            if (Code > 0)
            {
                databuf.WriteByte(Code);
            }
            for (int i = 0; i < 9; i++)
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
            if (Code > 0)
                return 0x0A;
            else
                return 0x09;
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
