using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DicomImageViewer
{
    class MainConsole
    {

        DicomDecoder dd;
        List<ushort> pixels16;
        int imageWidth;
        int imageHeight;
        int bitDepth;
        int samplesPerPixel;  // Updated 30 July 2010
        bool imageOpened;
        double winCentre;
        double winWidth;
        bool signedImage;
        int maxPixelValue;    // Updated July 2012
        int minPixelValue;
        fakepanel fp;


        public MainConsole()
        {
            dd = new DicomDecoder();
            fp = new fakepanel();
            pixels16 = new List<ushort>();
        }

        public void save(String filename){
            fp.SaveImage(filename);
        }
        public void readDicom(string fileName, string fileNameOnly)
        {
            dd.DicomFileName = fileName;

            TypeOfDicomFile typeOfDicomFile = dd.typeofDicomFile;

            if (typeOfDicomFile == TypeOfDicomFile.Dicom3File ||
                typeOfDicomFile == TypeOfDicomFile.DicomOldTypeFile)
            {
                imageWidth = dd.width;
                imageHeight = dd.height;
                bitDepth = dd.bitsAllocated;
                winCentre = dd.windowCentre;
                winWidth = dd.windowWidth;
                samplesPerPixel = dd.samplesPerPixel;
                signedImage = dd.signedImage;

                //imagePanelControl.NewImage = true;

              //  Text = "DICOM Image Viewer: " + fileNameOnly;


                pixels16.Clear();
                

                dd.GetPixels16(ref pixels16);

                // This is primarily for debugging purposes, 
                //  to view the pixel values as ascii data.
                //if (true)
                //{
                //    System.IO.StreamWriter file = new System.IO.StreamWriter(
                //               "C:\\imageSigned.txt");

                //    for (int ik = 0; ik < pixels16.Count; ++ik)
                //        file.Write(pixels16[ik] + "  ");

                //    file.Close();
                //}

                minPixelValue = pixels16.Min();
                maxPixelValue = pixels16.Max();

                // Bug fix dated 24 Aug 2013 - for proper window/level of signed images
                // Thanks to Matias Montroull from Argentina for pointing this out.
                if (dd.signedImage)
                {
                    winCentre -= short.MinValue;
                }

                if (Math.Abs(winWidth) < 0.001)
                {
                    winWidth = maxPixelValue - minPixelValue;
                }

                if ((winCentre == 0) ||
                    (minPixelValue > winCentre) || (maxPixelValue < winCentre))
                {
                    winCentre = (maxPixelValue + minPixelValue) / 2;
                }

                fp.Signed16Image = dd.signedImage;

                fp.SetParameters(ref pixels16, imageWidth, imageHeight,
                    winWidth, winCentre, true);
            }

        }

    }
}
