using DotNetty.Buffers;
using DoNetDrive.Protocol.Util;
using System;
using System.Text;


namespace DoNetDrive.Protocol.Fingerprint.Data
{
    /// <summary>
    /// 人员信息
    /// </summary>
    public class Person : IComparable<Person>
    {
        public static Encoding StringEncoding = Encoding.BigEndianUnicode;
        /// <summary>
        /// 用户号
        /// </summary>
        public uint UserCode;

        /// <summary>
        /// 卡号，取值范围 0x1-0xFFFFFFFF
        /// </summary>
        public UInt64 CardData;
        /// <summary>
        /// 卡密码,无密码不填。密码是4-8位的数字。
        /// </summary>
        public string Password;

        /// <summary>
        /// 截止日期，最大2089年12月31日
        /// </summary>
        public DateTime Expiry;

        /// <summary>
        /// 开门时段 取值范围：1-64；
        /// </summary>
        public int TimeGroup;


        /// <summary>
        /// 有效次数,取值范围：0-65535;<para/>
        /// 0表示次数用光了。65535表示不受限制
        /// </summary>
        public ushort OpenTimes;

        /// <summary>
        /// 用户身份
        /// 0 -- 普通用户
        /// 1 -- 管理员  
        /// </summary>
        public int Identity;

        /// <summary>
        ///卡片状态
        ///0 -- 普通卡
        ///1 -- 常开
        /// </summary>
        public int CardType;

        /// <summary>
        ///卡片状态
        ///0：正常状态；1：挂失；2：黑名单；3：已删除
        /// </summary>
        public int CardStatus;

        /// <summary>
        ///出入标记
        ///3、0  出入有效
        ///1  入有效
        ///2  出有效
        /// </summary>
        public int EnterStatus;

        /// <summary>
        /// 人员姓名
        /// </summary>
        public string PName;

        /// <summary>
        /// 人员编号
        /// </summary>
        public string PCode;

        /// <summary>
        /// 人员部门
        /// </summary>
        public string Dept;

        /// <summary>
        /// 人员职务
        /// </summary>
        public string Job;

        /// <summary>
        ///最近验证时间
        /// </summary>
        public DateTime RecordTime;

        /// <summary>
        /// 是否有人脸特征码
        /// </summary>
        public bool IsFaceFeatureCode;

        /// <summary>
        ///节假日权限
        /// </summary>
        public byte[] Holiday;

        /// <summary>
        /// 是否有指纹特征码
        /// 每个位表示一个指纹，一个人有10个指纹
        /// Bit0--指纹1，0-没有；1--有
        /// ...........
        /// bit9--指纹10
        /// </summary>
        public int FingerprintFeatureCodeCout;

        /// <summary>
        /// 创建一个人员
        /// </summary>
        public Person()
        {
            UserCode = 0;
            CardData = 0;
            Password = string.Empty;
            Expiry = DateTime.Now.AddYears(30);
            TimeGroup = 1;
            OpenTimes = 65535;
            Identity = 0;
            CardType = 0;
            CardStatus = 0;
            EnterStatus = 0;
            PName = string.Empty;
            PCode = string.Empty;
            Dept = string.Empty;
            Job = string.Empty;
            RecordTime = DateTime.Now;
            IsFaceFeatureCode = false;
            Holiday = new byte[] { (byte)255, (byte)255, (byte)255, (byte)255 };
            FingerprintFeatureCodeCout = 0;
        }
        /// <summary>
        /// 创建一个人员，并指定用户号和人员姓名
        /// </summary>
        public Person(uint uCode,string pName):this()
        {
            UserCode = uCode;
            PName = pName;
        }

        /// <summary>
        /// 创建一个人员，并指定用户号、密码、人员姓名、部门名称
        /// </summary>
        public Person(uint uCode, string sPWD,string pName,string dept) : this(uCode, pName)
        {
            Password = sPWD;
            Dept = dept;
        }


