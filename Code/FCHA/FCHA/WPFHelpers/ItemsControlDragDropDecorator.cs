using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace FCHA.WPFHelpers
{
    public class ItemsControlDragDropDecorator : BaseDecorator
    {
        private bool m_bIsMouseDown;
        private object m_data;
        private Point m_ptDragStartPosition;
        private bool m_bIsDragging;
        private DragAdorner m_itemAdorner;

        static ItemsControlDragDropDecorator()
        {
            ItemTypeProperty = DependencyProperty.Register("ItemType", typeof(Type), typeof(ItemsControlDragDropDecorator), new FrameworkPropertyMetadata(null));
            DataTemplateProperty = DependencyProperty.Register("DataTemplate", typeof(DataTemplate), typeof(ItemsControlDragDropDecorator), new FrameworkPropertyMetadata(null));
        }

        public ItemsControlDragDropDecorator() : base()
        {
            m_bIsMouseDown = false;
            m_bIsDragging = false;
            this.Loaded += new RoutedEventHandler(DraggableItemsControl_Loaded);
        }

        void DraggableItemsControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (!(base.DecoratedUIElement is ItemsControl))
            {
                throw new InvalidCastException(string.Format("ItemsControlDragDecorator cannot have child of type {0}", Child.GetType()));
            }
            ItemsControl itemsControl = (ItemsControl)DecoratedUIElement;
            itemsControl.AllowDrop = AllowDrop;
            itemsControl.PreviewMouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(_itemsControl_PreviewMouseLeftButtonDown);
            itemsControl.PreviewMouseMove += new System.Windows.Input.MouseEventHandler(_itemsControl_PreviewMouseMove);
            itemsControl.PreviewMouseLeftButtonUp += new System.Windows.Input.MouseButtonEventHandler(itemsControl_PreviewMouseLeftButtonUp);
            itemsControl.PreviewDrop += new DragEventHandler(itemsControl_PreviewDrop);
            itemsControl.PreviewQueryContinueDrag += new QueryContinueDragEventHandler(itemsControl_PreviewQueryContinueDrag);
            itemsControl.PreviewDragEnter += new DragEventHandler(itemsControl_PreviewDragEnter);
            itemsControl.PreviewDragOver += new DragEventHandler(itemsControl_PreviewDragOver);
            itemsControl.DragLeave += new DragEventHandler(itemsControl_DragLeave);
        }

        #region IDecorator Members

        #endregion

        #region Dependency Properties

        public static readonly DependencyProperty ItemTypeProperty;

        public Type ItemType
        {
            get { return (Type)base.GetValue(ItemTypeProperty); }
            set { base.SetValue(ItemTypeProperty, value); }
        }

        public static readonly DependencyProperty DataTemplateProperty;

        public DataTemplate DataTemplate
        {
            get { return (DataTemplate)base.GetValue(DataTemplateProperty); }
            set { base.SetValue(DataTemplateProperty, value); }
        }

        #endregion

        #region Button Events

        void itemsControl_PreviewMouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            ResetState((ItemsControl)sender);
            e.Handled = true;
        }

        void _itemsControl_PreviewMouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (m_bIsMouseDown)
            {
                ItemsControl itemsControl = (ItemsControl)sender;
                Point currentPosition = e.GetPosition(itemsControl);
                if ((m_bIsDragging == false) && (Math.Abs(currentPosition.X - m_ptDragStartPosition.X) > SystemParameters.MinimumHorizontalDragDistance) ||
                    (Math.Abs(currentPosition.Y - m_ptDragStartPosition.Y) > SystemParameters.MinimumVerticalDragDistance))
                {
                    DragStarted(itemsControl);
                }
            }
        }

        void _itemsControl_PreviewMouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            ItemsControl itemsControl = sender as ItemsControl;
            if (sender != null)
            {
                Point p = e.GetPosition(itemsControl);
                m_data = ItemsControlHelper.GetDataObjectFromItemsControl(itemsControl, p);
                if (m_data != null)
                {
                    m_bIsMouseDown = true;
                    m_ptDragStartPosition = p;
                }
            }
        }
        #endregion 

        #region Drag Events

        void itemsControl_DragLeave(object sender, DragEventArgs e)
        {
            DetachDragAdorner();
            e.Handled = true;
        }

        void itemsControl_PreviewDragOver(object sender, DragEventArgs e)
        {
            ItemsControl itemsControl = (ItemsControl)sender;
            if (e.Data.GetDataPresent(ItemType))
            {
                UpdateDragAdorner(e.GetPosition(itemsControl));
            }
            e.Handled = true;
        }

        void itemsControl_PreviewDragEnter(object sender, DragEventArgs e)
        {
            ItemsControl itemsControl = (ItemsControl)sender;
            if (e.Data.GetDataPresent(ItemType))
            {
                object data = e.Data.GetData(ItemType);
                InitializeDragAdorner(itemsControl, data, e.GetPosition(itemsControl));
            }
            e.Handled = true;
        }

        void itemsControl_PreviewQueryContinueDrag(object sender, QueryContinueDragEventArgs e)
        {
            if (e.EscapePressed)
            {
                e.Action = DragAction.Cancel;
                ResetState((ItemsControl)sender);
                DetachDragAdorner();
                e.Handled = true;
            }
        }

        void itemsControl_PreviewDrop(object sender, DragEventArgs e)
        {
            ItemsControl itemsControl = (ItemsControl)sender;
            if (e.Data.GetDataPresent(ItemType))
            {
                object itemToAdd = e.Data.GetData(ItemType);
                e.Effects = ((e.KeyStates & DragDropKeyStates.ControlKey) == 0) ?
                    DragDropEffects.Copy : DragDropEffects.Move;
                ItemsControlHelper.AddItem(itemsControl, itemToAdd, itemsControl.Items.Count);
            }
            else
            {
                e.Effects = DragDropEffects.None;
            }
            DetachDragAdorner();
            e.Handled = true;
        }

        #endregion

        #region Private Methods

        private void DragStarted(ItemsControl itemsControl)
        {
            if (m_data.GetType() != ItemType)
            {
                return;
            }
            m_bIsDragging = true;
            DataObject dObject = new DataObject(ItemType, m_data);
            itemsControl.AllowDrop = false;
            DragDropEffects e = DragDrop.DoDragDrop(itemsControl, dObject, DragDropEffects.Copy | DragDropEffects.Move);
            if ((e & DragDropEffects.Move) != 0)
            {
                ItemsControlHelper.RemoveItem(itemsControl, m_data);
            }
            ResetState(itemsControl);
        }

        private void ResetState(ItemsControl itemsControl)
        {
            m_bIsMouseDown = false;
            m_bIsDragging = false;
            m_data = null;
            itemsControl.AllowDrop = AllowDrop;
        }

        private void InitializeDragAdorner(ItemsControl itemsControl, object dragData, Point startPosition)
        {
            if (this.DataTemplate != null)
            {
                if (m_itemAdorner == null)
                {
                    var adornerLayer = AdornerLayer.GetAdornerLayer(itemsControl);
                    UIElement itemContainer = ItemsControlHelper.GetItemContainerFromItemsControl(itemsControl);
                    if (itemContainer != null)
                    {
                        m_itemAdorner = new DragAdorner(dragData, DataTemplate,
                                itemContainer, adornerLayer);
                        m_itemAdorner.UpdatePosition(startPosition.X, startPosition.Y);
                    }
                }
            }
        }

        private void UpdateDragAdorner(Point currentPosition)
        {
            if (m_itemAdorner != null)
            {
                m_itemAdorner.UpdatePosition(currentPosition.X, currentPosition.Y);
            }
        }

        private void DetachDragAdorner()
        {
            if (m_itemAdorner != null)
            {
                m_itemAdorner.Destroy();
                m_itemAdorner = null;
            }
        }

        #endregion
    }
}
