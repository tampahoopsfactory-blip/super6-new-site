using DotNetty.Buffers;
using DoNetDrive.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace DoNetDrive.Protocol.Door.Door8800.SystemParameter.FunctionParameter
{
    /// <summary>
    /// 设置智能防盗主机参数_参数
    /// </summary>
    public class WriteTheftAlarmSetting_Parameter : AbstractParameter
    {
        /// <summary>
        /// 防盗报警参数信息
        /// </summary>
        public TheftAlarmSetting Setting;

        /// <summary>
        /// 构建一个空的实例
        /// </summary>
        public WriteTheftAlarmSetting_Parameter() { }

        /// <summary>
        /// 使用防盗报警参数信息初始化实例
        /// </summary>
        /// <param name="_Setting">防盗报警参数信息</param>
        public WriteTheftAlarmSetting_Parameter(TheftAlarmSetting _Setting)
        {
            Setting = _Setting;
            if (!checkedParameter())
            {
                throw new ArgumentException("Setting Error");
            }
        }

        /// <summary>
        /// 检查参数
        /// </summary>
        /// <returns></returns>
        public override bool checkedParameter()
        {
            if (Setting == null)
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
            Setting = null;

            return;
        }

        /// <summary>
        /// 对防盗报警参数进行编码
        /// </summary>
        /// <param name="databuf"></param>
        /// <returns></returns>
        public override IByteBuffer GetBytes(IByteBuffer databuf)
        {
            databuf.WriteBoolean(Setting.Use);
            databuf.WriteByte(Setting.InTime);
            databuf.WriteByte(Setting.OutTime);
            Setting.BeginPassword = Utility.StringUtility.FillString(Setting.BeginPassword, 8, "F", false);
            Setting.ClosePassword = Utility.StringUtility.FillString(Setting.ClosePassword, 8, "F", false);
            databuf.WriteBytes(Setting.BeginPassword.HexToByte());
            databuf.WriteBytes(Setting.ClosePassword.HexToByte());
            databuf.WriteUnsignedShort(Setting.AlarmTime);
            return databuf;
        }


        /// <summary>
        /// 获取数据长度
        /// </summary>
        /// <returns></returns>
        public override int GetDataLen()
        {
            return 0x0D;
        }

        /// <summary>
        /// 对防盗报警参数进行解码
        /// </summary>
        /// <param name="databuf"></param>
        public override void SetBytes(IByteBuffer databuf)
        {
            if (Setting == null)
            {
                Setting = new TheftAlarmSetting();
            }
            Setting.Use = databuf.ReadBoolean();
            Setting.InTime = databuf.ReadByte();
            Setting.OutTime = databuf.ReadByte();

            byte[] btPwd = new byte[4];
            databuf.ReadBytes(btPwd, 0, 4);
            Setting.BeginPassword = ByteBufferUtil.HexDump(btPwd).Replace("f","");

            databuf.ReadBytes(btPwd, 0, 4);
            Setting.ClosePassword = ByteBufferUtil.HexDump(btPwd).Replace("f", "");

            Setting.AlarmTime = databuf.ReadUnsignedShort();
            return;
        }
    }
}