using System.Collections.Generic;

namespace HTMLCodeBuilder.TaggedElements
{
    public interface ITagElementsGroup
    {
        Dictionary<string, Dictionary<int, int>> ClassVsID { get; }

        string Code { get; }

        Dictionary<string, Dictionary<int, int>> TagVsID { get; }

        void addElementParam(int elemID, string paramKey, string paramVal);

        int addHTMLelement(ITagElement element);

        int addHTMLelement(ITagElement element, int parentID);

        ITagElement getElement(int id);

        List<int> getElementByClass(string className);
        
        List<int> getElementByTag(string tag);

        string getElementParam(int elemID, string paramKey);

        void mergeLists(ITagElementsGroup list);

        void buildCode();
        
        ITagElementsGroup getSubList(int nodeID);

        ITagElementsGroup getSubListCopy(int nodeID);
    }
}