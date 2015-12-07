using UnityEngine;
using System.Collections;
using System.IO;
using System;

public class LoadImages : MonoBehaviour 
{
    
	// Use this for initialization
	void Start () 
    {
        string format = "png";
        // string slicesPath = Application.dataPath + @"/Slices/";
        string slicesPath = "C:\\dcm\\out\\";

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
            Array.Sort(fis,new sortFileName());
            foreach (FileInfo fi in fis)
            {
                if (fi.Extension.ToLower().Contains(format))
                {
                    Debug.Log("add image number" + i + " named " + fi.FullName);
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


    private class sortFileName : IComparer
    {
        int IComparer.Compare(object a, object b)
        {
            FileInfo f1 = (FileInfo)a;
            FileInfo f2 = (FileInfo)b;
            String s1 = f1.FullName;
            string s2 = f2.FullName;
            // s1 = s1.Remove(0, 11);
            //s2 = s1.Remove(0, 11);
            //s1 = s1.Replace(".png","");
            //s2 = s2.Replace(".png","");
            //  Debug.Log(s1.Remove(0,11).Replace(".png","") + "|" + s2);
            int i1 = Int32.Parse(s1.Remove(0, 11).Replace(".png", ""));
            int i2 = Int32.Parse(s2.Remove(0, 11).Replace(".png", ""));
            if (i1 > i2)
            {
                return 1;
            }
            if (i1 < i2)
            {
                return -1;
            }
            else
            {
                return 0;
            }
        }
    }

}
