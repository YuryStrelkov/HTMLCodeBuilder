using HTMLCodeBuilder.HTMLelements;
using HTMLCodeBuilder.Nodes;
using System;
using System.Collections.Generic;

namespace HTMLCodeBuilder.TaggedElements
{
    public class TagElementsGroup : NodeList<ITagElement, string>
    {
        public string Code { get; protected set; }

        public bool HasTag(string tag)
        {
            return HasTag(RootID, tag);
        }

        public bool HasTag(int id, string tag)
        {
            if (GetNode(id).GetData().Tag.Equals(tag))
            {
                return true;
            }

            foreach (int ids in GetNode(id).GetChildren().Keys)
            {
                if (HasTag(ids, tag))
                {
                    return true;
                };
            }
            return false;
        }

        public bool HasClass(string Class)
        {
            return HasClass(RootID, Class);
        }

        public bool HasClass(int id, string Class)
        {
            if (GetNode(id).GetData().HasParam("class"))
            {
                if (GetNode(id).GetData().GetParam("class").Equals(Class))
                {
                return true;
                }
            }

            foreach (int ids in GetNode(id).GetChildren().Keys)
            {
                if (HasClass(ids, Class))
                {
                    return true;
                };
            }
            return false;
        }

        public bool HasID(string ID)
        {
            return HasID(RootID, ID);
        }

        public bool HasID(int id, string ID)
        {
            if (GetNode(id).GetData().HasParam("id"))
            {
                if (GetNode(id).GetData().GetParam("id").Equals(ID))
                {
                    return true;
                }
            }

            foreach (int ids in GetNode(id).GetChildren().Keys)
            {
                if (HasClass(ids, ID))
                {
                    return true;
                };
            }
            return false;
        }

        public List<int> GetElementByID(string ID)
        {
            return GetElementByTag(RootID, ID);
        }

        public List<int> GetElementByID(int id, string ID)
        {
            List<int> elements = new List<int>();

            GetElementByIdRecursive(ref elements, id, ID);

            return elements;
        }

        public List<int> GetElementByTag(string tag)
        {
            return GetElementByTag(RootID, tag);
        }

        public List<int> GetElementByTag(int id, string tag)
        {
            List<int> elements = new List<int>();

            GetElementByTagRecursive(ref elements, id, tag);

            return elements;
        }

        public List<int> GetElementByClass(string className)
        {
            return  GetElementByClass(RootID, className);
        }

        public List<int> GetElementByClass(int id, string className)
        {
            List<int> elements = new List<int>();

            GetElementByClassRecursive(ref elements, id, className);

            return elements;
        }

        private void GetElementByClassRecursive(ref List<int> elements, int node, string className)
        {
            if (GetElement(node).HasParam("class"))
            {
                if (GetElement(node).GetParam("class").Equals(className))
                {
                    elements.Add(node);
                }
            }

            foreach (int id in GetNode(node).GetChildren().Keys)
            {
                GetElementByClassRecursive(ref elements, id, className);
            }

        }

        private void GetElementByTagRecursive(ref List<int> elements, int node, string tagName)
        {
                         
                if (GetElement(node).Tag.Equals(tagName))
                {
                    elements.Add(node);
                }

            foreach (int id in GetNode(node).GetChildren().Keys)
            {
                GetElementByTagRecursive(ref elements, id, tagName);
            }
        }

        private void GetElementByIdRecursive(ref List<int> elements, int node, string className)
        {
            if (GetElement(node).HasParam("id"))
            {
                if (GetElement(node).GetParam("id").Equals(className))
                {
                    elements.Add(node);
                }
            }

            foreach (int id in GetNode(node).GetChildren().Keys)
            {
                GetElementByIdRecursive(ref elements, id, className);
            }

        }

        public ITagElement GetElement(int id)
        {
            if (ContainsNode(id))
            {
                return GetNode(id).GetData();
            }
            return GetNode(RootID).GetData();
        }

        public int AddElement(ITagElement element)
        {
            return AddElement(element, RootID);
        }

        public int AddElement(ITagElement element, int parentID)
        {
            int id = AddNode(element, parentID);

            return id;
        }

        public void AddElementParam(int elemID, string paramKey, string paramVal)
        {
            GetNode(elemID).GetData().AddParam(paramKey, paramVal);
        }

        public string GetElementParam(int elemID, string paramKey)
        {
            return GetNode(elemID).GetData().GetParam(paramKey);
        }
 
        public void saveCode(string path)
        {

            if (Code.Length == 0)
            {
                return;
            }
            System.IO.File.WriteAllText(path, Code);
        }

        public string BuildCode(int tab)
        {
            PageBuildProcess process = new PageBuildProcess(tab);

            ProcessNodes(process);

            Code = process.GetProcessResult();

            return Code;
        }

        public string BuildCode()
        {
            PageBuildProcess process = new PageBuildProcess();

            ProcessNodes(process);

            Code = process.GetProcessResult();

            return Code;
        }

        public void MergeGroups(TagElementsGroup list)
        {
            MergeGroups(RootID, list);
        }

        public void MergeGroups(int nodeID, TagElementsGroup list)
        {
            if (!ContainsNode(nodeID))
            {
                nodeID = RootID;
            }
            foreach (int key in list.GetNodeKeys())
            {
                AddNodeDirect(list.GetNode(key));
            }
            GetNode(nodeID).AddChild(GetNode(list.RootID));
        }
     
        public new TagElementsGroup GetSubListCopy(int nodeID)
        {
            TagElementsGroup group = new TagElementsGroup(base.GetSubListCopy(nodeID));

            return group;
        }

        public new TagElementsGroup GetSubList(int nodeID)
        {

            TagElementsGroup group = new TagElementsGroup(base.GetSubList(nodeID));

            return group;
        }

        private TagElementsGroup(NodeList<ITagElement, string> gr) :base()
        {  
            Nodes = gr.Nodes;

            RootID = gr.RootID;

            Code = ""; 
        }

        public TagElementsGroup(ITagElement data) : base()
        { 

            Node<ITagElement> node = new Node<ITagElement>(data);

            RootID = node.GetID();

            LastID = AddNode(node);
                  
            Code = "";
             
        }

        public new void Dispose()
        {
            Code = null;
            base.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
