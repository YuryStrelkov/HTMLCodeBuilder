using HTMLCodeBuilder.SVGelements;
using HTMLCodeBuilder.TaggedElements;
using System;
using System.Collections.Generic;
using System.Text;

namespace HTMLCodeBuilder.HTMLelements
{
    public enum StyleSelectorType
    {
        Tag = 0,
        Class= 1,
        ID = 2
    }

    public class HTMLPage : TagElementsGroup
    {
        public int HTMLPageID { get; protected set;}

        public int HTMLHeadID { get; protected set; }

        public int HTMLBodyID { get; protected set; }

        public int HTMLStyleID { get; protected set; }

        public int HTMLContentListID { get; protected set; }

        private int TablesCount = 0;

        private int TextBlockCount = 0;

        private int ImagesCount = 0;

        private TagElementsGroup LastImage;
        
        private TagElementsGroup LastGraphic;

        private TagElementsGroup LastTable;

        private TagElementsGroup LastTextBlock;
        
        private Dictionary<int, style> styles;
  
        private struct style
        {
            public string selector { get; private set; }

            public string code { get; private set; }

            public style(string selector_, string code_)
            {
                selector = selector_;

                code = code_;
            }
        }

        public void updateStyle(StyleSelectorType type, string selectorName, string code)
        {
            int codeID = (selectorName + code).GetHashCode();

            if (styles.ContainsKey(codeID))
            {
                return;
            }
            
            switch (type)
            {
                case StyleSelectorType.Class: selectorName = "." + selectorName; break;

                case StyleSelectorType.ID: selectorName = "#" + selectorName; break;

                case StyleSelectorType.Tag: break;
            }

            code = code.Replace(";", ";\n\t\t\t");

            styles.Add(codeID, new style(selectorName, code));
        }

        public TagElementsGroup addTable(int parentID, string subscr, bool enumirate, int w, int h,string[] headers)
        {
            int holderID = addElement(HTMLElements.CreateCenterAlign(180), parentID);

            if (subscr.Length != 0)
            {
                TagElementsGroup subscrGroup = HTMLElements.CreateSubscription(subscr);
 
                if (enumirate)
                {
                    TablesCount += 1;
                    List<int> nodes = subscrGroup.getElementByClass("subscription-enumeration");
                    subscrGroup.getNode(nodes[0]).getData().InnerString = "Таблица № " + TablesCount.ToString() + ". ";
                }
                mergeHTML(holderID, subscrGroup);
            }

            addElement(HTMLElements.CreateVerticalIndent(2.5), holderID);

            TagElementsGroup table = HTMLElements.CreateTable( w, h, headers);

            mergeHTML(holderID, table);

            addElement(HTMLElements.CreateVerticalIndent(2.5), parentID);

            LastTable = table;

            return table;
        }

        public TagElementsGroup addTextBlock(string blockName, string text, bool add2content)
        {
           return addTextBlock( HTMLBodyID, blockName, text, add2content);
        }

        public TagElementsGroup addTextBlock(int parentID, string blockName, string text, bool add2content)
        {

            TagElementsGroup textBlock =  HTMLElements.CreateTextBlock(blockName, text);
            
            mergeHTML(parentID, textBlock);

            if (add2content)
            {
                TextBlockCount += 1;
                updateContent(textBlock.RootID);
            }

            LastTextBlock = textBlock;

            return textBlock;

        }

        public TagElementsGroup addContent(int parentID, string contentTitle)
        {
            TagElementsGroup content = HTMLElements.CreatePageContent(contentTitle);

            List<int> ids = content.getElementByTag("ul");

            HTMLContentListID = ids[0]; 

            mergeHTML(parentID, content);

            return content;
        }   

        public void addGrap2D()
        {

        }

        public void GraphicTitle(string t)
        {
            if (LastGraphic != null)
            {
                SVGElements.GraphicTitle(LastGraphic,t);
            }
        }

        public void GraphicTitle(TagElementsGroup Graphic, string t)
        {
            SVGElements.GraphicTitle(Graphic, t);
        }

        public void GraphicXLabel(string x)
        {
            if (LastGraphic != null)
            {
                SVGElements.GraphicXLabel(LastGraphic, x);
            }
        }

        public void GraphicXLabel(TagElementsGroup Graphic, string t)
        {
            SVGElements.GraphicXLabel(Graphic, t);
        }

        public void GraphicYLabel(string y)
        {
            if (LastGraphic != null)
            {
                SVGElements.GraphicYLabel(LastGraphic, y);
            }
        }

        public void GraphicYLabel(TagElementsGroup Graphic, string t)
        {
            SVGElements.GraphicYLabel(Graphic, t);
        }
 
