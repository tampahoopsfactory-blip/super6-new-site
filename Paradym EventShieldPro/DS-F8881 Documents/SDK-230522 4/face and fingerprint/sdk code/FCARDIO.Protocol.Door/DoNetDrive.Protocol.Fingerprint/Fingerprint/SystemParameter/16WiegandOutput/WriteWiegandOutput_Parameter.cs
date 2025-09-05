using DotNetty.Buffers;
using DoNetDrive.Protocol.Door.Door8800;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoNetDrive.Protocol.Fingerprint.SystemParameter.WiegandOutput
{
    /// <summary>
    /// 设置 韦根输出
    /// </summary>
    public class WriteWiegandOutput_Parameter : AbstractParameter
    {
        /// <summary>
        /// 读卡字节
        /// 1 - 韦根26(三字节)
        /// 2 - 韦根34(四字节)
        /// 3 - 韦根26(二字节)
        /// 4 - 韦根66(八字节)
        /// 5 - 禁用
        /// </summary>
        public byte ReadCardByte;

        /// <summary>
        /// WG输出功能开关
        /// 1 - 启用
        /// 2 - 禁用
        /// </summary>
        public byte WGOutputSwitch;

        /// <summary>
        /// WG字节顺序
        /// 1 - 高位在前低位在后
        /// 2 - 低位在前高位在后
        /// </summary>
        public byte WGByteSort;

        /// <summary>
        /// 输出数据类型
        /// 1 - 输出用户号
        /// 2 - 输出人员卡号
        /// </summary>
        public byte OutputType;
        /// <summary>
        /// 构建一个空的实例
        /// </summary>
        public WriteWiegandOutput_Parameter() { }

        /// <summary>
        /// 初始化实例
        /// </summary>
        /// <param name="readCardByte">读卡字节</param>
        /// <param name="wGOutputSwitch">WG输出功能开关</param>
        /// <param name="wGByteSort">WG字节顺序</param>
        /// <param name="outputType">输出数据类型</param>
        public WriteWiegandOutput_Parameter(byte readCardByte , byte wGOutputSwitch, byte wGByteSort, byte outputType)
        {
            ReadCardByte = readCardByte;
            WGOutputSwitch = wGOutputSwitch;
            WGByteSort = wGByteSort;
            OutputType = outputType;
        }

        /// <summary>
        /// 检查参数
        /// </summary>
        /// <returns></returns>
        public override bool checkedParameter()
        {
            if (ReadCardByte > 5 || ReadCardByte < 1)
            {
                return false;
            }
            if (WGOutputSwitch > 2 || ReadCardByte < 1)
            {
                return false;
            }
            if (WGByteSort > 2 || WGByteSort < 1)
            {
                return false;
            }
            if (OutputType > 2 || OutputType < 1)
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
        /// 对记录存储方式参数进行编码
        /// </summary>
        /// <param name="databuf"></param>
        /// <returns></returns>
        public override IByteBuffer GetBytes(IByteBuffer databuf)
        {
            databuf.WriteByte(ReadCardByte);
            databuf.WriteByte(WGOutputSwitch);
            databuf.WriteByte(WGByteSort);
            databuf.WriteByte(OutputType);
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
        /// 对记录存储方式参数进行解码
        /// </summary>
        /// <param name="databuf"></param>
        public override void SetBytes(IByteBuffer databuf)
        {
            ReadCardByte = databuf.ReadByte();
            WGOutputSwitch = databuf.ReadByte();
            WGByteSort = databuf.ReadByte();
            OutputType = databuf.ReadByte();
        }
    }
}
