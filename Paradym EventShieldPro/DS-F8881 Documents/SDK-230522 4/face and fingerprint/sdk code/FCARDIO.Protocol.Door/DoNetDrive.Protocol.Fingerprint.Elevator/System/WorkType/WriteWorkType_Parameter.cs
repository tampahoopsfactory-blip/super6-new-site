using System;
using System.Collections.Generic;
using System.Text;
using DotNetty.Buffers;
using DoNetDrive.Protocol.Door.Door8800;

namespace DoNetDrive.Protocol.Fingerprint.Elevator
{
    /// <summary>
    /// 设置电梯工作模式的参数
    /// </summary>
    public class WriteWorkType_Parameter : AbstractParameter
    {
        /// <summary>
        /// 电梯工作模式
        /// 0--禁用电梯模式
        /// 1--启动电梯模式
        /// </summary>
        public byte WorkType;

        /// <summary>
        /// 构建一个空的实例
        /// </summary>
        public WriteWorkType_Parameter() { }

        /// <summary>
        /// 创建设置电梯工作模式的参数
        /// </summary>
        /// <param name="WorkType">电梯工作模式</param>
        public WriteWorkType_Parameter(byte WorkType)
        {
            this.WorkType = WorkType;
            checkedParameter();
        }

        /// <summary>
        /// 检查参数
        /// </summary>
        /// <returns></returns>
        public override bool checkedParameter()
        {
            if (WorkType < 0 || WorkType > 1)
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
        }

        /// <summary>
        /// 对电梯工作模式进行编码
        /// </summary>
        /// <param name="databuf"></param>
        /// <returns></returns>
        public override IByteBuffer GetBytes(IByteBuffer databuf)
        {
            return databuf.WriteByte(WorkType);
        }

        /// <summary>
        /// 获取数据长度
        /// </summary>
        /// <returns></returns>
        public override int GetDataLen()
        {
            return 1;
        }

        /// <summary>
        /// 对电梯工作模式进行解码
        /// </summary>
        /// <param name="databuf"></param>
        public override void SetBytes(IByteBuffer databuf)
        {

            WorkType = databuf.ReadByte();
        }
    }
}
