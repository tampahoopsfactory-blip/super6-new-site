using DoNetDrive.Core.Command;
using DotNetty.Buffers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoNetDrive.Protocol.Fingerprint.Person
{
    /// <summary>
    /// 添加人员及识别信息命令返回值
    /// </summary>
    public class AddPersonAndImage_Result : INCommandResult
    {
        /// <summary>
        /// 用户号
        /// </summary>
        public uint UserCode;

        /// <summary>
        /// 用户上传状态
        /// </summary>
        public bool UserUploadStatus;


        /// <summary>
        /// 识别信息上传状态
        /// 1--上传完毕；2--特征码无法识别；3--人员照片不可识别；4--人员照片或特征码重复
        /// </summary>
        public List<int> IdDataUploadStatus { get; set; }

        /// <summary>
        /// 识别信息重复的用户号
        /// </summary>
        public List<uint> IdDataRepeatUser { get; set; }

        /// <summary>
        /// 创建添加人员及识别信息命令返回值
        /// </summary>
        public AddPersonAndImage_Result(Data.Person person, List<IdentificationData> datas)
        {
            UserCode = person.UserCode;
            IdDataRepeatUser = new List<uint>();
            IdDataUploadStatus = new List<int>();

            for (int i = 0; i < datas.Count; i++)
            {
                IdDataRepeatUser.Add(0);
                IdDataUploadStatus.Add(0);
            }

        }

        public void Dispose()
        {
            IdDataRepeatUser = null;
            IdDataUploadStatus = null;
        }


    }
}
