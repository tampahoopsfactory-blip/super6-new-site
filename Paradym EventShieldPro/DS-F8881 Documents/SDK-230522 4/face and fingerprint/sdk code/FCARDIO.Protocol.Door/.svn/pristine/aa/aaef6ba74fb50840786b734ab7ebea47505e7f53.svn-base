using DotNetty.Buffers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
namespace DoNetDrive.Protocol.Door.Door8800.Utility
{
    /// <summary>
    /// String 工具类
    /// </summary>
    public class StringUtility
    {
        /// <summary>
        /// 填充字符串
        /// </summary>
        /// <param name="str">需要被填充的字符串</param>
        /// <param name="iLen">字符串长度</param>
        /// <param name="fillstr">填充的字符串</param>
        /// <returns></returns>
        public static string FillString(string str, int iLen, string fillstr)
        {
            return FillString(str, iLen, fillstr, true);
        }

        /// <summary>
        /// 填充字符串
        /// </summary>
        /// <param name="str">需要被填充的字符串</param>
        /// <param name="iLen">字符串长度</param>
        /// <param name="fillstr">填充的字符串</param>
        /// <param name="fill_right">是否右边填充</param>
        /// <returns></returns>
        public static string FillString(string str, int iLen, string fillstr, bool fill_right)
        {
            int iStrLen = 0;

            if (!string.IsNullOrEmpty(str))
            {
                iStrLen = str.Length;

                if (iStrLen == iLen)
                {
                    return str;
                }
            }

            if (iStrLen > iLen)
            {
                if (fill_right)
                {
                    return str.Substring(0, iLen);
                }
                else
                {
                    return str.Substring(iStrLen - iLen, iStrLen);
                }

            }
            StringBuilder sbuf = new StringBuilder(iLen);

            int iAddCount = iLen - iStrLen;
            for (int i = 0; i < iAddCount; i++)
            {
                sbuf.Append(fillstr);
                if (sbuf.Length > iAddCount)
                {
                    break;
                }
            }
            if (sbuf.Length > iAddCount)
            {
                sbuf.Length = iAddCount;
            }
            if (!string.IsNullOrEmpty(str))
            {
                if (fill_right)
                {
                    sbuf.Insert(0, str);
                }
                else
                {
                    sbuf.Append(str);
                }

            }
            return sbuf.ToString();
        }


        /// <summary>
        /// BCD格式日期时间转DateTime
        /// </summary>
        /// <param name="btTime">字节数组</param>
        /// <returns></returns>
        public static DateTime BCDTimeToDate_yyMMddhh(byte[] btTime)
        {
            btTime = BCDToByte(btTime);
            int year = uByte(btTime[0]);
            int month = uByte(btTime[1]);
            int dayOfMonth = uByte(btTime[2]);
            int hourOfDay = uByte(btTime[3]);

            if (year > 99)
            {
                return new DateTime();
            }
            if (month == 0 || month > 12)
            {
                return new DateTime();
            }
            if (dayOfMonth == 0 || dayOfMonth > 31)
            {
                return new DateTime();
            }
            if (hourOfDay > 23)
            {
                return new DateTime();
            }

            DateTime dTime = new DateTime(2000 + year, month, dayOfMonth, hourOfDay, 0, 0);
            return dTime;
        }

        /// <summary>
        /// BCD格式转字节数组
        /// </summary>
        /// <param name="iNum"></param>
        /// <returns></returns>
        public static byte[] BCDToByte(byte[] iNum)
        {
            int iLen = iNum.Length;
            for (int i = 0; i < iLen; i++)
            {
                iNum[i] = BCDToByte(iNum[i]);
            }
            return iNum;
        }

        /// <summary>
        /// BCD格式转字节
        /// </summary>
        /// <param name="iNum"></param>
        /// <returns></returns>
        public static byte BCDToByte(byte iNum)
        {
            int iValue = uByte(iNum);
            iValue = ((iValue / 16) * 10) + (iValue % 16);
            return (byte)iValue;
        }

        /// <summary>
        /// 字节转数值
        /// </summary>
        /// <param name="byte0"></param>
        /// <returns></returns>
        public static int uByte(byte byte0)
        {
            return byte0 & 0x000000ff;
        }

