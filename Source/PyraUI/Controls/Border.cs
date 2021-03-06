﻿using Pyratron.UI.Brushes;
using Pyratron.UI.Types;
using Pyratron.UI.Types.Properties;

namespace Pyratron.UI.Controls
{
    /// <summary>
    /// Draws a border and/or background around an element.
    /// </summary>
    public class Border : Decorator
    {
        public static readonly DependencyProperty<Brush> BackgroundProperty =
            DependencyProperty.Register<Border, Brush>(nameof(Background), Color.Transparent, new PropertyMetadata(MetadataOption.IgnoreInheritance));

        public static readonly DependencyProperty<int> CornderRadiusProperty =
            DependencyProperty.Register<Border, int>(nameof(CornerRadius), new PropertyMetadata(MetadataOption.IgnoreInheritance));

        public static readonly DependencyProperty<Thickness> BorderThicknessProperty =
            DependencyProperty.Register<Border, Thickness>(nameof(BorderThickness), new PropertyMetadata(MetadataOption.IgnoreInheritance));

        public static readonly DependencyProperty<Brush> BorderBrushProperty =
            DependencyProperty.Register<Border, Brush>(nameof(BorderBrush), new PropertyMetadata(MetadataOption.IgnoreInheritance));

        /// <summary>
        /// The area inside the border.
        /// </summary>
        public Thickness Padding
        {
            get { return GetValue(PaddingProperty); }
            set { SetValue(PaddingProperty, value); }
        }

        public static readonly DependencyProperty<Thickness> PaddingProperty =
            DependencyProperty.Register<Element, Thickness>(nameof(Padding),
                MetadataOption.IgnoreInheritance | MetadataOption.AffectsMeasure | MetadataOption.AffectsArrange);


        /// <summary>
        /// Background color of the border.
        /// </summary>
        public Brush Background
        {
            get { return GetValue(BackgroundProperty); }
            set { SetValue(BackgroundProperty, value); }
        }

        /// <summary>
        /// Border radius.
        /// </summary>
        public int CornerRadius
        {
            get { return GetValue(CornderRadiusProperty); }
            set { SetValue(CornderRadiusProperty, value); }
        }


        public Thickness BorderThickness
        {
            get { return GetValue(BorderThicknessProperty); }
            set { SetValue(BorderThicknessProperty, value); }
        }

        /// <summary>
        /// Brush to render the border with.
        /// </summary>
        public Brush BorderBrush
        {
            get { return GetValue(BorderBrushProperty); }
            set { SetValue(BorderBrushProperty, value); }
        }


        public Border(Manager manager) : base(manager)
        {
            
        }

        public override void Draw(float delta)
        {

            if (!BorderThickness.IsEmpty)
            {
                Manager.Renderer.DrawRectangle(Bounds, BorderBrush, BorderThickness, CornerRadius, LogicalParentBounds);
                Manager.Renderer.FillRectangle(Bounds.RemoveBorder(BorderThickness), Background,
                    CornerRadius - (BorderThickness.Min.IsClose(BorderThickness.Max) ? BorderThickness.Min + 1 : 0),
                    LogicalParentBounds);
            }
            else
                Manager.Renderer.FillRectangle(Bounds.RemoveBorder(BorderThickness), Background, CornerRadius,
                    LogicalParentBounds);

            base.Draw(delta);
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            Child?.Arrange(new Rectangle(BorderThickness + Padding, finalSize.Remove(BorderThickness).Remove(Padding)));

            return finalSize;
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            // Remove the border and padding, measure the element, and then add them back, as they must be ignored.
            availableSize = availableSize.Remove(BorderThickness).Remove(Padding);
            var size = base.MeasureOverride(availableSize) + BorderThickness + Padding;
            return size;
        }
    }
}