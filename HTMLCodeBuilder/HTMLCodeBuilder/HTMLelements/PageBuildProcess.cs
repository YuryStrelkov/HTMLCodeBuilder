using HTMLCodeBuilder.Nodes;
using HTMLCodeBuilder.TaggedElements;
using System.Text;

namespace HTMLCodeBuilder.HTMLelements
{
   public class PageBuildProcess : INodeProcess<ITagElement, string>
    {
        string tabCode = "";

        private StringBuilder code;

        public string getProcessResult()
        {
            return code.ToString();
        }

        private string GetTab(int level)
        {
              return new string('\t', level);
        }

        public void onStart(int level, Node<ITagElement> n)
        {
            tabCode = GetTab( level);
 
            if (n.getChildren().Count != 0)
            {
                code.Append(tabCode);
                code.Append(n.getData().expandElementOpenTag());
                code.Append("\n");
                return;
            }
            code.Append(tabCode);
            code.Append(n.getData().expandElementOpenTag());
        }

        public void onEnd(int level, Node<ITagElement> n)
        {
            tabCode = GetTab(level);

            if (n.getChildren().Count != 0)
            {
                code.Append(tabCode);
                code.Append(n.getData().expandElementCloseTag());
                code.Append("\n");
                return;
            }
            code.Append(n.getData().expandElementCloseTag());
            code.Append("\n");
        }

        public void process(int level, Node<ITagElement> n)
        {
           
        }

        public PageBuildProcess()
        {
         code = new StringBuilder();
        }
    }
}
