using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hextobin_wpf
{
    public class Command
    {
        public string command;
        public string response;
        public string? RawData { get; set; }
        public string? Cla { get; set; }
        public string? Ins { get; set; }
        public string? P1 { get; set; }
        public string? P2 { get; set; }
        public string? Lc { get; set; }
        public string? Data { get; set; }
        public string? Le { get; set; }

    }
}