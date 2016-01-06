using UnityEngine;
using System.Collections;
using System.IO;
using System;
using System.Reflection;

public class LoadImages : MonoBehaviour 
{
    private string slicesPath; // path of png slices

    public RayMarching RayMarchingPrefab;
    private RayMarching middleVRRay;

    // init function
    void Start()
    {
        this.slicesPath = PlayerPrefs.GetString("SlicesPath");    
        if (this.slicesPath == "")
        {
            Debug.LogError("No path for slices was found");
            return;
        }

        // get the middlevr camera and assign the raymarching on it
        GameObject camera = GameObject.Find("Camera0");
        this.middleVRRay = camera.AddComponent<RayMarching>();
        this.duplicate(this.RayMarchingPrefab, this.middleVRRay);
        
        // load the slices
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


        this.middleVRRay.Slices = new Texture2D[count];
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
                    middleVRRay.Slices[i++] = texture;
                }
            }
            middleVRRay.GenerateVolumeTexture();
        }
        catch (Exception e)
        {
            /*
            UnityEngine.Debug.LogError(e.Message);
			//StackTrace st = new StackTrace(e, true);
			//StackFrame frame = st.GetFrame(0);
			//Get the file name
			string fileName = frame.GetFileName();
			//Get the method name
			string methodName = frame.GetMethod().Name;
			int line = frame.GetFileLineNumber();
			UnityEngine.Debug.LogError("File " + fileName  + " Methode " + methodName + " line " + line);
            */
            Debug.LogError("An error occured lol");
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

    private RayMarching duplicate (RayMarching original, RayMarching copy)
    {
        BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Default | BindingFlags.DeclaredOnly;
        PropertyInfo[] pinfos = original.GetType().GetProperties(flags);
        foreach (var pinfo in pinfos)
        {
            if (pinfo.CanWrite)
            {
                try
                {
                    pinfo.SetValue(copy, pinfo.GetValue(original, null), null);
                }
                catch { } // In case of NotImplementedException being thrown. For some reason specifying that exception didn't seem to catch it, so I didn't catch anything specific.
            }
        }
        FieldInfo[] finfos = original.GetType().GetFields(flags);
        foreach (var finfo in finfos)
        {
            finfo.SetValue(copy, finfo.GetValue(original));
        }
        return copy;
    }

}
