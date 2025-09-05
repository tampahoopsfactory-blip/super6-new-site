using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoNetDrive.Protocol.Elevator.Test.Model
{
    public struct CommandResult
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public string SN { get; set; }

        public string Remote { get; set; }

        public string Time { get; set; }
        public string Timemill { get; set; }
    }
}
