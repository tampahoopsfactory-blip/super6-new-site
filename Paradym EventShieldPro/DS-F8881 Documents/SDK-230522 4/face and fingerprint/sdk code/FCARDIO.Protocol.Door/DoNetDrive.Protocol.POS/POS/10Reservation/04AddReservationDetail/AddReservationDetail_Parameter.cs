using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotNetty.Buffers;
using DoNetDrive.Protocol.POS.Data;
using DoNetDrive.Protocol.POS.TemplateMethod;

namespace DoNetDrive.Protocol.POS.Reservation
{
    public class AddReservationDetail_Parameter : TemplateParameter_Base<ReservationDetail>
    {
        public AddReservationDetail_Parameter()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        public List<ReservationDetail> ReservationDetailList;

        public AddReservationDetail_Parameter(List<ReservationDetail> list) : base(list)
        {
            ReservationDetailList = list;
        }

        protected override bool CheckedDeleteParameterItem(ReservationDetail reservation)
        {
            return true;
        }

        protected override bool CheckedParameterItem(ReservationDetail reservation)
        {
            if (reservation.CardData < 0)
            {
                return false;
            }
            if (reservation.ReservationDate.Year < 2000 || reservation.ReservationDate.Year > 2099)
            {
                return false;
            }
            return true;
        }
    }
}
