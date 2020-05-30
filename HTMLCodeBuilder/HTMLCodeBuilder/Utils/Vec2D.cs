using HTMLCodeBuilder.SVGelements;
using System;
using System.Runtime.InteropServices;

namespace HTMLCodeBuilder.Utils
{
    public enum Units: ushort
    {
        MM = 1,
        CM = 2,
        M = 3,
        PX = 4,
        IN = 5,
        NONE = 6
    }

    public static class PixPerUnit
    {
        public static readonly double PIXEL_PER_MM = 3.7795275591;

        public static readonly double PIXEL_PER_CM = 37.795275591;

        public static readonly double PIXEL_PER_M = 3779.5275591;

        public static readonly double PIXEL_PER_INCH = 96.000000001139995;
    }

    [Serializable]
    [StructLayout(LayoutKind.Explicit, Pack = 1, Size = 142)]
    public struct Vec2D : IEquatable<Vec2D>
    {
        #region Default vectors
        public static readonly Vec2D ZERO = new Vec2D(0,0);

        public static readonly Vec2D LEFT = new Vec2D(-1, 0);

        public static readonly Vec2D RIGHT = new Vec2D(1, 0);

        public static readonly Vec2D UP = new Vec2D(0, 1);

        public static readonly Vec2D DOWN = new Vec2D(0, -1);

        public static readonly Vec2D ONE = new Vec2D(1, 1);
        #endregion
          
        #region fields
        [FieldOffset(0)]
        private double x;//8 bytes

        [FieldOffset(8)] //8 bytes - offset
        private double y;//8 bytes

        [FieldOffset(16)] //16 bytes - offset
        private double xpix;//8 bytes

        [FieldOffset(24)] //24 bytes - offset
        private double ypix;//8 bytes

        [FieldOffset(32)] //32 bytes - offset
        private double ex;//8 bytes

        [FieldOffset(40)] //40 bytes - offset
        private double ey;//8 bytes

        [FieldOffset(48)] //48 bytes - offset
        private double length;//8 bytes

        [FieldOffset(56)] //56 bytes - offset
        private char[] unitsChar; // 2 - chars; each char size 2 bytes

        [FieldOffset(60)] //64 bytes - offset
        private char[] xs; // 16 - chars; each char size 2 bytes

        [FieldOffset(92)] //96 bytes - offset
        private char[] ys; // 16 - chars; each char size 2 bytes

        [FieldOffset(124)] //128 bytes - offset
        private double unitsScale;//8 bytes

        [FieldOffset(132)]
        private Units vecUnits;//2- bytes

        [FieldOffset(134)]
        private int xChars;//4- bytes

        [FieldOffset(138)]
        private int yChars;//4- bytes

        #endregion

        #region Getters ans setters
        public Units VecUnits { get { return vecUnits; } set { { UpdateUnits(value); } } }

        public double X { get { return x; } set { setX(value); } }

        public double Y { get { return y; } set { setY(value); } }

        public string Xs { get { return new string(xs, 16 - xChars, xChars); }}

        public string Ys { get { return new string(ys, 16 - yChars, yChars); }}

        public double Xpix { get { return xpix; } private set { xpix = value; } }

        public double Ypix { get { return ypix; } private set { ypix = value; } }

        public double Ex { get { return ex; } private set { ex = value; } }

        public double Ey { get { return ey; } private set { ey = value; } }

        public double Norm { get { return length; } private set { length = value; } }
        #endregion

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

            Xpix = x * unitsScale;

            updateXs();
        }

        private void setY(double y_)
        {
            y = y_;

            Ypix = y * unitsScale;

            updateYs();
        }

        private void updateXs()
        {
            xs = new string((char) 0, 16).ToCharArray();

            string tmp = SVGElements.num2str(x);

            if (VecUnits == Units.NONE)
            {
                xChars = Math.Min(14, tmp.Length);

                Array.Copy(tmp.ToCharArray(), 0, xs, xs.Length - xChars, xChars);

                return;
            }

            xChars = Math.Min(14, tmp.Length) + 2;

            Array.Copy(tmp.ToCharArray(), 0, xs, xs.Length- xChars, xChars-2);

            xs[xs.Length - 1] = unitsChar[1];

            xs[xs.Length - 2] = unitsChar[0];
        }

        private void updateYs()
        {
            ys = new string((char) 0, 16).ToCharArray();

            string tmp = SVGElements.num2str(y);

            if (VecUnits == Units.NONE)
            {
                yChars = Math.Min(14, tmp.Length);

                Array.Copy(tmp.ToCharArray(), 0, ys, ys.Length - yChars, yChars);

                return;
            }

            yChars = Math.Min(14, tmp.Length) + 2;

            Array.Copy(tmp.ToCharArray(), 0, ys, ys.Length - yChars, yChars-2);

            ys[ys.Length - 1] = unitsChar[1];

            ys[ys.Length - 2] = unitsChar[0];
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

                    unitsScale = PixPerUnit.PIXEL_PER_CM;

                    unitsChar[0] = 'c'; unitsChar[1] = 'm';

                    Xpix = X * unitsScale;

                    Ypix = Y * unitsScale;

                    updateXs();

                    updateYs();

                    break;
                case Units.MM:
                    unitsScale = PixPerUnit.PIXEL_PER_MM;

                    unitsChar[0] = 'm'; unitsChar[1] = 'm';

                    Xpix = X * unitsScale;

                    Ypix = Y * unitsScale;

                    updateXs();

                    updateYs();
                    break;
                case Units.M:

                    unitsScale = PixPerUnit.PIXEL_PER_M;

                    unitsChar[0] = 'm'; unitsChar[1] = (char) 0;

                    Xpix = X * unitsScale;

                    Ypix = Y * unitsScale;

                    updateXs();

                    updateYs();
                    break;
                case Units.IN:
                    unitsScale = PixPerUnit.PIXEL_PER_INCH;

                    unitsChar[0] = 'i'; unitsChar[1] = 'n';

                    Xpix = X * unitsScale;

                    Ypix = Y * unitsScale;

                    updateXs();

                    updateYs();
                    break;

                case Units.NONE:
                    unitsScale = 1;

                    unitsChar[0] = (char) 0; unitsChar[1] = (char) 0;

                    Xpix = X * unitsScale;

                    Ypix = Y * unitsScale;

                    updateXs();

                    updateYs();
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
            
            unitsScale = PixPerUnit.PIXEL_PER_MM;

            unitsChar = new char[2];

            unitsChar[0] = 'm'; unitsChar[1] = 'm';

            x = x_;

            y = y_;

            length = Math.Sqrt(x * x + y * y);

            ex = x / length;

            ey = y / length;

            xpix = x * unitsScale;

            ypix = y * unitsScale;

            xs = new string((char) 0, 16).ToCharArray();

            string tmp = SVGElements.num2str(x);

            xChars = Math.Min(14, tmp.Length)+2;

            Array.Copy(tmp.ToCharArray(), 0, xs, xs.Length - xChars, xChars-2);

            xs[xs.Length - 1] = unitsChar[1];

            xs[xs.Length - 2] = unitsChar[0];


            ys = new string((char) 0, 16).ToCharArray();

            tmp = SVGElements.num2str(y);

            yChars = Math.Min(14, tmp.Length)+2;

            Array.Copy(tmp.ToCharArray(), 0, ys, ys.Length - yChars, yChars - 2);

            ys[ys.Length - 1] = unitsChar[1];

            ys[ys.Length - 2] = unitsChar[0];
        }

    }

}
