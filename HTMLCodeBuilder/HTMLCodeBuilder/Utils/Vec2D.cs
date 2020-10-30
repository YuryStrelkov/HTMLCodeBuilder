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
    [StructLayout(LayoutKind.Explicit, Pack = 1, Size = 16)]

    public struct VectorUnits
    {
        public string UnitsString { get { return new string(UnitsCahr); } }

        public double UnitsScale { get { return unitsScale; }}

        public Units VUnits { get { return units; } }


        [FieldOffset(0)]
        char[] UnitsCahr;

        [FieldOffset(4)]
        Units units;

        [FieldOffset(8)]
        double unitsScale;

        public bool Equals(VectorUnits Other)
        {
            return unitsScale == Other.UnitsScale;
        }

        public void SetUnits(Units units_)
        {
            if (units == units_)
            {
                return;
            }

            units = units_;

            switch (units_)
            {
                case Units.CM:   UnitsCahr[0] = 'c'; UnitsCahr[1] = 'm'; unitsScale = PixPerUnit.PIXEL_PER_CM; break;
                case Units.IN:   UnitsCahr[0] = 'i'; UnitsCahr[1] = 'n'; unitsScale = PixPerUnit.PIXEL_PER_INCH; break;
                case Units.MM:   UnitsCahr[0] = 'm'; UnitsCahr[1] = 'm'; unitsScale = PixPerUnit.PIXEL_PER_MM; break;
                case Units.M:    UnitsCahr[0] = 'm'; UnitsCahr[1] = ' '; unitsScale = PixPerUnit.PIXEL_PER_M; break;
                case Units.PX:   UnitsCahr[0] = 'p'; UnitsCahr[1] = 'x'; unitsScale = 1; break;
                case Units.NONE: UnitsCahr[0] = ' '; UnitsCahr[1] = ' '; unitsScale = 1; break;
            }
        }

        public VectorUnits(Units units_)
        {
            unitsScale = 1;
            units = Units.NONE;
            UnitsCahr = new char[2];
            SetUnits(units_);
        }
    }

    [Serializable]
    [StructLayout(LayoutKind.Explicit, Pack = 1, Size = 48)]
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

        [FieldOffset(32)]
        private VectorUnits vecUnits;//2- bytes
        #endregion

        #region Getters ans setters
        public VectorUnits VecUnits { get { return vecUnits; } set { { updateUnits(value); } } }

        public double X { get { return x; } set { setX(value); } }

        public double Y { get { return y; } set { setY(value); } }

        public double Xpix { get { return xpix; } private set { xpix = value; } }

        public double Ypix { get { return ypix; } private set { ypix = value; } }

        public double Norm { get { return Math.Sqrt(X * X + Y * Y); }}

        public string Xs { get { return x.ToString() + vecUnits.UnitsString; } }

        public string Ys { get { return y.ToString() + vecUnits.UnitsString; } }

        #endregion

        public static double Dot(Vec2D a, Vec2D b)
        {
            return a.X * b.X + a.Y * b.Y;
        }

        public static Vec2D operator +(Vec2D a, double b)
        {
            return new Vec2D(a.X + b, a.Y + b);
        }

        public static Vec2D operator +(double b, Vec2D a)
        {
            return new Vec2D(a.X + b, a.Y + b);
        }

        public static Vec2D operator -(Vec2D a, double b)
        {
            return new Vec2D(a.X - b, a.Y - b);
        }

        public static Vec2D operator -(double b, Vec2D a)
        {
            return new Vec2D( b - a.X,  b - a.Y);
        }

        public static Vec2D operator *(Vec2D a, double b)
        {
            return new Vec2D(a.X * b, a.Y * b);
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
        
        public double Dot(Vec2D b)
        {
            return X * b.X + Y * b.Y;
        }

        public bool Equals(Vec2D other)
        {
            if (other.VecUnits.Equals(VecUnits) )
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

        private void setX(double x_)
        {
            x = x_;

            Xpix = x * vecUnits.UnitsScale;
        }

        private void setY(double y_)
        {
            y = y_;

            Ypix = y * vecUnits.UnitsScale;
        }

        private void updateUnits(VectorUnits newUnits)
        {
            
            if (newUnits.Equals(VecUnits))
            {
                return;
            }
            VecUnits = newUnits;

            vecUnits = newUnits;

            Xpix = X * vecUnits.UnitsScale;

            Ypix = Y * vecUnits.UnitsScale;
        }

        public Vec2D(double x_, double y_)
        {
            vecUnits = new VectorUnits(Units.MM);

            x = x_;

            y = y_;

            xpix = x * vecUnits.UnitsScale;

            ypix = y * vecUnits.UnitsScale;
        }

    }

}
