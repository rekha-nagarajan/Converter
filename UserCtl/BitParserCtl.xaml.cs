using System;
using System.Collections.Generic;
using System.IO;
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
using System.Xml.Serialization;

namespace hextobin_wpf.UserCtl
{
    /// <summary>
    /// Interaction logic for BitParserCtl.xaml
    /// </summary>
    public partial class BitParserCtl : UserControl
    {
        List<CheckBox> checkBoxes = new List<CheckBox>();
        
        string byteValue = "";
        public event EventHandler<string> UpdateClick;
        public static BitDiscretion content { get; set; }


        public BitParserCtl()
        {
            InitializeComponent();
        }

        public void Initialize(ByteDiscretionTagByte byteDiscretion)
        {

            CheckBoxCreate(byteDiscretion);
        }

        /// <summary>
        /// Create the checkbox
        /// </summary>
        private void CheckBoxCreate(ByteDiscretionTagByte byteDiscretion)
        {
            PnlCheck.Children.Clear();


            txtByte.Text = byteDiscretion.Name;

            int i = 0;

            foreach (var v in byteDiscretion.Bit)
            {

                CheckBox checkBox = new CheckBox

                {
                    Name = "bit" + i,
                    IsChecked = false,
                    Content = v.ifTrue,
                    Margin = new Thickness(2),

                };
                checkBox.FontSize = 13;
                checkBox.FontWeight = FontWeights.Normal;
                checkBox.Foreground = new SolidColorBrush(Color.FromRgb(0, 100, 0));
                checkBox.BorderBrush = new SolidColorBrush(Colors.LightGreen);
                checkBox.BorderThickness = new Thickness(2);
                i++;
                checkBox.Checked += CheckBox_Checked;
                checkBox.Unchecked += CheckBox_Checked;

                checkBoxes.Add(checkBox);

                PnlCheck.Children.Add(checkBox);
            }


            byteValue = "00";
        }



        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            Handle(sender as CheckBox);
        }
        /// <summary>
        /// Handle Check box
        /// </summary>
        /// <param name="checkBox"></param>

        void Handle(CheckBox checkBox)
        {
            bool flag = checkBox.IsChecked.Value;
            string name = checkBox.Name;
            string strIndex = name.Replace("bit", "");
            int index = Int32.Parse(strIndex);

            char newChar = '0';

            if (flag == true)
                newChar = '1';
            else
                newChar = '0';

            if (Utils.OnlyHexInString(byteValue))
            {
                byte[] hexByte = Utils.StringToByteArray(byteValue);

                Dictionary<string, string> outputList = Utils.ByteArrayToBinary(hexByte);
                for (int i = 1; i <= outputList.Count; i++)
                {
                    string temp = outputList["Byte" + i];

                    StringBuilder sb = new StringBuilder(temp);
                    sb[index] = newChar;
                    temp = sb.ToString();

                    StringBuilder sb1 = new StringBuilder(Utils.BinaryStringToHexString(temp));

                    byteValue = sb1.ToString();
                    UpdateClick.Invoke(this, byteValue);
                }
            }
        }
        /// <summary>
        /// Text Changed
        /// </summary>
        public void UpdateCheckBox(string value)
        {
            byteValue = value;
            if (Utils.OnlyHexInString(byteValue))
            {
                byte[] hexByte = Utils.StringToByteArray(byteValue);
                string str = Utils.ByteToBinary(hexByte[0]);
                int index = 0;
                foreach (var s in str)
                {
                    var selectedchk = checkBoxes.Find(s => s.Name == "bit" + index);
                    if (selectedchk != null)
                        setValue(selectedchk, s);

                    index++;
                }
            }
        }

        /// <summary>
        /// set the value for checkbox
        /// </summary>
        /// <param name="checkBox"></param>
        /// <param name="val"></param>
        private void setValue(CheckBox checkBox, char val)
        {
            if (val == '1')
                checkBox.IsChecked = true;
            else
                checkBox.IsChecked = false;
        }
    }
}