        public TagElementsGroup addGraphic(int parent, double w, double h)
        {
            TagElementsGroup container = new TagElementsGroup(HTMLElements.CreateCenterAlign());
            TagElementsGroup Graphic = SVGElements.CreateSVGGraphicXY(w, h);
            container.mergeGroups(Graphic);
            mergeHTML(parent, container);
            LastGraphic = Graphic;
            return Graphic;
        }

        public TagElementsGroup appendGraphic(double[] x, double[] y, string legend = "", string color = "")
        {
            if (LastGraphic==null)
            {
                return addGraphic(HTMLBodyID, 100, 100);
            }
            SVGElements.AppendSVGGraphic(ref LastGraphic, x, y,legend,color);
            mergeHTML(LastGraphic.getNode(LastGraphic.RootID).getParentID(), LastGraphic);
            return LastGraphic;
        }
        
        public TagElementsGroup appendGraphic(double[] x, double[] y, double[] z)
        {
            if (LastGraphic == null)
            {
                return addGraphic(HTMLBodyID, 100, 100);
            }
            SVGElements.AppendSVGGraphic(ref LastGraphic, x, y, z);
            mergeHTML(LastGraphic.getNode(LastGraphic.RootID).getParentID(), LastGraphic);
            return LastGraphic;
        }

        private void updateContent(int nodeID)
        {
            if(nodeID ==-1)
            {
                return;
            }
            if (HTMLContentListID == -1)
            {
                return;
            }

            int contentRecord = addNode(HTMLElements.Create("<li>"), HTMLContentListID);

            addElementParam(contentRecord, "class", "content-record");

            int contentRef = addNode(HTMLElements.Create("<a>"), contentRecord);

            addElementParam(contentRef, "href", "#" + getNode(nodeID).getData().getParam("id"));

            getNode(contentRef).getData().InnerString = getNode(nodeID).getData().getParam("#title");
        }

        private void mergeStyle(ITagElement element)
        {

            if (element.hasParam(".style"))
            {
                updateStyle(StyleSelectorType.Class, element.getParam("class"), element.getParam(".style"));

                element.remParam(".style");

                return;
            }

            if (element.hasParam("#style"))
            {
                updateStyle(StyleSelectorType.ID, element.getParam("id"), element.getParam("#style"));

                element.remParam("#style");

                return;
            }

            if (element.hasParam("..style"))
            {
                string[] cssCode = element.getParam("..style").Split('+');

                for (int i = 0; i < cssCode.Length; i += 2)
                {
                    updateStyle(StyleSelectorType.Class, cssCode[i], cssCode[i + 1]);
                }

                element.remParam("..style");

                return;
            }

            if (element.hasParam("##style"))
            {
                string[] cssCode = element.getParam("##style").Split('+');

                for (int i = 0; i < cssCode.Length; i += 2)
                {
                    updateStyle(StyleSelectorType.ID, cssCode[i], cssCode[i + 1]);
                }

                element.remParam("##style");

                return;
            }

        }

        public  void mergeHTML(int nodeID, TagElementsGroup list)
        {
            if (!containsNode(nodeID))
            {
                nodeID = RootID;
            }
            foreach (int key in list.getNodeKeys())
            {
                addNodeDirect(list.getNode(key));

                mergeStyle(list.getNode(key).getData());

            }
            getNode(nodeID).addChild(list.RootID);
        }

        public new int addElement(ITagElement element)
        {
            return addElement(element, HTMLBodyID);
        }

        public new int addElement(ITagElement element, int parentID)
        {
            mergeStyle(element);

            return base.addElement(element, parentID);
        }

        public new string buildCode()
        {
            DateTime start = DateTime.Now;

            StringBuilder stylesStr = new StringBuilder();

            stylesStr.Append("\n");

            foreach (int key in styles.Keys)
            {
                stylesStr.Append("\t\t\t");
                stylesStr.Append(styles[key].selector);
                stylesStr.Append("{\n\t\t\t");

                stylesStr.Append(styles[key].code);
                stylesStr.Append("}\n");
            }

            stylesStr.Append("\t\t");

            getNode(HTMLStyleID).getData().InnerString = stylesStr.ToString();

            Code = "<!DOCTYPE html>\n" + base.buildCode();

            DateTime end = DateTime.Now;

            Console.WriteLine("Time taken for HTML build: {0}", end - start);

            return Code;
        }

        public HTMLPage() : base(HTMLElements.CreateHTML())
        {
            HTMLContentListID = -1;
            styles = new Dictionary<int, style>();
            HTMLPageID = RootID;
            HTMLHeadID = addElement(HTMLElements.CreateHead());
            addElement(HTMLElements.CreateMeta(),  HTMLHeadID);
            HTMLStyleID = addElement(HTMLElements.CreateStyle(), HTMLHeadID);
            addElement(HTMLElements.CreateBody());
            TagElementsGroup pageContainer = HTMLElements.CreatePageContainer();
            mergeHTML(HTMLBodyID,pageContainer);
            HTMLBodyID = pageContainer.lastID;
       }
    
    }
}
