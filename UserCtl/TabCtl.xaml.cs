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
    /// Interaction logic for TabCtl.xaml
    /// </summary>
    public partial class TabCtl : UserControl
    {
        
        public TabCtl()
        {
            InitializeComponent();
            
        }
        
        public void InitializeTab(string parent)
           {
            tabTagParser.Items.Clear();
            tabTagParser.BorderBrush=new SolidColorBrush(Colors.LightGreen);
            tabTagParser.BorderThickness= new Thickness(4);
            tabTagParser.FontSize = 15;
            

            foreach (var tag in ConfigUtil.content.Tag)
            {
                TabItem tabItem = new TabItem();
             
                tabItem.Name = tag.Name;
                tabItem.Width = 200;
                tabItem.Height = 50;
                tabItem.Header = tag.Name;
                tabItem.BorderBrush = new SolidColorBrush(Colors.WhiteSmoke);
                tabItem.BorderThickness = new Thickness(3);
                tabItem.Background = new SolidColorBrush(Color.FromRgb(144, 238, 144));
                tabItem.Foreground = new SolidColorBrush(Color.FromRgb(0, 100, 0));
                tabItem.FontWeight = FontWeights.Bold;
                tabItem.Content = ChangeTab(tag.Id);
               
                tabTagParser.Items.Add(tabItem);
            }
        }
        private StackPanel ChangeTab(string tagId)
        {
            StackPanel pnlTag = new StackPanel();
            pnlTag.Children.Clear();
            TagParserCtl tagCtl = new TagParserCtl();
            tagCtl.InitializeTag(tagId);
            pnlTag.Children.Add(tagCtl);
            return pnlTag;

        } 
    }
}
