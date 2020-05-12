﻿using HTMLCodeBuilder.Nodes;
using HTMLCodeBuilder.TaggedElements;
using System;

namespace HTMLCodeBuilder.HTMLelements
{

    public static class HTMLElements
    {
        public static HTMLDocSettings HTMLsettings = new HTMLDocSettings();

        public static HTMLElement Create(string Tag)
        {
            string closeTag;

            if (Tag.StartsWith("<"))
            {
                closeTag = "</" + Tag.Substring(1);
                
                return new HTMLElement(Tag, closeTag);
            }

            return new HTMLElement("<"+ Tag+">", "</"+ Tag +">");
        }

        public static HTMLElement CreateDIV()
        {
            return new HTMLElement("<div>", "</div>");
        }

        public static HTMLElement CreateDIV(string InnerString)
        {
            HTMLElement element =  new HTMLElement("<div>", "</div>");
            element.InnerString = InnerString ;
            return element;
        }

        public static HTMLElement CreateSPAN()
        {
            return new HTMLElement("<span>", "</span>");
        }

        public static HTMLElement CreateSPAN(string InnerString)
        {
            HTMLElement element = new HTMLElement("<span>", "</span>");
            element.InnerString = InnerString;
            return element;
        }

        public static HTMLElement CreateA()
        {
            return new HTMLElement("<a>", "</a>");
        }

        public static HTMLElement CreateA(string InnerString)
        {
            HTMLElement element = new HTMLElement("<a>", "</a>");
            element.InnerString = InnerString;
            return element;
        }

        public static HTMLElement CreateParagraph()
        {
            return new HTMLElement("<p>", "</p>");
        }

        public static HTMLElement CreateParagraph(string InnerString)
        {
            HTMLElement element = new HTMLElement("<p>", "</p>");
            element.InnerString = InnerString;
            return element;
        }

        public static HTMLElement CreateTitle(string title)
        {
            HTMLElement elem =new  HTMLElement("<title>", "</title>");
            elem.InnerString = title;
            return   elem;
        }

        public static HTMLElement CreateHead()
        {
            return new HTMLElement("<head>", "</head>");
        }

        public static HTMLElement CreateHTML()
        {
            return new HTMLElement("<html>", "</html>");
        }

        public static HTMLElement CreateUL()
        {
            return new HTMLElement("<ul>", "</ul>");
        }

        public static HTMLElement CreateBody()
        {
            HTMLElement element = new HTMLElement("<body>", "</body>");
            element.appendParam("id","doucment-body");
            element.appendParam("#style", "background-color : " + HTMLsettings.getSetting(HTMLSettings.PageBackgroundColor));
            return element;
        }

        public static HTMLElement CreateStyle()
        {
            HTMLElement elem = new HTMLElement("<style>", "</style>");
            elem.appendParam("id", "doc-style-sheet");
            return elem;
        }

        public static HTMLElement CreateMeta()
        {
            return CreateMeta("utf - 8", "width = device - width, initial - scale = 1");
        }

        public static HTMLElement CreateMeta(string content)
        {
            return CreateMeta("utf - 8", content);
        }

        public static HTMLElement CreateMeta(string charset, string content)
        {
            HTMLElement elem = new HTMLElement("<meta>", "");
            elem.appendParam("charset", charset);
            elem.appendParam("content", content);
            elem.appendParam("name", "viewport");
            return elem;
        }

        public static HTMLElement CreateCenterAlign()
        {
            HTMLElement element = CreateDIV();
            element.appendParam("class", "center-align");
            element.appendParam(".style", "margin : 0 auto;width : 100%;background-color : " + HTMLsettings.getSetting(HTMLSettings.ContentBackgroundColor)+";");
            return element;
        }

        public static HTMLElement CreateCenterAlign(double width)
        {
            HTMLElement element = CreateDIV();
            element.appendParam("class", "center-align-"+ width.ToString().Replace(',', '_')+"mm");
            element.appendParam(".style", "margin : 0 auto;width : "+ width.ToString().Replace(',','.') + "mm;background-color : "
                                                            + HTMLsettings.getSetting(HTMLSettings.ContentBackgroundColor) + ";");
            return element;
        }

        public static HTMLElement CreateCenterAlign(string width)
        {
            HTMLElement element = CreateDIV();
            element.appendParam("class", "center-align-" + width.ToString().Replace(',', '_') + "mm");
            element.appendParam(".style", "margin : 0 auto;width : " + width + "mm;background-color : "
                                                            + HTMLsettings.getSetting(HTMLSettings.ContentBackgroundColor) + ";");
            return element;
        }

