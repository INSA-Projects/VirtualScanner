using UnityEngine;
using System.Diagnostics;
using System.Collections;

public class DicomPngConvertor : MonoBehaviour 
{
    private string dicomPath; // path of dicom

	void Start () 
    {
        this.dicomPath = PlayerPrefs.GetString("DICOMFolder");
        if (dicomPath == "")
        {
            UnityEngine.Debug.LogError("No DICOM folder was specified. Didn't you forget to specify the DICOM folder in the FolderSelection scene?");
            return;
        }
        StartCoroutine(conversionRoutine());
        
	}

    // routine converting dicom to png
    private IEnumerator conversionRoutine()
    {
        yield return StartCoroutine(this.launchCommandLineApp());
        PlayerPrefs.SetString("SlicesPath", this.dicomPath + @"/out/");
        Application.LoadLevel(2);
    }

    /// <summary>
    /// Launch the legacy application with some options set.
    /// </summary>
    private IEnumerator launchCommandLineApp()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();

        // Use ProcessStartInfo class
        ProcessStartInfo startInfo = new ProcessStartInfo();
        startInfo.FileName = Application.dataPath + @"/DicomConverter.exe";
        startInfo.Arguments = this.dicomPath;

        try
        {
            using (Process process = Process.Start(startInfo))
            {
                process.WaitForExit();
            }
        }
        catch (System.Exception e)
        {
            UnityEngine.Debug.LogError("Error executing converter:" + e.Message);
        }
    }

}
