using DotNetty.Buffers;
using DoNetDrive.Protocol.Door.Door8800;
using DoNetDrive.Common.Extensions;

namespace DoNetDrive.Protocol.Fingerprint.SystemParameter.DataEncryptionSwitch
{
    /// <summary>
    /// 数据包加密开关
    /// </summary>
    public class WriteDataEncryptionSwitch_Parameter : AbstractParameter
    {
        /// <summary>
        /// 开关 (1)
        /// </summary>
        public bool IsUse;

        /// <summary>
        /// 密钥
        /// </summary>
        public string SecretKey;

        /// <summary>
        /// 提供给继承类
        /// </summary>
        public WriteDataEncryptionSwitch_Parameter()
        {

        }

        /// <summary>
        /// 初始化参数
        /// </summary>
        /// <param name="isUse">开关</param>
        /// <param name="secretKey">密钥</param>
        public WriteDataEncryptionSwitch_Parameter(bool isUse, string secretKey)
        {
            IsUse = isUse;
            SecretKey = secretKey;
        }

        /// <summary>
        /// 检查参数
        /// </summary>
        /// <returns></returns>
        public override bool checkedParameter()
        {
            if (!SecretKey.IsHex())
            {
                return false;
            }
            if (SecretKey.Length < 64)
            {
                SecretKey =  new string('0', 64 - SecretKey.Length) + SecretKey;
            }
            if (SecretKey.Length > 64)
            {
                SecretKey = SecretKey.Substring(0, 64);
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
            databuf.WriteBoolean(IsUse);
            Util.StringUtil.HextoByteBuf(SecretKey, databuf);
            return databuf;
        }

        /// <summary>
        /// 获取数据长度
        /// </summary>
        /// <returns></returns>
        public override int GetDataLen()
        {
            return 33;
        }

        /// <summary>
        /// 对记录存储方式参数进行解码
        /// </summary>
        /// <param name="databuf"></param>
        public override void SetBytes(IByteBuffer databuf)
        {
            IsUse = databuf.ReadBoolean();
            SecretKey = ByteBufferUtil.HexDump(databuf);
        }
    }
}
