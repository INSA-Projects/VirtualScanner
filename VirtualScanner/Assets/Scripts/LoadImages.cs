using UnityEngine;
using System.Collections;
using System.IO;
using System;

public class LoadImages : MonoBehaviour 
{
    
	// Use this for initialization
	void Start () 
    {
        string format = "jpg";
        string slicesPath = Application.dataPath + @"/Slices/";
        
        DirectoryInfo dirInfo = new DirectoryInfo(slicesPath);
        int count = this.getNumberOfFiles(dirInfo, format);
        
        if (count < 2)
        {
            Debug.LogError("Couldn't build the result. Are you sure you put your " + format + " images into the \'" + slicesPath + "\' folder ?");
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
            Debug.LogError(e.Message);
        }
	}

	
	// Update is called once per frame
	void Update () 
    {
	
	}

    /// <summary>
    /// Returns the number of png files in the directory dir
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
            Debug.LogError(e.Message);
        }        
        return numberOfFiles;
    }

}
