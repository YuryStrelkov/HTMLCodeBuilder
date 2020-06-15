using System;
using System.Runtime.InteropServices;

namespace HTMLCodeBuilder.Utils
{
    [Serializable]
    [StructLayout(LayoutKind.Explicit, Pack = 1, Size = 288)]
     public struct XYWH : IEquatable<XYWH>
    {
        [FieldOffset(0)]
        private Vec2D xy;

        [FieldOffset(144)]
        private Vec2D wh;

        //public Vec2D XY { get { return xy; } }

        //public Vec2D WH { get { return wh; } }

        public double X { get { return xy.X; } set { xy.X = value; } }
        public double Y { get { return xy.Y; } set { xy.Y = value; } }
        public double W { get { return wh.X; } set { wh.X = value; } }
        public double H { get { return wh.Y; } set { wh.Y = value; } }


        public double Xpix { get { return xy.Xpix; }}
        public double Ypix { get { return xy.Ypix; }}
        public double Wpix { get { return wh.Xpix; }}
        public double Hpix { get { return wh.Ypix; }}

        public string Xs { get { return xy.Xs; }}
        public string Ys { get { return xy.Ys; }}
        public string Ws { get { return wh.Xs; }}
        public string Hs { get { return wh.Ys; }}

        public Units ElementUnits { get { return xy.VecUnits; } set { UpdateUnits(value); } } 
        
        private void UpdateUnits(Units units)
        {
            wh.VecUnits = units;
            xy.VecUnits = units;
        }

        public XYWH(double x, double y, double w, double h)
        {
            xy = new Vec2D(x, y);
            wh = new Vec2D(w, h);
        }

        public bool Equals(XYWH other)
        {
            if (!xy.Equals(other.xy))
            {
                return false;
            }
            if (!wh.Equals(other.wh))
            {
                return false;
            }
            return true;
        }
    }
}
