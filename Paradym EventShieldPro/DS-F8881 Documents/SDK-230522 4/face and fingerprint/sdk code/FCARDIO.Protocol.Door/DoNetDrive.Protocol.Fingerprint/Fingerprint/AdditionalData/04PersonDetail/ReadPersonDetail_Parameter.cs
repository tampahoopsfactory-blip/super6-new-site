using DotNetty.Buffers;
using DoNetDrive.Protocol.Door.Door8800;

namespace DoNetDrive.Protocol.Fingerprint.AdditionalData
{
    /// <summary>
    /// 查询人员附加数据详情
    /// </summary>
    public class ReadPersonDetail_Parameter : AbstractParameter
    {
        /// <summary>
        /// 用户号
        /// </summary>
        public uint UserCode;


        /// <summary>
        /// 初始化参数
        /// </summary>
        /// <param name="userCode">用户号</param>
        /// <param name="type">文件类型</param>
        /// <param name="serialNumber">序号</param>
        public ReadPersonDetail_Parameter(uint userCode)
        {
            UserCode = userCode;
        }

        /// <summary>
        /// 检查参数
        /// </summary>
        /// <returns></returns>
        public override bool checkedParameter()
        {
            if (UserCode < 0)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 获取数据长度
        /// </summary>
        /// <returns></returns>
        public override int GetDataLen()
        {
            return 4;
        }


        /// <summary>
        /// 释放资源
        /// </summary>
        public override void Dispose()
        {
            return;
        }

        /// <summary>
        /// 编码参数
        /// </summary>
        /// <param name="databuf"></param>
        /// <returns></returns>
        public override IByteBuffer GetBytes(IByteBuffer databuf)
        {
            databuf.WriteInt((int)UserCode);
            return databuf;
        }



        /// <summary>
        /// 解码参数
        /// </summary>
        /// <param name="databuf"></param>
        public override void SetBytes(IByteBuffer databuf)
        {

        }
    }
}
