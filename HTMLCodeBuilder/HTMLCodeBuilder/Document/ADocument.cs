using HTMLCodeBuilder.TaggedElements;
using System;
using System.Collections.Generic;
using System.Text;

namespace HTMLCodeBuilder.Document
{
    public enum StyleSelectorType
    {
        Tag = 0,
        Class = 1,
        ID = 2
    }

    public interface IStyle
    {
        string SelectorStartTab();

        string SelectorEndTab();

        string OpenSelector();

        string CloseSelector();

        string Selector();

        string Code();
    }

    public interface IStyle<T>
    {
        T SelectorStartTab();

        T SelectorEndTab();

        T OpenSelector();

        T CloseSelector();

        T Selector();

        T Code();
    }

    public abstract class ADocument : TagElementsGroup
    {
        public int DocumentID { get; protected set; }

        public int BodyID{ get; protected set;}

        public int HeadID { get; protected set; }

        public int StyleID { get; protected set; }

        public int ContentID { get; protected set; }

        protected int TablesCount = 0;

        protected int TextBlockCount = 0;

        protected int ImagesCount = 0;

        protected int LastTableID = -1;

        protected int LastTextBlockID = -1;

        protected int LastImageID = -1;
        
        protected Dictionary<int, IStyle> Styles;

        public int AddTextBlock(string blockName, string text, bool add2content)
        {
            return AddTextBlock(BodyID, blockName, text, add2content);
        }

        public abstract void UpdateStyle(StyleSelectorType type, string selectorName, string code);

        public abstract int AddTable(int parentID, string subscr, bool enumirate, string[] headers);

        public abstract int AddTextBlock(int parentID, string blockName, string text, bool add2content);

        public abstract int AddContent(int parentID, string contentTitle);

        public void GraphicTitle(string t)
        {
            GraphicTitle(LastImageID, t);
        }

        public abstract void GraphicTitle(int GraphicID, string t);

        public void GraphicXLabel(string x)
        {
            GraphicXLabel(LastImageID, x);
        }

        public abstract void GraphicXLabel(int GraphicID, string t);

        public void GraphicYLabel(string y)
        {
            GraphicYLabel(LastImageID, y);
        }

        public abstract void GraphicYLabel(int GraphicID, string t);

        public abstract int AddGraphic(int parent, double w, double h);

        public abstract int AppendGraphic(double[] x, double[] y, string legend = "", string color = "");

        public abstract int AppendGraphic(double[] x, double[] y, double[] z);

        public abstract void MergeStyle(ITagElement element);
     
        public void Merege(int nodeID, TagElementsGroup list)
        {
            if (!ContainsNode(nodeID))
            {
                nodeID = RootID;
            }
            foreach (int key in list.GetNodeKeys())
            {
                AddNodeDirect(list.GetNode(key));

                MergeStyle(list.GetNode(key).GetData());

            }
            GetNode(nodeID).AddChild(list.RootID);
        }

        public new int AddElement(ITagElement element)
        {
            return AddElement(element, BodyID);
        }

        public new int AddElement(ITagElement element, int parentID)
        {
            MergeStyle(element);

            return base.AddElement(element, parentID);
        }
        
        public new string BuildCode()
        {
            DateTime start = DateTime.Now;

            StringBuilder stylesStr = new StringBuilder();

            stylesStr.Append("\n");

            IStyle style;

            int keyID = -1;

            foreach (int key in Styles.Keys)
            {
                keyID = key;
                style = Styles[key];
                stylesStr.Append(style.SelectorStartTab());
                stylesStr.Append(style.Selector());
                stylesStr.Append(style.OpenSelector());
                stylesStr.Append(style.Code());
                stylesStr.Append(style.CloseSelector());
            }

            stylesStr.Append(Styles[keyID].SelectorEndTab());

            GetNode(StyleID).GetData().InnerString = stylesStr.ToString();

            Code = base.BuildCode();

            DateTime end = DateTime.Now;

            Console.WriteLine("Time taken for HTML build: {0}", end - start);

            return Code;
        }

        public ADocument(ITagElement data) : base(data)
        {
            Styles = new Dictionary<int, IStyle>();

            DocumentID = RootID;
        }

    }
}
