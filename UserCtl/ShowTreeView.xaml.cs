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
    /// Interaction logic for TreeView.xaml
    /// </summary>
    public partial class ShowTreeView : UserControl
    {
        List<APDU> apduList = new List<APDU>();
        ReadCardLog readCard = new ReadCardLog();
        public string FileName;

        /// <summary>
        /// 
        /// </summary>
        public ShowTreeView()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parentId"></param>
        public void Initialize(string parentId)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void btnConvert_Click(object sender, RoutedEventArgs e)
        {
            string fileName = @"D:\BERTLV\hextobin_wpf\hextobin_wpf\ConfigurationFile\CI_M-TIP01_T06_S01.txt";

            string filetxt = Utils.ReadFile(fileName);
            List<APDU> apduList = readCard.ParseLog(filetxt);
            TreeViewItem ParentItem = new TreeViewItem();
            ParentItem.Header = "ParseLog";
            treeview.Items.Add(ShowTree(apduList, ParentItem));

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            treeview.Items.Clear();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
        /// <param name="ParentItem"></param>
        private TreeViewItem ShowTree(List<APDU> apduList, TreeViewItem ParentItem)
        {

            foreach (var apdu in apduList)
            {
                TreeViewItem currentItem = new TreeViewItem();
               
                currentItem.Header ="Parse Command And Response ";
                currentItem.Items.Add(addCommand(apdu.command));
                currentItem.Items.Add(addResponse(apdu.response));
                
                ParentItem.Items.Add(currentItem);


            }
            return ParentItem;
        }

        private TreeViewItem addCommand(Command command)
        {
            TreeViewItem currentItem = new TreeViewItem();
            currentItem.Header = "Command : " + command.RawData;
            currentItem.Items.Add("CLA -> " + command.Cla);
            currentItem.Items.Add("INS -> " + command.Ins);
            currentItem.Items.Add("P1 -> " + command.P1);
            currentItem.Items.Add("P2 -> " + command.P2);
            currentItem.Items.Add("Lc -> " + command.Lc);
            currentItem.Items.Add("Data -> " + command.Data);
            currentItem.Items.Add("Le -> " + command.Le);

            return currentItem;
        }

        private TreeViewItem addResponse(Response response)
        {

            TreeViewItem currentItem = new TreeViewItem();
            //TreeViewItem childItem = new TreeViewItem();
            currentItem.Header = "Response : " + response.RawData;
            if (response.tlv != null && response.tlv.Count > 0)
            {
                TreeViewItem childItem = new TreeViewItem();
                childItem.Header = "Data";
                currentItem.Items.Add(addDataResponse(response.tlv, childItem));
            }
            currentItem.Items.Add("SW1 -> " + response.Sw1);
            currentItem.Items.Add("SW2 -> " + response.Sw2);

            return currentItem;
        }

        private TreeViewItem addDataResponse(List<Tlv> tlvList, TreeViewItem ParentItem)
        {
            foreach (Tlv tlv in tlvList)
            {
                TreeViewItem currentItem = new TreeViewItem();

                currentItem.Header = " Tag : " + tlv.Tag + "   Description : " + tlv.Description + "\n    Length:(" + tlv.Length + ")".ToUpper();
                currentItem.Items.Add(tlv.Value);

                if (tlv.subTag.Count > 0)
                {
                    currentItem = addDataResponse(tlv.subTag, currentItem);
                }
                ParentItem.Items.Add(currentItem);

            }
            return ParentItem;
        }

    }
}

