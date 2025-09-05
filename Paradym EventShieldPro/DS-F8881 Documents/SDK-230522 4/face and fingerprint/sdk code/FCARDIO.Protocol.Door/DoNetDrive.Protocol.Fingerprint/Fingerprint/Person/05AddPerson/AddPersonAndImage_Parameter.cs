using DoNetDrive.Protocol.Door.Door8800;
using DotNetty.Buffers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoNetDrive.Protocol.Fingerprint.Person
{
    /// <summary>
    /// 添加人员及识别信息命令的参数
    /// </summary>
    public class AddPersonAndImage_Parameter : AbstractParameter
    {
        /// <summary>
        /// 人员详情
        /// </summary>
        public Data.Person PersonDetail { get; private set; }

        /// <summary>
        /// 人员识别信息
        /// </summary>
        public List<IdentificationData> IdentificationDatas;

        /// <summary>
        /// 等待校验的时间，单位毫秒
        /// </summary>
        public int WaitVerifyTime;

        /// <summary>
        /// 如果发生照片重复消息时，是否等待重复详情，适用于人脸机固件版本4.24以上版本
        /// </summary>
        public bool WaitRepeatMessage;

        /// <summary>
        /// 创建添加人员及识别信息命令的参数
        /// </summary>
        /// <param name="person">人员详情</param>
        /// <param name="data">人员识别信息</param>
        public AddPersonAndImage_Parameter(Data.Person person, IdentificationData data)
            : this(person, new IdentificationData[] { data }, false)
        {
        }
        /// <summary>
        /// 创建添加人员及识别信息命令的参数
        /// </summary>
        /// <param name="person">人员详情</param>
        /// <param name="datas">人员识别信息</param>
        public AddPersonAndImage_Parameter(Data.Person person, IdentificationData[] datas)
            : this(person, datas, false)
        {
        }
        /// <summary>
        /// 创建添加人员及识别信息命令的参数
        /// </summary>
        /// <param name="person">人员详情</param>
        /// <param name="datas">人员识别信息</param>
        /// <param name="bWaitRepeatMessage">是否等待重复详情回馈</param>
        public AddPersonAndImage_Parameter(Data.Person person, IdentificationData[] datas, bool bWaitRepeatMessage)
        {
            PersonDetail = person;
            IdentificationDatas = new List<IdentificationData>();
            IdentificationDatas.AddRange(datas);
            WaitVerifyTime = 6000;
            WaitRepeatMessage = bWaitRepeatMessage;
        }

        public override bool checkedParameter()
        {
            if (PersonDetail == null) return false;
            if (PersonDetail.UserCode == 0 || PersonDetail.UserCode > uint.MaxValue) return false;
            if (PersonDetail.TimeGroup > 64 || PersonDetail.TimeGroup < 1) return false;
            if (PersonDetail.EnterStatus > 3 || PersonDetail.EnterStatus < 0) return false;
            if (PersonDetail.Expiry.Year > 2099 || PersonDetail.Expiry.Year < 2000) return false;

            if (PersonDetail.FingerprintFeatureCodeCout > 10) return false;

            if (PersonDetail.Holiday == null || PersonDetail.Holiday.Length > 4) return false;
            if (PersonDetail.Identity > 1 || PersonDetail.Identity < 0) return false;
            if (PersonDetail.CardType > 1 || PersonDetail.CardType < 0) return false;
            if (PersonDetail.CardStatus > 3 || PersonDetail.CardStatus < 0) return false;

            if (IdentificationDatas == null)
            {
                return false;
            }
            return true;
        }

        public override void Dispose()
        {
            IdentificationDatas?.Clear();
            IdentificationDatas = null;

        }

        public override IByteBuffer GetBytes(IByteBuffer databuf)
        {
            PersonDetail.GetBytes(databuf);
            return databuf;
        }

        public override int GetDataLen()
        {
            return PersonDetail.GetDataLen();
        }

        public override void SetBytes(IByteBuffer databuf)
        {
            PersonDetail.SetBytes(databuf);
        }
    }
}
