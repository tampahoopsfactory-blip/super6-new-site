using DotNetty.Buffers;
using DoNetDrive.Core.Data;
using DoNetDrive.Common.Extensions;
using DoNetDrive.Protocol.Util;
using System;

namespace DoNetDrive.Protocol.Elevator.FC8864.Password
{
    /// <summary>
    /// 表示一个密码表
    /// </summary>
    public class PasswordDetail : DoNetDrive.Protocol.Door.Door8800.Password.PasswordDetail
    {
        /// <summary>
        /// 端口集合 1-65
        /// </summary>
        public byte[] DoorNumList;

        /// <summary>
        /// 获取指定门是否有权限
        /// </summary>
        /// <param name="iDoor">门号，取值范围：1-4</param>
        /// <returns>true 有权限，false 无权限</returns>
        public new bool GetDoor(int iDoor)
        {
            if (iDoor < 1 || iDoor > 65)
            {

                throw new ArgumentException("Door 1-65");
            }
            iDoor -= 1;

            int iBitIndex = iDoor % 8;
            int iMaskValue = (int)Math.Pow(2, iBitIndex);
            int iByteValue = Door & iMaskValue;
            if (iBitIndex > 0)
            {
                iByteValue = iByteValue >> (iBitIndex);
            }
            return iByteValue == 1;
        }

        /// <summary>
        /// 判断是否相同
        /// </summary>
        /// <param name="other">要比较的密码</param>
        /// <returns></returns>
        public int CompareTo(PasswordDetail other)
        {
            if (other.Password == Password)
            {
                return 0;
            }

            else
            {
                return -1;
            }
        }

        /// <summary>
        /// 写入 要上传的密码信息
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public override IByteBuffer GetBytes(IByteBuffer data)
        {
            for (int i = 0; i < 8; i++)
            {
                byte[] list = new byte[8];
                for (int j = 0; j < 8; j++)
                {
                    list[j] = DoorNumList[i * 8 + j];
                }

                byte type = DoNetDrive.Common.NumUtil.BitToByte(list);
                data.WriteByte(type);
            }
            data.WriteByte(DoorNumList[64]);
            Password = StringUtil.FillHexString(Password, 8, "F", true);
            StringUtil.HextoByteBuf(Password, data);
            WritePassword(data);
            return data;
        }

        /// <summary>
        /// 获取每个添加密码长度
        /// </summary>
        /// <returns></returns>
        public override int GetDataLen()
        {
            return 13;
        }

        /// <summary>
        /// 获取每个删除密码长度
        /// </summary>
        /// <returns></returns>
        public override int GetDeleteDataLen()
        {
            return 4;
        }

        /// <summary>
        /// 读取密码
        /// </summary>
        /// <param name="data"></param>
        protected override void ReadPassword(IByteBuffer data)
        {

        }

        /// <summary>
        /// 写入密码
        /// </summary>
        /// <param name="data"></param>
        protected override void WritePassword(IByteBuffer data)
        {

        }

        /// <summary>
        /// 读取密码信息
        /// </summary>
        /// <param name="data"></param>
        public override void SetBytes(IByteBuffer data)
        {
            DoorNumList = new byte[65];
            for (int i = 0; i < 8; i++)
            {
                byte type = data.ReadByte();
                var bytelist = DoNetDrive.Common.NumUtil.ByteToBit(type);
                for (int j = 0; j < 8; j++)
                {
                    DoorNumList[i * 8 + j] = bytelist[j];
                }
            }
            DoorNumList[64] = data.ReadByte();
            //byte[] btData = new byte[4];
            //data.ReadBytes(btData, 0, 4);
            //Password = btData.ToHex().TrimEnd('F');
            Password = StringUtil.ByteBufToHex(data, 4);
            ReadPassword(data);
           
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
    }
}
