using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Collections;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace FCHA.WPFHelpers
{
    public static class ItemsControlHelper
    {
        public static void AddItem(ItemsControl itemsControl, object item, int insertIndex)
        {
            if (itemsControl.Items.IndexOf(item) == -1)
            {
                if (itemsControl.ItemsSource != null)
                {
                    IList iList = itemsControl.ItemsSource as IList;
                    if (iList != null)
                    {
                        iList.Add(item);
                    }
                    else
                    {
                        Type type = itemsControl.ItemsSource.GetType();
                        Type genericList = type.GetInterface("IList`1");
                        if (genericList != null)
                        {
                            type.GetMethod("Insert").Invoke(itemsControl.ItemsSource, new object[] { insertIndex, item });
                        }
                    }
                }
                else
                {
                    itemsControl.Items.Add(item);
                }
            }
        }

        public static void RemoveItem(ItemsControl itemsControl, object item)
        {
            if (item != null)
            {
                int index = itemsControl.Items.IndexOf(item);
                if (index != -1)
                {
                    if (itemsControl.ItemsSource != null)
                    {
                        IList iList = itemsControl.ItemsSource as IList;
                        if (iList != null)
                        {
                            iList.Remove(item);
                        }
                        else
                        {
                            Type type = itemsControl.ItemsSource.GetType();
                            Type genericList = type.GetInterface("IList`1");
                            if (genericList != null)
                            {
                                type.GetMethod("RemoveAt").Invoke(itemsControl.ItemsSource, new object[] { index });
                            }
                        }
                    }
                    else
                    {
                        itemsControl.Items.Remove(item);
                    }
                }
            }
        }

        public static object GetDataObjectFromItemsControl(ItemsControl itemsControl, Point p)
        {
            UIElement element = itemsControl.InputHitTest(p) as UIElement;
            while (element != null)
            {
                if (element == itemsControl)
                    return null;

                object data = itemsControl.ItemContainerGenerator.ItemFromContainer(element);
                if (data != DependencyProperty.UnsetValue)
                {
                    return data;
                }
                else
                {
                    element = VisualTreeHelper.GetParent(element) as UIElement;
                }
            }
            return null;
        }

        public static UIElement GetItemContainerFromItemsControl(ItemsControl itemsControl)
        {
            UIElement container = null;
            if (itemsControl != null && itemsControl.Items.Count > 0)
            {
                container = itemsControl.ItemContainerGenerator.ContainerFromIndex(0) as UIElement;
            }
            else
            {
                container = itemsControl;
            }
            return container;
        }
    }
}
