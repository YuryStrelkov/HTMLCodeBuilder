using HTMLCodeBuilder.TaggedElements;
using HTMLCodeBuilder.Utils;
using System.Collections.Generic;

namespace HTMLCodeBuilder.SVGelements
{
    public class SVGElement : TagElement
    {
        public Units SVGUnits { get { return units; } set { SetUnits(value); } }

        private Units units;
               
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

        public void SetSizes(double x, double y)
        {
            PosAndSize.W = x;
            PosAndSize.H = y;
        }

        public void SetPosition(double x, double y)
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

        public override string ExpandOpenTag(int tab)
        {
            TabLevel = tab;
            code.Append(GetTab(TabLevel));
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
                code.Append(isTraUpdate ? "translate(" + SVGElements.Num2str(translation.Xpix) + " " + SVGElements.Num2str(translation.Ypix) + ") " : "");
                code.Append(isRotUpdate ? "rotate(" + SVGElements.Num2str(rotation) + ") " : "");
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

        public override string ExpandCloseTag(int tab)
        {
            return GetTab(tab) +InnerString + CloseTag;
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

        private void SetUnits(Units units)
        {
            this.units = units;
         ///   translation.VecUnits = units;
            PosAndSize.ElementUnits = units;
        }

        private SVGElement():base()
        {
        }



        public SVGElement(string openTag, string closeTag, Units units) : base(openTag, closeTag)
        {
            PosAndSize = new XYWH(0, 0, 0, 0);

            translation = new Vec2D(0, 0);

            translation.VecUnits.SetUnits(Units.PX);

            scale = new Vec2D(1, 1);

            scale.VecUnits.SetUnits(Units.NONE);

            rotation = 0;

            SVGUnits = units;

            AddParam("class", openTag.Substring(1, openTag.Length - 2));

            OpenTag = openTag.Remove(openTag.Length - 1, 1); ;

            if (closeTag.Length == 0)
            {
                CloseTag = "/>";
                autoCloseTag = true;
            }
            Tag = OpenTag.Substring(1, OpenTag.Length - 1);
        }

        public SVGElement(string openTag, string closeTag) : base(openTag, closeTag)
        {
            PosAndSize = new XYWH(0,0,0,0);
            translation = new Vec2D(0, 0);
            translation.VecUnits.SetUnits(Units.PX);
            scale = new Vec2D(1, 1);
            scale.VecUnits.SetUnits(Units.NONE);
            rotation = 0;

            AddParam("class", openTag.Substring(1, openTag.Length - 2));

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
