using DotNetty.Buffers;
using DoNetDrive.Common.Extensions;
using DoNetDrive.Protocol.Door.Door8800.Utility;
using DoNetDrive.Protocol.Util;
using System;
using System.Collections.Generic;
using System.Linq;


namespace DoNetDrive.Protocol.Door.Door8800.Data
{
    /// <summary>
    /// 授权卡信息 抽象类
    /// </summary>
    public abstract class CardDetailBase : IComparable<CardDetailBase>
    {
        /// <summary>
        /// 卡号，取值范围 0x1-0xFFFFFFFF
        /// </summary>
        public abstract uint CardData {get;set;}

        /// <summary>
        /// 卡密码,无密码不填。密码是4-8位的数字。
        /// </summary>
        public string Password;

        /// <summary>
        /// 截止日期，最大2089年12月31日
        /// </summary>
        public DateTime Expiry;

        /// <summary>
        /// 开门时段；1-4门的开门时段；时段取值范围：1-64；
        /// <para>TimeGroup[0] -- 1门的时段；</para>
        /// <para>TimeGroup[1] -- 2门的时段；</para>
        /// <para>TimeGroup[2] -- 3门的时段；</para>
        /// <para>TimeGroup[3] -- 4门的时段；</para>
        /// </summary>
        public byte[] TimeGroup;


        /// <summary>
        /// 开门权限
        /// <para>1-4门的开门权限；false--无权，true--有权开门</para>
        /// <para>bit0 -- 1门的权限</para>
        /// <para>bit1 -- 2门的权限</para>
        /// <para>bit2 -- 3门的权限</para>
        /// <para>bit3 -- 4门的权限</para>
        /// </summary>
        public int Door;

        /// <summary>
        /// 有效次数,取值范围：0-65535;<para/>
        /// 0表示次数用光了。65535表示不受限制
        /// </summary>
        public int OpenTimes;

        /// <summary>
        /// 特权<para/>
        /// 0 -- 普通卡      <para/>
        /// 1 -- 首卡        <para/>
        /// 2 -- 常开        <para/>
        /// 3 -- 巡更        <para/>
        /// 4 -- 防盗设置卡  <para/>
        /// </summary>
        public int Privilege;

        /// <summary>
        ///卡片状态<para/>
        ///0：正常状态；1：挂失；2：黑名单
        /// </summary>
        public byte CardStatus;

        /// <summary>
        ///节假日权限
        /// </summary>
        public byte[] Holiday;

        /// <summary>
        ///使用节假日限制功能,节假日禁止开门
        /// </summary>
        public bool HolidayUse;

        /// <summary>
        ///出入标记；
        /// </summary>
        public int EnterStatus;

        /// <summary>
        ///最近一次读卡的记录时间
        /// </summary>
        public DateTime RecordTime;


        /// <summary>
        /// 从一个卡详情实例，复制一个副本
        /// </summary>
        /// <param name="sur"></param>
        public CardDetailBase(CardDetailBase sur)
        {
            CopyFrom(sur);
        }

        /// <summary>
        /// 从一个卡详情实例，复制一个副本
        /// </summary>
        /// <param name="sur"></param>
        public void CopyFrom(CardDetailBase sur)
        {
            CardData = sur.CardData;
            Password = sur.Password;
            Expiry = sur.Expiry;
            TimeGroup = (byte[])sur.TimeGroup.Clone();
            Door = sur.Door;
            OpenTimes = sur.OpenTimes;
            Privilege = sur.Privilege;
            CardStatus = sur.CardStatus;
            Holiday = (byte[])sur.Holiday.Clone();
            HolidayUse = sur.HolidayUse;
            EnterStatus = sur.EnterStatus;
            RecordTime = sur.RecordTime;
        }

