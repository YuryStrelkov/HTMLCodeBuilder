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

        protected bool autoCloseTag = false;

        protected string elementTabLevel;

        public void appendParam(string param, string val)
        {
            if (elementSettings.ContainsKey(param))
            {
                elementSettings[param] = val;
                return;
            }
            elementSettings.Add(param, val);
        }

        public abstract string expandElementOpenTag();

        public abstract string expandElementCloseTag();

        public string getParam(string key)
        {
            if (elementSettings.ContainsKey(key))
            {
                return elementSettings[key];
            }
            return "";
        }

        public void remParam(string key)
        {
            if (hasParam(key))
            {
                elementSettings.Remove(key);
            }
        }

        public bool hasParam(string key)
        {
            return elementSettings.ContainsKey(key);
        }

        public bool hasParamVal(string key, string val)
        {
            if (!hasParam(key))
            {
                return false;
            }

            return elementSettings[key].Equals(val);
        }

        public TagElement(string openTag, string closeTag)
        {
            elementSettings = new Dictionary<string, string>();

            OpenTag = OpenTag;

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