        public static HTMLElement CreateVerticalIndent(double indent)
        {
            HTMLElement HTMLindent = CreateDIV();
            string indentValStr = indent.ToString();
            HTMLindent.appendParam("class", "vertical-indent-" + indentValStr.Replace(",", "_") + "mm");
            HTMLindent.appendParam(".style", "background-color:rgba(250,250,250,255);width:100%;margin:0 auto;height:" + indentValStr.Replace(",", ".") + "mm;");
            return HTMLindent;
        }

        public static HTMLElement CreateVerticalIndent(string val)
        {
            HTMLElement HTMLindent = CreateDIV();
            HTMLindent.appendParam("class", "vertical-indent-" + val.Replace(",", "."));
            HTMLindent.appendParam(".style", "background-color:rgba(250,250,250,255);width:100%;margin:0 auto;height:" + val.Replace(",", ".") + ";");
            return HTMLindent;
        }

        public static TagElementsGroup CreatePageContainer()
        {
            TagElementsGroup container = new TagElementsGroup(CreateDIV());//CreateCenterAlign(HTMLsettings.getSetting(HTMLSettings.PageWigth)));
            container.getElement(container.RootID).appendParam(".style", "width : " + HTMLsettings.getSetting(HTMLSettings.PageWigth) + ";padding : 0;margin : 0 auto;background-color : " +
                                                                                       HTMLsettings.getSetting(HTMLSettings.ContentBackgroundColor) + ";");
            container.getElement(container.RootID).appendParam("class", "page-container");
            container.addElement(CreateVerticalIndent(HTMLsettings.getSetting(HTMLSettings.PageTopIndent)));
            int id = container.addElement(CreateDIV());
            container.getElement(id).appendParam("class", "page-fields-container");
            container.getElement(id).appendParam(".style", "width : " + HTMLsettings.getSetting(HTMLSettings.PageContentWidth) + ";padding-left : "+
                                                                        HTMLsettings.getSetting(HTMLSettings.PageLeftIndent) + ";margin: 0;");
            return container;
        }

        public static TagElementsGroup CreateTextBlock(string title, string text)
        {
            TagElementsGroup block = new TagElementsGroup(CreateCenterAlign());
    
            block.addElementParam(block.RootID, "id", "text-block-" + block.RootID.ToString());

            block.addElementParam(block.RootID, "#title", title);
            
            if (!string.IsNullOrEmpty(title))
            {
                int blockID = block.addElement(CreateParagraph());
                block.addElementParam(blockID, "class", "paragraph");
                block.addElementParam(blockID, "id", "text-block-paragraph-" + block.RootID.ToString());
                block.getElement(blockID).InnerString = title;
                block.addElementParam(blockID, ".style", "font-size: " + HTMLsettings.getSetting(HTMLSettings.ParagraphTitleFontSize)
                                                                       + ";text-indent : 10mm;text-align : left;line-height:1.2;margin: 0 auto;background-color : "
                                                                       + HTMLsettings.getSetting(HTMLSettings.ContentBackgroundColor)+ ";color : "
                                                                       + HTMLsettings.getSetting(HTMLSettings.ParagraphFontColor) + ";");

            }
            if (!string.IsNullOrEmpty(text))
            {
                int textNodeID = block.addElement(CreateDIV());//addNode(HTMLElements.CreateDIV(), BlockID);
                block.getElement(textNodeID).InnerString = text;
                block.addElementParam(textNodeID, "class", "paragraph-text");
                block.addElementParam(textNodeID, ".style", "font-size : " + HTMLsettings.getSetting(HTMLSettings.FontSize) 
                                                                           + ";text-indent : 10mm;text-align : justify;margin : 0 auto;line-height : 1.4;background-color : "
                                                                           + HTMLsettings.getSetting(HTMLSettings.ContentBackgroundColor) + ";color : "
                                                                           + HTMLsettings.getSetting(HTMLSettings.ContentFontColor) + ";");
            }

            block.addElement(CreateVerticalIndent(2.5));

            return block;
        }

        public static TagElementsGroup CreatePageContent(string title)
        {
            TagElementsGroup content = CreateTextBlock(title, "");

            content.addElementParam(content.RootID, "class", "content-list-holder");

            int nodeID = content.addElement(CreateUL());

            content.addElementParam(nodeID, "class", "content-list");

            content.addElementParam(nodeID, "id", "doc-content");

            string css = "content-list +font-size : " + HTMLsettings.getSetting(HTMLSettings.ContentFontSize) + ";list-style-type:none;+"+
                         "content-list a +text-decoration : none;display : block;text-align : left;line-height : 1.4;overflow: hidden;color : "+HTMLsettings.getSetting(HTMLSettings.ContentFontColor)+";+" +
                         "content-list a:hover +background-color : "+HTMLsettings.getSetting(HTMLSettings.ContentHoverColor) +";+" +
                         "content-list a:visited +color : "+ HTMLsettings.getSetting(HTMLSettings.ContentFontColor) + ";";

            content.addElementParam(nodeID, "..style", css);

            content.addElement(CreateVerticalIndent(2.5));

            return content;
        }

