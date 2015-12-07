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
        [STAThread]
        static void Main()
        {
        /*  Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());*/


            Console.WriteLine("Conversion DICOM -> PNG");
            MainConsole mc = new MainConsole();

            String[] files;
            String Path = "C:\\dcm\\";
            if (Directory.Exists(Path))
            {
                Console.WriteLine("Valid path : " + Path);
                files = Directory.GetFiles(Path, @"*.dcm", SearchOption.TopDirectoryOnly);
                Console.WriteLine(files.Length + "images found");
                int i = 0;
                foreach (String fileName in files)
                {
                    Console.WriteLine("compute : " + fileName);
                    mc.readDicom(fileName,"peutimporte");
                    mc.save("C:\\dcm\\out\\" + i + ".png");
                    i++;
                }

            }

            //mc.readDicom("C:\\dcm\\IM-0003-0110.dcm","IM-0003-0110.dcm");
            //mc.save("C:\\dcm\\out\\test.png");

      
        }
    }
}
