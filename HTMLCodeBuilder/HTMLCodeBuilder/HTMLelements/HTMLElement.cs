using HTMLCodeBuilder.TaggedElements;
using System.Collections.Generic;

namespace HTMLCodeBuilder.HTMLelements
{
    public class HTMLElement : TagElement
    {
        public override string ExpandOpenTag(int tab)
        {
            TabLevel = tab;

            code.Append(GetTab(TabLevel));

            code.Append(OpenTag);

            foreach (string key in elementSettings.Keys)
            {
                if (key.StartsWith("#")||key.StartsWith("."))
                {
                    continue;
                }
                code.Append(" ");
                code.Append(key);
                code.Append(" = ");
                code.Append('"');
                code.Append(elementSettings[key]);
                code.Append('"');
            }

            if (InnerString.Length != 0)
            {
                code.Append(">");
                return code.ToString();
            }

            if (autoCloseTag)
            {
                return code.ToString();
            }

            code.Append(">");

            return code.ToString();
        }

        public override string ExpandCloseTag(int tab)
        {
            return GetTab(tab) + InnerString + CloseTag;
        }

        public HTMLElement(string openTag, string closeTag):base(openTag, closeTag)
        {
            AddParam("class", openTag.Substring(1, openTag.Length-2));

            OpenTag = openTag.Remove(openTag.Length - 1, 1); ;
 
            if (closeTag.Length == 0)
            {
                CloseTag = "/>";
                autoCloseTag = true;
            }

            Tag = OpenTag.Substring(1, OpenTag.Length - 1);
        }

        protected HTMLElement():base()
        {
        }

        public override string ToString()
        {
            return OpenTag+">" + " " + InnerString + " " + CloseTag; 
        }

        public override ITagElement Copy()
        {
            HTMLElement element = new HTMLElement();
            element.elementSettings = new Dictionary<string, string>(elementSettings);
            element.Tag = string.Copy(Tag);
            element.OpenTag = string.Copy(OpenTag);
            element.CloseTag = string.Copy(CloseTag);
            element.InnerString = string.Copy(InnerString);
            element.autoCloseTag = autoCloseTag;
            return element;
        }
    }
}
