using HTMLCodeBuilder.TaggedElements;
using HTMLCodeBuilder.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HTMLCodeBuilder.SVGelements
{
    public static class SVGElements
    {
        private static Random rnd = new Random();

        private static int[] randColorGenerator()
        {
            return new int[3] { (int)(rnd.NextDouble() * 255), (int)(rnd.NextDouble() * 255), (int)(rnd.NextDouble() * 255) };
        }

        private static string randColorGenerator2Str()
        {
            int[] color = randColorGenerator();

            return "rgb(" + ((int)(rnd.NextDouble() * 255)).ToString() + ","
                          + ((int)(rnd.NextDouble() * 255)).ToString() + ","
                          + ((int)(rnd.NextDouble() * 255)).ToString() + ")";
        }

        public static double str2num(string str)
        {
            return double.Parse(str.Replace('.', ','));
        }

        public static string num2str(double val)
        {
            return val.ToString().Replace(',', '.');
        }
   
        public static void ChangeEach<T>(this T[] array, Func<T, T> mutator)
        {
            Parallel.For(0, array.Length, index =>
            {
                array[index] = mutator(array[index]);
            });
        }


        private static double parseSize2Pix(double x)
        {
            return x * PixPerUnit.PIXEL_PER_MM;
        }

        private static string parseSize2PixString(double x)
        {
            return num2str(x * PixPerUnit.PIXEL_PER_MM);
        }

        private static string parseSize2MMString(double x)
        {
            return num2str(x)+"mm";
        }

        public static SVGElement CreateSVGLine(double x1, double y1, double x2, double y2, string style)
        {
            SVGElement line = new SVGElement("<line>", "</line>");
            line.AddParam("x1", parseSize2PixString(x1));
            line.AddParam("x2", parseSize2PixString(x2));
            line.AddParam("y1", parseSize2PixString(y1));
            line.AddParam("y2", parseSize2PixString(y2));
            line.AddParam("style", style);
            return line;
        }
      
        public static SVGElement CreateSVGNode()
        {
            return CreateSVGNode(0, 0, 0);
        }

        public static SVGElement CreateSVGNode(double x, double y)
        {
            return CreateSVGNode( x,  y, 0);
        }

        public static SVGElement CreateSVGNode(double x, double y, double rot)
        {
             SVGElement node = new SVGElement("<g>", "</g>");

            node.AddParam("class", "transform-node");

            node.Move(x, y);

            node.Rotate(rot);

            node.Scale(1,1);

            return node;  
        }

        public static SVGElement CreateSVGText(string text, double xc, double yc, double rot)
        {
            SVGElement Text = new SVGElement("<text>", "</text>");
            Text.Rotate(rot);
            Text.Move(xc, yc);
            Text.InnerString = text;
            Text.AddParam("font-size", "14pt");
            Text.AddParam("font-family", "Times New Roman");
            Text.AddParam("style", "text-anchor : middle;");
            return Text;
        }

        public static SVGElement CreateSVGText(string text, double xc, double yc, double rot, string style)
        {
            SVGElement Text = CreateSVGText(text, xc, yc, rot);
            Text.AddParam("style", style);
            return Text;
        }

        public static SVGElement CreateSVGRect(double x, double y, double w, double h, string style)
        {
            SVGElement rect = new SVGElement("<rect>", "</rect>");
            rect.Move(x, y);
            rect.setSizes(w, h);
            rect.AddParam("style", style);
            return rect;
        }

        public static SVGElement CreateSVGPolyLine(double x_0, double y_0, ref double[] x, ref double[] y, string style)
        {
            SVGElement line = new SVGElement("<polyline>", "</polyline>");

            try
            {
                line.Move(x_0, y_0);

                line.Scale(1, -1);

                line.AddParam("class", "poly-line");

                StringBuilder points=new StringBuilder();

                for (int i = 0; i < x.Length - 1; i++)
                {
                    points.Append(num2str(x[i]));
                    points.Append(" ");
                    points.Append(num2str(y[i]));
                    points.Append(",");
                }
                points.Append(num2str(x[x.Length - 1]));
                points.Append(" ");
                points.Append(num2str(y[y.Length - 1]));
                line.AddParam( "points", points.ToString());
                line.AddParam( "style", style);
               
            }
            catch (Exception e)
            {
                Console.WriteLine("somethig goes wrong while poly-line creating...");
            }

            return line;

        }

        public static SVGElement CreateSVGDashedLine(double x1, double y1, double x2, double y2, string style)
        {
            SVGElement line = new SVGElement("<path>", "</path>");
            line.AddParam("fill", "none");
            line.AddParam("style", style);
            line.AddParam("stroke-dasharray", "5,5");
            line.AddParam("d", "M" + parseSize2PixString(x1) + " " + parseSize2PixString(y1) + " l" +
                                        parseSize2PixString(x2 - x1) + " " + parseSize2PixString(y2 - y1));
            return line;
        }

        public static SVGElement CreateSVGImage()
        {
            SVGElement image = new SVGElement("<image>", "</image>"); new SVGElement("<image>", "</image>");

            image.AddParam("xlink:href", "");

            image.AddParam("image-rendering", "optimizeSpeed");

            image.AddParam("preserveAspectRatio", "none");

            return image;
        }

        public static SVGElement CreateSVGImage(string src)
        {
            SVGElement image = CreateSVGImage();

            image.AddParam("xlink:href", src);

            return image;
        }

        public static SVGElement CreateSVGImage(int w, int h, double[] src, ColorMaps cmap)
        {
            Bitmap img = ImageProcessing.CreateImage(w, h);

            ImageProcessing.UpdateImage(ref img, src, cmap);

            string path = img.GetHashCode().ToString() + ".png";

            ImageProcessing.WriteImage(ref img, path);

            SVGElement image = CreateSVGImage(path);

            image.setSizes(w / PixPerUnit.PIXEL_PER_MM, h / PixPerUnit.PIXEL_PER_MM);

            return image;
        }

        public static SVGElement CreateSVGImage(Bitmap img)
        {
            string path = img.GetHashCode().ToString() + ".png";

            ImageProcessing.WriteImage(ref img, path);

            SVGElement image = CreateSVGImage(path);

            image.setSizes(img.Width / PixPerUnit.PIXEL_PER_MM, img.Height / PixPerUnit.PIXEL_PER_MM);

            return image;
        }

        public static SVGElement CreateSVGImage(int w, int h, byte[] src)
        {
            Bitmap img = ImageProcessing.CreateImage(w, h);

            ImageProcessing.UpdateImage(ref img, src);

            string path = img.GetHashCode().ToString() + ".png";

            ImageProcessing.WriteImage(ref img, path);

            SVGElement image = CreateSVGImage(path);

            image.setSizes(w / PixPerUnit.PIXEL_PER_MM, h / PixPerUnit.PIXEL_PER_MM);

            return image;
        }

        public static TagElementsGroup CreateSVG()
        {
            return new TagElementsGroup(new SVGElement("<svg>", "</svg>"));
        }

        public static TagElementsGroup CreateSVG(double x, double y, double w, double h)
        {
            SVGElement svg = new SVGElement("<svg>", "</svg>");
            svg.setPosition(x, y);
            svg.setSizes(w, h);
            return new TagElementsGroup(svg);
        }

        public static TagElementsGroup CreateSVG(double w, double h)
        {
            return CreateSVG(0, 0, w, h);
        }
   
        public static TagElementsGroup CreateSVGGraphicXY(double w,double h)
        {
            TagElementsGroup plotxy = CreateSVG(w, h);

            plotxy.AddElementParam(plotxy.RootID, "class", "graphicic-node");

            plotxy.AddElementParam(plotxy.RootID, "id", "graphicic-node-" + plotxy.RootID.ToString());

            return plotxy;

        }

        public static void AppendSVGGraphic(ref TagElementsGroup graphic, double[] xs, double[] ys, string legendText = "", string lineStyle = "")
        {
            if (!graphic.HasClass("graphicic-node"))
            {
                Console.WriteLine("It's not a graphicic handler...");
                return;
            }

            if (ys.Length != xs.Length)
            {
                Console.WriteLine("x and y vector dimension missmatch...");
                return;
            }

            double xmax = xs.Max(), xmin = xs.Min();

            double ymax = ys.Max(), ymin = ys.Min();

            if (!graphic.HasClass("axes"))
            {
                SVGElement node = (SVGElement)graphic.GetElement(graphic.RootID);

                double[] axseXS = new double[Math.Max((int)(11 * node.W / 170.0),3)], axseYS = new double[Math.Max((int)(11 * node.H / 100.0), 3)];

                double dx = (xmax - xmin) / (axseXS.Length - 1);

                double dy = (ymax - ymin) / (axseYS.Length - 1);

                for (int i = 0; i < axseXS.Length; i++)
                {
                    axseXS[i] = xmin + i * dx;
                }

                for (int i = 0; i < axseYS.Length; i++)
                {
                    axseYS[i] = ymin + i * dy;
                }
                SVGElement graphicBg = (SVGElement)graphic.GetElement(graphic.GetElementByClass("graphicic-node")[0]);

                TagElementsGroup axes = CreateSVGXYAxis(graphicBg.W, graphicBg.H, axseXS, axseYS);

                graphic.MergeGroups(axes);

            }

            string legendTextStyle = "";

            if (string.IsNullOrEmpty(lineStyle))
            {
                string color = randColorGenerator2Str();

                lineStyle = "fill:none;stroke-width:2px;stroke:" + color + ";";

                legendTextStyle = color + ";";
            }

            DrawSVGPolyLine(ref graphic, xs, ys, xmin, xmax, ymin, ymax, lineStyle);

            UpdateSVGLegend(ref graphic, legendText, legendTextStyle);
        }

        public static void AppendSVGGraphic(ref TagElementsGroup graphic, double[] xs, double[] ys, double[] zs)
        {
            if (!graphic.HasClass("graphicic-node"))
            {
                Console.WriteLine("It's not a graphicic handler...");
                return;
            }
 
            if (zs.Length != ys.Length*xs.Length)
            {
                Console.WriteLine("x and y vector dimension missmatch...");
                return;
            }

            double xmax = xs.Max(), xmin = xs.Min();

            double ymax = ys.Max(), ymin = ys.Min();

            double zmax = zs.Max(), zmin = zs.Min();

            if (!graphic.HasClass("axes"))
            {
                SVGElement node = (SVGElement)graphic.GetElement(graphic.RootID);

                double[] axseXS = new double[Math.Max((int)(11 * node.W / 170.0), 3)];

                double[] axseYS = new double[Math.Max((int)(11 * node.H / 100.0), 3)];

                double[] axseZS = new double[axseYS.Length];
                
                double dx = (xmax - xmin) / (axseXS.Length - 1);

                double dy = (ymax - ymin) / (axseYS.Length - 1);

                double dz = (zmax - zmin) / (axseZS.Length - 1);

                for (int i = 0; i < axseXS.Length; i++)
                {
                    axseXS[i] = xmin + i * dx;
                }
                for (int i = 0; i < axseYS.Length; i++)
                {
                    axseYS[i] = ymin + i * dy;
                    axseZS[i] = zmin + i * dz;
                }

                int graphicBgID = graphic.GetElementByClass("graphicic-node")[0];

                SVGElement graphicBg = (SVGElement)graphic.GetElement(graphicBgID);

                TagElementsGroup axes = CreateSVGXYZAxis(graphicBg.W, graphicBg.H, axseXS, axseYS, axseZS);

                graphic.MergeGroups(axes);

                ///Данные цветовой схемы отображения

                int HolderID = graphic.GetElementByClass("graphic-color-bar")[0];

                SVGElement holderElement = (SVGElement)graphic.GetElement(HolderID);

                if (graphic.HasChildrens(HolderID))
                {
                    graphic.RemoveChildrens(HolderID);
                }

                SVGElement colorbar = CreateSVGImage(ImageProcessing.CreateJetColorMap());

                colorbar.setSizes(holderElement.W, holderElement.H);

                graphic.AddElement(colorbar, HolderID);

                ///Данные двумерного распределения

                HolderID = graphic.GetElementByClass("graphic-data")[0];

                holderElement = (SVGElement)graphic.GetElement(HolderID);

                if (graphic.HasChildrens(HolderID))
                {
                    graphic.RemoveChildrens(HolderID);
                }

                SVGElement data = CreateSVGImage(xs.Length, ys.Length, zs, ColorMaps.JET);

                data.setSizes(holderElement.W, holderElement.H);

                graphic.AddElement(data, HolderID);

            }


        }

        public static void UpdateSVGLegend(ref TagElementsGroup graphic, string legendText, string style)
        {
             if (string.IsNullOrEmpty(legendText))
            {
                return;
            } 
            if (!graphic.HasClass("legend-holder"))
            {
                CreateSVGgraphicLegend(ref graphic);
            }
            int legendNodeID = graphic.GetElementByClass("legend-node")[0];

            int legendRectID = graphic.GetElementByClass("legend-holder")[0];

            SVGElement legendRect = (SVGElement)graphic.GetElement(legendRectID);

            SVGElement legendRecord = CreateSVGText(legendText, -4.5, legendRect.H + 4.75, 0);
            legendRecord.AddParam("style", "text-anchor:end; ");

            legendRect.setSizes(Math.Max(Math.Max(legendRect.W, legendText.Length * 2.75),8), legendRect.H + 6.5);
            legendRect.setPosition(-legendRect.W, legendRect.Y);
            graphic.AddElement(legendRecord, legendNodeID);
            graphic.AddElement(CreateSVGLine(-4, legendRect.H - 3.25, -1, legendRect.H - 3.25, "stroke-width : 0.5mm; stroke : " + style), legendNodeID);
        }

        public static void GraphicTitle(TagElementsGroup graphic, string title)
        {
            List<int> ids = graphic.GetElementByClass("title");
            if (ids.Count == 0)
            {
                return;
            }
            graphic.GetElement(graphic.GetElementByClass("title")[0]).InnerString = title;
        }

        public static void GraphicXLabel(TagElementsGroup graphic, string xlabel)
        {
            List<int> ids = graphic.GetElementByClass("x-label");
            if (ids.Count == 0)
            {
                return;
            }
            graphic.GetElement(ids[0]).InnerString = xlabel;
        }

        public static void GraphicYLabel(TagElementsGroup graphic, string ylabel)
        {
            List < int > ids = graphic.GetElementByClass("y-label");
            if (ids.Count==0)
            {
                return;
            }
            graphic.GetElement(ids[0]).InnerString = ylabel;
        }

        private static void DrawSVGPolyLine(ref TagElementsGroup axes, double[] xs, double[] ys, double xmin, double xmax, double ymin, double ymax, string style = "")
        {
            SVGElement elem = (SVGElement)axes.GetElement(axes.GetElementByClass("graphic-backround")[0]);

            double xOff = 1 / (xmax - xmin) * elem.Wpix;

            double yOff = 1 / (ymax - ymin) * elem.Hpix;

            xs.ChangeEach(x => x * xOff);

            ys.ChangeEach(x => x * yOff);

            xOff *= xmin;

            yOff *= ymax;

            SVGElement line;

            if (string.IsNullOrEmpty(style))
            {
                line = CreateSVGPolyLine(elem.X0 - xOff / PixPerUnit.PIXEL_PER_MM,
                                                          elem.Y0 + yOff / PixPerUnit.PIXEL_PER_MM, ref xs, ref ys, "fill:none;stroke-width:2px;stroke:rgb(0,0,0);");

                axes.AddElement(line, axes.GetElementByClass("charts-list-node")[0]);

                return;
            }

            line = CreateSVGPolyLine(elem.X0 - xOff / PixPerUnit.PIXEL_PER_MM,
                                                       elem.Y0 + yOff / PixPerUnit.PIXEL_PER_MM, ref xs, ref ys, style);

            axes.AddElement(line, axes.GetElementByClass("charts-list-node")[0]);
        }

        private static void CreateSVGgraphicLegend(ref TagElementsGroup axes)
        {
            List<int> nodes = axes.GetElementByClass("legend-node");

            if (nodes == null)
            {
                return;
            }
            SVGElement legend = CreateSVGRect(0, 0, 0, 0, "fill:rgb(255,255,255);stroke-width:1px;stroke:rgb(0,0,0)");

            legend.AddParam("class", "legend-holder");

            axes.AddElement(legend, nodes[0]);
        
        }

        private static TagElementsGroup CreateSVGXYAxis(double w, double h, double[] xs, double[] ys)
        {
            double[] AX_UP_LEFT = new double[] { 14.5, 5};

            double[] AX_W_H = new double[] { w - AX_UP_LEFT[0] - Math.Max(5, 0.03 * w), h - 13 };

            double[] AX_CENTER_W_H = new double[] { AX_W_H[0] / 2, AX_W_H[1] / 2 };

            double[] AX_X_TEXT_POS = new double[] { -Math.Max(0.07 * w, 8.5), -0.5 };

            double[] AX_Y_TEXT_POS = new double[] { h - 5, h - 9};
            
            SVGElement axNodeBack = new SVGElement("<g>", "</g>");

            axNodeBack.Move(AX_UP_LEFT[0], AX_UP_LEFT[1]);

            axNodeBack.setSizes(AX_W_H[0], AX_W_H[1]);

            axNodeBack.AddParam("class", "graphic-backround");

            TagElementsGroup axNodeBackground = new TagElementsGroup(axNodeBack);

           /// axNodeBackground.AddElement(CreateSVGRect(-AX_UP_LEFT[0], -AX_UP_LEFT[1], w, h, "fill:none;stroke-width:1px;stroke:rgb(255,0,0)"));

            ///axNodeBackground.AddElement(CreateSVGRect(0, 0, w, h, "fill:none;stroke-width:1px;stroke:rgb(0,255,0)"));

            SVGElement charts = new SVGElement("<g>", "</g>"); 

            charts.AddParam("class", "charts-list-node");

            axNodeBackground.AddElement(charts);


            TagElementsGroup axTitle = new TagElementsGroup(CreateSVGNode());

            axTitle.AddElementParam(axTitle.RootID,"class", "axes-subscriptions");
                
            SVGElement title = CreateSVGText("", AX_CENTER_W_H[0], -1, 0);
            title.AddParam("class", "title-node");
            title.AddParam("class", "title");
            title.AddParam("font-size", "14pt");
            axTitle.AddElement(title);

            SVGElement xlabel = CreateSVGText("", AX_CENTER_W_H[0], AX_Y_TEXT_POS[0], 0);
            xlabel.AddParam( "class", "x-label-node");
            xlabel.AddParam("class", "x-label");
            xlabel.AddParam("font-size", "14pt");
            axTitle.AddElement(xlabel);

            SVGElement ylabel = CreateSVGText("", AX_X_TEXT_POS[0], AX_CENTER_W_H[1], -90);
            ylabel.AddParam( "class", "y-label-node");
            ylabel.AddParam( "class", "y-label");
            ylabel.AddParam("font-size", "14pt");
            axTitle.AddElement(ylabel);
           
            axNodeBackground.MergeGroups(axTitle);

            TagElementsGroup axesNode = new TagElementsGroup(CreateSVGNode());

            axesNode.AddElementParam(axesNode.RootID,"class", "axes");

            double[] powAxis = rescaleAxis(ref xs, ref ys);

            if (powAxis[0] != 0)
            {
                SVGElement text = CreateSVGText("+", AX_W_H[0] - 5, AX_Y_TEXT_POS[0] + 0.5, 45);
                text.AddParam("font-size", "12pt");
                text.AddParam("style", "text-anchor:end");
                axesNode.AddElement(text);

                text = CreateSVGText("10", AX_W_H[0], AX_Y_TEXT_POS[0], 0);
                text.AddParam("font-size", "12pt");
                text.AddParam("style", "text-anchor:end");
                axesNode.AddElement(text);

                if (powAxis[0] != 1)
                {
                    text = CreateSVGText(powAxis[0].ToString(), AX_W_H[0], AX_Y_TEXT_POS[0] - 2, 0);
                    text.AddParam( "font-size", "7pt");
                    text.AddParam( "class", "ax-x-mult");
                    text.AddParam("style", "text-anchor:start");
                    axesNode.AddElement(text);
                }

            }

            if (powAxis[1] != 0)
            {
                SVGElement text = CreateSVGText("+", 1, -0.5, 45);
                text.AddParam( "font-size", "12pt");
                text.AddParam("style", "text-anchor:end");
                axesNode.AddElement(text);

                text = CreateSVGText("10", 6, -1, 0);
                text.AddParam("font-size", "12pt");
                text.AddParam("style", "text-anchor:end");
                axesNode.AddElement(text);

                if (powAxis[1] != 1)
                {
                    text = CreateSVGText(powAxis[1].ToString(), 6, -3, 0);
                    text.AddParam( "font-size", "7pt");
                    text.AddParam("style", "text-anchor:start");
                    text.AddParam("class", "ax-y-mult");
                    axesNode.AddElement(text);
                }

            }
            
            numAlongY(ref axesNode, ys[ys.Length - 1], AX_X_TEXT_POS[1], 1.25,             AX_W_H[0], 0,         0.25, "stroke-width: 0.25mm; stroke: black;");

            numAlongX(ref axesNode, xs[0], 0,                AX_Y_TEXT_POS[1], 0,         AX_W_H[1], 0.25, "stroke-width: 0.25mm; stroke: black;");

            numAlongY(ref axesNode, ys[0], AX_X_TEXT_POS[1], AX_W_H[1] + 1.25, AX_W_H[0], AX_W_H[1], -0.25, "stroke-width: 0.25mm; stroke: black;");

            numAlongX(ref axesNode, xs[xs.Length - 1], AX_W_H[0], AX_Y_TEXT_POS[1], AX_W_H[0], AX_W_H[1], -0.25, "stroke-width: 0.25mm; stroke: black;");

            double dx = AX_W_H[0] / (xs.Length - 1);

            double dy = AX_W_H[1] / (ys.Length - 1);

            double x_curr, y_curr;

            for (int i = 1; i < ys.Length-1; i++)
            {
                y_curr = dy * i;
                
                numAlongY(ref axesNode, ( ys[ys.Length - 1 - i]), AX_X_TEXT_POS[1], 1.25 + y_curr,    AX_W_H[0], y_curr,    0, "stroke-width: 0.5mm; stroke: black;");
            }

            for (int i = 1; i < xs.Length - 1; i++)
            {
                x_curr = dx * i;

                numAlongX(ref axesNode, xs[i] , x_curr, AX_Y_TEXT_POS[1], x_curr, AX_W_H[1], 0, "stroke-width: 0.5mm; stroke: black;");
            }

            axesNode.AddElement(CreateSVGRect(0, 0, AX_W_H[0], AX_W_H[1], "fill:none;stroke-width:1px;stroke:rgb(0,0,0)"));

            axNodeBackground.MergeGroups(axesNode);
            
            SVGElement legendnode = new SVGElement("<g>", "</g>");

            legendnode.AddParam("class", "legend-node");

            legendnode.Move(AX_W_H[0] - 5 / PixPerUnit.PIXEL_PER_MM, 5 / PixPerUnit.PIXEL_PER_MM);

            axNodeBackground.AddElement(legendnode);

            return axNodeBackground;
        }

        private static TagElementsGroup CreateSVGXYZAxis(double w, double h, double[] xs, double[] ys, double[] zs)
        {

            double[] AX_UP_LEFT = new double[] { 14.5, 5 };

            double[] AX_W_H = new double[] { w - AX_UP_LEFT[0] - 14, h - 13 };

            double[] AX_CENTER_W_H = new double[] { AX_W_H[0] / 2, AX_W_H[1] / 2 };

            double[] AX_X_TEXT_POS = new double[] { AX_W_H[0] + 5.5, -0.5 };

            double[] AX_Y_TEXT_POS = new double[] { h - 5, h - 9 };

            
            SVGElement axNodeBack = new SVGElement("<g>", "</g>");

            axNodeBack.Move(AX_UP_LEFT[0], AX_UP_LEFT[1]);

            axNodeBack.setSizes(AX_W_H[0], AX_W_H[1]);

            axNodeBack.AddParam("class", "graphic-backround");

          
            TagElementsGroup axNodeBackground = new TagElementsGroup(axNodeBack);

            TagElementsGroup axTitle = new TagElementsGroup(CreateSVGNode());

            axTitle.AddElementParam(axTitle.RootID, "class", "axes-subscriptions");

            SVGElement title = CreateSVGText("", AX_CENTER_W_H[0], -1, 0);
            title.AddParam("class", "title-node");
            title.AddParam("class", "title");
            title.AddParam("font-size", "14pt");
            axTitle.AddElement(title);

            SVGElement xlabel = CreateSVGText("", AX_CENTER_W_H[0], AX_Y_TEXT_POS[0], 0);
            xlabel.AddParam("class", "x-label-node");
            xlabel.AddParam("class", "x-label");
            xlabel.AddParam("font-size", "14pt");
            axTitle.AddElement(xlabel);

            SVGElement ylabel = CreateSVGText("", -AX_UP_LEFT[0]*0.75, AX_CENTER_W_H[1], -90);
            ylabel.AddParam("class", "y-label-node");
            ylabel.AddParam("class", "y-label");
            ylabel.AddParam("font-size", "14pt");
            axTitle.AddElement(ylabel);

            axNodeBackground.MergeGroups(axTitle);

            TagElementsGroup axesNode = new TagElementsGroup(CreateSVGNode());

            axesNode.AddElementParam(axesNode.RootID, "class", "axes");

            double[] powAxis = rescaleAxis(ref xs, ref ys, ref zs);

            if (powAxis[0] != 0)
            {
                SVGElement text = CreateSVGText("+", AX_W_H[0] + 3 , AX_Y_TEXT_POS[0] + 0.5, 45);
                text.AddParam("font-size", "12pt");
                text.AddParam("style", "text-anchor:end");
                axesNode.AddElement(text);

                text = CreateSVGText("10", AX_W_H[0]+ 8, AX_Y_TEXT_POS[0], 0);
                text.AddParam("font-size", "12pt");
                text.AddParam("style", "text-anchor:end");
                axesNode.AddElement(text);

                if (powAxis[0] != 1)
                {
                    text = CreateSVGText(powAxis[0].ToString(), AX_W_H[0]+8, AX_Y_TEXT_POS[0] - 2, 0);
                    text.AddParam("font-size", "7pt");
                    text.AddParam("class", "ax-x-mult");
                    text.AddParam("style", "text-anchor:start");
                    axesNode.AddElement(text);
                }

            }

            if (powAxis[1] != 0)
            {
                SVGElement text = CreateSVGText("+", 1, -0.5, 45);
                text.AddParam("font-size", "12pt");
                text.AddParam("style", "text-anchor:end");
                axesNode.AddElement(text);

                text = CreateSVGText("10", 6, -1, 0);
                text.AddParam("font-size", "12pt");
                text.AddParam("style", "text-anchor:end");
                axesNode.AddElement(text);

                if (powAxis[1] != 1)
                {
                    text = CreateSVGText(powAxis[1].ToString(), 6, -3, 0);
                    text.AddParam("font-size", "7pt");
                    text.AddParam("style", "text-anchor:start");
                    text.AddParam("class", "ax-y-mult");
                    axesNode.AddElement(text);
                }

            }

            double cmapoff = AX_W_H[0] + 5.0;

            if (powAxis[2] != 0)
            {
                SVGElement text = CreateSVGText("+", cmapoff - 5, -0.5, 45);
                text.AddParam("font-size", "12pt");
                text.AddParam("style", "text-anchor:end");
                axesNode.AddElement(text);

                text = CreateSVGText("10", cmapoff , -1, 0);
                text.AddParam("font-size", "12pt");
                text.AddParam("style", "text-anchor:end");
                axesNode.AddElement(text);

                if (powAxis[2] != 1)
                {
                    text = CreateSVGText(powAxis[2].ToString(), cmapoff , -3, 0);
                    text.AddParam("font-size", "7pt");
                    text.AddParam("style", "text-anchor:start");
                    text.AddParam("class", "ax-z-mult");
                    axesNode.AddElement(text);
                }

            }
            
            numAlongY(ref axesNode, ys[ys.Length - 1], AX_X_TEXT_POS[1], 1.25, AX_W_H[0], 0, 0.25, "stroke-width: 0.25mm; stroke: black;");

            numAlongX(ref axesNode, xs[0], 0, AX_Y_TEXT_POS[1], 0, AX_W_H[1], 0.25, "stroke-width: 0.25mm; stroke: black;");

            numAlongZ(ref axesNode, zs[zs.Length - 1], AX_X_TEXT_POS[0], 1.25, cmapoff, 0, 0.25, "stroke-width: 0.25mm; stroke: black;");
            

            numAlongY(ref axesNode, ys[0], AX_X_TEXT_POS[1], AX_W_H[1] + 1.25, AX_W_H[0], AX_W_H[1], -0.25, "stroke-width: 0.25mm; stroke: black;");

            numAlongX(ref axesNode, xs[xs.Length - 1], AX_W_H[0], AX_Y_TEXT_POS[1], AX_W_H[0], AX_W_H[1], -0.25, "stroke-width: 0.25mm; stroke: black;");

            numAlongZ(ref axesNode, zs[0], AX_X_TEXT_POS[0], AX_W_H[1]+1.25, cmapoff, AX_W_H[1], -0.25, "stroke-width: 0.25mm; stroke: black;");
            
            double dx = AX_W_H[0] / (xs.Length - 1);

            double dy = AX_W_H[1] / (ys.Length - 1);

            double y_curr,x_curr;

            for (int i = 1; i < xs.Length - 1; i++)
            {
                x_curr = dx * i;
                
                numAlongX(ref axesNode, xs[i], x_curr, AX_Y_TEXT_POS[1], x_curr, AX_W_H[1], 0, "stroke-width: 0.5mm; stroke: black;");
            }

            for (int i = 1; i < ys.Length - 1; i++)
            {
                y_curr = dy * i;
                
                numAlongY(ref axesNode, (ys[ys.Length - 1 - i]), AX_X_TEXT_POS[1], 1.25 + y_curr, AX_W_H[0], y_curr, 0, "stroke-width: 0.5mm; stroke: black;");

                numAlongZ(ref axesNode, (zs[ys.Length - 1 - i]), AX_X_TEXT_POS[0], 1.25 + y_curr, cmapoff, y_curr, 0, "stroke-width: 0.5mm; stroke: black;");
            }


            axesNode.AddElement(CreateSVGRect(0, 0, AX_W_H[0], AX_W_H[1], "fill:none;stroke-width:1px;stroke:rgb(0,0,0)"));

            axesNode.AddElement(CreateSVGRect(AX_W_H[0] + 1, 0, 4, AX_W_H[1], "fill:none;stroke-width:1px;stroke:rgb(0,0,0)"));

           ///axesNode.AddElement(CreateSVGRect(-AX_UP_LEFT[0], -AX_UP_LEFT[1], w, h, "fill:none;stroke-width:1px;stroke:rgb(255,0,0)"));

            SVGElement axNodeData = new SVGElement("<g>", "</g>");

            axNodeData.setSizes(AX_W_H[0], AX_W_H[1]);

            axNodeData.AddParam("class", "graphic-data");

            axNodeBackground.AddElement(axNodeData);

            SVGElement axсColorBarNode = new SVGElement("<g>", "</g>");

            axсColorBarNode.setSizes(4, AX_W_H[1]);

            axсColorBarNode.Move(AX_W_H[0] + 1, 0);

            axсColorBarNode.AddParam("class", "graphic-color-bar");

            axNodeBackground.AddElement(axсColorBarNode);

            axNodeBackground.MergeGroups(axesNode);

            return axNodeBackground;
        }

        private static void numAlongZ(ref TagElementsGroup parentGroup, double val, double xText, double yText, double xGrid, double yGrid, double dMarker, string style)
        {

            string sval = string.Format("{0,12:0.00}", val);
            SVGElement text = CreateSVGText(sval, xText, yText, 0);
            text.AddParam("font-size", "12pt");
            text.AddParam("class", "z-grid-val");
            text.AddParam("style", "text-anchor:start;");
            parentGroup.AddElement(text);
            parentGroup.AddElement(CreateSVGLine(xGrid - 0.25, yGrid+ dMarker+0.25, xGrid - 0.25 , yGrid + dMarker - 0.25, style));
        }

        private static void numAlongY(ref TagElementsGroup parentGroup, double val, double xText, double yText, double xGrid, double yGrid, double dMarker,string style)
        {
            string sval = string.Format("{0,12:0.00}", val);
            SVGElement text = CreateSVGText(sval, xText, yText, 0);
            text.AddParam("font-size", "12pt");
            text.AddParam("class", "y-grid-val");
            text.AddParam("style", "text-anchor:end");
            parentGroup.AddElement(text);
            parentGroup.AddElement(CreateSVGLine(0.25, yGrid + 0.25 + dMarker, 0.25, yGrid - 0.25 + dMarker, style));
            parentGroup.AddElement(CreateSVGDashedLine(0, yGrid, xGrid, yGrid, "stroke-width: 0.125mm; stroke: black;"));
        }

        private static void numAlongX(ref TagElementsGroup parentGroup, double val, double xText, double yText, double xGrid, double yGrid, double dMarker, string style)
        {
            string sval = string.Format("{0,12:0.00}", val);
            SVGElement text = CreateSVGText(sval, xText, yText, 0);
            text.AddParam("font-size", "12pt");
            text.AddParam("class", "x-grid-val");
            parentGroup.AddElement(text);
            parentGroup.AddElement( CreateSVGLine(xGrid + dMarker, yGrid, xGrid + dMarker, yGrid - 0.5, style));
            parentGroup.AddElement(CreateSVGDashedLine(xGrid, yGrid, xGrid, 0, "stroke-width: 0.125mm; stroke: black;"));
        }
        
        private static double[] rescaleAxis(ref double[] valsX, ref double[] valsY)
        {
            return new double[2] { getPow10(ref valsX), getPow10(ref valsY)};
        }

        private static double[] rescaleAxis(ref double[] valsX, ref double[] valsY, ref double[] valsZ)
        {
            return new double[3] { getPow10(ref valsX), getPow10(ref valsY), getPow10(ref valsZ) };
        }

        private static double getPow10( ref double[] vals )
        {

            double scale = Math.Max(Math.Abs(vals.Max()), Math.Abs(vals.Min()));
 
            double pow = Math.Log10(scale);
             
            pow = Math.Floor(pow);

            double div = Math.Pow(10, pow);

            vals.ChangeEach(x => x / div);
 
            return pow;
        }

    }
}
