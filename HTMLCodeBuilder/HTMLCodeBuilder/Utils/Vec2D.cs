using HTMLCodeBuilder.SVGelements;
using System;
using System.Runtime.InteropServices;

namespace HTMLCodeBuilder.Utils
{
    public enum Units
    {
        MM = 1,
        CM = 2,
        M = 3,
        PX = 4,
        IN = 5,
        NONE = 6
    }

    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct Vec2D : IEquatable<Vec2D>
    {
        public static readonly Vec2D ZERO = new Vec2D(0,0);

        public static readonly Vec2D LEFT = new Vec2D(-1, 0);

        public static readonly Vec2D RIGHT = new Vec2D(1, 0);

        public static readonly Vec2D UP = new Vec2D(0, 1);

        public static readonly Vec2D DOWN = new Vec2D(0, -1);

        public static readonly Vec2D ONE = new Vec2D(1, 1);

        public static readonly double PIXEL_PER_MM = 3.7795275591;

        public static readonly double PIXEL_PER_CM = 37.795275591;

        public static readonly double PIXEL_PER_M = 3779.5275591;

        public static readonly double PIXEL_PER_INCH = 96.000000001139995;

        private double x, y;

        private double UnitsScale;

        private Units vecUnits;

        private string UnitsStr;

        public Units VecUnits { get { return vecUnits; } set { { UpdateUnits(value); } } }

        public double X { get { return x; } set { setX(value); } }

        public double Y { get { return y; } set { setY(value); } }

        public string Xs { get; private set; }

        public string Ys { get; private set; }

        public double Xpix { get; private set; }

        public double Ypix { get; private set; }

        public double Ex { get; private set; }

        public double Ey { get; private set; }

        public double Norm { get; private set; }

        public static double dot(Vec2D a, Vec2D b)
        {
            return a.X * b.X + a.Y * b.Y;
        }

        public double dot(Vec2D b)
        {
            return X * b.X + Y * b.Y;
        }

        public static Vec2D operator *(Vec2D a, double b)
        {
            return new Vec2D(a.X * b, a.Y * b);
        }

        public void normalize()
        {
            Norm = Math.Sqrt(X * X + Y * Y);
            Ex = X / Norm;
            Ey = Y / Norm;
        }

        public static Vec2D operator *(double b, Vec2D a)
        {
            return new Vec2D(a.X * b, a.Y * b);
        }

        public static Vec2D operator *(Vec2D a, Vec2D b)
        {
            return new Vec2D(a.X * b.X, a.Y * b.Y);
        }

        public static Vec2D operator /(Vec2D a, Vec2D b)
        {
            return new Vec2D(a.X / b.X, a.Y / b.Y);
        }

        public static Vec2D operator /(double b, Vec2D a)
        {
            return new Vec2D(a.X / b, a.Y / b);
        }

        public static Vec2D operator /(Vec2D a, double b)
        {
            return new Vec2D(a.X / b, a.Y / b);
        }

        private void setX(double x_)
        {
            x = x_;
            Xpix = x * UnitsScale;
            Xs = SVGElements.num2str(x) + UnitsStr;
        }

        private void setY(double y_)
        {
            y = y_;
            Ypix = y * UnitsScale;
            Ys = SVGElements.num2str(y) + UnitsStr;
        }

        private void UpdateUnits(Units newUnits)
        {
            if (newUnits == VecUnits)
            {
                return;
            }

            vecUnits = newUnits;

            switch (VecUnits)
            {
                case Units.CM:

                    UnitsScale = PIXEL_PER_CM;

                    UnitsStr = "cm";

                    Xpix = X * UnitsScale;

                    Ypix = Y * UnitsScale;

                    Xs = SVGElements.num2str(x) + UnitsStr;

                    Ys = SVGElements.num2str(y) + UnitsStr;
                    break;
                case Units.MM:
                    UnitsScale = PIXEL_PER_MM;

                    UnitsStr = "mm";

                    Xpix = X * UnitsScale;

                    Ypix = Y * UnitsScale;

                    Xs = SVGElements.num2str(x) + UnitsStr;

                    Ys = SVGElements.num2str(y) + UnitsStr;
                    break;
                case Units.M:

                    UnitsScale = PIXEL_PER_M;

                    UnitsStr = "m";

                    Xpix = X * UnitsScale;

                    Ypix = Y * UnitsScale;

                    Xs = SVGElements.num2str(x) + UnitsStr;

                    Ys = SVGElements.num2str(y) + UnitsStr;
                    break;
                case Units.IN:
                    UnitsScale = PIXEL_PER_INCH;

                    UnitsStr = "in";

                    Xpix = X * UnitsScale;

                    Ypix = Y * UnitsScale;

                    Xs = SVGElements.num2str(x) + UnitsStr;

                    Ys = SVGElements.num2str(y) + UnitsStr;
                    break;

                case Units.NONE:
                    UnitsScale = 1;

                    UnitsStr = "";

                    Xpix = X * UnitsScale;

                    Ypix = Y * UnitsScale;

                    Xs = SVGElements.num2str(x) + UnitsStr;

                    Ys = SVGElements.num2str(y) + UnitsStr;
                    break;
            }
        }

        public bool Equals(Vec2D other)
        {
            if (other.VecUnits != VecUnits)
            {
                return false;
            }
            if (other.X != X)
            {
                return false;
            }
            if (other.Y != Y)
            {
                return false;
            }

            return true;
        }

        public Vec2D(double x_, double y_)
        {
            vecUnits = Units.MM;

            UnitsStr = "mm";

            UnitsScale = PIXEL_PER_MM;

            x = x_;

            y = y_;

            Norm = Math.Sqrt(x * x + y * y);

            Ex = x / Norm;

            Ey = y / Norm;

            Xs = SVGElements.num2str(x) + UnitsStr;

            Ys = SVGElements.num2str(y) + UnitsStr;

            Xpix = x * UnitsScale;

            Ypix = y * UnitsScale;

        }

    }

}
