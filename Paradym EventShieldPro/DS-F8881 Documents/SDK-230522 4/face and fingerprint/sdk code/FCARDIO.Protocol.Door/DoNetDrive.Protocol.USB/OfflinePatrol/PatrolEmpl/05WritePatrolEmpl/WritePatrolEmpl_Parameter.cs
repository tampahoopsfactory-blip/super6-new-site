using DotNetty.Buffers;
using DoNetDrive.Core.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoNetDrive.Protocol.USB.OfflinePatrol.PatrolEmpl.WritePatrolEmpl
{
    /// <summary>
    /// 添加巡更人员
    /// </summary>
    public class WritePatrolEmpl_Parameter : AbstractParameter
    {
        /// <summary>
        /// 默认的缓冲区大小
        /// </summary>
        protected int MaxBufSize = 350;


        /// <summary>
        /// 被删除巡更人员列表
        /// </summary>
        public List<Data.PatrolEmpl> PatrolEmplList;

        /// <summary>
        /// 初始化参数
        /// </summary>
        /// <param name="list"></param>
        public WritePatrolEmpl_Parameter(List<Data.PatrolEmpl> list)
        {
            PatrolEmplList = list;
        }


        /// <summary>
        /// 检查参数
        /// </summary>
        /// <returns></returns>
        public override bool checkedParameter()
        {
            if (PatrolEmplList == null || PatrolEmplList.Count == 0)
                throw new ArgumentException("PatrolEmplList Error!");
            foreach (var patrolEmpl in PatrolEmplList)
            {
                if (patrolEmpl.CardData == 0 || patrolEmpl.CardData > 16777215)
                    throw new ArgumentException("PatrolEmpl.CardData Error!");
                //if (string.IsNullOrEmpty(patrolEmpl.Name) || patrolEmpl.Name.Length > 5)
                //    throw new ArgumentException("PatrolEmpl.Name Error!");
                if (patrolEmpl.PCode < 1 || patrolEmpl.PCode > 999)
                    throw new ArgumentException("PatrolEmpl.PCode Error!");
            }
            return true;
        }

        /// <summary>
        /// 没有实现
        /// </summary>
        /// <param name="databuf"></param>
        public override void SetBytes(IByteBuffer databuf)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 没有实现
        /// </summary>
        /// <returns></returns>
        public override int GetDataLen()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 没有实现
        /// </summary>
        /// <param name="databuf"></param>
        /// <returns></returns>
        public override IByteBuffer GetBytes(IByteBuffer databuf)
        {
            throw new NotImplementedException();
        }
    }
}
