using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace FCHA.WPFHelpers
{
    public class DragAdorner : Adorner
    {
        private ContentPresenter m_contentPresenter;
        private AdornerLayer m_adornerLayer;
        private double m_leftOffset;
        private double m_topOffset;

        public DragAdorner(object data, DataTemplate dataTemplate, UIElement adornedElement, AdornerLayer adornerLayer)
            : base(adornedElement)
        {
            m_adornerLayer = adornerLayer;

            m_contentPresenter = new ContentPresenter()
            { Content = data, ContentTemplate = dataTemplate, Opacity = 0.75 };

            m_adornerLayer.Add(this);
        }

        protected override Size MeasureOverride(Size constraint)
        {
            m_contentPresenter.Measure(constraint);
            return m_contentPresenter.DesiredSize;
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            m_contentPresenter.Arrange(new Rect(finalSize));
            return finalSize;
        }

        protected override Visual GetVisualChild(int index)
        {
            return m_contentPresenter;
        }

        protected override int VisualChildrenCount
        {
            get { return 1; }
        }

        public void UpdatePosition(double left, double top)
        {
            m_leftOffset = left;
            m_topOffset = top;
            if (m_adornerLayer != null)
            {
                m_adornerLayer.Update(this.AdornedElement);
            }
        }

        public override GeneralTransform GetDesiredTransform(GeneralTransform transform)
        {
            GeneralTransformGroup result = new GeneralTransformGroup();
            result.Children.Add(base.GetDesiredTransform(transform));
            result.Children.Add(new TranslateTransform(m_leftOffset, m_topOffset));
            return result;
        }

        public void Destroy()
        {
            m_adornerLayer.Remove(this);
        }
    }
}
