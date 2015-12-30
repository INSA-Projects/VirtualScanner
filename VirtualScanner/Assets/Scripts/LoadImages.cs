using UnityEngine;
using System.Collections;
using System.IO;
using System;
using System.Diagnostics;

public class LoadImages : MonoBehaviour 
{
    private string slicesPath; // path of png slices
    private string dicomPath; // path of dicom slices


    void Start()
    {
        this.slicesPath = Application.dataPath + @"/Slices/";
        this.dicomPath = PlayerPrefs.GetString("DICOMFolder");
        if (dicomPath == "")
        {
            UnityEngine.Debug.LogError("No DICOM folder was specified. Didn't you forget to specify the DICOM folder in the FolderSelection scene?");
            return;
        }

        convertDicomToPng();
        loadSlices();
    }


    // COCO
    void convertDicomToPng()
    {
        // dicom files are in "this.dicomPath"
        // resulting png slices must be stored in "this.slicesPath"
        LaunchCommandLineApp(this.dicomPath);
        this.slicesPath = this.dicomPath + @"/out/";
    }

    /// <summary>
    /// Launch the legacy application with some options set.
    /// </summary>
    static void LaunchCommandLineApp(string dicompath)
    {

        // Use ProcessStartInfo class
        ProcessStartInfo startInfo = new ProcessStartInfo();
      //  startInfo.CreateNoWindow = true;
      //  startInfo.UseShellExecute = true;
        startInfo.FileName = Application.dataPath + @"/DicomConverter.exe";
       // UnityEngine.Debug.LogError(startInfo.FileName);
        //   startInfo.WindowStyle = ProcessWindowStyle.Hidden;
        startInfo.Arguments = dicompath;

        try
        {
            // Start the process with the info we specified.
            // Call WaitForExit and then the using statement will close.
            using (Process exeProcess = Process.Start(startInfo))
            {
                exeProcess.WaitForExit();
            }
        }
        catch (Exception e)
        {

            UnityEngine.Debug.LogError("Error executing converter :" + e.Message);
            // Log error.
        }
    }



/// <summary>
/// Load the png slices into the scene
/// </summary>
void loadSlices () 
    {
        string format = "png";

        DirectoryInfo dirInfo = new DirectoryInfo(this.slicesPath);
        int count = this.getNumberOfFiles(dirInfo, format);
        
        if (count < 2)
        {
            UnityEngine.Debug.LogError("Couldn't build the result. Are you sure you put your " + format + " images into the \'" + this.slicesPath + "\' folder ?");
            return;
        }

        RayMarching ray = Camera.main.GetComponent<RayMarching>();
        ray.Slices = new Texture2D[count];

        try
        {
            int i = 0;
            FileInfo[] fis = dirInfo.GetFiles();
            foreach (FileInfo fi in fis)
            {
                if (fi.Extension.ToLower().Contains(format))
                {
                    var fileData = File.ReadAllBytes (fi.FullName);
                    Texture2D texture = new Texture2D (2, 2);
                    texture.LoadImage (fileData);
                    ray.Slices[i++] = texture;
                }
            }
            ray.GenerateVolumeTexture();
        }
        catch (Exception e)
        {
            UnityEngine.Debug.LogError(e.Message);
        }
	}


    /// <summary>
    /// Returns the number of files of type "format" in the directory "dir"
    /// </summary>
    /// <param name="dir"></param>
    /// <returns></returns>
    private int getNumberOfFiles(DirectoryInfo dir, string format)
    {
        format = format.ToLower();
        int numberOfFiles = 0;
        try
        {
            FileInfo[] fis = dir.GetFiles();
            foreach (FileInfo fi in fis)
            {
                if (fi.Extension.ToLower().Contains(format))
                {
                    numberOfFiles++;
                }
            }
        }
        catch (Exception e)
        {
            UnityEngine.Debug.LogError(e.Message);
        }        
        return numberOfFiles;
    }

}
