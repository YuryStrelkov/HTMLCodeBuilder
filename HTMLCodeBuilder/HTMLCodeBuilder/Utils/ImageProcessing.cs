using System;
using System.Linq;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace HTMLCodeBuilder.Utils
{
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
        byte getR(double a);
        byte getG(double a);
        byte getB(double a);
        byte getA(double a);
    }
    /// <summary>
    /// Input value belongs to [0,1]
    /// </summary>
    public struct JET : ICmap
    {
        public byte getA(double a)
        {
            return 1;
        }

        public byte getB(double ordinal)
        {
            ordinal *= 4;

            return (byte)(255 * Math.Min(Math.Max(Math.Min(ordinal + 0.5, -ordinal + 2.5), 0), 1));
        }

        public byte getG(double ordinal)
        {
            ordinal *= 4;

            return (byte)(255 * Math.Min(Math.Max(Math.Min(ordinal - 0.5, -ordinal + 3.5), 0), 1));
        }

        public byte getR(double ordinal)
        {
            ordinal *= 4;

            return (byte)(255 * Math.Min(Math.Max(Math.Min(ordinal - 1.5, -ordinal + 4.5), 0), 1));
        }
 

    }

    /// Input value belongs to [0,1]

    public struct GRAY : ICmap
    {
        public byte getA(double a)
        {
            return (byte)(255.0);
        }

        public byte getB(double a)
        {
            return (byte)(255.0*a);
        }

        public byte getG(double a)
        {
            return (byte)(255.0 * a);
        }

        public byte getR(double a)
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

                colorsVals[index * 4]     = jet.getR(gray);
                colorsVals[index * 4 + 1] = jet.getG(gray );
                colorsVals[index * 4 + 2] = jet.getB(gray );
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
                colorsVals[index * 4]     = gray.getR((colors - index) / colors);
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
                    bytes[rowStrideIdx + i]     = colorSetter.getR((array[cellidx] - min) / abs);
                    bytes[rowStrideIdx + i + 1] = colorSetter.getG((array[cellidx] - min) / abs);
                    bytes[rowStrideIdx + i + 2] = colorSetter.getB((array[cellidx] - min) / abs);
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
