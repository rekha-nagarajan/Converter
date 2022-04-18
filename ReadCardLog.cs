using hextobin_wpf.UserCtl;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace hextobin_wpf
{
    public class ReadCardLog
    {
        List<APDU> apduList = new List<APDU>();
        
      
        public List<APDU> ParseLog(string Text)
        {
            
            var textSplit = Text.Split("IFD -");
            foreach (var apduTxt in textSplit)
            {
                if (apduTxt.ToUpper().Contains("ATR") || (!apduTxt.ToUpper().Contains("ICC")))
                    continue;
                var apduSplit = apduTxt.Split("ICC -");
                if (apduSplit.Count() > 1)
                {
                    APDU apdu = new APDU();
                    apdu.command = CommandParse(apduSplit[0].Replace("\r\n", "").Replace("*", ""));
                    apdu.response = ResponseParse(apduSplit[1].Replace("\r\n", "").Replace("*", ""));
                    apduList.Add(apdu);
                   
                }
            }
            return apduList;

        }
        public Command CommandParse(string data)
        {
            
            data = data.Replace(" ", "");
            Command command = new Command();
            command.RawData = data;
            command.Cla = data.Substring(0, 2);
            command.Ins = data.Substring(2, 2);
            command.P1 = data.Substring(4, 2);
            command.P2 = data.Substring(6, 2);
            if (data.Length == 10)
            {
                command.Le = data.Substring(8, 2);
            }
            else if (data.Length > 10)
            {
                command.Lc = data.Substring(8, 2);
                int Lc_int = Int32.Parse(command.Lc, System.Globalization.NumberStyles.HexNumber);
                if (data.Length > Lc_int)
                {
                    command.Data = data.Substring(10, Lc_int * 2);
                    command.Le = data.Substring(10, 0);
                }
                else
                {
                    command.Data = data.Substring(10);
                }
            }
            return command;
        }
        public Response ResponseParse(string data)
        {
            
            data = data.Replace(" ", "");
            Response response = new Response();
            if(string.IsNullOrEmpty(data))
                return response;
            if(data.Length > 4 )
            data = data.Substring(2);
            response.RawData = data;
            response.Data = data.Substring(0, data.Length - 4);
            response.Sw1 = data.Substring(data.Length - 4, 2);
            response.Sw2 = data.Substring(data.Length - 2);
            response.tlv = Tlv.tlvparse(response.Data);
            return response;
        }
    }
}
