using HTMLCodeBuilder.Nodes;
using System.Collections.Generic;
using System.Text;

namespace HTMLCodeBuilder.TaggedElements
{
    public abstract class TagElement : ICopy<ITagElement>, ITagElement
    {
        protected Dictionary<string, string> elementSettings;

        protected StringBuilder code;

        public string Tag { get; protected set; }

        public string OpenTag { get; protected set; }

        public string CloseTag { get; protected set; }

        public string InnerString { get; set; }

        protected int TabLevel = 0;

        protected bool autoCloseTag = false;

        protected string elementTabLevel;

        protected string GetTab(int level)
        {
            return new string('\t', level);
        }

        public void AddParam(string param, string val)
        {
            if (elementSettings.ContainsKey(param))
            {
                elementSettings[param] = val;
                return;
            }
            elementSettings.Add(param, val);
        }

        public abstract string ExpandOpenTag(int tab);

        public abstract string ExpandCloseTag(int tab);

        public string GetParam(string key)
        {
            if (elementSettings.ContainsKey(key))
            {
                return elementSettings[key];
            }
            return "";
        }

        public void RemoveParam(string key)
        {
            if (HasParam(key))
            {
                elementSettings.Remove(key);
            }
        }

        public bool HasParam(string key)
        {
            return elementSettings.ContainsKey(key);
        }

        public bool HasParamVal(string key, string val)
        {
            if (!HasParam(key))
            {
                return false;
            }

            return elementSettings[key].Equals(val);
        }

        public TagElement(string openTag, string closeTag)
        {
            elementSettings = new Dictionary<string, string>();

            OpenTag = openTag;

            CloseTag = closeTag;

            InnerString = "";

            code = new StringBuilder();

        }

        protected TagElement()
        {
            code = new StringBuilder();
        }

        public bool Equals(ITagElement element)
        {
            if (!element.Tag.Equals(Tag))
            {
                return false;
            }
            if (!element.OpenTag.Equals(OpenTag))
            {
                return false;
            }
            if (!element.CloseTag.Equals(CloseTag))
            {
                return false;
            }
            if (!element.CloseTag.Equals(CloseTag))
            {
                return false;
            }
            if (!element.InnerString.Equals(InnerString))
            {
                return false;
            }

            return true;
        }

        public abstract ITagElement Copy();

    }
}
