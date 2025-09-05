using DotNetty.Buffers;
using DoNetDrive.Protocol.Elevator.FC8864.Data;
using DoNetDrive.Protocol.Door.Door8800.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using DoNetDrive.Common.Extensions;

namespace DoNetDrive.Protocol.Elevator.Test.Model
{
    /// <summary>
    /// 卡信息的UI包装器
    /// </summary>
    public class CardDetail_UI : INotifyPropertyChanged
    {
        public CardDetailBase CardDetail;

        /// <summary>
        /// 包装一个卡信息
        /// </summary>
        /// <param name="card"></param>
        public CardDetail_UI(CardDetailBase card)
        {
            CardDetail = card;
        }

        /// <summary>
        /// 获取出入标志
        /// </summary>
        /// <param name="iDoor">门号从 1-4 </param>
        public string GetEnterStatus(int iDoor)
        {
            int EnterStatus = CardDetail.GetEnterStatusValue(iDoor);
            switch (EnterStatus)
            {
                case 1:
                    return "入有效";
                case 2:
                    return "出有效";
                default:
                    return "出入有效";
            }
        }




        private static StringBuilder mStrBuf = new StringBuilder(1024);



        /// <summary>
        /// 选择
        /// </summary>
        private bool _Selected;
        /// <summary>
        /// 选择
        /// </summary>
        public bool Selected
        {
            get { return _Selected; }
            set
            {
                _Selected = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// 序号
        /// </summary>
        public string CardIndex { get; set; }

        /// <summary>
        /// 卡号
        /// </summary>
        public string CardData
        {
            get
            {
                return CardDetail.CardData.ToString();
                //mStrBuf.Clear();
                //mStrBuf.Append(CardDetail.CardData.ToString("d20")).Append("(0x").Append(CardDetail.CardData.ToString("X16")).Append(")");
                //return mStrBuf.ToString();
            }
        }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password
        {
            get
            {
                if (string.IsNullOrEmpty(CardDetail.Password))
                {
                    return string.Empty;
                }
                return CardDetail.Password.ToUpper().Replace("F", string.Empty);
            }
        }

        /// <summary>
        /// 有效期
        /// </summary>
        public string Expiry { get { return CardDetail.Expiry.ToDateTimeStr(); } }

        /// <summary>
        /// 表示卡状态的列表
        /// </summary>
        public static string[] CardStatusList = new string[] { "正常", "挂失卡", "黑名单" };

        /// <summary>
        /// 卡片状态
        /// </summary>
        public string CardStatus
        {
            get
            {
                return CardDetail.CardStatus < 3 ? CardStatusList[CardDetail.CardStatus] : CardDetail.CardStatus.ToString();
            }
        }

        /// <summary>
        /// 开门次数 (0)已失效
        /// </summary>
        public const string OpenTimes_Invalid = "(0)已失效";

        /// <summary>
        /// 开门次数 无限制(65535)
        /// </summary>
        public const string OpenTimes_Off = "无限制(65535)";

        /// <summary>
        /// 有效次数
        /// </summary>
        public string OpenTimes
        {
            get
            {
                return CardDetail.OpenTimes == 0 ?
                OpenTimes_Invalid :
                CardDetail.OpenTimes == 65535 ?
                OpenTimes_Off : CardDetail.OpenTimes.ToString() + "次";
            }
        }


        /// <summary>
        /// 门权限
        /// </summary>
        public string doorAccess
        {
            get
            {
                mStrBuf.Clear();

                for (int i = 1; i <= 4; i++)
                {
                    mStrBuf.Append("门").Append(i).Append(":");
                    mStrBuf.Append(CardDetail.GetDoor(i) ? "有" : "无");
                    mStrBuf.Append(";");
                }

                return mStrBuf.ToString();
            }
        }

        /// <summary>
        /// 开门时段
        /// </summary>
        public string TimeGroup
        {
            get
            {
                mStrBuf.Clear();

                for (int i = 1; i <= 4; i++)
                {
                    mStrBuf.Append("门").Append(i).Append(":");
                    mStrBuf.Append(CardDetail.GetTimeGroup(i).ToString("00"));
                    mStrBuf.Append(";");
                }

                return mStrBuf.ToString();
            }
        }


        /// <summary>
        /// 表示卡特权的列表
        /// </summary>
        public static string[] CardPrivilegeList = new string[] { "普通卡", "首卡", "常开", "巡更", "防盗设置卡", "管理卡" };

        /// <summary>
        /// 特权
        /// </summary>
        public string Privilege
        {
            get
            {
                if (CardDetail.Privilege < 5)
                    return CardPrivilegeList[CardDetail.Privilege];
                else
                    return CardDetail.Privilege.ToString();
            }
        }

        /// <summary>
        /// 节假日
        /// </summary>
        public string Holiday
        {
            get
            {
                mStrBuf.Clear();
                if (CardDetail.HolidayUse)
                {
                    for (int i = 1; i <= 32; i++)
                    {
                        mStrBuf.Append(CardDetail.GetHolidayValue(i) ? "1" : "0");
                    }
                }
                else
                {
                    mStrBuf.Append("节假日不受限制！");
                }


                return mStrBuf.ToString();
            }
        }

        /// <summary>
        /// 出入状态
        /// </summary>
        public string EnterStatus
        {
            get
            {
                mStrBuf.Clear();

                for (int i = 1; i <= 4; i++)
                {
                    mStrBuf.Append("门").Append(i).Append(":");
                    mStrBuf.Append(GetEnterStatus(i)).Append("；");
                }

                return mStrBuf.ToString();
            }
        }

        /// <summary>
        /// 最近读卡时间
        /// </summary>
        public string ReadCardDate { get { return CardDetail.RecordTime.ToDateTimeStr(); } }

        /// <summary>
        ///  在属性值更改时发生。
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
