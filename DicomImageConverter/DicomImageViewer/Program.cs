using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.IO;

namespace DicomImageViewer
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        //[STAThread]
        static int Main(string[] args)
        {
        /*  Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());*/


            Console.WriteLine("Conversion DICOM -> PNG");
            MainConsole mc = new MainConsole();

            if (args == null)
            {
                return 1;
            }

            String[] files;
            String Path = args[0];

            if (Directory.Exists(Path))
            {
                //Console.WriteLine("Valid path : " + Path);
                files = Directory.GetFiles(Path, @"*.dcm", SearchOption.TopDirectoryOnly);
                //Console.WriteLine(files.Length + "images found");
                
                string subPath = System.IO.Path.Combine(Path, "out");
                System.IO.Directory.CreateDirectory(subPath);

                int i = 1;
                foreach (String fileName in files)
                {
                    //Console.WriteLine("compute : " + i + " name : "  + fileName);
                    mc.readDicom(fileName, "peutimporte");

                    string prefix = "";
                    int power = 1000000;
                    while (i < power)
                    {
                        prefix += "0";
                        power /= 10;
                    }
                    
                    string filenamepng = prefix + i +  ".png";
                    mc.save(System.IO.Path.Combine(subPath, filenamepng));
                    i++;
                }

            }
            else
            {
                return 1;
            }

            return 0;

            //mc.readDicom("C:\\dcm\\IM-0003-0110.dcm","IM-0003-0110.dcm");
            //mc.save("C:\\dcm\\out\\test.png");

        }
    }
}
