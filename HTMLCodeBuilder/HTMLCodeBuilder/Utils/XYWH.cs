using System;
using System.Runtime.InteropServices;

namespace HTMLCodeBuilder.Utils
{
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct XYWH : IEquatable<XYWH>
    {
        private Vec2D xy, wh;

        public Vec2D XY { get { return xy; } }

        public Vec2D WH { get { return wh; } }

        public double X { get { return xy.X; } set { xy.X = value; } }
        public double Y { get { return xy.Y; } set { xy.Y = value; } }
        public double W { get { return wh.X; } set { wh.X = value; } }
        public double H { get { return wh.Y; } set { wh.Y = value; } }


        public double Xpix { get { return xy.Xpix; } private set { } }
        public double Ypix { get { return xy.Ypix; } private set { } }
        public double Wpix { get { return wh.Xpix; } private set { } }
        public double Hpix { get { return wh.Ypix; } private set { } }

        public string Xs { get { return xy.Xs; } private set { } }
        public string Ys { get { return xy.Ys; } private set { } }
        public string Ws { get { return wh.Xs; } private set { } }
        public string Hs { get { return wh.Ys; } private set { } }

        public XYWH(double x, double y, double w, double h)
        {
            xy = new Vec2D(x, y);
            wh = new Vec2D(w, h);
        }

        public bool Equals(XYWH other)
        {
            if (!XY.Equals(other.XY))
            {
                return false;
            }
            if (!WH.Equals(other.WH))
            {
                return false;
            }
            return true;
        }
    }
}
