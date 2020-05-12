using HTMLCodeBuilder.TaggedElements;
using HTMLCodeBuilder.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

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
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = mutator(array[i]);
            }
        }

        private static double parseSize2Pix(double x)
        {
            return x * SVGElement.PIXEL_PER_MM;
        }

        private static string parseSize2PixString(double x)
        {
            return num2str(x * SVGElement.PIXEL_PER_MM);
        }

        private static string parseSize2MMString(double x)
        {
            return num2str(x)+"mm";
        }

        public static SVGElement CreateSVGLine(double x1, double y1, double x2, double y2, string style)
        {
            SVGElement line = new SVGElement("<line>", "</line>");
            line.appendParam("x1", parseSize2PixString(x1));
            line.appendParam("x2", parseSize2PixString(x2));
            line.appendParam("y1", parseSize2PixString(y1));
            line.appendParam("y2", parseSize2PixString(y2));
            line.appendParam("style", style);
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

            node.appendParam("class", "transform-node");

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
            Text.appendParam("font-size", "14pt");
            Text.appendParam("font-family", "Times New Roman");
            Text.appendParam("style", "text-anchor : middle;");
            return Text;
        }

        public static SVGElement CreateSVGText(string text, double xc, double yc, double rot, string style)
        {
            SVGElement Text = CreateSVGText(text, xc, yc, rot);
            Text.appendParam("style", style);
            return Text;
        }

        public static SVGElement CreateSVGRect(double x, double y, double w, double h, string style)
        {
            SVGElement rect = new SVGElement("<rect>", "</rect>");
            rect.Move(x, y);
            rect.setSizes(w, h);
            rect.appendParam("style", style);
            return rect;
        }

        public static SVGElement CreateSVGPolyLine(double x_0, double y_0, ref double[] x, ref double[] y, string style)
        {
            SVGElement line = new SVGElement("<polyline>", "</polyline>");

            try
            {
                line.Move(x_0, y_0);

                line.Scale(1, -1);

                line.appendParam("class", "poly-line");

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
                line.appendParam( "points", points.ToString());
                line.appendParam( "style", style);
               
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
            line.appendParam("fill", "none");
            line.appendParam("style", style);
            line.appendParam("stroke-dasharray", "5,5");
            line.appendParam("d", "M" + parseSize2PixString(x1) + " " + parseSize2PixString(y1) + " l" +
                                        parseSize2PixString(x2 - x1) + " " + parseSize2PixString(y2 - y1));
            return line;
        }

        public static SVGElement CreateSVGImage()
        {
            SVGElement image = new SVGElement("<image>", "</image>"); new SVGElement("<image>", "</image>");

            image.appendParam("xlink:href", "");

            image.appendParam("image-rendering", "optimizeSpeed");

            image.appendParam("preserveAspectRatio", "none");

            return image;
        }

        public static SVGElement CreateSVGImage(string src)
        {
            SVGElement image = CreateSVGImage();

            image.appendParam("xlink:href", src);

            return image;
        }

        public static SVGElement CreateSVGImage(int w, int h, double[] src, ColorMaps cmap)
        {
            Bitmap img = ImageProcessing.CreateImage(w, h);

            ImageProcessing.UpdateImage(ref img, src, cmap);

            string path = img.GetHashCode().ToString() + ".png";

            ImageProcessing.WriteImage(ref img, path);

            SVGElement image = CreateSVGImage(path);

            image.setSizes(w / SVGElement.PIXEL_PER_MM, h / SVGElement.PIXEL_PER_MM);

            return image;
        }

        public static SVGElement CreateSVGImage(Bitmap img)
        {
            string path = img.GetHashCode().ToString() + ".png";

            ImageProcessing.WriteImage(ref img, path);

            SVGElement image = CreateSVGImage(path);

            image.setSizes(img.Width / SVGElement.PIXEL_PER_MM, img.Height / SVGElement.PIXEL_PER_MM);

            return image;
        }

        public static SVGElement CreateSVGImage(int w, int h, byte[] src)
        {
            Bitmap img = ImageProcessing.CreateImage(w, h);

            ImageProcessing.UpdateImage(ref img, src);

            string path = img.GetHashCode().ToString() + ".png";

            ImageProcessing.WriteImage(ref img, path);

            SVGElement image = CreateSVGImage(path);

            image.setSizes(w / SVGElement.PIXEL_PER_MM, h / SVGElement.PIXEL_PER_MM);

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

            plotxy.addElementParam(plotxy.RootID, "class", "graphicic-node");

            plotxy.addElementParam(plotxy.RootID, "id", "graphicic-node-" + plotxy.RootID.ToString());

            return plotxy;

        }

        public static void AppendSVGGraphic(ref TagElementsGroup graphic, double[] xs, double[] ys, string legendText = "", string lineStyle = "")
        {
            if (!graphic.hasClass("graphicic-node"))
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

            if (!graphic.hasClass("axes"))
            {
                double[] axseXS = new double[11], axseYS = new double[11];

                double dx = (xmax - xmin) / 10;

                double dy = (ymax - ymin) / 10;

                for (int i = 0; i < 11; i++)
                {
                    axseXS[i] = xmin + i * dx;
                    axseYS[i] = ymin + i * dy;
                }
                SVGElement graphicBg = (SVGElement)graphic.getElement(graphic.getElementByClass("graphicic-node")[0]);

                TagElementsGroup axes = CreateSVGXYAxis(graphicBg.W, graphicBg.H, axseXS, axseYS);

                graphic.mergeGroups(axes);

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
            if (!graphic.hasClass("graphicic-node"))
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

            if (!graphic.hasClass("axes"))
            {
                double[] axseXS = new double[11], axseYS = new double[11], axseZS = new double[11]; ;

                double dx = (xmax - xmin) / 10;

                double dy = (ymax - ymin) / 10;

                double dz = (zmax - zmin) / 10;

                for (int i = 0; i < 11; i++)
                {
                    axseXS[i] = xmin + i * dx;
                    axseYS[i] = ymin + i * dy;
                    axseZS[i] = zmin + i * dz;
                }

                int graphicBgID = graphic.getElementByClass("graphicic-node")[0];

                SVGElement graphicBg = (SVGElement)graphic.getElement(graphicBgID);

                TagElementsGroup axes = CreateSVGXYZAxis(graphicBg.W, graphicBg.H, axseXS, axseYS, axseZS);

                graphic.mergeGroups(axes);

                ///Данные цветовой схемы отображения

                int HolderID = graphic.getElementByClass("graphic-color-bar")[0];

                SVGElement holderElement = (SVGElement)graphic.getElement(HolderID);

                if (graphic.hasChildrens(HolderID))
                {
                    graphic.removeChildrens(HolderID);
                }

                SVGElement colorbar = CreateSVGImage(ImageProcessing.CreateJetColorMap());

                colorbar.setSizes(holderElement.W, holderElement.H);

                graphic.addElement(colorbar, HolderID);

                ///Данные двумерного распределения

                HolderID = graphic.getElementByClass("graphic-data")[0];

                holderElement = (SVGElement)graphic.getElement(HolderID);

                if (graphic.hasChildrens(HolderID))
                {
                    graphic.removeChildrens(HolderID);
                }

                SVGElement data = CreateSVGImage(xs.Length, ys.Length, zs, ColorMaps.JET);

                data.setSizes(holderElement.W, holderElement.H);

                graphic.addElement(data, HolderID);

            }


        }

        public static void UpdateSVGLegend(ref TagElementsGroup graphic, string legendText, string style)
        {
             if (string.IsNullOrEmpty(legendText))
            {
                return;
            } 
            if (!graphic.hasClass("legend-holder"))
            {
                CreateSVGgraphicLegend(ref graphic);
            }
            int legendNodeID = graphic.getElementByClass("legend-node")[0];

            int legendRectID = graphic.getElementByClass("legend-holder")[0];

            SVGElement legendRect = (SVGElement)graphic.getElement(legendRectID);

            SVGElement legendRecord = CreateSVGText(legendText, -4.5, legendRect.H + 4.75, 0);
            legendRecord.appendParam("style", "text-anchor:end; ");

            legendRect.setSizes(Math.Max(Math.Max(legendRect.W, legendText.Length * 2.75),8), legendRect.H + 6.5);
            legendRect.setPosition(-legendRect.W, legendRect.Y);
            graphic.addElement(legendRecord, legendNodeID);
            graphic.addElement(CreateSVGLine(-4, legendRect.H - 3.25, -1, legendRect.H - 3.25, "stroke-width : 0.5mm; stroke : " + style), legendNodeID);
        }

        public static void GraphicTitle(TagElementsGroup graphic, string title)
        {
            if (!graphic.hasClass("title"))
            {
                return;
            }
            graphic.getElement(graphic.getElementByClass("title")[0]).InnerString = title;
        }

        public static void GraphicXLabel(TagElementsGroup graphic, string xlabel)
        {
            if (!graphic.hasClass("x-label"))
            {
                return;
            }
            graphic.getElement(graphic.getElementByClass("x-label")[0]).InnerString = xlabel;
        }

        public static void GraphicYLabel(TagElementsGroup graphic, string ylabel)
        {
            if (!graphic.hasClass("y-label"))
            {
                return;
            }
            graphic.getElement(graphic.getElementByClass("y-label")[0]).InnerString = ylabel;
        }

        private static void DrawSVGPolyLine(ref TagElementsGroup axes, double[] xs, double[] ys, double xmin, double xmax, double ymin, double ymax, string style = "")
        {
            SVGElement elem = (SVGElement)axes.getElement(axes.getElementByClass("graphic-backround")[0]);

            double xOff = 1 / (xmax - xmin) * elem.Wpix;

            double yOff = 1 / (ymax - ymin) * elem.Hpix;

            xs.ChangeEach(x => x * xOff);

            ys.ChangeEach(x => x * yOff);

            xOff *= xmin;

            yOff *= ymax;

            SVGElement line;

            if (string.IsNullOrEmpty(style))
            {
                line = CreateSVGPolyLine(elem.X0 - xOff / SVGElement.PIXEL_PER_MM,
                                                          elem.Y0 + yOff / SVGElement.PIXEL_PER_MM, ref xs, ref ys, "fill:none;stroke-width:2px;stroke:rgb(0,0,0);");

                axes.addElement(line, axes.getElementByClass("charts-list-node")[0]);

                return;
            }

            line = CreateSVGPolyLine(elem.X0 - xOff / SVGElement.PIXEL_PER_MM,
                                                       elem.Y0 + yOff / SVGElement.PIXEL_PER_MM, ref xs, ref ys, style);

            axes.addElement(line, axes.getElementByClass("charts-list-node")[0]);
        }

        private static void CreateSVGgraphicLegend(ref TagElementsGroup axes)
        {
            List<int> nodes = axes.getElementByClass("legend-node");

            if (nodes == null)
            {
                return;
            }
            SVGElement legend = CreateSVGRect(0, 0, 0, 0, "fill:rgb(255,255,255);stroke-width:1px;stroke:rgb(0,0,0)");

            legend.appendParam("class", "legend-holder");

            axes.addElement(legend, nodes[0]);
        
        }

        private static TagElementsGroup CreateSVGXYAxis(double w, double h, double[] xs, double[] ys)
        {
            double[] AX_UP_LEFT = new double[] { Math.Max(12, 0.1 * w), Math.Max(0.07 * h, 5) };

            double[] AX_W_H = new double[] { w - AX_UP_LEFT[0] - Math.Max(5, 0.03 * w), 0.825 * h };

            double[] AX_CENTER_W_H = new double[] { AX_W_H[0] / 2, AX_W_H[1] / 2 };

            double[] AX_X_TEXT_POS = new double[] { -Math.Max(0.07 * w, 8.5), -0.5 };

            double[] AX_Y_TEXT_POS = new double[] { Math.Max(h * 0.9125, h - 8.25), Math.Max(h * 0.9125, h - 8.25) - 4.5 };
            
            SVGElement axNodeBack = new SVGElement("<g>", "</g>");

            axNodeBack.Move(AX_UP_LEFT[0], AX_UP_LEFT[1]);

            axNodeBack.setSizes(AX_W_H[0], AX_W_H[1]);

            axNodeBack.appendParam("class", "graphic-backround");

            TagElementsGroup axNodeBackground = new TagElementsGroup(axNodeBack); 

            SVGElement charts = new SVGElement("<g>", "</g>"); 

            charts.appendParam("class", "charts-list-node");

            axNodeBackground.addElement(charts);


            TagElementsGroup axTitle = new TagElementsGroup(CreateSVGNode());

            axTitle.addElementParam(axTitle.RootID,"class", "axes-subscriptions");
                
            SVGElement title = CreateSVGText("", AX_CENTER_W_H[0], -0.025 * h, 0);
            title.appendParam("class", "title-node");
            title.appendParam("class", "title");
            title.appendParam("font-size", "14pt");
            axTitle.addElement(title);

            SVGElement xlabel = CreateSVGText("", AX_CENTER_W_H[0], AX_Y_TEXT_POS[0], 0);
            xlabel.appendParam( "class", "x-label-node");
            xlabel.appendParam("class", "x-label");
            xlabel.appendParam("font-size", "14pt");
            axTitle.addElement(xlabel);

            SVGElement ylabel = CreateSVGText("", AX_X_TEXT_POS[0], AX_CENTER_W_H[1], -90);
            ylabel.appendParam( "class", "y-label-node");
            ylabel.appendParam( "class", "y-label");
            ylabel.appendParam("font-size", "14pt");
            axTitle.addElement(ylabel);
           
            axNodeBackground.mergeGroups(axTitle);

            TagElementsGroup axesNode = new TagElementsGroup(CreateSVGNode());

            axesNode.addElementParam(axesNode.RootID,"class", "axes");

            double[] powAxis = rescaleAxis(ref xs, ref ys);

            if (powAxis[0] != 0)
            {
                SVGElement text = CreateSVGText("+", AX_W_H[0] - 5, AX_Y_TEXT_POS[0] + 0.5, 45);
                text.appendParam("font-size", "12pt");
                text.appendParam("style", "text-anchor:end");
                axesNode.addElement(text);

                text = CreateSVGText("10", AX_W_H[0], AX_Y_TEXT_POS[0], 0);
                text.appendParam("font-size", "12pt");
                text.appendParam("style", "text-anchor:end");
                axesNode.addElement(text);

                if (powAxis[0] != 1)
                {
                    text = CreateSVGText(powAxis[0].ToString(), AX_W_H[0], AX_Y_TEXT_POS[0] - 2, 0);
                    text.appendParam( "font-size", "7pt");
                    text.appendParam( "class", "ax-x-mult");
                    text.appendParam("style", "text-anchor:start");
                    axesNode.addElement(text);
                }

            }

            if (powAxis[1] != 0)
            {
                SVGElement text = CreateSVGText("+", 1, -0.5, 45);
                text.appendParam( "font-size", "12pt");
                text.appendParam("style", "text-anchor:end");
                axesNode.addElement(text);

                text = CreateSVGText("10", 6, -1, 0);
                text.appendParam("font-size", "12pt");
                text.appendParam("style", "text-anchor:end");
                axesNode.addElement(text);

                if (powAxis[1] != 1)
                {
                    text = CreateSVGText(powAxis[1].ToString(), 6, -3, 0);
                    text.appendParam( "font-size", "7pt");
                    text.appendParam("style", "text-anchor:start");
                    text.appendParam("class", "ax-y-mult");
                    axesNode.addElement(text);
                }

            }
            
            numAlongY(ref axesNode, ys[ys.Length - 1].ToString(), AX_X_TEXT_POS[1], 1.25,             AX_W_H[0], 0,         0.25, "stroke-width: 0.25mm; stroke: black;");

            numAlongX(ref axesNode, xs[0].ToString(), 0,                AX_Y_TEXT_POS[1], 0,         AX_W_H[1], 0.25, "stroke-width: 0.25mm; stroke: black;");

            numAlongY(ref axesNode, ys[0].ToString(), AX_X_TEXT_POS[1], AX_W_H[1] + 1.25, AX_W_H[0], AX_W_H[1], -0.25, "stroke-width: 0.25mm; stroke: black;");

            numAlongX(ref axesNode, xs[xs.Length - 1].ToString(), AX_W_H[0], AX_Y_TEXT_POS[1], AX_W_H[0], AX_W_H[1], -0.25, "stroke-width: 0.25mm; stroke: black;");

            double dx = AX_W_H[0] / (xs.Length - 1);

            double dy = AX_W_H[1] / (ys.Length - 1);

            double x_curr, y_curr;

            for (int i = 1; i < ys.Length-1; i++)
            {
                y_curr = dy * i;

                x_curr = dx * i;

                numAlongY(ref axesNode, ( ys[ys.Length - 1 - i]).ToString(), AX_X_TEXT_POS[1], 1.25 + y_curr,    AX_W_H[0], y_curr,    0, "stroke-width: 0.5mm; stroke: black;");
                numAlongX(ref axesNode, xs[i].ToString(), x_curr,           AX_Y_TEXT_POS[1], x_curr,         AX_W_H[1], 0, "stroke-width: 0.5mm; stroke: black;");
            }

            axesNode.addElement(CreateSVGRect(0, 0, AX_W_H[0], AX_W_H[1], "fill:none;stroke-width:1px;stroke:rgb(0,0,0)"));

            axNodeBackground.mergeGroups(axesNode);
            
            SVGElement legendnode = new SVGElement("<g>", "</g>");

            legendnode.appendParam("class", "legend-node");

            legendnode.Move(AX_W_H[0] - 5 / SVGElement.PIXEL_PER_MM, 5 / SVGElement.PIXEL_PER_MM);

            axNodeBackground.addElement(legendnode);

            return axNodeBackground;
        }

        private static TagElementsGroup CreateSVGXYZAxis(double w, double h, double[] xs, double[] ys, double[] zs)
        {
            double[] AX_UP_LEFT = new double[] { Math.Max(12, 0.1 * w), Math.Max(0.07 * h, 5) };

            double[] AX_W_H = new double[] { w - AX_UP_LEFT[0] - Math.Max(14, 0.1 * w), 0.825 * h };

            double[] AX_CENTER_W_H = new double[] { AX_W_H[0] / 2, AX_W_H[1] / 2 };

            double[] AX_X_TEXT_POS = new double[] { AX_W_H[0]*1.035+0.5, -0.5 };

            double[] AX_Y_TEXT_POS = new double[] { Math.Max(h * 0.9125, h - 8.25), Math.Max(h * 0.9125, h - 8.25) - 4.5 };

            
            SVGElement axNodeBack = new SVGElement("<g>", "</g>");

            axNodeBack.Move(AX_UP_LEFT[0], AX_UP_LEFT[1]);

            axNodeBack.setSizes(AX_W_H[0], AX_W_H[1]);

            axNodeBack.appendParam("class", "graphic-backround");

            TagElementsGroup axNodeBackground = new TagElementsGroup(axNodeBack);
            

            TagElementsGroup axTitle = new TagElementsGroup(CreateSVGNode());

            axTitle.addElementParam(axTitle.RootID, "class", "axes-subscriptions");

            SVGElement title = CreateSVGText("", AX_CENTER_W_H[0], -0.025 * h, 0);
            title.appendParam("class", "title-node");
            title.appendParam("class", "title");
            title.appendParam("font-size", "14pt");
            axTitle.addElement(title);

            SVGElement xlabel = CreateSVGText("", AX_CENTER_W_H[0], AX_Y_TEXT_POS[0], 0);
            xlabel.appendParam("class", "x-label-node");
            xlabel.appendParam("class", "x-label");
            xlabel.appendParam("font-size", "14pt");
            axTitle.addElement(xlabel);

            SVGElement ylabel = CreateSVGText("", -AX_UP_LEFT[0]*0.75, AX_CENTER_W_H[1], -90);
            ylabel.appendParam("class", "y-label-node");
            ylabel.appendParam("class", "y-label");
            ylabel.appendParam("font-size", "14pt");
            axTitle.addElement(ylabel);

            axNodeBackground.mergeGroups(axTitle);

            TagElementsGroup axesNode = new TagElementsGroup(CreateSVGNode());

            axesNode.addElementParam(axesNode.RootID, "class", "axes");

            double[] powAxis = rescaleAxis(ref xs, ref ys, ref zs);

            if (powAxis[0] != 0)
            {
                SVGElement text = CreateSVGText("+", AX_W_H[0] - 5, AX_Y_TEXT_POS[0] + 0.5, 45);
                text.appendParam("font-size", "12pt");
                text.appendParam("style", "text-anchor:end");
                axesNode.addElement(text);

                text = CreateSVGText("10", AX_W_H[0], AX_Y_TEXT_POS[0], 0);
                text.appendParam("font-size", "12pt");
                text.appendParam("style", "text-anchor:end");
                axesNode.addElement(text);

                if (powAxis[0] != 1)
                {
                    text = CreateSVGText(powAxis[0].ToString(), AX_W_H[0], AX_Y_TEXT_POS[0] - 2, 0);
                    text.appendParam("font-size", "7pt");
                    text.appendParam("class", "ax-x-mult");
                    text.appendParam("style", "text-anchor:start");
                    axesNode.addElement(text);
                }

            }

            if (powAxis[1] != 0)
            {
                SVGElement text = CreateSVGText("+", 1, -0.5, 45);
                text.appendParam("font-size", "12pt");
                text.appendParam("style", "text-anchor:end");
                axesNode.addElement(text);

                text = CreateSVGText("10", 6, -1, 0);
                text.appendParam("font-size", "12pt");
                text.appendParam("style", "text-anchor:end");
                axesNode.addElement(text);

                if (powAxis[1] != 1)
                {
                    text = CreateSVGText(powAxis[1].ToString(), 6, -3, 0);
                    text.appendParam("font-size", "7pt");
                    text.appendParam("style", "text-anchor:start");
                    text.appendParam("class", "ax-y-mult");
                    axesNode.addElement(text);
                }

            }

            double cmapoff = AX_W_H[0] * 1.035;

            if (powAxis[2] != 0)
            {
                SVGElement text = CreateSVGText("+", cmapoff + 2, -0.5, 45);
                text.appendParam("font-size", "12pt");
                text.appendParam("style", "text-anchor:end");
                axesNode.addElement(text);

                text = CreateSVGText("10", cmapoff + 7, -2, 0);
                text.appendParam("font-size", "12pt");
                text.appendParam("style", "text-anchor:end");
                axesNode.addElement(text);

                if (powAxis[2] != 1)
                {
                    text = CreateSVGText(powAxis[2].ToString(), cmapoff + 7, -4, 0);
                    text.appendParam("font-size", "7pt");
                    text.appendParam("style", "text-anchor:start");
                    text.appendParam("class", "ax-z-mult");
                    axesNode.addElement(text);
                }

            }

            numAlongY(ref axesNode, ys[ys.Length - 1].ToString(), AX_X_TEXT_POS[1], 1.25, AX_W_H[0], 0, 0.25, "stroke-width: 0.25mm; stroke: black;");

            numAlongX(ref axesNode, xs[0].ToString(), 0, AX_Y_TEXT_POS[1], 0, AX_W_H[1], 0.25, "stroke-width: 0.25mm; stroke: black;");

            numAlongZ(ref axesNode, zs[zs.Length - 1].ToString(), AX_X_TEXT_POS[0], 1.25, cmapoff, 0, 0.25, "stroke-width: 0.25mm; stroke: black;");
            

            numAlongY(ref axesNode, ys[0].ToString(), AX_X_TEXT_POS[1], AX_W_H[1] + 1.25, AX_W_H[0], AX_W_H[1], -0.25, "stroke-width: 0.25mm; stroke: black;");

            numAlongX(ref axesNode, xs[xs.Length - 1].ToString(), AX_W_H[0], AX_Y_TEXT_POS[1], AX_W_H[0], AX_W_H[1], -0.25, "stroke-width: 0.25mm; stroke: black;");

            numAlongZ(ref axesNode, zs[0].ToString(), AX_X_TEXT_POS[0], AX_W_H[1]+1.25, cmapoff, AX_W_H[1], -0.25, "stroke-width: 0.25mm; stroke: black;");
            
            double dx = AX_W_H[0] / (xs.Length - 1);

            double dy = AX_W_H[1] / (ys.Length - 1);

            double y_curr,x_curr;

            for (int i = 1; i < ys.Length - 1; i++)
            {
                y_curr = dy * i;

                x_curr = dx * i;

                numAlongY(ref axesNode, (ys[ys.Length - 1 - i]).ToString(), AX_X_TEXT_POS[1], 1.25 + y_curr, AX_W_H[0], y_curr, 0, "stroke-width: 0.5mm; stroke: black;");

                numAlongZ(ref axesNode, (zs[ys.Length - 1 - i]).ToString(), AX_X_TEXT_POS[0], 1.25 + y_curr, cmapoff, y_curr, 0, "stroke-width: 0.5mm; stroke: black;");

                numAlongX(ref axesNode, xs[i].ToString(), x_curr, AX_Y_TEXT_POS[1], x_curr, AX_W_H[1], 0, "stroke-width: 0.5mm; stroke: black;");
            }

            axesNode.addElement(CreateSVGRect(0, 0, AX_W_H[0], AX_W_H[1], "fill:none;stroke-width:1px;stroke:rgb(0,0,0)"));

            axesNode.addElement(CreateSVGRect(AX_W_H[0] * 1.01, 0, 0.025 * AX_W_H[0], AX_W_H[1], "fill:none;stroke-width:1px;stroke:rgb(0,0,0)"));

            /// axesNode.addElement(CreateSVGRect(-AX_UP_LEFT[0], -AX_UP_LEFT[1], w, h, "fill:none;stroke-width:1px;stroke:rgb(255,0,0)"));

            SVGElement axNodeData = new SVGElement("<g>", "</g>");

            axNodeData.setSizes(AX_W_H[0], AX_W_H[1]);

            axNodeData.appendParam("class", "graphic-data");

            axNodeBackground.addElement(axNodeData);


            SVGElement axсColorBarNode = new SVGElement("<g>", "</g>");

            axсColorBarNode.setSizes(0.025 * AX_W_H[0], AX_W_H[1]);

            axсColorBarNode.Move(AX_W_H[0] * 1.01, 0);

            axсColorBarNode.appendParam("class", "graphic-color-bar");

            axNodeBackground.addElement(axсColorBarNode);


            axNodeBackground.mergeGroups(axesNode);

            return axNodeBackground;
        }

        private static void numAlongZ(ref TagElementsGroup parentGroup, string val, double xText, double yText, double xGrid, double yGrid, double dMarker, string style)
        {
            SVGElement text = CreateSVGText(val, xText, yText, 0);
            text.appendParam("font-size", "12pt");
            text.appendParam("class", "z-grid-val");
            text.appendParam("style", "text-anchor:start;");
            parentGroup.addElement(text);
            parentGroup.addElement(CreateSVGLine(xGrid - 0.25, yGrid+ dMarker+0.25, xGrid - 0.25 , yGrid + dMarker - 0.25, style));
        }

        private static void numAlongY(ref TagElementsGroup parentGroup, string val, double xText, double yText, double xGrid, double yGrid, double dMarker,string style)
        {
            SVGElement text = CreateSVGText(val, xText, yText, 0);
            text.appendParam("font-size", "12pt");
            text.appendParam("class", "y-grid-val");
            text.appendParam("style", "text-anchor:end");
            parentGroup.addElement(text);
            parentGroup.addElement(CreateSVGLine(0.25, yGrid + 0.25 + dMarker, 0.25, yGrid - 0.25 + dMarker, style));
            parentGroup.addElement(CreateSVGDashedLine(0, yGrid, xGrid, yGrid, "stroke-width: 0.125mm; stroke: black;"));
        }

        private static void numAlongX(ref TagElementsGroup parentGroup, string val, double xText, double yText, double xGrid, double yGrid, double dMarker, string style)
        {
            SVGElement text = CreateSVGText(val, xText, yText, 0);
            text.appendParam("font-size", "12pt");
            text.appendParam("class", "x-grid-val");
            parentGroup.addElement(text);
            parentGroup.addElement( CreateSVGLine(xGrid + dMarker, yGrid, xGrid + dMarker, yGrid - 0.5, style));
            parentGroup.addElement(CreateSVGDashedLine(xGrid, yGrid, xGrid, 0, "stroke-width: 0.125mm; stroke: black;"));
        }
        
        private static double[] rescaleAxis(ref double[] valsX, ref double[] valsY)
        {
            double[] pow = new double[2];
            pow[0] = getPow10(ref valsX);
            pow[1] = getPow10(ref valsY);
            return pow; 
        }

        private static double[] rescaleAxis(ref double[] valsX, ref double[] valsY, ref double[] valsZ)
        {
            double[] pow = new double[3];
            pow[0] = getPow10(ref valsX);
            pow[1] = getPow10(ref valsY);
            pow[2] = getPow10(ref valsZ);
            return pow;
        }

        private static double getPow10( ref double[] vals )
        {
            double max = Math.Abs(vals.Max());

            double min = Math.Abs(vals.Min());

            double pow = Math.Log10(max);
             
            pow = Math.Floor(pow);

            double div = Math.Pow(10, pow);

            vals.ChangeEach(x => x / div);
 
            return pow;
        }

    }
}
