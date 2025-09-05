using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataMonitoring.Model
{
    public class ConnectorModel
    {
        public string SN { get; set; }
        public string Password { get; set; }
        public string RemoteIP { get; set; }
        public int RemotePort { get; set; }

        public string Key { get; set; }
    }
}
