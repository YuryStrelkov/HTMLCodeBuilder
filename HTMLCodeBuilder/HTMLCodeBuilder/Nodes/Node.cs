using System.Collections.Generic;
using System.Linq;

namespace HTMLCodeBuilder.Nodes
{
    public class Node<T> : ICopy<Node<T>> where T : ICopy<T>
    {
        /// <summary>
        /// Возвращает ID, который можно добавить сейчас, но не использует его
        /// </summary>
        /// <returns>ID</returns>
        public static int getAvailableID()
        {
            return instanceCounter.getInstanceIdAvailable();
        }
        /// <summary>
        /// Счётчик количества созданных объектов на данный момент
        /// </summary>
        private static InstanceCounter instanceCounter = new InstanceCounter();
        /// <summary>
        /// Хранимые данные
        /// </summary>
        private T data;
        /// <summary>
        /// Родительский ID
        /// </summary>
        private int parentId;
        /// <summary>
        /// Собственный ID
        /// </summary>
        private int Id;
        /// <summary>
        /// ID дочерних элементов
        /// </summary>
        private Dictionary<int, int> childrens;
        /// <summary>
        /// Метод доступа для обращения к хранимым данным
        /// </summary>
        /// <returns>хранимые данные</returns>
        public T getData()
        {
            return data;
        }
        /// <summary>
        /// Метод доступа для получения собственного ID
        /// </summary>
        /// <returns>собственный ID</returns>
        public int getID()
        {
            return Id;
        }
        /// <summary>
        /// Метод доступа для установки родительского нода
        /// </summary>
        /// <param name="n">новый родительский нод</param>
        public void setParentID(Node<T> n)
        {
            parentId = n.getID();
        }
        /// <summary>
        /// Метод доступа для установки родительского ID
        /// </summary>
        /// <param name="id">новый родительский ID</param>
        public void setParentID(int id)
        {
            parentId = id;
        }

        public int getParentID()
        {
            return parentId;
        }

        public int[] getChildrenIDs()
        {
            return childrens.Keys.ToArray();
        }

        public Dictionary<int, int> getChildren()
        {
            return childrens;
        }

        public void removeChild(int id)
        {
            if (!childrens.ContainsKey(id))
            {
                return;
            }
            childrens.Remove(id);
        }

        public void addChild(Node<T> child)
        {
            addChild(child.getID());
            child.parentId = getID();
        }

        public void addChild(int id)
        {
            // исключение зацикливания
            if (id == getID())
            {
                return;
            }

            if (childrens.ContainsKey(id))
            {
                return;
            }
            childrens.Add(id, id);
        }

        public Node<T> Copy()
        {
            return new Node<T>(this);
        }

        public bool Equals(Node<T> n)
        {
            if (n.parentId != parentId)
            {
                return false;
            }
          
            if (!n.data.Equals(data))
            {
                return false;
            }

            if(!n.childrens.Equals(childrens))
            {
                return false;
            }
            return true;
        }

        private Node(Node<T> n)
        {
            data = n.data.Copy();

            parentId = n.parentId;

            childrens = new Dictionary<int, int>(n.childrens);

            Id = instanceCounter.getInstanceId();
        }

        public Node( int parentID, T data_)
        {
         
            data = data_;

            parentId = parentID;

            childrens = new Dictionary<int, int>();

            Id = instanceCounter.getInstanceId();
        }
        
        public Node(T data_)
        {

            data = data_;

            parentId = -1;

            childrens = new Dictionary<int, int>();

            Id = instanceCounter.getInstanceId();

        }

        public override string ToString()
        {
            return "ID : " + getID() + " data : " + data.ToString(); 
        }

        ~Node()
        {
          instanceCounter.removeInstance(Id);
        }

   }

}
