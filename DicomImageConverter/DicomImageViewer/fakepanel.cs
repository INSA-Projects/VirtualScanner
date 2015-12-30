using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Imaging;

namespace DicomImageViewer
{
    class fakepanel
    {
        List<byte> pix8;
        List<ushort> pix16;
        List<byte> pix24;
        Bitmap bmp;
        int hOffset;
        int vOffset;
        int hMax;
        int vMax;
        int imgWidth;
        int imgHeight;
        int panWidth;
        int panHeight;
        bool newImage;
        bool goodImage;

        ImageBitsPerPixel bpp;

        // For Window Level
        int winMin;
        int winMax;
        int winCentre;
        int winWidth;
        int winWidthBy2;
        int deltaX;
        int deltaY;

        Point ptWLDown;
        double changeValWidth;
        double changeValCentre;
        bool rightMouseDown;
        bool imageAvailable;
        public bool signed16Image;

        byte[] lut8;
        byte[] lut16;
        byte[] imagePixels8;
        byte[] imagePixels16;
        byte[] imagePixels24;
        int sizeImg;
        int sizeImg3;
        MainForm mf;


        public fakepanel()
        {

            pix16 = new List<ushort>();
            lut16 = new byte[65536];
            goodImage = true;
        }

        public void SetParameters(ref List<ushort> arr, int wid, int hei, double windowWidth,
         double windowCentre, bool resetScroll)
        {
            bpp = ImageBitsPerPixel.Sixteen;
            imgWidth = wid;
            imgHeight = hei;
            winWidth = Convert.ToInt32(windowWidth);
            winCentre = Convert.ToInt32(windowCentre);

            sizeImg = imgWidth * imgHeight;
            sizeImg3 = sizeImg * 3;
            double sizeImg3By4 = sizeImg3 / 4.0;

         //   DetermineMouseSensitivity();

            pix16 = arr;
            imagePixels16 = new byte[sizeImg3];

        //    mf = mainFrm;

            imageAvailable = true;
            if (bmp != null)
                bmp.Dispose();
            ResetValues();
            ComputeLookUpTable16();
            bmp = new Bitmap(imgWidth, imgHeight, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            CreateImage16();

        }

        public void SaveImage(String fileName)
        {
            if (bmp != null && goodImage)
            {
               bmp.Save(fileName, ImageFormat.Png);
            }
            //else if (bmp != null && !goodImage)
            //{
            //    Console.WriteLine("save " + fileName);
            //    bmp.Save(fileName, ImageFormat.Png);

            //}
        }

        public bool Signed16Image
        {
            set { signed16Image = value; }
        }


        // Linear interpolation here too
        private void ComputeLookUpTable16()
        {
            int range = 4000;
          //  if (range < 1) range = 1;
             double factor = 255.0 / range;
            int i;


            for (i = 0; i < 65536; ++i)
            {
                if (i <= winMin)
                {
                    lut16[i] = 0;
                }

                else if (i >= winMax)
                {
                    lut16[i] = 255;
                }
                else
                {
                    lut16[i] = (byte)((i - winMin) * factor);
                }
            }

         //   checkColor();
        }


        private void checkColor()
        {
            int nb0 = 0;
            int nb255 = 0;
            for (int i = 0; i < 65536; ++i)
            {
                if (lut16[i] == 0)
                {
                    nb0++;
                }

                else if (lut16[i] == 0)
                {
                    nb255++;
                }

            }

            Console.WriteLine("Color : " + nb0 + " black " + nb255 + " White");
        }


        // Create a bitmap on the fly, using 16-bit grayscale pixel data
        private void CreateImage16()
        {
            BitmapData bmd = bmp.LockBits(new Rectangle(0, 0, imgWidth, imgHeight),
               System.Drawing.Imaging.ImageLockMode.ReadOnly, bmp.PixelFormat);
            int nb0 = 0;
            int nbelse = 0;
            int nbSum =  0;
            int nbw = 0;
            unsafe
            {
                int pixelSize = 3;
                int i, j, j1, i1;
                byte b;

                for (i = 0; i < bmd.Height; ++i)
                {
                    byte* row = (byte*)bmd.Scan0 + (i * bmd.Stride);
                    i1 = i * bmd.Width;

                    for (j = 0; j < bmd.Width; ++j)
                    {
                        b = lut16[pix16[i * bmd.Width + j]];
                        j1 = j * pixelSize;
                        row[j1] = b;            // Red
                        row[j1 + 1] = b;        // Green
                        row[j1 + 2] = b;        // Blue
                        if (b == 0)
                        {
                            nb0++;
                        }
                        else
                        {
                            nbelse++;
                            nbSum += b;
                            if (b == 255){
                                nbw++;
                            }
                            else
                            {
                               
                            }
                        }
                    }
                }

            }
            
            bmp.UnlockBits(bmd);
            if(nbelse == 234042 || nbelse == 234043) //( nbSum > 59680700 && nbSum < 59680750 ) )
            {
                goodImage = false;
      //          Console.WriteLine("nb0 " + nb0 + " sum " + nbSum + " nbw : " + nbw);

            }
            else
            {
                goodImage = true;
            }

        }


        public void ResetValues()
        {
            winMax = Convert.ToInt32(winCentre + 0.5 * winWidth);
            winMin = winMax - winWidth;
         //   UpdateMainForm();
        }

    }
}
