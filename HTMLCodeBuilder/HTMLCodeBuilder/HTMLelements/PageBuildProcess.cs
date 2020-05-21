using HTMLCodeBuilder.Nodes;
using HTMLCodeBuilder.TaggedElements;
using System.Text;

namespace HTMLCodeBuilder.HTMLelements
{
   public class PageBuildProcess : INodeProcess<ITagElement, string>
    {
        private int startLevel;

        private StringBuilder code;

        public string getProcessResult()
        {
            return code.ToString();
        }

        public void onStart(int level, Node<ITagElement> n)
        {
            if (n.getChildren().Count!= 0)
            {
                code.Append(n.getData().expandOpenTag(startLevel + level));
                code.Append("\n");
                return;
            }
            code.Append(n.getData().expandOpenTag(startLevel + level));
        }

        public void onEnd(int level, Node<ITagElement> n)
        {
            if (n.getChildren().Count != 0)
            {
                code.Append(n.getData().expandCloseTag(startLevel+level));
                code.Append("\n");
                return;
            }
            code.Append(n.getData().expandCloseTag(0));
            code.Append("\n");
        }

        public void process(int level, Node<ITagElement> n)
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
