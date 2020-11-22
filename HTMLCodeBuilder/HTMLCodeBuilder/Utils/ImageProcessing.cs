using System;
using System.Linq;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Collections.Generic;

namespace HTMLCodeBuilder.Utils
{

    public struct Point3D
    {
        public double X { get; set; }

        public double Y { get; set; }

        public double Z { get; set; }

        public Point3D(double x,double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }
    }

    public struct Tris
    {
        public static Point3D ClipPosX = new Point3D() { X = 1, Y = 0, Z = 0 };

        public static Point3D ClipNegX = new Point3D() { X = -1, Y = 0, Z = 0 };

        public static Point3D ClipPosY = new Point3D() { X = 0, Y = 1, Z = 0 };

        public static Point3D ClipNegY = new Point3D() { X = 0, Y = -1, Z = 0 };
        
        public Point3D P1 { get { return p1; } set { p1 = value; } }

        public Point3D P2 { get { return p2; } set { p2 = value; } }

        public Point3D P3 { get { return p3; } set { p3 = value; } }

        private Point3D p1;

        private Point3D p2;

        private Point3D p3;

        public void SortVerticesY()
        {
            if (p1.Y > p2.Y)
            {
                SwapPoints(ref p1, ref p2);
            }
            if (p1.Y > p3.Y)
            {
                SwapPoints(ref p1, ref p3);
            }
            if (p2.Y > p3.Y)
            {
                SwapPoints(ref p2, ref p3);
            }
        }

        private void SwapPoints(ref Point3D p1, ref Point3D p2)
        {
            Point3D dummyItem = p1;
            p1 = p2;
            p2 = dummyItem;
        }

        public List<Tris> ClipTris()
        {

        }

        public Tris(Point3D p_1, Point3D p_2, Point3D p_3)
        {
            p1 = p_1;
            p2 = p_2;
            p3 = p_3;
            SortVerticesY();
        }

        public Tris(double x1, double y1, double z1,
                    double x2, double y2, double z2,
                    double x3, double y3, double z3)
        {
            p1 = new Point3D(x1, y1, z1);
            p2 = new Point3D(x2, y2, z2);
            p3 = new Point3D(x3, y3, z3);
            SortVerticesY();
        }

    }




    public enum ColorMaps
    {
        GRAY = 0,
        JET = 1,
        HSV = 2,
        FALSECOLORS = 3,
        INFERNO = 4
    }

    public interface ICmap
    {
        byte GetR(double a);
        byte GetG(double a);
        byte GetB(double a);
        byte GetA(double a);
    }
    /// <summary>
    /// Input value belongs to [0,1]
    /// </summary>
    public struct JET : ICmap
    {
        public byte GetA(double a)
        {
            return 1;
        }

        public byte GetB(double ordinal)
        {
            ordinal *= 4;

            return (byte)(255 * Math.Min(Math.Max(Math.Min(ordinal + 0.5, -ordinal + 2.5), 0), 1));
        }

        public byte GetG(double ordinal)
        {
            ordinal *= 4;

            return (byte)(255 * Math.Min(Math.Max(Math.Min(ordinal - 0.5, -ordinal + 3.5), 0), 1));
        }

        public byte GetR(double ordinal)
        {
            ordinal *= 4;

            return (byte)(255 * Math.Min(Math.Max(Math.Min(ordinal - 1.5, -ordinal + 4.5), 0), 1));
        }
 

    }

    /// Input value belongs to [0,1]

    public struct GRAY : ICmap
    {
        public byte GetA(double a)
        {
            return (byte)(255.0);
        }

        public byte GetB(double a)
        {
            return (byte)(255.0*a);
        }

        public byte GetG(double a)
        {
            return (byte)(255.0 * a);
        }

        public byte GetR(double a)
        {
            return (byte)(255.0 * a);
        }
    }

    
    public static class ImageProcessing
    {
        public static readonly JET jet = new JET();

        public static readonly GRAY gray = new GRAY();

        private static int  CalcStride(int w, int bytesPerPix)
        {
            int stride = w * bytesPerPix;

            if (stride % 4 != 0)
            {
                stride = stride + 4 - stride % 4;
            }

            return stride;
        }






