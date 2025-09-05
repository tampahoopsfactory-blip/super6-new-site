using DoNetDrive.Core.Command;
using DoNetDrive.Protocol.Door.Door8800;
using System;

namespace DoNetDrive.Protocol.Fingerprint.AdditionalData
{
    /// <summary>
    /// 删除人员照片/指纹/人脸特征码
    /// </summary>
    public class DeleteFeatureCode : Door8800Command_WriteParameter
    {
        /// <summary>
        /// 初始化命令
        /// </summary>
        /// <param name="cd">包含命令所需的远程主机详情 （IP、端口、SN、密码、重发次数等）</param>
        /// <param name="par"></param>
        public DeleteFeatureCode(INCommandDetail cd, DeleteFeatureCode_Parameter par) : base(cd, par) { }

        /// <summary>
        /// 将命令打包成一个Packet，准备发送
        /// </summary>
        protected override void CreatePacket0()
        {
            DeleteFeatureCode_Parameter model = _Parameter as DeleteFeatureCode_Parameter;
            Packet(0x0B, 0x06, 0x00, Convert.ToUInt32(model.GetDataLen()), model.GetBytes(GetNewCmdDataBuf(model.GetDataLen())));
        }

        /// <summary>
        /// 检查命令参数
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected override bool CheckCommandParameter(INCommandParameter value)
        {
            DeleteFeatureCode_Parameter model = value as DeleteFeatureCode_Parameter;
            if (model == null)
            {
                return false;
            }
            return model.checkedParameter();
        }

        
    }
}
