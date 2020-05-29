using System.Collections.Generic;

namespace HTMLCodeBuilder.Nodes
{
    public class NodeList <T,NodeProcessResult>  where T:ICopy<T>
    {
        public Dictionary<int, Node<T>> Nodes { get; protected set;}

        public int LastID { get; protected set; }

        public int RootID { get; protected set; }

        public bool TryToGetNode(out Node<T> n, int nodeID)
        {
            if (ContainsNode(nodeID))
            {
                n= Nodes[nodeID];

                return true;
            }

            n = new Node<T>();

            return false;
        }

        public bool HasChildrens(int node)
        {
            return Nodes[node].getChildrenIDs().Length != 0;
        }

        public void RemoveChildrens(int node)
        {
            foreach (int i in Nodes[node].getChildrenIDs())
            {
                RemoveNode(i);
            }
        }

        public Node<T> GetNode(int nodeID)
        {
            if (Nodes.ContainsKey(nodeID))
            {
                return Nodes[nodeID];
            }
            return Nodes[RootID];
        }

        public T GetNodeData(int nodeID)
        {
            return GetNode(nodeID).getData();
        }

        public Dictionary<int, Node<T>>.KeyCollection GetNodeKeys()
        {
            return Nodes.Keys;
        }

        protected void AddNodeDirect(Node<T>n)
        {
            if (!ContainsNode(n.getID()))
            {
                Nodes.Add(n.getID(), n);
            }
        }

        public int AddNode(T data)
        {
            return AddNode(data, RootID);
        }

        protected int AddNode(Node<T> node)
        {
            return AddNode(node, RootID);
        }

        public int AddNode(T data, int parentID)
        {
    
            Node<T> n = new Node<T>(data);

            return AddNode(n, parentID);
        }

        protected int AddNode( Node<T> node, int parentID)
        {
            if (ContainsNode(parentID))
            {
                node.setParentID(parentID);
                Nodes.Add(node.getID(), node);
                Nodes[parentID].addChild(node);
                LastID = node.getID();
                return LastID;
            }
            node.setParentID(RootID);
            Nodes.Add(node.getID(), node);
            Nodes[RootID].addChild(node);
            LastID = node.getID();
            return LastID;
        }

        protected bool ContainsNode(int key)
        {
            return Nodes.ContainsKey(key);
        }

        protected void MergeNodeLists(int nodeID, NodeList<T, NodeProcessResult> list )
        {
            if (!Nodes.ContainsKey(nodeID))
            {
                nodeID = RootID;
            }

            GetNode(nodeID).addChild(list.GetNode(list.RootID));

            foreach (int key in list.GetNodeKeys())
            {
                Nodes.Add(key, list.GetNode(key));
            }
        }

        protected void RemoveNode(int nodeID )
        {
            if (!Nodes.ContainsKey(nodeID))
            {
                return;
            }
            foreach (int key in Nodes[nodeID].getChildren().Keys)
            {
                RemoveNode(key);
            }
            Nodes.Remove(nodeID);
        }
        /// <summary>
        /// Возвращает ссылку на поддерево
        /// </summary>
        /// <param name="nodeID"></param>
        /// <returns></returns>
        protected NodeList<T, NodeProcessResult> GetSubList(int nodeID)
        {
            if (!Nodes.ContainsKey(nodeID))
            {
                return null;
            }

            NodeList<T, NodeProcessResult> result = new NodeList<T, NodeProcessResult>(Nodes[nodeID]);

            SubListsReference(ref result, nodeID);

            return result;
        }

        private void SubListsReference(ref NodeList<T, NodeProcessResult> list, int nodeID)
        {
            foreach (int key in Nodes[nodeID].getChildren().Keys)
            {
                list.AddNodeDirect(Nodes[key]);
                SubListsReference(ref list, key);
            }
        }
        /// <summary>
        /// Возвращает копию поддерева
        /// </summary>
        /// <param name="nodeID"></param>
        /// <returns></returns>
        protected NodeList<T, NodeProcessResult> GetSubListCopy(int nodeID)
        {
            if (!Nodes.ContainsKey(nodeID))
            {
                return null;
            }
            NodeList<T, NodeProcessResult> result = new NodeList<T, NodeProcessResult>(Nodes[nodeID].getData().Copy());
            SubListsCopy(ref result, nodeID, result.RootID);
            return result;
        }

        protected void SubListsCopy(ref NodeList<T, NodeProcessResult> list, int nodeID, int newParentID)
        {
            foreach (int key in Nodes[nodeID].getChildrenIDs())
            {
                Node<T> newNode = Nodes[key].Copy();

                list.GetNode(newParentID).addChild(newNode);

                list.AddNodeDirect(newNode);

                SubListsCopy(ref list, key, newNode.getID());
            }
        }

        protected void ProcessNodes(INodeProcess<T, NodeProcessResult> process)
        {
            int level = 0;

            ProcessNodes(level, Nodes[RootID], process);

        }

        private void ProcessNodes(int level,Node<T> node, INodeProcess<T, NodeProcessResult> process)
        {
             process.onStart(level, node);

            if (node.getChildren().Count == 0)
            {
              process.onEnd(level, node);
              return;
            }

            foreach (int nodeKey in node.getChildren().Keys)
            {
                ProcessNodes(level + 1, Nodes[nodeKey], process);
            }

            process.onEnd(level, node);

        }
        
        private NodeList(Node<T> n)
        {
            Nodes = new Dictionary<int, Node<T>>();

            RootID = n.getID();

            Nodes.Add(n.getID(), n);

            LastID = RootID;
        }

        protected NodeList()
        {
            LastID = -1;
            RootID = -1;
            Nodes = new Dictionary<int, Node<T>>();
        }
        
        public NodeList(T data)
        {
            Nodes = new Dictionary<int, Node<T>>();

            Node<T> n = new Node<T>(data);

            RootID = n.getID();

            LastID = AddNode(n, RootID); 
       }

    }
}
