using DoNetDrive.Core.Command;
using System.Collections.Generic;

namespace DoNetDrive.Protocol.Fingerprint.Person
{
    /// <summary>
    /// 
    /// </summary>
    public class ReadPersonDataBase_Result : INCommandResult
    {
        /// <summary>
        /// 读取到的人员列表
        /// </summary>
        public List<Data.Person> PersonList;

        /// <summary>
        /// 读取到的人员数量
        /// </summary>
        public int DataBaseSize;

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            PersonList?.Clear();
            PersonList = null;
        }

        /// <summary>
        /// 创建结构
        /// </summary>
        /// <param name="cardList">卡列表</param>
        /// <param name="dataBaseSize">读取到的卡数量</param>
        /// <param name="cardType">卡数据库类型</param>
        public ReadPersonDataBase_Result(List<Data.Person> personList)
        {
            PersonList = personList;
            DataBaseSize = personList.Count;
        }
    }
}
