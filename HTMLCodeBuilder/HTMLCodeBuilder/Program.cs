using HTMLCodeBuilder.HTMLelements;
using HTMLCodeBuilder.TaggedElements;
using System;

namespace HTMLBuilderExecutable
{
    class Program
    {
        static void Main(string[] args)
        {
            HTMLPage page = new HTMLPage();

            page.addElement(HTMLElements.CreateTitle("Титул"), page.HTMLHeadID);
            page.addContent(page.HTMLBodyID, "Оглавление");

                    for (int i = 1 ; i <= 3;i++)
                    {
                        page.addTextBlock("Text block "+i.ToString(), "This is text block "+ i.ToString()+"."+
                            "This is text block " + i.ToString() + "."+
                            "This is text block " + i.ToString() + "."+
                            "This is text block " + i.ToString() + "."+
                            "This is text block " + i.ToString() + "."+
                            "This is text block " + i.ToString() + ".", true);
                    }

    HTMLTable table = page.addTable(page.HTMLBodyID,"Хуи",true,new string[3]{ "1","2","3" });
            table.appendRecord("a");
            table.appendRecord("a");
            table.appendRecord("a");
            table.appendRecord("a"); table.appendRecord("a");
            table.appendRecord("a"); table.appendRecord("a");
            table.appendRecord("a"); table.appendRecord("a");
            table.appendRecord("a"); table.appendRecord("a");
            table.appendRecord("a"); table.appendRecord("a");
            table.appendRecord("a"); table.appendRecord("a");
            table.appendRecord("a"); table.appendRecord("a");
            table.appendRecord("a"); table.appendRecord("a"); table.appendRecord("a");
            table.appendRecord("a"); table.appendRecord("a"); table.appendRecord("a");
            table.appendRecord("a"); table.appendRecord("a"); table.appendRecord("a");
            table.appendRecord("a"); table.appendRecord("a"); table.appendRecord("a");
            table.appendRecord("a"); table.appendRecord("a"); table.appendRecord("a");
            table.appendRecord("a"); table.appendRecord("a"); table.appendRecord("a");
            table.appendRecord("a"); table.appendRecord("a"); table.appendRecord("a");
      
            /*HTMLElements.EditTableCell(table, 0, 0, "хуууй");
            HTMLElements.EditTableCell(table, 0, 1, "хуууй");
            HTMLElements.EditTableCell(table, 0, 2, "хуууй");*/

            //page.addGraph(page.HTMLBodyID,new double[] { 0, 1, 2}, new double[] { 0,1,2 },"x","y","t"); 
            page.addGraphic(page.HTMLBodyID,170,100);
            //для всех графиков должны быть одинаковые x
            //для всех графиков должны быть одинаковые y
            page.appendGraphic(new double[] { 0, 1, 2 }, new double[] { 0,   1,   1.2 },"1");
            page.appendGraphic(new double[] { 0, 1, 2 }, new double[] { 0,  .1, 1.2 },  "2");
            page.appendGraphic(new double[] { 0, 1, 2 }, new double[] { 0, 1.4,  1.2 }, "3");
            page.GraphicTitle("A");
            page.GraphicXLabel("x");
            page.GraphicYLabel("y");

            page.addGraphic(page.HTMLBodyID, 170, 100);
           


            int M = 321, N = 423;

            double[] array = new double[ M * N ];
            double[] arrayX = new double[M];
            double[] arrayY = new double[N];

            int stride = 0;

            for (int i = 0; i < M; i++)
            {
                arrayX[i] = i;
            }
            for (int i = 0; i < N; i++)
            {
                arrayY[i] = i;
            }
            for (int i = 0; i < N; i++)
            {
                stride = i * M ;

                for (int j = 0; j < M ; j++)
                {
                    array[stride+j] = i+j;
                }
            }


            page.appendGraphic(arrayX, arrayY, array);

            page.GraphicTitle("A");
            page.GraphicXLabel("x");
            page.GraphicYLabel("y");


            page.buildCode();

            page.saveCode("page.html");

            Console.ReadKey();

              
        }
    }
}