        /// <summary>
        /// 比较人员，使用人员用户号进行排序
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public int CompareTo(Person o)
        {
            if (o.UserCode == UserCode)
            {
                return 0;
            }
            else if (UserCode < o.UserCode)
            {
                return -1;
            }
            else if (UserCode > o.UserCode)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// 获取指定序号的节假日开关状态
        /// </summary>
        /// <param name="iIndex">取值范围 1-30</param>
        /// <returns>开关状态 开关true 表示启用，false 表示禁用</returns>
        public bool GetHolidayValue(int iIndex)
        {
            if (iIndex <= 0 || iIndex > 32)
            {
                throw new ArgumentException("iIndex= 1 -- 32");
            }
            iIndex -= 1;
            //计算索引所在的字节位置
            int iByteIndex = iIndex / 8;
            int iBitIndex = iIndex % 8;
            int iByteValue = Holiday[iByteIndex] & 0x000000ff;
            int iMaskValue = (int)Math.Pow(2, iBitIndex);
            iByteValue = iByteValue & iMaskValue;
            return ((iByteValue & iMaskValue) > 0);

        }

        /// <summary>
        /// 设置指定序号的节假日开关状态
        /// </summary>
        /// <param name="iIndex">取值范围 1-30</param>
        /// <param name="bUse">开关状态 开关true 表示启用，false 表示禁用</param>
        public void SetHolidayValue(int iIndex, bool bUse)
        {
            if (iIndex <= 0 || iIndex > 32)
            {
                throw new ArgumentException("iIndex= 1 -- 32");
            }

            iIndex -= 1;
            //计算索引所在的字节位置
            int iByteIndex = iIndex / 8;
            int iBitIndex = iIndex % 8;
            int iByteValue = Holiday[iByteIndex] & 0x000000ff;
            int iMaskValue = (int)Math.Pow(2, iBitIndex);
            bool bOldValue = ((iByteValue & iMaskValue) > 0);
            if (bUse == bOldValue)
            {
                return;
            }
            if (bUse)
            {
                iByteValue = iByteValue | iMaskValue;
            }
            else
            {
                iByteValue = iByteValue ^ iMaskValue;
            }

            Holiday[iByteIndex] = (byte)iByteValue;

        }

        /// <summary>
        /// 获取一个人员详情实例，序列化到buf中的字节占比
        /// </summary>
        /// <returns></returns>
        public int GetDataLen()
        {
            return 0xA1;//161字节
        }


        /// <summary>
        /// 从buf中读取人员详情数据
        /// </summary>
        /// <param name="data"></param>
        public virtual void SetBytes(IByteBuffer data)
        {
            UserCode = (uint)data.ReadInt();
            CardData = (UInt64)data.ReadLong();
            Password = StringUtil.ByteBufToHex(data, 4);
            Expiry = TimeUtil.BCDTimeToDate_yyMMddhhmm(data);

            TimeGroup = data.ReadByte();

            OpenTimes = data.ReadUnsignedShort();

            Identity = data.ReadByte();
            CardType = data.ReadByte();
            CardStatus = data.ReadByte();
            PName = Util.StringUtil.GetString(data, 30, StringEncoding);
            PCode = Util.StringUtil.GetString(data, 30, StringEncoding);
            Dept = Util.StringUtil.GetString(data, 30, StringEncoding);
            Job = Util.StringUtil.GetString(data, 30, StringEncoding);

            Holiday = new byte[4];
            data.ReadBytes(Holiday, 0, 4);
            EnterStatus = data.ReadByte();

            RecordTime = TimeUtil.BCDTimeToDate_yyMMddhhmmss(data);

            IsFaceFeatureCode = data.ReadBoolean();
            FingerprintFeatureCodeCout = (int)data.ReadUnsignedShort();
            //FingerprintFeatureCodeList = new byte[10];
            //for (int i = 0; i < 2; i++)
            //{
            //    byte type = data.ReadByte();
            //    var bytelist = DoNetDrive.Common.NumUtil.ByteToBit(type);
            //    for (int j = 0; j < bytelist.Length; j++)
            //    {
            //        if (i * 8 + j > 9)
            //            break;
            //        FingerprintFeatureCodeList[i * 8 + j] = bytelist[j];
            //    }
            //}
        }

        /// <summary>
        /// 将人员详情实例写入到buf中
        /// </summary>
        /// <param name="data"></param>
        public virtual IByteBuffer GetBytes(IByteBuffer data)
        {
            data.WriteInt((int)UserCode);
            data.WriteLong((long)CardData);
            Password = StringUtil.FillHexString(Password, 8, "F", true);
            StringUtil.HextoByteBuf(Password, data);
            TimeUtil.DateToBCD_yyMMddhhmm(data, Expiry);
            data.WriteByte(TimeGroup);
            data.WriteUnsignedShort(OpenTimes);

            data.WriteByte(Identity);
            data.WriteByte(CardType);
            data.WriteByte(CardStatus);
            Util.StringUtil.WriteString(data, PName, 30, StringEncoding);
            Util.StringUtil.WriteString(data, PCode, 30, StringEncoding);
            Util.StringUtil.WriteString(data, Dept, 30, StringEncoding);
            Util.StringUtil.WriteString(data, Job, 30, StringEncoding);

            data.WriteBytes(Holiday, 0, 4);

            data.WriteByte(EnterStatus);

            //最近验证时间 不写入
            for (int i = 0; i < 6; i++)
            {
                data.WriteByte(0);
            }

            data.WriteBoolean(IsFaceFeatureCode);
            data.WriteShort(FingerprintFeatureCodeCout);
            return data;
            //for (int i = 0; i < 2; i++)
            //{
            //    byte[] list = new byte[8];
            //    for (int j = 0; j < 8; j++)
            //    {
            //        if (i * 8 + j == FingerprintFeatureCodeList.Length)
            //            break;
            //        list[j] = FingerprintFeatureCodeList[i * 8 + j];
            //    }

            //    byte type = DoNetDrive.Common.NumUtil.BitToByte(list);
            //    data.WriteByte(type);
            //}

        }

    }
}
