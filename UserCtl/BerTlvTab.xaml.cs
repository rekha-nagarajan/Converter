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
    /// Interaction logic for BerTlvTab.xaml
    /// </summary>

    public partial class BerTlvTab : UserControl
    {
        ParentTabParent parenttab = new ParentTabParent();
        public static TagDescription tagDescription { get; set; }
        public BerTlvTab()
        {
            InitializeComponent();
        }
        public void Initialize(string parentId)
        {
            btnConvert.BorderThickness = new Thickness(0);
            btnConvert.FontWeight = FontWeights.Bold;
            btnConvert.Foreground = new SolidColorBrush(Color.FromRgb(0, 100, 0));
            btnConvert.Background = new SolidColorBrush(Colors.Transparent);

            btnClear.BorderThickness = new Thickness(0);
            btnClear.FontWeight = FontWeights.Bold;
            btnClear.Foreground = new SolidColorBrush(Color.FromRgb(0, 100, 0));
            btnClear.Background = new SolidColorBrush(Colors.Transparent);

        }
        private void btnConvert_Click(object sender, RoutedEventArgs e)
        {

            DeserializeData();

            string hexValue = Convert.ToString(txt_hex.Text);
            var hex = hexValue.Replace(" ", "");
            BerTlvTab t = new BerTlvTab();
            var tlvList = t.tlvparse(hex);
            TreeViewItem ParentItem = new TreeViewItem();
            ParentItem.Header = "ParseLog";

            treeview.Items.Add(Print(tlvList, ParentItem));
        }
        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            txt_hex.Text = "";
            
            treeview.Items.Clear();
        }

        /// <summary>
        /// convert string to byte Array
        /// </summary>
        /// <param name="hex"></param>
        /// <returns></returns>
        public static byte[] StringToByteArray(string hex)
        {
            if ((hex.Length % 2) != 0)
                hex = "0" + hex;

            return Enumerable.Range(0, hex.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                             .ToArray();
        }
        /// <summary>
        /// parse the tlv data
        /// </summary>
        /// <param name="hex"></param>
        /// <returns></returns>
        public List<Tlv> tlvparse(string hex)
        {

            byte[] rawData = StringToByteArray(hex);
            List<Tlv> tlvList = new List<Tlv>();

            for (int i = 0; i < rawData.Length;)
            {
                Tlv tlv = new Tlv();

                //parse tag
                bool constructedtag = (Convert.ToByte(rawData[i]) & 0x20) == 0x20;
                bool morebytes = (Convert.ToByte(rawData[i]) & 0x1F) == 0x1F;
                if (morebytes)
                {
                    tlv.Tag += rawData[i].ToString("x2");
                    i++;
                }
                tlv.Tag += rawData[i].ToString("x2");
                i++;

                tlv.Description = GetTagDescription(tlv.Tag);

                //parse length
                if (rawData[i].ToString("x2").Equals("81"))
                    i++;
                tlv.Length = rawData[i].ToString("x2");
                i++;
                int leng = Int32.Parse(tlv.Length, System.Globalization.NumberStyles.HexNumber);

                //parse value
                for (int j = i; j < leng + i; j++)
                {
                    tlv.Value += rawData[j].ToString("x2");
                }

                i = i + leng;
                if (constructedtag && !string.IsNullOrEmpty(tlv.Value))
                    tlv.subTag.AddRange(tlvparse(tlv.Value));

                tlvList.Add(tlv);

            }

            return tlvList;
        }
        /// <summary>
        /// Get Tag Description from the XML1.File
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        private string GetTagDescription(string tag)
        {
            TagDescription tagDesc = new TagDescription();
            string result = "";

            if (tagDescription != null)
            {

                foreach (var t in tagDescription.Tag)
                {

                    if (string.Compare(t.Tag, tag, true) == 0)
                    {
                        result = t.Description;
                        break;
                    }
                }
            }

            return result;
        }


        /// <summary>
        /// Print the data
        /// </summary>
        /// <param name="tlvList"></param>
        /// <param name="space"></param>
        private TreeViewItem Print(List<Tlv> tlvList, TreeViewItem ParentItem)
        {
            foreach (Tlv tlv in tlvList)
            {
                TreeViewItem currentItem = new TreeViewItem();

                currentItem.Header = " Tag : " + tlv.Tag + "   Description : " + tlv.Description + "\n    Length:(" + tlv.Length + ")";
                currentItem.Items.Add(tlv.Value);

                if (tlv.subTag.Count > 0)
                {
                    currentItem = Print(tlv.subTag, currentItem);
                }
                ParentItem.Items.Add(currentItem);

            }
            return ParentItem;
        }
        /// <summary>
        /// Deserialize the data
        /// </summary>
        public static void DeserializeData()
        {
            string filePath = @"D:\BERTLV\hextobin_wpf\hextobin_wpf\ConfigurationFile\TagDescription.xml";
            string fileText = File.ReadAllText(filePath);

            XmlSerializer serializer = new XmlSerializer(typeof(TagDescription));
            StringReader rdr = new StringReader(fileText);
            tagDescription = (TagDescription)serializer.Deserialize(rdr);

        }

       
    }

}



