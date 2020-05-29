namespace HTMLCodeBuilder.Nodes
{
    public interface INodeProcess<T,ResultType> where T : ICopy<T>
    {
        ResultType GetProcessResult();

        void OnStart(int level, Node<T> n);

        void Process(int level, Node<T> n);

        void OnEnd  (int level, Node<T> n);
    }
}
