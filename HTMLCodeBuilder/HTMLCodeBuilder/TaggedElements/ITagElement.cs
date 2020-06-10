using HTMLCodeBuilder.Nodes;
using System;

namespace HTMLCodeBuilder.TaggedElements
{
    public interface ITagElement:ICopy<ITagElement>
    {
        string CloseTag { get; }

        string InnerString { get; set; }

        string OpenTag { get; }

        string Tag { get; }

        void AddParam(string param, string val);

        string ExpandCloseTag(int tab);

        string ExpandOpenTag(int tab);

        string GetParam(string key);

        bool HasParam(string key);

        bool HasParamVal(string key, string val);

        void RemoveParam(string key);
    }
}