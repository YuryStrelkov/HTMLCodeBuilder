using HTMLCodeBuilder.TaggedElements;

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


        public static HTMLElement CreateObject(string dataRef, string dataType)
        {
            HTMLElement obj = new HTMLElement("<object>", "</object>");
            obj.AddParam("data", dataRef);
            obj.AddParam("type", dataType);
            return obj;
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
            element.AddParam("id","doucment-body");
            element.AddParam("#style", "background-color : " + HTMLsettings.GetSetting(HTMLSettings.PageBackgroundColor));
            return element;
        }

        public static HTMLElement CreateStyle()
        {
            HTMLElement elem = new HTMLElement("<style>", "</style>");
            elem.AddParam("id", "doc-style-sheet");
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
            elem.AddParam("charset", charset);
            elem.AddParam("content", content);
            elem.AddParam("name", "viewport");
            return elem;
        }

        public static HTMLElement CreateCenterAlign()
        {
            HTMLElement element = CreateDIV();
            element.AddParam("class", "center-align");
            element.AddParam(".style", "text-align : center;margin : 0 auto;width : 100%;background-color : " + HTMLsettings.GetSetting(HTMLSettings.ContentBackgroundColor)+";");
            return element;
        }

        public static HTMLElement CreateCenterAlign(double width)
        {
            HTMLElement element = CreateDIV();
            element.AddParam("class", "center-align-"+ width.ToString().Replace(',', '_')+"mm");
            element.AddParam(".style", "text-align : center;margin : 0 auto;width : " + width.ToString().Replace(',','.') + "mm;background-color : "
                                                            + HTMLsettings.GetSetting(HTMLSettings.ContentBackgroundColor) + ";");
            return element;
        }

        public static HTMLElement CreateCenterAlign(string width)
        {
            HTMLElement element = CreateDIV();
            element.AddParam("class", "center-align-" + width.ToString().Replace(',', '_') + "mm");
            element.AddParam(".style", "text-align : center;margin : 0 auto;width : " + width + "mm;background-color : "
                                                            + HTMLsettings.GetSetting(HTMLSettings.ContentBackgroundColor) + ";");
            return element;
        }

        public static HTMLElement CreateVerticalIndent(double indent)
        {
            HTMLElement HTMLindent = CreateDIV();
            string indentValStr = indent.ToString();
            HTMLindent.AddParam("class", "vertical-indent-" + indentValStr.Replace(",", "_") + "mm");
            HTMLindent.AddParam(".style", "background-color:rgba(250,250,250,255);width:100%;margin:0 auto;height:" + indentValStr.Replace(",", ".") + "mm;");
            return HTMLindent;
        }

        public static HTMLElement CreateVerticalIndent(string val)
        {
            HTMLElement HTMLindent = CreateDIV();
            HTMLindent.AddParam("class", "vertical-indent-" + val.Replace(",", "."));
            HTMLindent.AddParam(".style", "background-color:rgba(250,250,250,255);width:100%;margin:0 auto;height:" + val.Replace(",", ".") + ";");
            return HTMLindent;
        }

        public static TagElementsGroup CreatePageContainer()
        {
            TagElementsGroup container = new TagElementsGroup(CreateDIV());
            container.AddElementParam(container.RootID,".style", "width : " + HTMLsettings.GetSetting(HTMLSettings.PageWigth) + ";padding : 0;margin : 0 auto;background-color : " +
                                                                                       HTMLsettings.GetSetting(HTMLSettings.ContentBackgroundColor) + ";");
            container.AddElementParam(container.RootID, "class", "page-container");
            container.AddElement(CreateVerticalIndent(HTMLsettings.GetSetting(HTMLSettings.PageTopIndent)));
            int id = container.AddElement(CreateDIV());
            container.AddElementParam(id, "class", "page-fields-container");
            container.AddElementParam(id, ".style", "width : " + HTMLsettings.GetSetting(HTMLSettings.PageContentWidth) + ";padding-left : "+
                                                                        HTMLsettings.GetSetting(HTMLSettings.PageLeftIndent) + ";margin: 0;");
            return container;
        }

        public static TagElementsGroup CreateTextBlock(string title, string text)
        {
            TagElementsGroup block = new TagElementsGroup(CreateCenterAlign());
    
            block.AddElementParam(block.RootID, "id", "text-block-" + block.RootID.ToString());

            block.AddElementParam(block.RootID, "#title", title);
            
            if (!string.IsNullOrEmpty(title))
            {
                int blockID = block.AddElement(CreateParagraph());
                block.AddElementParam(blockID, "class", "paragraph");
                block.AddElementParam(blockID, "id", "text-block-paragraph-" + block.RootID.ToString());
                block.GetElement(blockID).InnerString = title;
                block.AddElementParam(blockID, ".style", "font-size: " + HTMLsettings.GetSetting(HTMLSettings.ParagraphTitleFontSize)
                                                                       + ";text-indent : 10mm;text-align : left;line-height:1.2;margin: 0 auto;background-color : "
                                                                       + HTMLsettings.GetSetting(HTMLSettings.ContentBackgroundColor)+ ";color : "
                                                                       + HTMLsettings.GetSetting(HTMLSettings.ParagraphFontColor) + ";");

            }
            if (!string.IsNullOrEmpty(text))
            {
                int textNodeID = block.AddElement(CreateDIV());//addNode(HTMLElements.CreateDIV(), BlockID);
                block.GetElement(textNodeID).InnerString = text;
                block.AddElementParam(textNodeID, "class", "paragraph-text");
                block.AddElementParam(textNodeID, ".style", "font-size : " + HTMLsettings.GetSetting(HTMLSettings.FontSize) 
                                                                           + ";text-indent : 10mm;text-align : justify;margin : 0 auto;line-height : 1.4;background-color : "
                                                                           + HTMLsettings.GetSetting(HTMLSettings.ContentBackgroundColor) + ";color : "
                                                                           + HTMLsettings.GetSetting(HTMLSettings.ContentFontColor) + ";");
            }

            block.AddElement(CreateVerticalIndent(2.5));

            return block;
        }

        public static TagElementsGroup CreatePageContent(string title)
        {
            TagElementsGroup content = CreateTextBlock(title, "");

            content.AddElementParam(content.RootID, "class", "content-list-holder");

            int nodeID = content.AddElement(CreateUL());

            content.AddElementParam(nodeID, "class", "content-list");

            content.AddElementParam(nodeID, "id", "doc-content");

            string css = "content-list +font-size : " + HTMLsettings.GetSetting(HTMLSettings.ContentFontSize) + ";list-style-type:none;+"+
                         "content-list a +text-decoration : none;display : block;text-align : left;line-height : 1.4;overflow: hidden;color : "+HTMLsettings.GetSetting(HTMLSettings.ContentFontColor)+";+" +
                         "content-list a:hover +background-color : "+HTMLsettings.GetSetting(HTMLSettings.ContentHoverColor) +";+" +
                         "content-list a:visited +color : "+ HTMLsettings.GetSetting(HTMLSettings.ContentFontColor) + ";";

            content.AddElementParam(nodeID, "..style", css);

            content.AddElement(CreateVerticalIndent(2.5));

            return content;
        }

        public static TagElementsGroup CreateImageGridHolder(int rowCapacity)
        {
            TagElementsGroup holder = new TagElementsGroup(CreateDIV());

            holder.AddElementParam(holder.RootID, "class", "grid-holder");

            holder.AddElementParam(holder.RootID, "id", "grid-holder-"+ holder.RootID);

            holder.AddElementParam(holder.RootID, "#style","width : 100%");

            holder.AddElementParam(holder.RootID, "#row-capacity", rowCapacity.ToString());

            holder.AddElementParam(holder.RootID, "#char", "65");
            
            HTMLElement container = CreateDIV();

            container.AddParam("class", "container");

            HTMLElement containerLeftClear = CreateDIV();

            containerLeftClear.AddParam(".style", "clear: left;");

            container.AddParam(".style", "text-align : center;");

            holder.AddElement(container, holder.RootID);

            holder.AddElement(containerLeftClear, holder.RootID);

            holder.MergeGroups(CreateSubscription("lololo",2.5,2.5));

            return holder;
        }

        public static TagElementsGroup CreateSubscription(string subscr)
        {
            return CreateSubscription(subscr, 0, 0);
        }

        public static TagElementsGroup CreateSubscription(string subscr, double topIndent)
        {
            return CreateSubscription( subscr,  topIndent, 0);
        }

        public static TagElementsGroup CreateSubscription(string subscr,double topIndent, double bottonIndent)
        {
            TagElementsGroup subscrGroup = new TagElementsGroup(CreateCenterAlign());

            subscrGroup.AddElementParam(subscrGroup.RootID,"class", "container-subscription");

            subscrGroup.AddElementParam(subscrGroup.RootID, ".style", "text-align:center;");

            if (topIndent!=0)
            {
                subscrGroup.AddElement(CreateVerticalIndent(topIndent));
            }

            int nodeID = subscrGroup.AddElement(CreateSPAN());

            subscrGroup.AddElementParam(nodeID, "class", "subscription-text-holder");

            int enumirationNodeID = subscrGroup.AddElement(CreateSPAN(), nodeID);
            subscrGroup.AddElementParam(enumirationNodeID, "class", "subscription-enumeration");
            subscrGroup.AddElementParam(enumirationNodeID, ".style", "font-weight: bold;");

            int textNodeID = subscrGroup.AddElement(CreateSPAN(), nodeID);
            subscrGroup.AddElementParam(textNodeID, "class", "subscription-text");
            subscrGroup.GetElement(textNodeID).InnerString = subscr;
            subscrGroup.AddElementParam(textNodeID, ".style", "background-color:" + HTMLsettings.GetSetting(HTMLSettings.ContentBackgroundColor) + ";text-align:center;font-size:"
                +HTMLsettings.GetSetting(HTMLSettings.ContentFontSize)+";");

            if (bottonIndent != 0)
            {
                subscrGroup.AddElement(CreateVerticalIndent(topIndent));
            }
            return subscrGroup;
        }
    }
}
