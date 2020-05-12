using HTMLCodeBuilder.HTMLelements;
using HTMLCodeBuilder.Nodes;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

        public bool hasTag(string tag)
        {
            return TagVsID.ContainsKey(tag);
        }

        public bool hasClass(string tag)
        {
            return ClassVsID.ContainsKey(tag);
        }

        public List<int> getElementByTag(string tag)
        {
            if (!TagVsID.ContainsKey(tag))
            {
                return null;
            }
            return TagVsID[tag].Keys.ToList(); ;
        }

        public List<int> getElementByClass(string className)
        {
            if (!ClassVsID.ContainsKey(className))
            {
                return null;
            }
            return ClassVsID[className].Keys.ToList();
        }

        private void updateClassAndTags(Node<ITagElement> elementNode)
        {
            string param = elementNode.getData().Tag;

            if (TagVsID.ContainsKey(param))
            {
                if (!TagVsID[param].ContainsKey(elementNode.getID()))
                {
                    TagVsID[param].Add(elementNode.getID(), elementNode.getID());
                }
            }
            else
            {
                TagVsID.Add(param, new Dictionary<int, int>());

                TagVsID[param].Add(elementNode.getID(), elementNode.getID());
            }
            
            param = elementNode.getData().getParam("class");

            if (ClassVsID.ContainsKey(param))
            {
                if (!ClassVsID[param].ContainsKey(elementNode.getID()))
                {
                    ClassVsID[param].Add(elementNode.getID(), elementNode.getID());
                }
            }
            else
            {
                ClassVsID.Add(param, new Dictionary<int, int>());

                ClassVsID[param].Add(elementNode.getID(), elementNode.getID());
            }
        }

        public ITagElement getElement(int id)
        {
            if (containsNode(id))
            {
                return getNode(id).getData();
            }
            return getNode(RootID).getData();
        }

        public int addElement(ITagElement element)
        {
            return addElement(element, RootID);
        }

        public int addElement(ITagElement element, int parentID)
        {
            int id = addNode(element, parentID);

            updateClassAndTags(getNode(id));

            LastElement = element;

            return id;
        }

        public void addElementParam(int elemID, string paramKey, string paramVal)
        {
            getNode(elemID).getData().appendParam(paramKey, paramVal);

            if (paramKey.Equals("class"))
            {
                updateClassAndTags(getNode(elemID));
            }
        }

        public string getElementParam(int elemID, string paramKey)
        {
            return getNode(elemID).getData().getParam(paramKey);
        }
 
        public void saveCode(string path)
        {

            if (Code.Length == 0)
            {
                return;
            }
            System.IO.File.WriteAllText(path, Code);
        }

        public string buildCode()
        {
            PageBuildProcess process = new PageBuildProcess();
            
            processNodes(process);

            Code += process.getProcessResult();

            return Code;
        }

        public void mergeGroups(TagElementsGroup list)
        {
            mergeGroups(RootID, list);
        }

        public void mergeGroups(int nodeID, TagElementsGroup list)
        {
            if (!containsNode(nodeID))
            {
                nodeID = RootID;
            }

            LastGroup = list;

            foreach (int key in list.getNodeKeys())
            {
                addNodeDirect(list.getNode(key));
            }

            foreach (string key in list.ClassVsID.Keys)
            {
                if (ClassVsID.ContainsKey(key))
                {
                    foreach (int paramKey in list.ClassVsID[key].Keys)
                    {
                        if (ClassVsID[key].ContainsKey(paramKey))
                        {
                            continue;
                        }
                        ClassVsID[key].Add(paramKey, paramKey);
                    }
                    continue;
                }

                ClassVsID.Add(key, list.ClassVsID[key]);
            }


            foreach (string key in list.TagVsID.Keys)
            {
                if (TagVsID.ContainsKey(key))
                {
                    foreach (int paramKey in list.TagVsID[key].Keys)
                    {
                        if (TagVsID[key].ContainsKey(paramKey))
                        {
                            continue;
                        }
                        TagVsID[key].Add(paramKey, paramKey);
                    }
                    continue;
                }
                TagVsID.Add(key, list.TagVsID[key]);
            }
            getNode(nodeID).addChild(getNode(list.RootID));
        }
     
        public new TagElementsGroup getSubListCopy(int nodeID)
        {
            TagElementsGroup group = new TagElementsGroup(base.getSubListCopy(nodeID));
            group.ClassVsID = new Dictionary<string, Dictionary<int, int>>(ClassVsID);
            group.TagVsID = new Dictionary<string, Dictionary<int, int>>(TagVsID);
            return group;
        }

        public new TagElementsGroup getSubList(int nodeID)
        {
            TagElementsGroup group = new TagElementsGroup(base.getSubList(nodeID));
            group.ClassVsID = ClassVsID;
            group.TagVsID = TagVsID;
            return group;
        }

        private TagElementsGroup(NodeList<ITagElement, string> gr) :base()
        {  
            Nodes = gr.Nodes;
            RootID = gr.RootID;
            lastID = gr.lastID;
            Code = ""; 
        }

        public TagElementsGroup(ITagElement data) : base()
        { 
            ClassVsID = new Dictionary<string, Dictionary<int, int>>();

            TagVsID = new Dictionary<string, Dictionary<int, int>>();

            Node<ITagElement> node = new Node<ITagElement>(data);

            RootID = node.getID();

            lastID = addNode(node);

            updateClassAndTags(node);
                  
            Code = "";
             
        }
    }
}
