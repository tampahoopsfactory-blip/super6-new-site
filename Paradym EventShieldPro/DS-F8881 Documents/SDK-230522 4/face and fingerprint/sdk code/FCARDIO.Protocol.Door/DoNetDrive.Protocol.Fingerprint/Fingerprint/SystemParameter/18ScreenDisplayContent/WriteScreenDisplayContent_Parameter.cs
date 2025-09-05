using DotNetty.Buffers;
using DoNetDrive.Protocol.Door.Door8800;

namespace DoNetDrive.Protocol.Fingerprint.SystemParameter.ScreenDisplayContent
{
    /// <summary>
    /// 设置 屏幕显示内容 参数
    /// </summary>
    public class WriteScreenDisplayContent_Parameter : AbstractParameter
    {
        /// <summary>
        /// 显示内容选项
        /// 1 - 人名
        /// 2 - 人员编号
        /// 3 - 部门
        /// 4 - 职务
        /// 5 - 人员照片
        /// 6 - 卡号
        /// 7 - 记录照片
        /// 8 - 记录时间
        /// 9 - 用户号
        /// </summary>
        public byte[] DisplayList;

        /// <summary>
        /// 构建一个空的实例
        /// </summary>
        public WriteScreenDisplayContent_Parameter()
        {
            DisplayList = new byte[9];
            DisplayList[0] = 1;
            DisplayList[3] = 1;
            DisplayList[5] = 1;
            DisplayList[4] = 1;
        }

        /// <summary>
        /// 初始化实例
        /// </summary>
        /// <param name="displayList">显示内容选项</param>
        public WriteScreenDisplayContent_Parameter(byte[] displayList)
        {
            DisplayList = displayList;
        }

        /// <summary>
        /// 检查参数
        /// </summary>
        /// <returns></returns>
        public override bool checkedParameter()
        {
            if (DisplayList == null || DisplayList.Length != 9)
            {
                return false;
            }
            foreach (var item in DisplayList)
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
        /// 对记录存储方式参数进行编码
        /// </summary>
        /// <param name="databuf"></param>
        /// <returns></returns>
        public override IByteBuffer GetBytes(IByteBuffer databuf)
        {
            return databuf.WriteBytes(DisplayList);
        }

        /// <summary>
        /// 获取数据长度
        /// </summary>
        /// <returns></returns>
        public override int GetDataLen()
        {
            return 0x09;
        }

        /// <summary>
        /// 对记录存储方式参数进行解码
        /// </summary>
        /// <param name="databuf"></param>
        public override void SetBytes(IByteBuffer databuf)
        {
            databuf.ReadBytes(DisplayList);
        }
    }
}
