using HTMLCodeBuilder.TaggedElements;
using System;
using System.Collections.Generic;

namespace HTMLCodeBuilder.SVGelements
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

    public struct Vec2D
    {
        private Units vecUnits;

        private double UnitsScale;

        private string UnitsStr;

        private double x, y;

        public Units VecUnits { get { return vecUnits; }  set { { UpdateUnits(value); } } }

        public double X { get { return x; } set { setX(value); } }

        public double Y { get { return y; } set { setY(value); } }

        public string Xs { get; private set; }

        public string Ys { get; private set; }

        public double Xpix { get; private set; }

        public double Ypix { get; private set; }
        
        public double Ex { get; private set; }

        public double Ey { get; private set; }
 
        public double Norm { get; set; }
        
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

        public  void normalize()
        {
            Norm = Math.Sqrt(X * X + Y * Y);
            Ex = X / Norm;
            Ey = Y / Norm;
        }

        public static Vec2D operator *(double b, Vec2D a)
        {
            return new Vec2D(a.X * b, a.Y * b);
        }

        public static Vec2D operator *(Vec2D  a, Vec2D b)
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

        public static Vec2D operator /(Vec2D a, double  b)
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

                    UnitsScale = SVGElement.PIXEL_PER_CM;

                    UnitsStr = "cm";

                    Xpix = X * UnitsScale;

                    Ypix = Y * UnitsScale;

                    Xs =SVGElements.num2str(x) + UnitsStr;

                    Ys = SVGElements.num2str(y) + UnitsStr;
                    break;
                case Units.MM:
                    UnitsScale = SVGElement.PIXEL_PER_MM;

                    UnitsStr = "mm";

                    Xpix = X * UnitsScale;

                    Ypix = Y * UnitsScale;

                    Xs = SVGElements.num2str(x) + UnitsStr;

                    Ys = SVGElements.num2str(y) + UnitsStr;
                    break;
                case Units.M:

                    UnitsScale = SVGElement.PIXEL_PER_M;

                    UnitsStr = "m";

                    Xpix = X * UnitsScale;

                    Ypix = Y * UnitsScale;

                    Xs = SVGElements.num2str(x) + UnitsStr;

                    Ys = SVGElements.num2str(y) + UnitsStr;
                    break;
                case Units.IN:
                    UnitsScale = SVGElement.PIXEL_PER_INCH;

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

        public Vec2D(double x_,double y_)
        {
            vecUnits = Units.MM;

            UnitsStr = "mm";

            UnitsScale = SVGElement.PIXEL_PER_MM;

            x = x_;

            y = y_;

            Norm = Math.Sqrt(x * x + y * y);

            Ex = x / Norm;

            Ey = y / Norm;

            Xs = SVGElements.num2str(x)+ UnitsStr;

            Ys = SVGElements.num2str(y) + UnitsStr;

            Xpix = x * UnitsScale;

            Ypix = y * UnitsScale;

        }

    }

    public struct XYWH
    {
        private Vec2D xy,wh;

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
    }

    public class SVGElement : TagElement
    {
        public static double PIXEL_PER_MM = 3.7795275591;

        public static double PIXEL_PER_CM = 37.795275591;

        public static double PIXEL_PER_M = 3779.5275591;

        public static double PIXEL_PER_INCH = 96.000000001139995;
           
        public double W { get { return PosAndSize.W; } }

        public double H { get { return PosAndSize.H; } }

        public double Wpix { get { return PosAndSize.Wpix; } }

        public double Hpix { get { return PosAndSize.Hpix; } }
        /// <summary>
        /// Origin x
        /// </summary>
        public double X0 { get { return PosAndSize.X; } }
        /// <summary>
        /// Origin y
        /// </summary>
        public double Y0 { get { return PosAndSize.Y; } }
        /// <summary>
        /// Translate x
        /// </summary>
        public double X { get { return translation.X; } }
        /// <summary>
        /// Translate y
        /// </summary>
        public double Y { get { return translation.Y; } }

        public double ScaleX { get { return scale.X; } }

        public double ScaleY { get { return scale.Y; } }

        public double Rotation { get { return rotation; } }
        
        private bool isTraUpdate = false;

        private bool isScaUpdate = false;

        private bool isRotUpdate = false;

        private bool transformModified = false;

        private XYWH PosAndSize;

        private Vec2D translation;

        private Vec2D scale;

        private double rotation;

        public void setSizes(double x, double y)
        {
            PosAndSize.W = x;
            PosAndSize.H = y;
        }

        public void setPosition(double x, double y)
        {
            PosAndSize.X = x;
            PosAndSize.Y = y;
        }

        public void Move(double x, double y)
        {
            translation.X = x;
            translation.Y = y;
            isTraUpdate = true;
            transformModified = true;
        }

        public void Scale(double x, double y)
        {
            scale.X = x;
            scale.Y = y;
            isScaUpdate = true;
            transformModified = true;
        }

        public void Rotate(double x)
        {
            rotation = x;
            isRotUpdate = true;
            transformModified = true;
        }

        public override string expandElementOpenTag()
        {
             code.Append(OpenTag);

            foreach (string key in elementSettings.Keys)
            {
                if (key.StartsWith("#") || key.StartsWith("."))
                {
                    continue;
                }
                code.Append(" ");
                code.Append(key);
                code.Append(" = ");
                code.Append('"');
                code.Append(elementSettings[key]);
                code.Append('"');
            }

            if (transformModified)
            {
                code.Append(" transform = ");
                code.Append('"');
                code.Append(isTraUpdate ? "translate(" + SVGElements.num2str(translation.Xpix) + " " + SVGElements.num2str(translation.Ypix) + ") " : "");
                code.Append(isRotUpdate ? "rotate(" + SVGElements.num2str(rotation) + ") " : "");
                code.Append(isScaUpdate ? "scale(" + scale.Xs + " " + scale.Ys + ")" :"");
                code.Append('"');
            }

            code.Append(PosAndSize.W != 0 ? " width = " + '"' + PosAndSize.Ws+'"':"");
            code.Append(PosAndSize.H != 0 ? " height = " + '"' + PosAndSize.Hs + '"':"");
            code.Append(PosAndSize.X != 0 ? " x = " + '"' + PosAndSize.Xs + '"':"");
            code.Append(PosAndSize.Y != 0 ? " y = " + '"' + PosAndSize.Ys + '"':"");

            if (InnerString.Length != 0)
            {
                code.Append("> ");
                return code.ToString();
            }

            if (autoCloseTag)
            {
                return code.ToString();
            }

            code.Append("> ");

            return code.ToString();
                      
        }

        public override string expandElementCloseTag()
        {
            return InnerString + CloseTag;
        }

        public override string ToString()
        {
            return OpenTag + ">" + " " + InnerString + " " + CloseTag;
        }

        public override ITagElement Copy()
        {
            SVGElement element = new SVGElement();
            element.elementSettings = new Dictionary<string, string>(elementSettings);
            element.Tag = string.Copy(Tag);
            element.OpenTag = string.Copy(OpenTag);
            element.CloseTag = string.Copy(CloseTag);
            element.InnerString = string.Copy(InnerString);
            element.autoCloseTag = autoCloseTag;
            return element;
        }

        private SVGElement():base()
        {
        }

        public SVGElement(string openTag, string closeTag) : base(openTag, closeTag)
        {
            PosAndSize = new XYWH(0,0,0,0);
            translation = new Vec2D(0, 0);
            translation.VecUnits = Units.PX;
            scale = new Vec2D(1, 1);
            scale.VecUnits = Units.NONE;
            rotation = 0;

            appendParam("class", openTag.Substring(1, openTag.Length - 2));

            OpenTag = openTag.Remove(openTag.Length - 1, 1); ;

            if (closeTag.Length == 0)
            {
                CloseTag = "/>";
                autoCloseTag = true;
            }

            Tag = OpenTag.Substring(1, OpenTag.Length - 1);


        }
    }
}
