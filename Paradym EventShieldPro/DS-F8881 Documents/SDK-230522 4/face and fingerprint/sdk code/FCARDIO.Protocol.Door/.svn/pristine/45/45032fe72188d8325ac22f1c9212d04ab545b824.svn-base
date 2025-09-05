using DotNetty.Buffers;
using DoNetDrive.Protocol.Door.Door8800;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoNetDrive.Protocol.Fingerprint.Person
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class WritePerson_ParameterBase : AbstractParameter
    {
        /// <summary>
        /// 需要写入的卡列表
        /// </summary>
        public List<Data.Person> PersonList;


        /// <summary>
        /// 创建 将人员列表写入到控制器非排序区 指令的参数
        /// </summary>
        /// <param name="personList">需要写入的卡列表</param>
        public WritePerson_ParameterBase(List<Data.Person> personList)
        {
            PersonList = personList;
            if (!checkedParameter())
            {
                throw new ArgumentException("personList Error");
            }
        }

        /// <summary>
        /// 检查人员列表参数，任何情况下都不能为空，元素数不能为0,列表元素不能为空
        /// </summary>
        /// <returns></returns>
        public override bool checkedParameter()
        {
            if (PersonList == null) return false;


            if (PersonList.Count == 0) return false;

            return CheckedParameterItem(PersonList);
        }

        /// <summary>
        /// 检查每个人员
        /// </summary>
        /// <param name="personList">人员列表</param>
        /// <returns></returns>
        protected abstract bool CheckedParameterItem(List<Data.Person> personList);

        /// <summary>
        /// 释放资源
        /// </summary>
        public override void Dispose()
        {
            PersonList = null;
        }

        /// <summary>
        /// 不实现此功能
        /// </summary>
        /// <param name="databuf"></param>
        /// <returns></returns>
        public override IByteBuffer GetBytes(IByteBuffer databuf)
        {
            return databuf;
        }

        /// <summary>
        /// 不实现此功能
        /// </summary>
        /// <returns></returns>
        public override int GetDataLen()
        {
            return 0;
        }

        /// <summary>
        /// 不实现此功能
        /// </summary>
        /// <param name="databuf"></param>
        public override void SetBytes(IByteBuffer databuf)
        {
            return;
        }
    }
}
