using DoNetDrive.Protocol.Door.Door8800;
using DotNetty.Buffers;
using System;
using System.Net;
using DoNetDrive.Common.Extensions;
using System.Text;

namespace DoNetDrive.Protocol.Fingerprint.SystemParameter
{
    /// <summary>
    /// 设置认证模式的参数
    /// </summary>
    public class WriteAuthenticationMode_Parameter : AbstractParameter
    {
        /// <summary>
        /// 认证模式,取值范围：1-5；
        /// 1、标准模式 默认值；2、人脸+密码；3、卡+人脸；4、多人考勤；5、人证比对
        /// </summary>
        public int AuthenticationMode;

        /// <summary>
        /// 构建一个空的实例
        /// </summary>
        public WriteAuthenticationMode_Parameter() { AuthenticationMode = 1;}

        /// <summary>
        /// 创建设置认证模式的参数
        /// </summary>
        /// <param name="iAuthenticationMode">认证模式,取值范围：1-5；1、标准模式 默认值；2、人脸+密码；3、卡+人脸；4、多人考勤；5、人证比对</param>
        public WriteAuthenticationMode_Parameter(int iAuthenticationMode)
        {
            AuthenticationMode = iAuthenticationMode;
        }

        /// <summary>
        /// 检查参数
        /// </summary>
        /// <returns></returns>
        public override bool checkedParameter()
        {
            if (AuthenticationMode <= 0 || AuthenticationMode >= 6)
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
            return 1;
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
            databuf.WriteByte(AuthenticationMode);

            return databuf;
        }



        /// <summary>
        /// 解码参数
        /// </summary>
        /// <param name="databuf"></param>
        public override void SetBytes(IByteBuffer databuf)
        {

            AuthenticationMode = databuf.ReadByte();
        }
    }
}
