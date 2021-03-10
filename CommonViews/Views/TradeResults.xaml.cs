using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using CommonViews.Utils;
using DataStructures;

namespace CommonViews.Views
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
