using HTMLCodeBuilder.Document;
using HTMLCodeBuilder.SVGelements;
using HTMLCodeBuilder.TaggedElements;
using System;
using System.Collections.Generic;

namespace HTMLCodeBuilder.HTMLelements
{
    public struct HTMLStyle:IStyle
    {
        private string selector;

        private string code;

        public string Selector()
        {
            return selector;
        }

        public string Code()
        {
            return code;
        }

        public string SelectorStartTab()
        {
            return "\t\t\t";
        }

        public string SelectorEndTab()
        {
            return "\t\t";
        }

        public string OpenSelector()
        {
            return "{\n\t\t\t";
        }

        public string CloseSelector()
        {
            return "}\n";
        }

        public HTMLStyle(string selector_, string code_)
        {
            selector = selector_;

            code = code_;
        }
    }

    public class HTMLDocument : ADocument
    {
        public override void UpdateStyle(StyleSelectorType type, string selectorName, string code)
        {
            int codeID = (selectorName + code).GetHashCode();

            if (Styles.ContainsKey(codeID))
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

            Styles.Add(codeID, new HTMLStyle(selectorName, code));
        }

        public override int AddTable(int parentID, string subscr, bool enumirate, string[] headers)
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
                Merege(holderID, subscrGroup);
            }

            AddElement(HTMLElements.CreateVerticalIndent(2.5), holderID);

            HTMLTable table = new HTMLTable( headers);

           int tableID =  AddElement(table, holderID);

            AddElement(HTMLElements.CreateVerticalIndent(2.5), parentID);
            
