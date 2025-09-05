using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotNetty.Buffers;

namespace DoNetDrive.Protocol.Elevator.FC8864.Door.Relay
{
    /// <summary>
    /// 继电器 参数
    /// </summary>
    public class WriteRelay_Parameter : AbstractParameter
    {
        /// <summary>
        /// 继电器输出方式 集合（共65个，第65继电器是代按继电器）
        /// 1 - 不输出（默认）
        /// 2 - 输出
        /// 3 - 读卡切换输出状态（当读到合法卡后自动自动切换到当前相反的状态。）例如卷闸门
        /// </summary>
        public byte[] OutputFormatList;
        /// <summary>
        /// 提供给继承类
        /// </summary>
        public WriteRelay_Parameter()
        {

        }

        /// <summary>
        /// 初始化参数
        /// </summary>
        /// <param name="list">继电器输出方式 集合</param>
        public WriteRelay_Parameter(byte[] list)
        {
            OutputFormatList = list;
        }
        /// <summary>
        /// 检查参数
        /// </summary>
        /// <returns></returns>
        public override bool checkedParameter()
        {
            if (OutputFormatList == null || OutputFormatList.Length != 65)
            {
                return false;
            }
            foreach (int item in OutputFormatList)
            {
                if (item < 1 || item > 3)
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

        }

        /// <summary>
        /// 对参数进行编码
        /// </summary>
        /// <param name="databuf"></param>
        /// <returns></returns>
        public override IByteBuffer GetBytes(IByteBuffer databuf)
        {

            return databuf.WriteBytes(OutputFormatList) ;
        }

        /// <summary>
        /// 获取数据长度
        /// </summary>
        /// <returns></returns>
        public override int GetDataLen()
        {
            return 0x41;
        }

        /// <summary>
        /// 对参数进行解码
        /// </summary>
        /// <param name="databuf"></param>
        public override void SetBytes(IByteBuffer databuf)
        {
            OutputFormatList = new byte[65];
            databuf.ReadBytes(OutputFormatList);
        }
    }
}
