using HTMLCodeBuilder.Nodes;
using HTMLCodeBuilder.TaggedElements;
using System.Text;

namespace HTMLCodeBuilder.HTMLelements
{
   public class PageBuildProcess : INodeProcess<ITagElement, string>
    {
        private int startLevel;

        private StringBuilder code;

        public string GetProcessResult()
        {
            return code.ToString();
        }

        public void OnStart(int level, Node<ITagElement> n)
        {
            if (n.GetChildren().Count!= 0)
            {
                code.Append(n.GetData().ExpandOpenTag(startLevel + level));
                code.Append("\n");
                return;
            }
            code.Append(n.GetData().ExpandOpenTag(startLevel + level));
        }

        public void OnEnd(int level, Node<ITagElement> n)
        {
            if (n.GetChildren().Count != 0)
            {
                code.Append(n.GetData().ExpandCloseTag(startLevel+level));
                code.Append("\n");
                return;
            }
            code.Append(n.GetData().ExpandCloseTag(0));
            code.Append("\n");
        }

        public void Process(int level, Node<ITagElement> n)
        {
           
        }

        public PageBuildProcess(int start_level)
        {
            startLevel = start_level;
            code = new StringBuilder();
        }

        public PageBuildProcess()
        {
         startLevel = 0;
         code = new StringBuilder();
        }
    }
}