        private static void InterpolateTriangleDepth(ref Bitmap depth, Point3D P1, Point3D P2, Point3D P3)
        {
            if ( (P2.Y - P1.Y)/(P2.X - P1.X ) == (P3.Y - P2.Y) / (P3.X - P2.X))
            {
                return;
            }
         

            List<int> indeces = new List<int>();
            List<double> depthValues = new List<double>();

        }

        public static void UpdateImage(ref Bitmap image, double[] values)
        {
            int pixSizeInBytes = Image.GetPixelFormatSize(image.PixelFormat) / 8;
            
            if (values.Length % pixSizeInBytes != 0)
            {
                Console.WriteLine("pixel format error");
                return;
            }

            if (values.Length / 3 > image.Width * image.Height)
            {
                Console.WriteLine("array bigger than image");
                return;
            }

            UpdateImage(ref image, ConvertToBytes(values, image.Width, image.Height, pixSizeInBytes));
        }

        /*public static void UpdateImage(ref Bitmap image, int [] indeces,  double [] values)
        {
            int pixSizeInBytes = Image.GetPixelFormatSize(image.PixelFormat) / 8;
            
            if (values.Length % pixSizeInBytes!=0)
            {
                Console.WriteLine("pixel format error");
                return;
            }

            if (values.Length/3 > image.Width* image.Height)
            {
                Console.WriteLine("array bigger than image");
                return;
            }

            UpdateImage(ref image, indeces, ConvertToBytes(values, image.Width, image.Height, pixSizeInBytes));
        }*/

        public static void UpdateImage(ref Bitmap image, double []values, ColorMaps cmap)
        {
            byte[] imgBytes = ConvertToBytes(values, image.Width, image.Height, cmap);

            UpdateImage(ref image, imgBytes);
        }

        public static void UpdateImage(ref Bitmap image, byte[] values)
        {
            DateTime start = DateTime.Now;
            unsafe
            {
                BitmapData picData = image.LockBits(new Rectangle(0, 0, image.Width, image.Height), ImageLockMode.ReadWrite, image.PixelFormat);

                int stride = picData.Stride;

                IntPtr pixel =  picData.Scan0;

                Marshal.Copy(values, 0, pixel, values.Length);

                image.UnlockBits(picData);
            }
            DateTime end = DateTime.Now;
            Console.WriteLine("Time taken for ImageWrite : {0}", end - start);
        }
        /// исправить!
        /*private static void UpdateImage(ref Bitmap image, int[] indeces,  byte[] values)
        {
            unsafe
            {
                BitmapData picData = image.LockBits(new Rectangle(0, 0, image.Width, image.Height), ImageLockMode.ReadWrite, image.PixelFormat);

                byte* pixel = (byte*)picData.Scan0;

                Parallel.For(0, indeces.Length, index =>
                {
                    pixel[indeces[index] * 3] =     values[index * 3];
                    pixel[indeces[index] * 3 + 1] = values[index * 3 + 1];
                    pixel[indeces[index] * 3 + 2] = values[index * 3 + 2];
                });

                image.UnlockBits(picData);
            }
        }*/
 
        public static Bitmap CreateJetColorMap(int colors)
        {
            Bitmap map = CreateImage( 1, colors);

            byte[] colorsVals = new byte[colors * 4];

             Parallel.For(0, colors, index =>
            {
                double gray = (1.0*index  / colors);

                colorsVals[index * 4]     = jet.GetR(gray);
                colorsVals[index * 4 + 1] = jet.GetG(gray );
                colorsVals[index * 4 + 2] = jet.GetB(gray );
            });

            UpdateImage(ref map, colorsVals);

            return map;
        }

        public static Bitmap CreateJetColorMap()
        {
            return CreateJetColorMap(64);
        }

        public static Bitmap CreateGrayColorMap(int colors)
        {
            Bitmap map = CreateImage(1, colors);

            byte[] colorsVals = new byte[colors * 4];

            Parallel.For(0, colors, index =>
            {
                colorsVals[index * 4]     = gray.GetR((colors - index) / colors);
                colorsVals[index * 4 + 1] = colorsVals[index * 4];
                colorsVals[index * 4 + 2] = colorsVals[index * 4];
            });

            UpdateImage(ref map, colorsVals);

            return map;
        }

