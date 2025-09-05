using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotNetty.Buffers;

namespace DoNetDrive.Protocol.Door.Door8800.SystemParameter.FunctionParameter
{
    /// <summary>
    /// 设置语音播报语音段开关_参数
    /// </summary>
    public class WriteBroadcast_Parameter : AbstractParameter
    {
        /// <summary>
        /// 语音播报语音段开关（语音段对照可参考《Door8800语音表》 每个开关true 表示启用，false 表示禁用）
        /// </summary>
        public BroadcastDetail Broadcast;

        /// <summary>
        /// 构建一个空的实例
        /// </summary>
        public WriteBroadcast_Parameter() { }

        /// <summary>
        /// 使用语音播报语音段开关参数初始化实例
        /// </summary>
        /// <param name="data">语音播报语音段开关参数</param>
        public WriteBroadcast_Parameter(byte[] data)
        {
            Broadcast = new BroadcastDetail(data);
        }

        /// <summary>
        /// 检查参数
        /// </summary>
        /// <returns></returns>
        public override bool checkedParameter()
        {
            if (Broadcast == null)
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
        /// 对语音播报语音段开关参数进行编码
        /// </summary>
        /// <param name="databuf"></param>
        /// <returns></returns>
        public override IByteBuffer GetBytes(IByteBuffer databuf)
        {
            return databuf.WriteBytes(Broadcast.Broadcast);
        }

        /// <summary>
        /// 获取数据长度
        /// </summary>
        /// <returns></returns>
        public override int GetDataLen()
        {
            return 0x0A;
        }

        /// <summary>
        /// 对语音播报语音段开关参数进行解码
        /// </summary>
        /// <param name="databuf"></param>
        public override void SetBytes(IByteBuffer databuf)
        {
            if (Broadcast == null)
            {
                Broadcast = new BroadcastDetail();
            }
            for (int i = 0; i < 10; i++)
            {
                Broadcast.Broadcast[i] = databuf.ReadByte();
            }
        }
    }
}