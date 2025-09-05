using DotNetty.Buffers;
using DoNetDrive.Core.Command;
using DoNetDrive.Protocol.POS.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DoNetDrive.Protocol.POS.TemplateMethod;

namespace DoNetDrive.Protocol.POS.Reservation
{
    public class ReadDataBase_Result : TemplateResult_Base
    {
        /// <summary>
        /// 
        /// </summary>
        public List<ReservationDetail> ReservationDetails;

        /// <summary>
        /// 创建结构
        /// </summary>
        public ReadDataBase_Result(List<ReservationDetail> DataList)
        {
            this.ReservationDetails = DataList;
        }

        public override void Dispose()
        {
            ReservationDetails = null;
        }
    }
}
