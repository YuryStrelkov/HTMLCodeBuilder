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

        private HTMLTable LastTable;

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

        public HTMLTable addTable(int parentID, string subscr, bool enumirate, string[] headers)
        {
            int holderID = AddElement(HTMLElements.CreateCenterAlign(180), parentID);

            if (subscr.Length != 0)
            {
                TagElementsGroup subscrGroup = HTMLElements.CreateSubscription(subscr);
 
                if (enumirate)
                {
                    TablesCount += 1;
                    List<int> nodes = subscrGroup.GetElementByClass("subscription-enumeration");
                    subscrGroup.GetNode(nodes[0]).GetData().InnerString = "Таблица № " + TablesCount.ToString() + ". ";
                }
                mergeHTML(holderID, subscrGroup);
            }

            AddElement(HTMLElements.CreateVerticalIndent(2.5), holderID);

            HTMLTable table = new HTMLTable( headers);

            AddElement(table, holderID);

            AddElement(HTMLElements.CreateVerticalIndent(2.5), parentID);

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

            List<int> ids = content.GetElementByTag("ul");

            HTMLContentListID = ids[0]; 

            mergeHTML(parentID, content);

            return content;
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
            TagElementsGroup container;

            TagElementsGroup Graphic;

            if (GetElement(parent).GetParam("class").Equals("grid-holder"))
            {
                double rowCapacity = double.Parse(GetElementParam(parent, "#row-capacity"));//#row-capacity

                int characterMarker = int.Parse(GetElementParam(parent, "#char"));

                AddElementParam(parent, "#char", (characterMarker + 1).ToString());//.ToCharArray()[0];

                int containerID = GetElementByClass(parent, "container")[0];

                int div = AddElement (HTMLElements.CreateDIV(), containerID);

                AddElementParam(div, "class", "graphic-holder");

                Graphic = SVGElements.CreateSVGGraphicXY(w / rowCapacity, h);
                
                mergeHTML(div, Graphic);

                int span = AddElement(HTMLElements.CreateSPAN(), div);

                AddElementParam(span, "class", "subscription-enumeration");

                GetElement(span).InnerString = "("+ Convert.ToChar(characterMarker) + ")";

                GetElement(span).AddParam("class", "grid-letter-pointer");

                updateStyle(StyleSelectorType.Class, "grid-letter-pointer", "font-weight : bold;");

                updateStyle(StyleSelectorType.Class, "graphic-holder", "width : " + (100 / rowCapacity).ToString().Replace(',', '.')+ "%; float : left;");
                


                int elemID =  GetElementByClass(parent, "subscription-enumeration")[0];

                GetElement(elemID).InnerString = "Pic. № " + ImagesCount;

                elemID = GetElementByClass(parent,"subscription-text")[0];

                GetElement(elemID).InnerString = "SomeText";

                LastGraphic = Graphic;

                return Graphic;
            }


            container = new TagElementsGroup(HTMLElements.CreateCenterAlign());

            Graphic = SVGElements.CreateSVGGraphicXY(w, h);

            container.MergeGroups(Graphic);

            mergeHTML(parent, container);
                        
            TagElementsGroup  ss = HTMLElements.CreateSubscription("Picture");

            int ss_num = ss.GetElementByClass("subscription-enumeration")[0];

            ImagesCount++;

            ss.GetElement(ss_num).InnerString = "Pic. № "+ ImagesCount+". ";

            mergeHTML(container.RootID, ss);

            LastGraphic = Graphic;

            return Graphic;
        }

        public TagElementsGroup appendGraphic(double[] x, double[] y, string legend = "", string color = "")
        {
            if (LastGraphic == null)
            {
                return addGraphic(HTMLBodyID, 100, 100);
            }

            SVGElements.AppendSVGGraphic(ref LastGraphic, x, y,legend,color);

            mergeHTML(LastGraphic.GetNode(LastGraphic.RootID).GetParentID(), LastGraphic);

            return LastGraphic;
        }
        
        public TagElementsGroup appendGraphic(double[] x, double[] y, double[] z)
        {
            if (LastGraphic == null)
            {
                return addGraphic(HTMLBodyID, 100, 100);
            }

            SVGElements.AppendSVGGraphic(ref LastGraphic, x, y, z);

            mergeHTML(LastGraphic.GetNode(LastGraphic.RootID).GetParentID(), LastGraphic);

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

            int contentRecord = AddNode(HTMLElements.Create("<li>"), HTMLContentListID);

            AddElementParam(contentRecord, "class", "content-record");

            int contentRef = AddNode(HTMLElements.Create("<a>"), contentRecord);

            AddElementParam(contentRef, "href", "#" + GetNode(nodeID).GetData().GetParam("id"));

            GetNode(contentRef).GetData().InnerString = GetNode(nodeID).GetData().GetParam("#title");
        }

        private void mergeStyle(ITagElement element)
        {

            if (element.HasParam(".style"))
            {
                updateStyle(StyleSelectorType.Class, element.GetParam("class"), element.GetParam(".style"));

                element.RemoveParam(".style");

                return;
            }

            if (element.HasParam("#style"))
            {
                updateStyle(StyleSelectorType.ID, element.GetParam("id"), element.GetParam("#style"));

                element.RemoveParam("#style");

                return;
            }

            if (element.HasParam("..style"))
            {
                string[] cssCode = element.GetParam("..style").Split('+');

                for (int i = 0; i < cssCode.Length; i += 2)
                {
                    updateStyle(StyleSelectorType.Class, cssCode[i], cssCode[i + 1]);
                }

                element.RemoveParam("..style");

                return;
            }

            if (element.HasParam("##style"))
            {
                string[] cssCode = element.GetParam("##style").Split('+');

                for (int i = 0; i < cssCode.Length; i += 2)
                {
                    updateStyle(StyleSelectorType.ID, cssCode[i], cssCode[i + 1]);
                }

                element.RemoveParam("##style");

                return;
            }

        }

        public  void mergeHTML(int nodeID, TagElementsGroup list)
        {
            if (!ContainsNode(nodeID))
            {
                nodeID = RootID;
            }
            foreach (int key in list.GetNodeKeys())
            {
                AddNodeDirect(list.GetNode(key));

                mergeStyle(list.GetNode(key).GetData());

            }
            GetNode(nodeID).AddChild(list.RootID);
        }

        public new int AddElement(ITagElement element)
        {
            return AddElement(element, HTMLBodyID);
        }

        public new int AddElement(ITagElement element, int parentID)
        {
            mergeStyle(element);

            return base.AddElement(element, parentID);
        }

        public new string BuildCode()
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

            GetNode(HTMLStyleID).GetData().InnerString = stylesStr.ToString();

            Code = "<!DOCTYPE html>\n" + base.BuildCode();

            DateTime end = DateTime.Now;

            Console.WriteLine("Time taken for HTML build: {0}", end - start);

            return Code;
        }

        public HTMLPage() : base(HTMLElements.CreateHTML())
        {
            HTMLContentListID = -1;
            styles = new Dictionary<int, style>();
            HTMLPageID = RootID;
            HTMLHeadID = AddElement(HTMLElements.CreateHead());
            AddElement(HTMLElements.CreateMeta(),  HTMLHeadID);
            HTMLStyleID = AddElement(HTMLElements.CreateStyle(), HTMLHeadID);
            AddElement(HTMLElements.CreateBody());
            TagElementsGroup pageContainer = HTMLElements.CreatePageContainer();
            mergeHTML(HTMLBodyID,pageContainer);
            HTMLBodyID = pageContainer.LastID;
       }
    
    }
}
