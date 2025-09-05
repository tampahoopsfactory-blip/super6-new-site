using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotNetty.Buffers;

namespace DoNetDrive.Protocol.Door.Door8800.SystemParameter.FunctionParameter
{
    /// <summary>
    /// 设置门内人数限制_参数
    /// </summary>
    public class WriteEnterDoorLimit_Parameter : AbstractParameter
    {
        /// <summary>
        /// 门内人数限制信息
        /// </summary>
        public DoorLimit Limit;

        /// <summary>
        /// 构建一个空的实例
        /// </summary>
        public WriteEnterDoorLimit_Parameter() { }

        /// <summary>
        /// 使用门内人数限制初始化实例
        /// </summary>
        /// <param name="_Limit">门内人数限制信息</param>
        public WriteEnterDoorLimit_Parameter(DoorLimit _Limit)
        {
            Limit = _Limit;
            if (!checkedParameter())
            {
                throw new ArgumentException("Limit Error");
            }
        }

        /// <summary>
        /// 检查参数
        /// </summary>
        /// <returns></returns>
        public override bool checkedParameter()
        {
            if (Limit == null)
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
            Limit = null;

            return;
        }

        /// <summary>
        /// 对门内人数限制参数进行编码
        /// </summary>
        /// <param name="databuf"></param>
        /// <returns></returns>
        public override IByteBuffer GetBytes(IByteBuffer databuf)
        {
            databuf.WriteInt(Convert.ToInt32(Limit.GlobalLimit));

            for (int i = 0; i < 4; i++)
            {
                databuf.WriteInt(Convert.ToInt32(Limit.DoorLimitArray[i]));
            }

            for (int i = 0; i < 4; i++)
            {
                databuf.WriteInt(Convert.ToInt32(Limit.DoorEnterArray[i]));
            }

            return databuf;
        }

        /// <summary>
        /// 获取数据长度
        /// </summary>
        /// <returns></returns>
        public override int GetDataLen()
        {
            return 0x24;
        }

        /// <summary>
        /// 对门内人数限制参数进行解码
        /// </summary>
        /// <param name="databuf"></param>
        public override void SetBytes(IByteBuffer databuf)
        {
            if (Limit == null)
            {
                Limit = new DoorLimit();
            }
            Limit.GlobalLimit = databuf.ReadUnsignedInt(); //全局上限
            for (int i = 0; i < 4; i++) //1-4门上限
            {
                Limit.DoorLimitArray[i] = databuf.ReadUnsignedInt();
            }
            Limit.GlobalEnter = databuf.ReadUnsignedInt(); //全局人数
            for (int j = 0; j < 4; j++) //1-4门人数
            {
                Limit.DoorEnterArray[j] = databuf.ReadUnsignedInt();
            }
        }
    }
}