        public static TagElementsGroup CreateTable(int w, int h, string[] headers)
        {

            if (w != headers.Length && headers.Length != 0)
            {
                w = Math.Min(w, headers.Length);
            }

            TagElementsGroup table = new TagElementsGroup(new HTMLElement("<table>", "</table>"));
            
            table.addElementParam(table.RootID, "class", "table-data");

            table.addElementParam(table.RootID, "id", "table-" + new Random().NextDouble().GetHashCode().ToString());

            string style_width = "width : " + (100.0 / w).ToString().Replace(',','.') + "%";

            string css = table.getElementParam(table.RootID, "id") + "+width:100%;border-collapse: collapse;text-align : center;+"+
                         table.getElementParam(table.RootID, "id") + " td+border:1px solid " + HTMLsettings.getSetting(HTMLSettings.TableBoderColor) + ";"+ style_width + ";+"+
                         table.getElementParam(table.RootID, "id")+ " th+border:1px solid " + HTMLsettings.getSetting(HTMLSettings.TableBoderColor) + ";";

            table.addElementParam(table.RootID, "##style", css);

            if (headers.Length != 0)
            {
                int tr = table.addElement(new HTMLElement("<tr>", "</tr>"), table.RootID);

                for (int i = 0; i < headers.Length; i++)
                {
                    int th = table.addElement(new HTMLElement("<th>", "</th>"), tr);
                    table.addElementParam(th, "class", "table-header-cell");
                    table.getElement(th).InnerString = headers[i];
                }
            }

            for (int i = 0; i < h; i++)
            {
                int tr = table.addElement(new HTMLElement("<tr>", "</tr>"), table.RootID);

                for (int j = 0; j < w; j++)
                {
                    int td = table.addElement(new HTMLElement("<td>", "</td>"), tr);
                    table.addElementParam(td, "class", "table-cell");
                }
            }
            return table;
        }

        public static TagElementsGroup CreateSubscription(string subscr)
        {
            TagElementsGroup subscrGroup = new TagElementsGroup(CreateCenterAlign());

            subscrGroup.addElementParam(subscrGroup.RootID,"class", "container-subscription");

            subscrGroup.addElementParam(subscrGroup.RootID, ".style", "text-align:center;");

            subscrGroup.addElement(CreateVerticalIndent(2.5));

            int nodeID = subscrGroup.addElement(CreateSPAN());

            subscrGroup.addElementParam(nodeID, "class", "subscription-text");
            subscrGroup.getElement(nodeID).InnerString = subscr;
            subscrGroup.addElementParam(nodeID, ".style", "background-color:" + HTMLsettings.getSetting(HTMLSettings.ContentBackgroundColor) + ";text-align:center;font-size:"
                +HTMLsettings.getSetting(HTMLSettings.ContentFontSize)+";");

            nodeID = subscrGroup.addElement(CreateSPAN(), nodeID);
            subscrGroup.addElementParam(nodeID, "class", "subscription-enumeration");
            subscrGroup.addElementParam(nodeID, ".style", "font-weight: bold;");


            return subscrGroup;
        }

        public static TagElementsGroup CreateGridHolder(int rows, int coloms)
        {
            TagElementsGroup grholder = CreateTable(coloms, rows, new string[] {});

            string style_width = "width : " + (100.0 / coloms).ToString() + "%";

            string css = grholder.getElementParam(grholder.RootID, "id") + "+width : 100%;border-collapse : collapse;text-align : center;+" +
                         grholder.getElementParam(grholder.RootID, "id") + " td+border : none;" + style_width + ";+" +
                         grholder.getElementParam(grholder.RootID, "id") + " th+border : none;";

            grholder.addElementParam(grholder.RootID, "##style", css);
            return grholder;
        }
        
        public static void EditTableCell(TagElementsGroup table, int row, int col, string val)
        {
            if (col < 0 || row < 0)
            {
                return;
            }

            int [] tableChildrens = table.getNode(table.RootID).getChildrenIDs();

            if (tableChildrens.Length - 1 < row)
            {
                row = tableChildrens.Length - 1;
            }
                tableChildrens = table.getNode(tableChildrens[row + 1]).getChildrenIDs();//cells

            if (tableChildrens.Length  < col)
            {
                col = tableChildrens.Length - 1 ;
            }

            Node<ITagElement> n = table.getNode(tableChildrens[col]);

            n.getData().InnerString = val;
        }
    }
}
