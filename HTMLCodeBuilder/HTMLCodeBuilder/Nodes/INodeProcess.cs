namespace HTMLCodeBuilder.Nodes
{
    public interface INodeProcess<T,ResultType> where T : ICopy<T>
    {
        ResultType getProcessResult();

        void onStart(int level, Node<T> n);

        void process(int level, Node<T> n);

        void onEnd  (int level, Node<T> n);
    }
}
