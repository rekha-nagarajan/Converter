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
    /// Interaction logic for ParentTab.xaml
    /// </summary>
    public partial class ParentTabCtl : UserControl
    {
        ParentTabParent parenttab = new ParentTabParent();
        public ParentTabCtl()
        {
            InitializeComponent();
        }
   
        public void InitializeTab()
        {
            ptabParser.Items.Clear();
            ptabParser.BorderBrush = new SolidColorBrush(Colors.LightGreen);
            ptabParser.BorderThickness = new Thickness(4);
            ptabParser.FontSize = 15;


            foreach (var parent in ConfigUtil.content1.Parent)
            {
                TabItem tabItem = new TabItem();

                tabItem.Name = parent.Name;
                tabItem.Width = 200;
                tabItem.Height = 50;
                tabItem.Header = parent.DisplayValue;
                tabItem.BorderBrush = new SolidColorBrush(Colors.WhiteSmoke);
                tabItem.BorderThickness = new Thickness(3);
                tabItem.Background = new SolidColorBrush(Color.FromRgb(144, 238, 144));
                tabItem.Foreground = new SolidColorBrush(Color.FromRgb(0, 100, 0));
                tabItem.FontWeight = FontWeights.Bold;
                tabItem.Content = ChangeTab(parent.Id);

                ptabParser.Items.Add(tabItem);
              // ptabParser.Items.Add(berTlvTab);

            }
        }
        public StackPanel ChangeTab(string parentId)
        {
            StackPanel pnlTag = new StackPanel();
            pnlTag.Children.Clear();
            if (parentId == "1")
            {
                TabCtl tabCtl = new TabCtl();
                tabCtl.InitializeTab(parentId);
                pnlTag.Children.Add(tabCtl);
            }
            
            else if (parentId == "2")
            {
                ShowTreeView treeView = new ShowTreeView();
                treeView.Initialize(parentId);
                pnlTag.Children.Add(treeView);
                
            }
            else if(parentId == "3")
            {
                BerTlvTab ber = new BerTlvTab();
                ber.Initialize(parentId);
                pnlTag.Children.Add(ber);
            }
            return pnlTag;
            
        }

       
       
    }
}



    