        public static Bitmap CreateGrayColorMap()
        {
            return CreateGrayColorMap(64);
        }

        public static Bitmap CreateImage(int w, int h)
        {
            return  CreateImage(w, h, PixelFormat.Format24bppRgb);
        }

        public static Bitmap CreateImage(int w, int h, PixelFormat format)
        {
            return new Bitmap(w, h, format);
        }

        public static Bitmap CreateImage(int w,int h, double[] array)
        {
            int format = array.Length % 3 == 0 ? 3 : 1;
            
            if (w*h != array.Length/format)
            {
                throw new Exception("image w*h not equal to pixels number");
            }

            Bitmap pic;

            double max = array.Max(), min = array.Min();

            double abs = max - min;

            pic = new Bitmap(w, h, PixelFormat.Format24bppRgb);

            UpdateImage(ref pic, array);

            return pic;
        }

        public static void WriteImage(ref Bitmap img, string path)
        {
            try
            {
                img.Save(path);
            }
            catch (Exception e)
            {
                Console.WriteLine("Something goes wrong while saving image to path : " + path);
            }

        }

        public static Bitmap ReadImage(string path)
        {
            Bitmap img = null;

            try
            {
                img = new Bitmap(path);
            }
            catch (Exception e)
            {
                Console.WriteLine("Something goes wrong while saving image to path : " + path);
            }

            return img;

        }
        
        private static byte[] ConvertToBytes(double[] array, int w, int h, ColorMaps cmap)
        {
            if (array.Length != h * w )
            {
                throw new Exception("Bytes array length are not equal to w*h");
            }

            double min = array.Min();

            double max = array.Max();

            double abs = max - min;

            // int stride = w * 3;

            //stride += (stride * 3) % 4;

            int stride = CalcStride( w, 3);

            byte[] bytes = new byte[stride * h];
            

            ICmap colorSetter;

            switch (cmap)
            {
                case ColorMaps.GRAY: colorSetter = gray; break ;

                case ColorMaps.JET: colorSetter = jet; break;

                case ColorMaps.HSV: colorSetter = jet; break;

                case ColorMaps.FALSECOLORS: colorSetter = jet; break;

                case ColorMaps.INFERNO: colorSetter = jet; break;

                default: colorSetter = jet; break;
            }

            Parallel.For(0, h, row =>
            {
                int rowStrideIdx = row * stride;

                int rowidx = row * w;

                int cellidx = rowidx;

                for (int i = 0; i < w * 3; i+=3)
                {
                    cellidx = rowidx + i / 3;
                    bytes[rowStrideIdx + i]     = colorSetter.GetR((array[cellidx] - min) / abs);
                    bytes[rowStrideIdx + i + 1] = colorSetter.GetG((array[cellidx] - min) / abs);
                    bytes[rowStrideIdx + i + 2] = colorSetter.GetB((array[cellidx] - min) / abs);
                }
            });

            return bytes;

        }

        private static byte[] ConvertToBytes(double[] array, int w, int h, int bytesPerPix)
        {
            if (array.Length != h * w * bytesPerPix)
            {
                throw new Exception("Bytes array length are not equal to w*h");
            }

            double min = array.Min();

            double max = array.Max();

            double abs = max - min;

            //int stride = w * bytesPerPix;
            ///!!!! int stride = (w + (w * bytesPerPix) % 4)*bytesPerPix; - возможно
            //if (stride % 4 != 0)
            //{
            //    stride = stride + 4 - stride % 4;
            //}
            int stride = CalcStride(w, 3);

            byte[] bytes = new byte[stride * h];

            w = w * bytesPerPix;

            Parallel.For(0, h, row =>
            {
                int rowStrideIdx = row * stride;

                int rowidx = row * w;

                for (int i = 0; i < w; i++)
                {
                    bytes[rowStrideIdx + i] = ((byte)(255 * (array[rowidx + i] - min) / abs));
                }
            });
            return bytes;
        }

    }
}
