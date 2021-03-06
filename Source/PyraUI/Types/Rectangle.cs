﻿using System;
using System.ComponentModel;
using System.Globalization;

namespace Pyratron.UI.Types
{
    public struct Rectangle
    {
        public static readonly Rectangle Empty = new Rectangle();
        public static readonly Rectangle Infinity = new Rectangle(0, 0, double.PositiveInfinity, double.PositiveInfinity);

        public Rectangle(double x, double y, double width, double height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        public Rectangle(Point location, double width, double height)
        {
            X = location.X;
            Y = location.Y;
            Width = width;
            Height = height;
        }

        public Rectangle(Point location, Size size) : this(location.X, location.Y, size.Width, size.Height)
        {
        }

        public Rectangle(Size size) : this(Point.Zero, size)
        {
        }

        public Size Size => new Size(Width, Height);

        public Point Point => new Point(X, Y);

        public double X { get; set; }

        public double Y { get; set; }

        public double Width { get; set; }

        public double Height { get; set; }

        [Browsable(false)]
        public double Left => X;

        [Browsable(false)]
        public double Top => Y;

        [Browsable(false)]
        public double Right => X + Width;

        [Browsable(false)]
        public double Bottom => Y + Height;

        [Browsable(false)]
        public bool IsEmpty => this == Empty;

        [Browsable(false)]
        public bool IsInfinity => this == Infinity;

        [Browsable(false)]
        public Point TopLeft => new Point(Left, Top);


        [Browsable(false)]
        public Point BottomLeft => new Point(Left, Bottom);


        [Browsable(false)]
        public Point TopRight => new Point(Right, Top);


        [Browsable(false)]
        public Point BottomRight => new Point(Right, Bottom);

        public static implicit operator Rectangle(Thickness thickness)
            =>
                new Rectangle(thickness.Left, thickness.Top, thickness.Right - thickness.Left,
                    thickness.Bottom - thickness.Top);

        public bool Contains(double x, double y) => X <= x &&
                                                    x < X + Width &&
                                                    Y <= y &&
                                                    y < Y + Height;


        public static bool operator ==(Rectangle left, Rectangle right) => left.Equals(right);

        public static bool operator !=(Rectangle left, Rectangle right) => !(left == right);

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public bool Equals(Rectangle other)
        {
            return X.Equals(other.X) && Y.Equals(other.Y) && Width.Equals(other.Width) && Height.Equals(other.Height);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = X.GetHashCode();
                hashCode = (hashCode * 397) ^ Y.GetHashCode();
                hashCode = (hashCode * 397) ^ Width.GetHashCode();
                hashCode = (hashCode * 397) ^ Height.GetHashCode();
                return hashCode;
            }
        }

        public bool Contains(Point pt) => Contains(pt.X, pt.Y);

        public bool Contains(Rectangle rect) => (X <= rect.X) &&
                                                ((rect.X + rect.Width) <= (X + Width)) &&
                                                (Y <= rect.Y) &&
                                                ((rect.Y + rect.Height) <= (Y + Height));

        public bool Intersects(Rectangle rect) => (rect.X < X + Width) &&
                                                  (X < (rect.X + rect.Width)) &&
                                                  (rect.Y < Y + Height) &&
                                                  (Y < rect.Y + rect.Height);

        public override string ToString()
            => "X=" + X.ToString(CultureInfo.CurrentCulture) + ",Y=" + Y.ToString(CultureInfo.CurrentCulture) +
               ",Width=" + Width.ToString(CultureInfo.CurrentCulture) +
               ",Height=" + Height.ToString(CultureInfo.CurrentCulture);

        /// <summary>
        /// Extend the rectangle dimensions by the specified size.
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        public Rectangle Extend(Size size) => new Rectangle(X, Y, Width + size.Width, Height + size.Height);

        /// <summary>
        /// Extend the rectangle to include the specified thickness.
        /// </summary>
        public Rectangle Extend(Thickness thickness)
            =>
                new Rectangle(0, 0, thickness.Right + thickness.Left + Width,
                    thickness.Bottom + thickness.Top + Height);

        /// <summary>
        /// Offset the rectangle by the specified point. (Add to the X and Y)
        /// </summary>
        public Rectangle Offset(Point position) => new Rectangle(X + position.X, Y + position.Y, Width, Height);

        /// <summary>
        /// Offset the rectangle by the specified rectangle coordinates. (Add to the X and Y)
        /// </summary>
        public Rectangle Offset(Rectangle rectangle) => new Rectangle(X + rectangle.X, Y + rectangle.Y, Width, Height);

        /// <summary>
        /// Limit the area of this rectangle to the specified bounds.
        /// </summary>
        public Rectangle FitToBounds(Rectangle bounds)
        {
            var x1 = Math.Max(X, bounds.X);
            var x2 = Math.Min(X + Width, bounds.X + bounds.Width);
            var y1 = Math.Max(Y, bounds.Y);
            var y2 = Math.Min(Y + Height, bounds.Y + bounds.Height);

            return x2 >= x1 && y2 >= y1
                ? new Rectangle(x1, y1, x2 - x1, y2 - y1)
                : Empty;
        }

        /// <summary>
        /// Determines if the rectangle is equal to another rectangle within margin of error (because of floating point precision).
        /// </summary>
        public bool IsClose(Rectangle rectangle) => Point.IsClose(rectangle.Point) && Size.IsClose(rectangle.Size);

        /// <summary>
        /// Remove the specified thickness from the outsides of the rectangle in order to move the rectangle further inward. (ADDING to the X and Y values)
        /// </summary>
        public Rectangle RemoveBorder(Thickness borderThickness)
            =>
                new Rectangle(X + borderThickness.Left, Y + borderThickness.Right, Width - borderThickness.Width,
                    Height - borderThickness.Height);

        /// <summary>
        /// Add the specified thickness from the outsides of the rectangle in order to move the rectangle further outward. (SUBTRACTING from the X and Y values)
        /// </summary>
        public Rectangle AddBorder(Thickness borderThickness)
            =>
                new Rectangle(X - borderThickness.Left, Y - borderThickness.Right, Width + borderThickness.Width,
                    Height + borderThickness.Height);

        /// <summary>
        /// Remove the specified thickness from the rectangle to make it smaller.
        /// </summary>
        public Rectangle Remove(Thickness thickness)
            =>
                new Rectangle(X -thickness.Left, Y - thickness.Right, Width -thickness.Width,
                    Height - thickness.Height);

        public Rectangle Min(Rectangle other) => new Rectangle(X, Y, Math.Min(Width, other.Width), Math.Min(Height,other.Height));
    }
}