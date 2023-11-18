using ElectronicJournal.ViewModels;
using Microsoft.Xaml.Behaviors;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ElectronicJournal.Views.Tools
{
    public class MessagesListViewScrollBehavior : Behavior<ListView>
    {
        private int _minHeightOneElement = 100;

        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject?.AddHandler(routedEvent: ScrollViewer.ScrollChangedEvent, handler: new ScrollChangedEventHandler(OnScrollChanged));
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject?.RemoveHandler(routedEvent: ScrollViewer.ScrollChangedEvent, handler: new ScrollChangedEventHandler(OnScrollChanged));
        }

        private void OnScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            ScrollViewer scrollViewer = FindScrollViewer(depObj: sender as DependencyObject);

            if (scrollViewer is null)
                return;

            if (scrollViewer.VerticalOffset != scrollViewer.ScrollableHeight || scrollViewer.ScrollableHeight < _minHeightOneElement)
                return;

            MessagesVM vm = AssociatedObject.DataContext as MessagesVM;
            if (vm != null)
                vm.LoadMoreMessages.Execute(parameter: null);
        }

        private ScrollViewer FindScrollViewer(DependencyObject depObj)
        {
            if (depObj is null)
                return null;

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(reference: depObj); i++)
            {
                var child = VisualTreeHelper.GetChild(reference: depObj, childIndex: i);

                if (child is ScrollViewer scrollViewer)
                    return scrollViewer;

                var result = FindScrollViewer(depObj: child);

                if (result != null)
                    return result;
            }

            return null;
        }
    }
}
