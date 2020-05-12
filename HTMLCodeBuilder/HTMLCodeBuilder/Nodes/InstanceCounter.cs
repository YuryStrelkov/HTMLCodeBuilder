using System.Collections.Generic;

namespace HTMLCodeBuilder.Nodes
{
    /// <summary>
    /// Используется для остчёта количества созданных объектов заданного класса
    /// </summary>
    public class InstanceCounter
    {
        private int InstanceCount = 0;

        private object mutex;

        private Stack<int> deletedInstances;

        public void removeInstance(int id)
        {
            lock (mutex)
            {
                InstanceCount--;
                deletedInstances.Push(id);
            }
        }

        public int getInstanceIdAvailable()
        {

            lock (mutex)
            {
                if (deletedInstances.Count != 0)
                {
                    return deletedInstances.Peek();
                }

                return InstanceCount+1;
            }
        }

        public int getInstanceId()
        {
            lock (mutex)
            {
                if (deletedInstances.Count != 0)
                {
                    InstanceCount++;
                  ///  Console.WriteLine("Instance ID : " + deletedInstances.Peek());
                    return deletedInstances.Pop();
                }
                //Console.WriteLine("Instance ID : " + (InstanceCount + 1));
                return InstanceCount++;
            }
        }

        public InstanceCounter()
        {
            mutex = new object();
            deletedInstances = new Stack<int>();
        }
    }
}
