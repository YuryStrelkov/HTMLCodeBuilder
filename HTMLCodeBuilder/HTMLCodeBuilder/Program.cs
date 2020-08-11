using HTMLCodeBuilder.Document;
using HTMLCodeBuilder.HTMLelements;
using HTMLCodeBuilder.TaggedElements;
using System;


namespace HTMLBuilderExecutable
{
    class Program
    {
        static void Main(string[] args)
        {
          ADocument page = new HTMLDocument();

            page.AddElement(HTMLElements.CreateTitle("Титул"), page.HeadID);
            page.AddContent(page.BodyID, "Оглавление");

                    for (int i = 1 ; i <= 3;i++)
                    {
                        page.AddTextBlock("Text block "+i.ToString(), "This is text block "+ i.ToString()+"."+
                            "This is text block " + i.ToString() + "."+
                            "This is text block " + i.ToString() + "."+
                            "This is text block " + i.ToString() + "."+
                            "This is text block " + i.ToString() + "."+
                            "This is text block " + i.ToString() + ".", true);
                    }

            int table = page.AddTable(page.BodyID,"item",true,new string[3]{ "3","1","2" });
            HTMLTable t = (HTMLTable)page.GetElement(table);
            t.appendRecord("a");
            t.appendRecord("a");
            t.appendRecord("a");
            t.appendRecord("a");
            t.appendRecord("a");

            page.AddGraphic(page.BodyID,170,50);
            //для всех графиков должны быть одинаковые x
            //для всех графиков должны быть одинаковые y
            page.AppendGraphic(new double[] { 100, 200, 300 }, new double[] { 0,   1,   1.2 },"1");
            page.AppendGraphic(new double[] { 100, 200, 300 }, new double[] { 0,  .1, 1.2 },  "2");
            page.AppendGraphic(new double[] { 100, 200, 300 }, new double[] { 0, 1.4,  1.2 }, "3");
            page.GraphicTitle("A");
            page.GraphicXLabel("x");
            page.GraphicYLabel("y");



            page.AddGraphic(page.BodyID, 170, 100);
            page.AppendGraphic(new double[] { 0, 1, 2 }, new double[] { 0, 1, 1.2 }, "1");
            page.GraphicTitle("A");
            page.GraphicXLabel("x");
            page.GraphicYLabel("y");

            page.AddGraphic(page.BodyID, 170, 100);

            int M = 600, N = 800;

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
 
            page.AppendGraphic(arrayX, arrayY, array);
            page.GraphicTitle("A");
            page.GraphicXLabel("x");
            page.GraphicYLabel("y");

            TagElementsGroup gridholder = HTMLElements.CreateImageGridHolder(3);

            page.Merege(page.BodyID, gridholder);
            
             page.AddGraphic(gridholder.RootID, 170, 50);



            page.AppendGraphic(new double[] { 0, 1, 2 }, new double[] { 0, 1, 1.2 }, "1");


            page.AddGraphic(gridholder.RootID, 170, 50);

            page.AppendGraphic(new double[] { 0, 1, 2 }, new double[] { 0, 1, 1.2 }, "1");

            page.AddGraphic(gridholder.RootID, 170, 50);
            page.AppendGraphic(new double[] { 0, 1, 2 }, new double[] { 0, 1, 1.2 }, "1");
            page.AddGraphic(gridholder.RootID, 170, 50);
            page.AppendGraphic(new double[] { 0, 1, 2 }, new double[] { 0, 1, 1.2 }, "1");

            page.AddGraphic(gridholder.RootID, 170, 50);
            page.AppendGraphic(arrayX, arrayY, array);

            page.AddGraphic(gridholder.RootID, 170, 50);
            page.AppendGraphic(arrayX, arrayY, array);

            page.BuildCode();

            Console.ReadKey();

            page.SaveCode("page.html");

            page.Dispose();

            Console.ReadKey();
        }
    }
}
