using DoNetDrive.Core.Command;
using DoNetDrive.Core.Connector;
using DoNetDrive.Core.Packet;
using DoNetDrive.Protocol.Door.Door8800;
using DoNetDrive.Protocol.OnlineAccess;
using DotNetty.Buffers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoNetDrive.Protocol.Fingerprint.AdditionalData
{
    /// <summary>
    /// 读取大型文件 可读取超过16M的文件
    /// </summary>
    public class ReadFile : BaseCombinedCommand
    {
        /// <summary>
        /// 封装的读取文件指令
        /// </summary>
        ReadFileSubCommand scReadFile;

        /// <summary>
        /// 初始化命令
        /// </summary>
        /// <param name="cd">包含命令所需的远程主机详情 （IP、端口、SN、密码、重发次数等）</param>
        /// <param name="par"></param>
        public ReadFile(INCommandDetail cd, ReadFile_Parameter par) : base(cd, par) { }

        /// <summary>
        /// 将命令打包成一个Packet，准备发送
        /// </summary>
        protected override void CreatePacket0()
        {

            ReadFile_Parameter model = _Parameter as ReadFile_Parameter;
            scReadFile = new ReadFileSubCommand(this);
            scReadFile.BeginRead(model.UserCode, model.Type, model.SerialNumber);

            
        }

        /// <summary>
        /// 检查命令参数
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected override bool CheckCommandParameter(INCommandParameter value)
        {
            ReadFile_Parameter model = value as ReadFile_Parameter;
            if (model == null)
            {
                return false;
            }
            return model.checkedParameter();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="oPck"></param>
        protected override void CommandNext0(OnlineAccessPacket oPck)
        {
            scReadFile?.CommandNext(oPck);
        }


        /// <summary>
        /// 命令执行完毕
        /// </summary>
        /// <param name="subCmd"></param>
        public override void SubCommandOver(ISubCommand subCmd)
        {
            var rst = new ReadFile_Result();
            _Result = rst;
            ReadFileSubCommand sc = subCmd as ReadFileSubCommand;
            rst.UserCode = sc.UserCode;
            rst.FileType = sc.FileType;
            rst.Result = sc.FileResult;
            if(rst.Result)
            {
                rst.FileHandle = sc.FileHandle;
                rst.FileSize = sc.FileSize;
                rst.FileCRC = sc.FileCRC;
                rst.FileDatas = sc.FileDatas;
            }
            scReadFile.Release();
            scReadFile = null;
            CommandCompleted();
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        protected override void Release1()
        {
            base.Release1();
            if (scReadFile != null)
            {
                scReadFile.Release();
                scReadFile = null;
            }
        }
    }
}