        /// <summary>
        /// 初始化卡详情实例中的数值
        /// </summary>
        public CardDetailBase()
        {
            OpenTimes = 65535;
            Password = string.Empty;
            Expiry = DateTime.Now.AddYears(5);
            TimeGroup = new byte[] { 1, 1, 1, 1 };
            Door = 15;
            Privilege = 0;
            CardStatus = 0;
            Holiday = new byte[] { (byte)255, (byte)255, (byte)255, (byte)255 };
            RecordTime = DateTime.Now;
            EnterStatus = 0;
            HolidayUse = true;
        }

        /// <summary>
        /// 比较卡号是否一致
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public virtual int CompareTo(CardDetailBase o)
        {
            if (o.CardData == CardData)
            {
                return 0;
            }
            else if (CardData < o.CardData)
            {
                return -1;
            }
            else if (CardData > o.CardData)
            {
                return 1;
            }
            else
            {
                return 0;
            }

        }


        /// <summary>
        /// 获取一个卡详情实例，序列化到buf中的字节占比
        /// </summary>
        /// <returns></returns>
        public abstract int GetDataLen();

        /// <summary>
        /// 将卡号序列化并写入buf中
        /// </summary>
        /// <param name="data"></param>
        public abstract void WriteCardData(IByteBuffer data);

        /// <summary>
        /// 将卡详情实例写入到buf中
        /// </summary>
        /// <param name="data"></param>
        public virtual void GetBytes(IByteBuffer data)
        {
            WriteCardData(data);
            //Password = btData.ToHex();
            Password = StringUtil.FillHexString(Password, 8, "F", true);
            StringUtil.HextoByteBuf(Password, data);



            TimeUtil.DateToBCD_yyMMddhhmm(data, Expiry);


            data.WriteBytes(TimeGroup, 0, 4);

            data.WriteShort(OpenTimes);

            int bData = (Door << 4) + (Privilege & 0x7);//特权
            if (HolidayUse)
            {
                bData = bData | 8;
            }
            data.WriteByte(bData);
            data.WriteByte(CardStatus);

            data.WriteBytes(Holiday, 0, 4);

            data.WriteByte(EnterStatus);
            //不写入最近读卡时间
            for (int i = 0; i < 6; i++)
            {
                data.WriteByte(0);
            }
        }

        /// <summary>
        /// 从buf中读取卡号
        /// </summary>
        /// <param name="data"></param>
        public abstract void ReadCardData(IByteBuffer data);

        /// <summary>
        /// 从buf中读取卡详情数据
        /// </summary>
        /// <param name="data"></param>
        public virtual void SetBytes(IByteBuffer data)
        {
            ReadCardData(data);

            Password = StringUtil.ByteBufToHex(data, 4);

            Expiry = TimeUtil.BCDTimeToDate_yyMMddhhmm(data);

            data.ReadBytes(TimeGroup, 0, 4);

            OpenTimes = data.ReadUnsignedShort();

            int bData = data.ReadByte();//特权
            Door = bData >> 4;
            bData = bData & 0xF;
            Privilege = bData & 7;
            HolidayUse = (bData & 8) == 8;
            CardStatus = data.ReadByte();

            data.ReadBytes(Holiday, 0, 4);
            EnterStatus = data.ReadByte();

            RecordTime = TimeUtil.BCDTimeToDate_yyMMddhhmmss(data);
        }


        /// <summary>
        ///获取指定门的开门时段号
        /// </summary>
        /// <param name="iDoor">取值范围1-4</param>
        /// <returns>开门时段号</returns>
        public int GetTimeGroup(int iDoor)
        {
            if (iDoor < 0 || iDoor > 4)
            {

                throw new ArgumentException("Door 1-4");
            }
            return TimeGroup[iDoor - 1];
        }
        /// <summary>
        /// 设置指定门的开门时段号
        /// </summary>
        /// <param name="iDoor">门号，取值范围：1-4</param>
        /// <param name="iNum">开门时段号，取值范围：1-64</param>
        public void SetTimeGroup(int iDoor, int iNum)
        {
            if (iDoor < 0 || iDoor > 4)
            {

                throw new ArgumentException("Door 1-4");
            }

            if (iNum < 0 || iNum > 64)
            {

                throw new ArgumentException("Num 1-64");
            }
            TimeGroup[iDoor - 1] = (byte)iNum;
        }


