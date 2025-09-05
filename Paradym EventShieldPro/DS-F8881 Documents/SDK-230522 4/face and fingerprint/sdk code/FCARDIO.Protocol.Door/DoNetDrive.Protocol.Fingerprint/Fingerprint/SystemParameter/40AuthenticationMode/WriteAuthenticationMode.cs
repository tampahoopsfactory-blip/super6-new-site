using System;
using DoNetDrive.Core.Command;
using DoNetDrive.Protocol.Door.Door8800;

namespace DoNetDrive.Protocol.Fingerprint.SystemParameter
{
    /// <summary>
    /// 设置人脸机认证模式
    /// 2021年2月22日 添加 人脸机固件 5.36以上版本支持
    /// 1、标准模式 默认值；2、人脸+密码；3、卡+人脸；4、多人考勤；5、人证比对
    /// </summary>
    public class WriteAuthenticationMode : Door8800Command_WriteParameter
    {
        /// <summary>
        /// 创建设置人脸机认证模式的命令
        /// </summary>
        /// <param name="cd">包含命令所需的远程主机详情 （IP、端口、SN、密码、重发次数等）</param>
        /// <param name="par"></param>
        public WriteAuthenticationMode(INCommandDetail cd, WriteAuthenticationMode_Parameter par) : base(cd, par) { }

        /// <summary>
        /// 将命令打包成一个Packet，准备发送
        /// </summary>
        protected override void CreatePacket0()
        {
            var model = _Parameter as WriteAuthenticationMode_Parameter;
            Packet(0x01, 0x33, 0x00, 1, model.GetBytes(GetNewCmdDataBuf(1)));
        }
    }
}
