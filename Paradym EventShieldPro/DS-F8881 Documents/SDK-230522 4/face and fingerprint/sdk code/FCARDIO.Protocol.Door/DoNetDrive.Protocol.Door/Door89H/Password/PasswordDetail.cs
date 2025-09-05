using DotNetty.Buffers;
using DoNetDrive.Protocol.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoNetDrive.Protocol.Door.Door89H.Password
{
    /// <summary>
    /// Door89H 表示一个密码表
    /// </summary>
    public class PasswordDetail : Door8800.Password.PasswordDetail
    {
       
        /// <summary>
        /// 将密码序列化并写入buf中
        /// </summary>
        /// <param name="data"></param>
        protected override void WritePassword(IByteBuffer data)
        {
            data.WriteShort(OpenTimes);
            TimeUtil.DateToBCD_yyMMddhhmm(data, Expiry);
        }

        /// <summary>
        /// 从buf中读取密码
        /// </summary>
        /// <param name="data"></param>
        protected override void ReadPassword(IByteBuffer data)
        {
            OpenTimes = data.ReadUnsignedShort();
            Expiry = TimeUtil.BCDTimeToDate_yyMMddhhmm(data);
        }

        /// <summary>
        /// 写入 要删除的密码信息
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public override IByteBuffer GetDeleteBytes(IByteBuffer data)
        {
            Password = StringUtil.FillHexString(Password, 8, "F", true);
            StringUtil.HextoByteBuf(Password, data);
            return data;
        }

        /// <summary>
        /// 获取每个添加密码长度
        /// </summary>
        /// <returns></returns>
        public override int GetDataLen()
        {
            return 12;
        }

        /// <summary>
        /// 获取每个删除密码长度
        /// </summary>
        /// <returns></returns>
        public override int GetDeleteDataLen()
        {
            return 4;
        }
    }
}
