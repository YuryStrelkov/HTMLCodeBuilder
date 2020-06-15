using System.Collections.Generic;

namespace HTMLCodeBuilder.TaggedElements
{
    public interface ITagElementsGroup
    {
        Dictionary<string, Dictionary<int, int>> ClassVsID { get; }

        string Code { get; }

        Dictionary<string, Dictionary<int, int>> TagVsID { get; }

        void AddElementParam(int elemID, string paramKey, string paramVal);

        int AddElement(ITagElement element);

        int AddElement(ITagElement element, int parentID);

        ITagElement GetElement(int id);

        List<int> GetElementByClass(string className);
        
        List<int> GetElementByTag(string tag);

        string GetElementParam(int elemID, string paramKey);

        void MergeLists(ITagElementsGroup list);

        void buildCode();
        
        ITagElementsGroup getSubList(int nodeID);

        ITagElementsGroup getSubListCopy(int nodeID);
    }
}