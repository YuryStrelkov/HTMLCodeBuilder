using HTMLCodeBuilder.HTMLelements;
using HTMLCodeBuilder.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace HTMLBuilderExecutable
{
    class Program
    {
        static void Main(string[] args)
        {
          HTMLPage page = new HTMLPage();

            page.AddElement(HTMLElements.CreateTitle("Титул"), page.HTMLHeadID);
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


            page.BuildCode();

            page.saveCode("page.html");
            Console.ReadKey();

            /*   List<HTMLElement> elems1=new List<HTMLElement>();
               Stopwatch sw = new Stopwatch();
               int size = 100000;
               sw.Start();

               for (int i=0;i< size; i++)
               {
                   elems1.Add(HTMLElements.CreateCenterAlign());
               }
               sw.Stop();
               Console.WriteLine("List time : "+sw.Elapsed);

               sw.Reset();

               sw.Start();
               HTMLElement[] elems2=new HTMLElement[size];

               for (int i = 0; i < size; i++)
               {
                   elems2[i] = HTMLElements.CreateCenterAlign();
               }
               sw.Stop();
               Console.WriteLine("Array time : " + sw.Elapsed);


               char[] arrayC = new char[100000];

               for (int i=0;i<arrayC.Length ;i+=5)
               {
                   arrayC[i] = 'S';
                   arrayC[i + 1] = 'y';
                   arrayC[i + 2] = 'k';
                   arrayC[i + 3] = 'a';
                   arrayC[i + 4] = '\n';
               }
               sw.Reset();
               sw.Start();

               arrayC.ToString();
               sw.Stop();
               Console.WriteLine("ToString time : " + sw.Elapsed);
               ///Console.WriteLine(arrayC);
       */

            char[] arrayC = new char[20];

            for (int i = 0; i < arrayC.Length; i += 10)
            {
                arrayC[i] = 'S';
                arrayC[i + 1] = 'y';
                arrayC[i + 2] = 'k';
                arrayC[i + 3] = 'a';
                arrayC[i + 4] = '\n';
                arrayC[i + 5] = '\0';
                arrayC[i + 6] = '\0';
                arrayC[i + 7] = '\0';
                arrayC[i + 8] = '\0';
                arrayC[i + 9] = '\0';
            }
            Console.WriteLine(arrayC);

            //    Console.WriteLine(Marshal.SizeOf(arrayC) + " bytes");
        unsafe
            {
                Console.WriteLine(Marshal.SizeOf(typeof(XYWH)) + " bytes");
            }

            Console.ReadKey();
        }
    }
}
