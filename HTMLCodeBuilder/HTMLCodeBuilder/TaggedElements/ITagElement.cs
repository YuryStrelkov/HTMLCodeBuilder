using HTMLCodeBuilder.Nodes;

namespace HTMLCodeBuilder.TaggedElements
{
    public interface ITagElement:ICopy<ITagElement>
    {
        string CloseTag { get; }

        string InnerString { get; set; }

        string OpenTag { get; }

        string Tag { get; }

        void appendParam(string param, string val);

        string expandElementCloseTag();

        string expandElementOpenTag();

        string getParam(string key);

        bool hasParam(string key);

        bool hasParamVal(string key, string val);

        void remParam(string key);
    }
}