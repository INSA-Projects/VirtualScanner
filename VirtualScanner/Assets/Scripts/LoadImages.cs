using UnityEngine;
using System.Collections;
using System.IO;
using System;

public class LoadImages : MonoBehaviour 
{
    private string slicesPath; // path of png slices

    // init function
    void Start()
    {
        this.slicesPath = PlayerPrefs.GetString("SlicesPath");    
        if (this.slicesPath == "")
        {
            Debug.LogError("No path for slices was found");
            return;
        }
        
        loadSlices();
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