            return tableID;
        }

        public override int  AddTextBlock(int parentID, string blockName, string text, bool add2content)
        {
            TagElementsGroup textBlock =  HTMLElements.CreateTextBlock(blockName, text);
            
            Merege(parentID, textBlock);

            if (add2content)
            {
                TextBlockCount += 1;

                UpdateContent(textBlock.RootID);
            }

            LastTextBlockID = textBlock.RootID;

            return textBlock.RootID;
        }

        public override int AddContent(int parentID, string contentTitle)
        {
            TagElementsGroup content = HTMLElements.CreatePageContent(contentTitle);

            List<int> ids = content.GetElementByTag("ul");

            ContentID = ids[0];

            Merege(parentID, content);

            return content.RootID;
        }   

        public override void GraphicTitle(int ID, string title)
        {
            List<int> ids = GetElementByClass(ID,"title");
            if (ids.Count == 0)
            {
                return;
            }
            GetElement(ids[0]).InnerString = title;
        }
    
        public override void GraphicXLabel(int ID, string x)
        {
            List<int> ids = GetElementByClass(ID, "x-label");
            if (ids.Count == 0)
            {
                return;
            }
            GetElement(ids[0]).InnerString = x;
        }
        
        public override void GraphicYLabel(int ID, string y)
        {
            List<int> ids = GetElementByClass(ID, "y-label");
            if (ids.Count == 0)
            {
                return;
            }
            GetElement(ids[0]).InnerString = y;
        }
 
        public override int AddGraphic(int parent, double w, double h)
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

                Merege(div, Graphic);

                int span = AddElement(HTMLElements.CreateSPAN(), div);

                AddElementParam(span, "class", "subscription-enumeration");

                GetElement(span).InnerString = "("+ Convert.ToChar(characterMarker) + ")";

                GetElement(span).AddParam("class", "grid-letter-pointer");

                UpdateStyle(StyleSelectorType.Class, "grid-letter-pointer", "font-weight : bold;");

                UpdateStyle(StyleSelectorType.Class, "graphic-holder", "width : " + (100 / rowCapacity).ToString().Replace(',', '.')+ "%; float : left;");

                int elemID =  GetElementByClass(parent, "subscription-enumeration")[0];

                GetElement(elemID).InnerString = "Pic. № " + ImagesCount;

                elemID = GetElementByClass(parent,"subscription-text")[0];

                GetElement(elemID).InnerString = "SomeText";

                LastImageID = Graphic.RootID;

                return Graphic.RootID;
            }
            container = new TagElementsGroup(HTMLElements.CreateCenterAlign());

            Graphic = SVGElements.CreateSVGGraphicXY(w, h);

            container.MergeGroups(Graphic);

            Merege(parent, container);
            
            TagElementsGroup  ss = HTMLElements.CreateSubscription("Picture",2.5,2.5);

            int ss_num = ss.GetElementByClass("subscription-enumeration")[0];

            ImagesCount++;

            ss.GetElement(ss_num).InnerString = "Pic. № "+ ImagesCount+". ";

            Merege(container.RootID, ss);

            LastImageID = Graphic.RootID;

            return Graphic.RootID;
        }

        public override int AppendGraphic(double[] x, double[] y, string legend = "", string color = "")
        {
            if (LastImageID == -1)
            {
                return AddGraphic(BodyID, 100, 100);
            }

            SVGElements.AppendSVGGraphic(this, LastImageID, x, y,legend,color);

            return LastImageID;
        }
        
        public override int AppendGraphic(double[] x, double[] y, double[] z)
        {
            if (LastImageID == -1)
            {
                return AddGraphic(BodyID, 100, 100);
            }

            SVGElements.AppendSVGGraphic(this, LastImageID, x, y, z);

            return LastImageID;
        }

        private void UpdateContent(int nodeID)
        {
            if(nodeID ==-1)
            {
                return;
            }
            if (ContentID == -1)
            {
                return;
            }

            int contentRecord = AddNode(HTMLElements.Create("<li>"), ContentID);

            AddElementParam(contentRecord, "class", "content-record");

            int contentRef = AddNode(HTMLElements.Create("<a>"), contentRecord);

            AddElementParam(contentRef, "href", "#" + GetNode(nodeID).GetData().GetParam("id"));

            GetNode(contentRef).GetData().InnerString = GetNode(nodeID).GetData().GetParam("#title");
        }

        public override void MergeStyle(ITagElement element)
        {
            if (element.HasParam(".style"))
            {
                UpdateStyle(StyleSelectorType.Class, element.GetParam("class"), element.GetParam(".style"));

                element.RemoveParam(".style");

                return;
            }
            if (element.HasParam("#style"))
            {
                UpdateStyle(StyleSelectorType.ID, element.GetParam("id"), element.GetParam("#style"));

                element.RemoveParam("#style");

                return;
            }
            if (element.HasParam("..style"))
            {
                string[] cssCode = element.GetParam("..style").Split('+');

                for (int i = 0; i < cssCode.Length; i += 2)
                {
                    UpdateStyle(StyleSelectorType.Class, cssCode[i], cssCode[i + 1]);
                }

                element.RemoveParam("..style");

                return;
            }
            if (element.HasParam("##style"))
            {
                string[] cssCode = element.GetParam("##style").Split('+');

                for (int i = 0; i < cssCode.Length; i += 2)
                {
                    UpdateStyle(StyleSelectorType.ID, cssCode[i], cssCode[i + 1]);
                }

                element.RemoveParam("##style");

                return;
            }
        }
 
        public new string BuildCode()
        {
            return "<!DOCTYPE html>\n" + base.BuildCode();
        }

        public HTMLDocument() : base(HTMLElements.CreateHTML())
        {
            HeadID = AddElement(HTMLElements.CreateHead());

            AddElement(HTMLElements.CreateMeta(), HeadID);

            StyleID = AddElement(HTMLElements.CreateStyle(), HeadID);

            AddElement(HTMLElements.CreateBody());

            TagElementsGroup pageContainer = HTMLElements.CreatePageContainer();

            Merege(BodyID,pageContainer);

            BodyID = pageContainer.LastID;
       }
    
    }
}
