using HTMLCodeBuilder.HTMLelements;
using HTMLCodeBuilder.Nodes;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HTMLCodeBuilder.TaggedElements
{
    public class TagElementsGroup : NodeList<ITagElement, string>
        {
        /// <summary>
        /// Список всех ID группы рассортированных по классам
        /// </summary>
        public Dictionary<string, Dictionary<int,int>> ClassVsID { get; private set; }
        /// <summary>
        /// Список всех ID группы рассортированных по тегам
        /// </summary>
        public Dictionary<string, Dictionary<int,int>> TagVsID { get; private set; }

        protected ITagElement LastElement;

        protected TagElementsGroup LastGroup;

        public string Code { get; protected set; }

        public bool HasTag(string tag)
        {
            return TagVsID.ContainsKey(tag);
        }

        public bool HasClass(string tag)
        {
            return ClassVsID.ContainsKey(tag);
        }

        public List<int> GetElementByTag(string tag)
        {
            if (!TagVsID.ContainsKey(tag))
            {
                return null;
            }
            return TagVsID[tag].Keys.ToList();
        }

        public List<int> GetElementByClass(string className)
        {
            if (!ClassVsID.ContainsKey(className))
            {
                return null;
            }
            return ClassVsID[className].Keys.ToList();
        }

        private void updateClassAndTags(Node<ITagElement> elementNode)
        {
            string param = elementNode.GetData().Tag;

            if (TagVsID.ContainsKey(param))
            {
                if (!TagVsID[param].ContainsKey(elementNode.GetID()))
                {
                    TagVsID[param].Add(elementNode.GetID(), elementNode.GetID());
                }
            }
            else
            {
                TagVsID.Add(param, new Dictionary<int, int>());

                TagVsID[param].Add(elementNode.GetID(), elementNode.GetID());
            }
            
            param = elementNode.GetData().GetParam("class");

            if (ClassVsID.ContainsKey(param))
            {
                if (!ClassVsID[param].ContainsKey(elementNode.GetID()))
                {
                    ClassVsID[param].Add(elementNode.GetID(), elementNode.GetID());
                }
            }
            else
            {
                ClassVsID.Add(param, new Dictionary<int, int>());

                ClassVsID[param].Add(elementNode.GetID(), elementNode.GetID());
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

            updateClassAndTags(GetNode(id));

            LastElement = element;

            return id;
        }

        public void AddElementParam(int elemID, string paramKey, string paramVal)
        {
            GetNode(elemID).GetData().AddParam(paramKey, paramVal);

            if (paramKey.Equals("class"))
            {
                updateClassAndTags(GetNode(elemID));
            }
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

            Code = process.getProcessResult();

            return Code;
        }

        public string BuildCode()
        {
            PageBuildProcess process = new PageBuildProcess();

            ProcessNodes(process);

            Code = process.getProcessResult();

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

            LastGroup = list;

            foreach (int key in list.GetNodeKeys())
            {
                AddNodeDirect(list.GetNode(key));
                updateClassAndTags(list.GetNode(key));
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

            ClassVsID = new Dictionary<string, Dictionary<int, int>>();

            TagVsID = new Dictionary<string, Dictionary<int, int>>();

            Parallel.ForEach(Nodes,(node)=> { updateClassAndTags(node.Value); });

            Code = ""; 
        }

        public TagElementsGroup(ITagElement data) : base()
        { 
            ClassVsID = new Dictionary<string, Dictionary<int, int>>();

            TagVsID = new Dictionary<string, Dictionary<int, int>>();

            Node<ITagElement> node = new Node<ITagElement>(data);

            RootID = node.GetID();

            LastID = AddNode(node);

            updateClassAndTags(node);
                  
            Code = "";
             
        }
    }
}
