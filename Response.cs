using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hextobin_wpf
{
    public class Response
    {
        public string? response;

        public string? RawData { get; set; }
        public List<Tlv>? tlv { get; set; }
        public string? Data { get; set; }
        public string? Sw1 { get; set; }
        public string? Sw2 { get; set; }
    }
    
}
