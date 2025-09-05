using DotNetty.Buffers;
using DoNetDrive.Protocol.Fingerprint.Data;
using System.Collections.Generic;

namespace DoNetDrive.Protocol.Fingerprint.Person
{
    /// <summary>
    /// 删除人员参数
    /// </summary>
    public class DeletePerson_Parameter : WritePerson_ParameterBase
    {

        /// <summary>
        /// 初始化 指令的参数
        /// </summary>
        /// <param name="personList">需要删除的人员列表</param>
        public DeletePerson_Parameter(List<Data.Person> personList) : base(personList) { }

        /// <summary>
        /// 检查每个人员
        /// </summary>
        /// <param name="personList"></param>
        /// <returns></returns>
        protected override bool CheckedParameterItem(List<Data.Person> personList)
        {
            foreach (var person in personList)
            {
                if (person.UserCode == 0 || person.UserCode > uint.MaxValue) return false;
            }

            return true;
        }
    }
}
