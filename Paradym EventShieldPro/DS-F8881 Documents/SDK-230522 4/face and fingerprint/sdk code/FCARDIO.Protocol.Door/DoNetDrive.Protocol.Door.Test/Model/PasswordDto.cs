using DoNetDrive.Protocol.Door.Door8800.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoNetDrive.Protocol.Door.Test.Model
{
    public class PasswordDto
    {

        /// <summary>
        /// 开门次数
        /// </summary>
        public int OpenTimes { get; set; }

        /// <summary>
        /// 有效期
        /// </summary>
        public DateTime Expiry { get; set; }

        public bool Selected { get; set; }


        public string Password { get; set; }

        public bool Door1 { get; set; }
        public bool Door2 { get; set; }
        public bool Door3 { get; set; }
        public bool Door4 { get; set; }

        public string Doors {
            get
            {
                return (Door1 ? "门1," : "") + (Door2 ? "门2," : "") + (Door3 ? "门3," : "") + (Door4 ? "门4" : "");
            }
        }

        public void SetDoors(Door8800.Password.PasswordDetail password)
        {
            Door1 = password.GetDoor(1);
            Door2 = password.GetDoor(2);
            Door3 = password.GetDoor(3);
            Door4 = password.GetDoor(4);
        }

        public void SetDoors(string binary)
        {
            var list = binary.ToCharArray();
            Door1 = list[0] == '1';
            Door2 = list[1] == '1';
            Door3 = list[2] == '1';
            Door4 = list[3] == '1';

        }

    }
}
