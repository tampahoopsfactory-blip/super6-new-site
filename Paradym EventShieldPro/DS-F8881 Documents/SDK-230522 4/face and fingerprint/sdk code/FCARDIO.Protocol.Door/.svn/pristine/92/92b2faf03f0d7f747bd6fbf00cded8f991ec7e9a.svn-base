using DotNetty.Buffers;
using System.Collections.Generic;

namespace DoNetDrive.Protocol.POS.TemplateMethod
{
    /// <summary>
    /// 模板抽象命令参数基类
    /// </summary>
    public abstract class TemplateParameter_Base<T> : AbstractParameter where T : TemplateData_Base, new()
    {
        /// <summary>
        /// 要写入的元素集合
        /// </summary>
        public List<T> DataList { get; set; }

        public bool mIsDeleteCommand;

        /// <summary>
        /// 
        /// </summary>
        public TemplateParameter_Base(List<T> DataList)
        {
            this.DataList = DataList;
        }

        /// <summary>
        /// 提供给继承类使用
        /// </summary>
        public TemplateParameter_Base()
        {

        }

        /// <summary>
        /// 检查参数
        /// </summary>
        /// <returns></returns>
        public override bool checkedParameter()
        {
            if (DataList == null || DataList.Count == 0)
            {
                return false;
            }
            if (mIsDeleteCommand)
            {
                foreach (var item in DataList)
                {
                    if (!CheckedDeleteParameterItem(item))
                    {
                        return false;
                    }
                }
            }
            else
            {
                foreach (var item in DataList)
                {
                    if (!CheckedParameterItem(item))
                    {
                        return false;
                    }
                }
            }
           

            return true;
        }

        /// <summary>
        /// 检查每个密码
        /// </summary>
        /// <param name="password">密码信息</param>
        /// <returns></returns>
        protected abstract bool CheckedParameterItem(T data);

        /// <summary>
        /// 检查每个密码
        /// </summary>
        /// <param name="password">密码信息</param>
        /// <returns></returns>
        protected abstract bool CheckedDeleteParameterItem(T data);

        /// <summary>
        /// 释放资源
        /// </summary>
        public override void Dispose()
        {
            DataList = null;
        }

        /// <summary>
        /// 不实现此功能
        /// </summary>
        /// <param name="databuf"></param>
        public override void SetBytes(IByteBuffer databuf)
        {
            return;
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
        /// <returns></returns>
        public virtual int GetDeleteDataLen()
        {
            return 0;
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
    }
}
