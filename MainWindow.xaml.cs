using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using hextobin_wpf.UserCtl;

namespace hextobin_wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class Hextobin : Window
    {
        public Hextobin()
        {
            InitializeComponent();

            Initialize();
        }
        private void Initialize()
        {
            string configFile = @"D:\BERTLV\hextobin_wpf\hextobin_wpf\ConfigurationFile\config.xml";
            string parentfile = @"D:\BERTLV\hextobin_wpf\hextobin_wpf\ConfigurationFile\ParentTab.xml";
            ConfigUtil.DeserializeData(configFile);
            ConfigUtil.DeserializeData1(parentfile);
            pnlmain.Children.Clear();
            ParentTabCtl ptabCtl = new ParentTabCtl();
            ptabCtl.InitializeTab();
            
            pnlmain.Children.Add(ptabCtl);
        }
        //public void Bertlv()
        //{
        //    parenttab = ConfigUtil.GetParentDetails();
        //    pnlmain.Children.Clear();
        //    BerTlvTab tabCtl = new BerTlvTab();
        //    tabCtl.Initialize();

        //    pnlmain.Children.Add(tabCtl);

        //}
    }
    }

