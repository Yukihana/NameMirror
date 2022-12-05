using CSX.DotNet6.Y2022.ReferencedNaming.Types;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace CSX.Wpf.Y2022.RNGui.Support.Sorting
{
    internal class GridSortBehaviour
    {
        // Properties
        #region DProp : Sorter
        public static readonly DependencyProperty SorterProperty =
            DependencyProperty.RegisterAttached(
                "Sorter",
                typeof(IComparer),
                typeof(GridSortBehaviour));
        public static IComparer GetSorter(DataGridColumn column) => (IComparer)column.GetValue(SorterProperty);
        public static void SetSorter(DataGridColumn column, IComparer value) => column.SetValue(SorterProperty, value);
        #endregion
        #region DProp : EnableNaturalSort
        public static readonly DependencyProperty SortingEnabledProperty =
           DependencyProperty.RegisterAttached(
               "SortingEnabled",
               typeof(bool),
               typeof(GridSortBehaviour),
               new UIPropertyMetadata(false, OnEnableNaturalSortChanged));
        // Getter
        public static bool GetSortingEnabled(DataGrid grid)
            => (bool)grid.GetValue(SortingEnabledProperty);
        // Setter
        public static void SetSortingEnabled(DataGrid grid, bool value)
            => grid.SetValue(SortingEnabledProperty, value);
        #endregion

        // Methods
        private static void OnEnableNaturalSortChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            // Validate
            var grid = (DataGrid)d;
            if (grid == null) return;

            // Subscription
            if ((bool)e.NewValue && !(bool)e.OldValue)
                grid.Sorting += HandleSorting;
            else
                grid.Sorting -= HandleSorting;
        }
        private static void HandleSorting(object sender, DataGridSortingEventArgs e)
        {
            // Validate
            var grid = (DataGrid)sender;
            if (grid == null || !GetSortingEnabled(grid)) return;

            // Get direction
            var direction = e.Column.SortDirection == ListSortDirection.Ascending
                ? ListSortDirection.Descending
                : ListSortDirection.Ascending;

            // Set event response
            e.Handled = true;
            e.Column.SortDirection = direction;

            // Get the collection source and sort
            if (grid.ItemsSource == null)
                return;
            var collection = grid.ItemsSource.Cast<object>();

            // Prepare sorter
            var comparer = GetSorter(e.Column);
            PropertyInfo? property = collection.GetType().GetGenericArguments()[0].GetProperty(e.Column.SortMemberPath);
            var result = direction == ListSortDirection.Ascending
                ? collection.OrderBy(x => property?.GetValue(x) as string ?? string.Empty, comparer as IComparer<object>).ToArray()
                : collection.OrderByDescending(x => property?.GetValue(x) as string ?? string.Empty, comparer as IComparer<object>).ToArray();

            // Reflect changes in original
            if (collection is IList list)
            {
                for (int i = 0; i < result.Length; i++)
                {
                    if (Equals(result[i], list[i]))
                        continue;
                    int j = list.IndexOf(result[i]);
                    (list[i], list[j]) = (list[j], list[i]);
                }
            }

            // Refresh the view
            CollectionViewSource.GetDefaultView(grid.ItemsSource).Refresh();

            /* View-Only sorting
            // Get sort target
            if (CollectionViewSource.GetDefaultView(grid.ItemsSource) is not ListCollectionView source)
                throw new InvalidOperationException("Type mismatch");
            // Assign sorter
            source.CustomSort = new NaturalSorter(direction, e.Column.SortMemberPath);
            */
        }
    }
}