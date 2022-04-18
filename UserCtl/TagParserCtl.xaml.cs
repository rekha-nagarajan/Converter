using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace hextobin_wpf.UserCtl
{
    /// <summary>
    /// Interaction logic for TagParserCtl.xaml
    /// </summary>
    public partial class TagParserCtl : UserControl
    {
        ByteDiscretionTag tagDiscretion = new ByteDiscretionTag();
        List<BitParserCtl> bitParserCtlList = new List<BitParserCtl>();
        BitParserCtl bitParserCtl;
        public TagParserCtl()
        {
            InitializeComponent();
        }

        public void InitializeTag(string tag)
        {
            pnlTag.Children.Clear();
            string temp = "";
            tagDiscretion = ConfigUtil.GetTagDetails(tag);
            int i = 1;
            foreach (var byte1 in tagDiscretion.Byte)
            {
                bitParserCtl = new BitParserCtl();
                bitParserCtl.UpdateClick += BitParserCtl_UpdateClick;
                bitParserCtl.Name = "Byte" + i;
                bitParserCtl.Initialize(byte1);
                bitParserCtlList.Add(bitParserCtl);
                pnlTag.Children.Add(bitParserCtl);
                i++;
                temp += "00";
            }
            txt_hex.Text = Utils.AddSpace(temp);
        }

        private void BitParserCtl_UpdateClick(object? sender, string e)
        {
            string value = e as string;
            var v = sender as BitParserCtl;
            int p = Int32.Parse(v.Name.Replace("Byte", ""));
            p = (p * 2) - 2;

            var aStringBuilder = new StringBuilder(txt_hex.Text.Replace(" ", ""));
            aStringBuilder.Remove(p , 2);
            aStringBuilder.Insert(p , value);

            txt_hex.Text = Utils.AddSpace(aStringBuilder.ToString());
        }

        private void txt_hex_Keyup(object sender, KeyEventArgs e)
          {
            
            if (e.Key == Key.Enter)
            {
                string value = txt_hex.Text.Replace(" ","");
                value = value.PadLeft(4, '0');
                var list = Utils.SplitTwoDigit(value);
                int i = 1;
                foreach (var v in list)
                {
                    string name = "Byte" + i;
                    var cByte = bitParserCtlList.FirstOrDefault(s => s.Name == name);
                    cByte.UpdateCheckBox(v);
                    i++;
                }
                txt_hex.Text = Utils.AddSpace(value);
           
            }
        }
    }
}
