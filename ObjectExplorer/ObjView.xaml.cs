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

namespace ObjectExplorer
{
    /// <summary>
    /// Interaction logic for ObjView.xaml
    /// </summary>
    public partial class ObjView : UserControl
    {
        public ObjView()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void TreeViewItem_MouseRightButtonDown(object sender, MouseEventArgs e)
        {
            TreeViewItem item = sender as TreeViewItem;
            if (item != null)
            {
                item.Focus();
                e.Handled = true;
            }
        }

        private void TreeViewItem_MouseRightButtonUp(object sender, MouseEventArgs e)
        {
            ContextMenu contextMenu = new ContextMenu();
            TreeViewItem item = sender as TreeViewItem;
            if (item != null)
            {
                var prop = (this.DataContext as ObjectPropertyController);
                prop.TreeViewItemRigthClickCommand.Execute(item);
                contextMenu.ItemsSource = prop.ContextOperations;
                this.ContextMenu = contextMenu;
                contextMenu.IsOpen = true;

            }
        }
    }
}
