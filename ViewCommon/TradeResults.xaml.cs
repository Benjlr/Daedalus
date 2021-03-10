using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Shapes;

namespace Viewer
{
    /// <summary>
    /// Interaction logic for TradeResults.xaml
    /// </summary>
    public partial class TradeResults : Window
    {
        public TradeResults() {
            InitializeComponent();
        }

        private SortAdorner listViewSortAdorner;
        private GridViewColumnHeader listViewSortCol;
        private void sort_click(object sender, RoutedEventArgs e) {
            if (!(sender is GridViewColumnHeader column)) return;
            var sortBy = column.Tag.ToString();
            if (listViewSortCol != null) {
                AdornerLayer.GetAdornerLayer(listViewSortCol).Remove(listViewSortAdorner);
                Portfolio.Items.SortDescriptions.Clear();
            }

            var newDir = ListSortDirection.Ascending;
            if (Equals(listViewSortCol, column) && listViewSortAdorner.Direction == newDir)
                newDir = ListSortDirection.Descending;

            listViewSortCol = column;
            listViewSortAdorner = new SortAdorner(listViewSortCol, newDir);
            AdornerLayer.GetAdornerLayer(listViewSortCol).Add(listViewSortAdorner);
            Portfolio.Items.SortDescriptions.Add(new SortDescription(sortBy, newDir));


            ((OpenViewModel)DataContext).Reorder(sortBy, newDir);
        }
    }
}
