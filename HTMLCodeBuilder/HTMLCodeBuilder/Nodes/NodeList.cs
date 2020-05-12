using System.Collections.Generic;

namespace HTMLCodeBuilder.Nodes
{
    public class NodeList <T,NodeProcessResult>  where T:ICopy<T>
    {
        public Dictionary<int, Node<T>> Nodes { get; protected set;}

        public int lastID { get; protected set; }

        public int RootID { get; protected set; }

        public bool tryToGetNode(out Node<T> n, int nodeID)
        {
            if (containsNode(nodeID))
            {
                n= Nodes[nodeID];

                return true;
            }

            n = null;

            return false;
        }

        public bool hasChildrens(int node)
        {
            return Nodes[node].getChildrenIDs().Length != 0;
        }

        public void removeChildrens(int node)
        {
            foreach (int i in Nodes[node].getChildrenIDs())
            {
                removeNode(i);
            }
        }

        public Node<T> getNode(int nodeID)
        {
            if (Nodes.ContainsKey(nodeID))
            {
                return Nodes[nodeID];
            }
            return Nodes[RootID];
        }

        public T getNodeData(int nodeID)
        {
            return getNode(nodeID).getData();
        }

        public Dictionary<int, Node<T>>.KeyCollection getNodeKeys()
        {
            return Nodes.Keys;
        }

        protected void addNodeDirect(Node<T>n)
        {
            if (!containsNode(n.getID()))
            {
                Nodes.Add(n.getID(), n);
            }
        }

        public int addNode(T data)
        {
            return addNode(data, RootID);
        }

        protected int addNode(Node<T> node)
        {
            return addNode(node, RootID);
        }

        public int addNode(T data, int parentID)
        {
    
            Node<T> n = new Node<T>(data);

            return addNode(n, parentID);
        }

        protected int addNode( Node<T> node, int parentID)
        {
            if (containsNode(parentID))
            {
                node.setParentID(parentID);
                Nodes.Add(node.getID(), node);
                Nodes[parentID].addChild(node);
                lastID = node.getID();
                return lastID;
            }
            node.setParentID(RootID);
            Nodes.Add(node.getID(), node);
            Nodes[RootID].addChild(node);
            lastID = node.getID();
            return lastID;
        }

        protected bool containsNode(int key)
        {
            return Nodes.ContainsKey(key);
        }

        protected void mergeNodeLists(int nodeID, NodeList<T, NodeProcessResult> list )
        {
            if (!Nodes.ContainsKey(nodeID))
            {
                nodeID = RootID;
            }

            getNode(nodeID).addChild(list.getNode(list.RootID));

            foreach (int key in list.getNodeKeys())
            {
                Nodes.Add(key, list.getNode(key));
            }
        }

        protected void removeNode(int nodeID )
        {
            if (!Nodes.ContainsKey(nodeID))
            {
                return;
            }
            foreach (int key in Nodes[nodeID].getChildren().Keys)
            {
                removeNode(key);
            }
            Nodes.Remove(nodeID);
        }
        /// <summary>
        /// Возвращает ссылку на поддерево
        /// </summary>
        /// <param name="nodeID"></param>
        /// <returns></returns>
        protected NodeList<T, NodeProcessResult> getSubList(int nodeID)
        {
            if (!Nodes.ContainsKey(nodeID))
            {
                return null;
            }

            NodeList<T, NodeProcessResult> result = new NodeList<T, NodeProcessResult>(Nodes[nodeID]);

            subListsReference(ref result, nodeID);

            return result;
        }

        private void subListsReference(ref NodeList<T, NodeProcessResult> list, int nodeID)
        {
            foreach (int key in Nodes[nodeID].getChildren().Keys)
            {
                list.addNodeDirect(Nodes[key]);
                subListsReference(ref list, key);
            }
        }
        /// <summary>
        /// Возвращает копию поддерева
        /// </summary>
        /// <param name="nodeID"></param>
        /// <returns></returns>
        protected NodeList<T, NodeProcessResult> getSubListCopy(int nodeID)
        {
            if (!Nodes.ContainsKey(nodeID))
            {
                return null;
            }
            NodeList<T, NodeProcessResult> result = new NodeList<T, NodeProcessResult>(Nodes[nodeID].getData().Copy());
            subListsCopy(ref result, nodeID, result.RootID);
            return result;
        }

        protected void subListsCopy(ref NodeList<T, NodeProcessResult> list, int nodeID, int newParentID)
        {
            foreach (int key in Nodes[nodeID].getChildrenIDs())
            {
                Node<T> newNode = Nodes[key].Copy();

                list.getNode(newParentID).addChild(newNode);

                list.addNodeDirect(newNode);

                subListsCopy(ref list, key, newNode.getID());
            }
        }

        protected void processNodes(INodeProcess<T, NodeProcessResult> process)
        {
            int level = 0;

            processNodes(level, Nodes[RootID], process);

        }

        private void processNodes(int level,Node<T> node, INodeProcess<T, NodeProcessResult> process)
        {
             process.onStart(level, node);

            if (node.getChildren().Count == 0)
            {
              process.onEnd(level, node);
              return;
            }

            foreach (int nodeKey in node.getChildren().Keys)
            {
                processNodes(level + 1, Nodes[nodeKey], process);
            }

            process.onEnd(level, node);

        }
        
        private NodeList(Node<T> n)
        {
            Nodes = new Dictionary<int, Node<T>>();

            RootID = n.getID();

            Nodes.Add(n.getID(), n);

            lastID = RootID;
        }

        protected NodeList()
        {
            lastID = -1;
            RootID = -1;
            Nodes = new Dictionary<int, Node<T>>();
        }
        
        public NodeList(T data)
        {
            Nodes = new Dictionary<int, Node<T>>();

            Node<T> n = new Node<T>(data);

            RootID = n.getID();
     
            lastID = addNode(n, RootID); 
       }

    }
}
