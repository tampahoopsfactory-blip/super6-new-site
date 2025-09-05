using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DoNetDrive.Protocol.Door.Test.Model
{
    public class WeekTimeGroupDto
    {
        public int WeekDayIndex { get; set; }
        private bool _id0;
        private bool _id1;
        private bool _id2;
        private bool _id3;
        public bool id0
        {
            get { return _id0; }
            set
            {
                _id0 = value;
                //if (_id0)
                //{
                //    _id1 = _id2 = _id3 = false;
                //}
            }
        }
        public bool id1 { get { return _id1; } set {
                _id1 = value;
                //if (_id1)
                //{
                //    _id0 = _id2 = _id3 = false;
                //}
            } }
        public bool id2 { get { return _id2; } set {
                _id2 = value;
                //if (_id2)
                //{
                //    _id1 = _id0 = _id3 = false;
                //}
            } }
        public bool id3 { get { return _id3; } set {
                _id3 = value;
                //if (_id3)
                //{
                //    _id1 = _id2 = _id0 = false;
                //}
            } }

        public string IsEx { get; set; }
        public string Ex { get; set; }

        public string WeekDay { get; set; }

        private string _StartTime;
        private string _EndTime;
        public string StartTime
        {
            get { return _StartTime; }
            set
            {
                Regex r = new Regex(@"([01]\d|2[0123]):([0-5]\d|59)$");
                if (r.IsMatch(value))
                {
                    _StartTime = value;
                }
                else
                {
                    _StartTime = "00:00";
                }
            }
        }
        public string EndTime
        {
            get { return _EndTime; }
            set
            {
                Regex r = new Regex(@"([01]\d|2[0123]):([0-5]\d|59)$");
                if (r.IsMatch(value))
                {
                    _EndTime = value;
                }
                else
                {
                    _EndTime = "00:00";
                }
            }
        }

    }
}