        /// <summary>
        /// DateTime转BCD格式日期时间
        /// </summary>
        /// <param name="btData">字节数组</param>
        /// <param name="date">DateTime</param>
        public static void DateToBCD_yyMMddhh(byte[] btData, DateTime date)
        {
            if (date == null)
            {
                for (int i = 0; i < 4; i++)
                {
                    btData[i] = 0;
                }
            }
            else
            {
                btData[0] = (byte)(date.Year - 2000);
                btData[1] = (byte)(date.Month);
                btData[2] = (byte)date.Day;
                btData[3] = (byte)date.Hour;
                btData = ByteToBCD(btData);
            }
        }

        /// <summary>
        /// 字节数组转BCD格式
        /// </summary>
        /// <param name="iNum">字节数组</param>
        /// <returns></returns>
        public static byte[] ByteToBCD(byte[] iNum)
        {
            int iLen = iNum.Length;
            for (int i = 0; i < iLen; i++)
            {
                iNum[i] = ByteToBCD(iNum[i]);
            }
            return iNum;
        }

        /// <summary>
        /// 字节转BCD格式
        /// </summary>
        /// <param name="iNum">字节</param>
        /// <returns></returns>
        public static byte ByteToBCD(byte iNum)
        {
            int iValue = uByte(iNum);
            iValue = (iValue / 10) * 16 + (iValue % 10);
            return (byte)iValue;
        }

        /// <summary>
        /// 字节转换成IP格式字符串
        /// </summary>
        /// <param name="data">包含参数结构的缓冲区</param>
        /// <param name="strbuilder">字符串</param>
        public static void ReadIPByByteBuf(IByteBuffer data, StringBuilder strbuilder)
        {
            for (int i = 0; i < 4; i++)
            {
                strbuilder.Append(data.ReadByte());
                if (i < 3)
                {
                    strbuilder.Append(".");
                }
            }

        }

        /// <summary>
        /// 1字节转8Bit
        /// </summary>
        /// <param name="iNum"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static byte[] ByteToBit(byte iNum)
        {
            byte[] iBit = new byte[8];
            int iValue = 0, iMask = 1;

            for (int i = 0; i < 8; i++)
            {
                iValue = iNum & iMask;
                if (iValue > 0) iBit[i] = 1;
                iMask *= 2;
            }
            return iBit;
        }


        /// <summary>
        /// IP地址及与之相同格式的结构转换成数组字节
        /// </summary>
        /// <param name="IP"></param>
        /// <param name="buf"></param>
        public static void SaveIPToByteBuf(string IP, IByteBuffer buf)
        {
            if (!CheckIP(IP))
            {
                for (int i = 0; i < 4; i++)
                {
                    buf.WriteByte(0);
                }
                return;
            }
            String[] ipList = IP.Split('.');
            int iLen = ipList.Length;
            for (int i = 0; i < iLen; i++)
            {
                buf.WriteByte(Convert.ToInt32(ipList[i]));
            }
        }
        /// <summary>
        /// 检查IP地址及与之相同格式数据
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static bool CheckIP(string ip)
        {
            if (string.IsNullOrEmpty(ip))
            {
                return false;
            }
            string[] ipList = ip.Split('.');
            int iLen = ipList.Length;
            if (iLen != 4)
            {
                return false;
            }
            try
            {
                int iValue;
                for (int i = 0; i < iLen; i++)
                {
                    iValue = Convert.ToInt32(ipList[i]);
                    if (iValue < 0 || iValue > 255)
                    {
                        return false;
                    }
                }
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 获取一个随机数
        /// </summary>
        /// <param name="iMin">最小值</param>
        /// <param name="iMax">最大值</param>
        /// <returns></returns>
        public static int GetRandomNum(int iMin, int iMax)
        {
            Random mRandom = new Random();
            var rnd = mRandom.NextDouble();
            return iMin + (int)(rnd * (iMax - iMin + 1));
        }
        
        /// <summary>
        /// 时间时和分组合
        /// </summary>
        /// <param name="beginTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <returns></returns>
        public static string TimeHourAndMinuteStr(DateTime beginTime, DateTime endTime)
        {
            StringBuilder buf = new StringBuilder(15);
            buf.Append(beginTime.ToString("HH:mm"));
            buf.Append(" - ");
            buf.Append(endTime.ToString("HH:mm"));
            return buf.ToString();
        }

        






    }
}
