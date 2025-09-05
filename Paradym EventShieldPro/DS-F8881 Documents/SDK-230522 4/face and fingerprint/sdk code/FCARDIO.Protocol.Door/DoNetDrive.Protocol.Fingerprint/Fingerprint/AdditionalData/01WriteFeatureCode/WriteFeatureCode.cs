using DoNetDrive.Core.Command;
using DoNetDrive.Protocol.Door.Door8800;
using DoNetDrive.Protocol.OnlineAccess;
using System;

namespace DoNetDrive.Protocol.Fingerprint.AdditionalData
{
    /// <summary>
    /// 写入头像照片\指纹\人脸特征码
    /// </summary>
    public class WriteFeatureCode : BaseCombinedCommand
    {
        /// <summary>
        /// 封装的写文件命令
        /// </summary>
        WriteFileSubCommand _SubCommand;

        /// <summary>
        /// 初始化命令
        /// </summary>
        /// <param name="cd">包含命令所需的远程主机详情 （IP、端口、SN、密码、重发次数等）</param>
        /// <param name="par"></param>
        public WriteFeatureCode(INCommandDetail cd, WriteFeatureCode_Parameter par) : base(cd, par) { }

        /// <summary>
        /// 将命令打包成一个Packet，准备发送
        /// </summary>
        protected override void CreatePacket0()
        {

            WriteFeatureCode_Parameter model = _Parameter as WriteFeatureCode_Parameter;
            _SubCommand = new WriteFileSubCommand(this);
            _SubCommand.WaitRepeatMessage = model.WaitRepeatMessage;
            _SubCommand.WaitVerifyTime = model.WaitVerifyTime;
            _SubCommand.BeginWrite(model.UserCode, model.FileType, model.FileNum, model.FileDatas);
        }

        /// <summary>
        /// 检查命令参数
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected override bool CheckCommandParameter(INCommandParameter value)
        {
            WriteFeatureCode_Parameter model = value as WriteFeatureCode_Parameter;
            if (model == null)
            {
                return false;
            }
            return model.checkedParameter();

        }


        /// <summary>
        /// 检查返回值
        /// </summary>
        /// <param name="oPck"></param>
        protected override void CommandNext0(OnlineAccessPacket oPck)
        {
            _SubCommand?.CommandNext(oPck);

        }
        /// <summary>
        /// 命令执行完毕
        /// </summary>
        /// <param name="subCmd"></param>
        public override void SubCommandOver(ISubCommand subCmd)
        {
            var rst = new WriteFeatureCode_Result();
            _Result = rst;
            WriteFileSubCommand sc = subCmd as WriteFileSubCommand;
            rst.FileHandle = sc.FileHandle;
            rst.Result = sc.FileResult;
            rst.RepeatUser = sc.RepeatUser;
            subCmd.Release();
            _SubCommand = null;
            CommandCompleted();
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        protected override void Release1()
        {
            base.Release1();
            if(_SubCommand != null)
            {
                _SubCommand.Release();
                _SubCommand = null;
            }
        }



    }
}