        /// <summary>
        /// 获取指定门是否有权限
        /// </summary>
        /// <param name="iDoor">门号，取值范围：1-4</param>
        /// <returns>true 有权限，false 无权限。</returns>
        public bool GetDoor(int iDoor)
        {
            if (iDoor < 0 || iDoor > 4)
            {

                throw new ArgumentException("Door 1-4");
            }
            iDoor -= 1;
            int iMaskValue = (int)Math.Pow(2, iDoor);
            return ((Door & iMaskValue) > 0);
        }

        /// <summary>
        /// 设置指定门是否有权限
        /// </summary>
        /// <param name="iDoor">门号，取值范围：1-4</param>
        /// <param name="bUse">true 有权限，false 无权限。</param>
        public void SetDoor(int iDoor, bool bUse)
        {
            if (iDoor < 0 || iDoor > 4)
            {

                throw new ArgumentException("Door 1-4");
            }
            iDoor -= 1;
            int iMaskValue = (int)Math.Pow(2, iDoor);
            bool bOldValue = (Door & iMaskValue) > 0;
            if (bUse == bOldValue)
            {
                return;
            }
            if (bUse)
            {
                Door = Door | iMaskValue;
            }
            else
            {
                Door = Door ^ iMaskValue;
            }
        }

        /// <summary>
        ///普通开门卡
        /// </summary>
        public bool IsNormal()
        {
            return Privilege == 0;
        }

        /// <summary>
        ///普通开门卡--五特权开门卡
        /// </summary>
        public void SetNormal()
        {
            Privilege = 0;
        }

        /// <summary>
        ///首卡特权卡
        /// </summary>
        public bool IsPrivilege()
        {
            return Privilege == 1;
        }

        /// <summary>
        ///首卡特权卡
        /// </summary>
        public void SetPrivilege()
        {
            Privilege = 1;
        }

        /// <summary>
        ///常开特权卡
        /// </summary>
        public bool IsTiming()
        {
            return Privilege == 2;
        }

        /// <summary>
        ///常开特权卡
        /// </summary>
        public void SetTiming()
        {
            Privilege = 2;
        }

        /// <summary>
        ///巡更卡
        /// </summary>
        public bool IsGuardTour()
        {
            return Privilege == 3;
        }

        /// <summary>
        ///巡更卡
        /// </summary>
        public void SetGuardTour()
        {
            Privilege = 3;
        }

        /// <summary>
        ///防盗设置卡
        /// </summary>
        public bool IsAlarmSetting()
        {
            return Privilege == 4;
        }

        /// <summary>
        ///防盗设置卡
        /// </summary>
        public void SetAlarmSetting()
        {
            Privilege = 4;
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
        /// 获取出入标志
        /// </summary>
        /// <param name="iDoor">门号，取值范围：1-4</param>
        /// <returns>出入标志：0--出入有效；1--入有效；2--出有效</returns>
        public int GetEnterStatusValue(int iDoor)
        {
            int iCount = (iDoor - 1) * 2;//移位数量
            int iStatusMap = 3 << iCount;
            int iStatus = (EnterStatus & iStatusMap) >> iCount;
            return iStatus;
        }


        /// <summary>
        /// 设置出入标志
        /// </summary>
        /// <param name="iDoor">门号，取值范围：1-4</param>
        /// <param name="iStatus">出入标志：0--出入有效；1--入有效；2--出有效</param>
        public void SetEnterStatusValue(int iDoor, int iStatus)
        {
            int iCount = (iDoor - 1) * 2;//移位数量
            int iStatusMap = 3 << iCount;
            iStatus = iStatus << iCount;

            int iTmpStatus = EnterStatus | iStatusMap;
            iTmpStatus = ~iTmpStatus;
            iTmpStatus = iTmpStatus | iStatusMap;
            iTmpStatus = ~iTmpStatus;

            EnterStatus = iTmpStatus | iStatus;
        }

    }
}
