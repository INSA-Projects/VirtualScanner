using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class FolderSelector : MonoBehaviour 
{
    public Text folderPath; // dicom folder
    public Button launchButton; 


	void Start () 
    {
        this.folderPath.text = "No folder selected";
        this.launchButton.gameObject.SetActive(false);
	}
	
    /// <summary>
    /// Allow the user to select the folder containing DICOM files
    /// </summary>
    public void SelectFolder()
    {
        this.folderPath.text = EditorUtility.OpenFolderPanel("Select folder containing DICOM files", "", "");
        if (this.folderPath.text == "")
        {
            this.folderPath.text = "No folder selected";
            this.launchButton.gameObject.SetActive(false);
        }
        else
        {
            this.launchButton.gameObject.SetActive(true);
        }

    }

    /// <summary>
    /// Launch the 3D scene
    /// </summary>
    public void LaunchScene()
    {
        PlayerPrefs.SetString("DICOMFolder", this.folderPath.text);
        Application.LoadLevel(1);
    }
}
