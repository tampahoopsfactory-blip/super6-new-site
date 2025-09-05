using DoNetDrive.Core.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DoNetDrive.Common.Extensions;
using DoNetDrive.Core.Connector;

namespace DoNetDrive.Protocol.POS.Protocol
{
    /// <summary>
    /// 支持 DES加密协议的设备命令详情，包含设备SN和秘钥
    /// </summary>
    public class DESDriveCommandDetail : AbstractCommandDetail
    {
        /// <summary>
        /// 用于ASCII 字符串数据编解码
        /// </summary>
        protected static Encoding ASCII = Encoding.ASCII;

        /// <summary>
        /// 目标设备的SN
        /// </summary>
        protected byte[] mSN;
        /// <summary>
        /// 数据包的加密秘钥
        /// </summary>
        protected byte[] mPassword;

        /// <summary>
        /// 创建一个用于DES加密协议的命令详情，包含SN和key
        /// </summary>
        /// <param name="cnt">通道详情</param>
        /// <param name="dstSN">目标SN</param>
        /// <param name="key">命令包的秘钥</param>
        public DESDriveCommandDetail(INConnectorDetail cnt, string dstSN, string key) : base(cnt)
        {

            SN = dstSN;
            Password = key;
        }

        /// <summary>
        /// 创建一个用于DES加密协议的命令详情，包含SN和key
        /// </summary>
        /// <param name="cnt">通道详情</param>
        /// <param name="dstSN">目标SN</param>
        /// <param name="key">命令包的秘钥</param>
        public DESDriveCommandDetail(INConnectorDetail cnt, byte[] dstSN, byte[] key) : base(cnt)
        {
            SNByte = dstSN;
            PasswordByte = key;
        }

        /// <summary>
        /// 目标设备的SN
        /// </summary>
        public virtual byte[] SNByte
        {
            get
            {
                return mSN;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentException("SN Is Error");
                }
                if (value.Length != 16)
                {
                    throw new ArgumentException("SN Is Error");
                }
                mSN = value;
            }
        }

        /// <summary>
        /// 数据包的秘钥
        /// </summary>
        public virtual byte[] PasswordByte
        {
            get
            {
                return mPassword;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentException("Password Is Error");
                }
                if (value.Length != 8)
                {
                    throw new ArgumentException("Password Is Error");
                }
                mPassword = value;
            }

        }

        /// <summary>
        /// 目标设备的SN字符串
        /// </summary>
        public virtual string SN
        {
            get
            {
                if (mSN == null) return string.Empty;
                return ASCII.GetString(mSN);
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentException("SN Is Error");
                }
                if (ASCII.GetByteCount(value) != 16)
                {
                    throw new ArgumentException("SN Is Error");
                }

                mSN = ASCII.GetBytes(value);
            }
        }

        /// <summary>
        /// 数据包的秘钥十六进制字符串
        /// </summary>
        public virtual string Password
        {
            get
            {
                if (mPassword == null) return string.Empty;

                return mPassword.ToHex();
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentException("Password Is Error");
                }
                if (ASCII.GetByteCount(value) != 16)
                {
                    throw new ArgumentException("Password Is Error");
                }
                if (!value.IsHex())
                {
                    throw new ArgumentException("Password Is Error");
                }

                mPassword = value.HexToByte();
            }
        }

        /// <summary>
        /// 返回一个浅表副本
        /// </summary>
        /// <returns></returns>
        public override object Clone()
        {
            return MemberwiseClone();
        }

        /// <summary>
        /// 比较连接通道指向和SN是否相同
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public override bool Equals(INCommandDetail other)
        {
            if (other is DESDriveCommandDetail)
            {
                DESDriveCommandDetail sur = other as DESDriveCommandDetail;

                if (Connector.Equals(sur.Connector))
                {
                    return sur.mSN.BytesEquals(mSN);

                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }

        }

        /// <summary>
        /// 释放当前实例所使用的资源
        /// </summary>
        protected override void Release0()
        {
            mSN = null;
            mPassword = null;
        }

        /// <summary>
        /// 打印详情信息
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            if (Connector == null)
            {
                return $"SN:{SN}";
            }
            return $"{Connector.ToString()} SN:{SN}";
        }
    }
}
