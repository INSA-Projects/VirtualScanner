using UnityEngine;
using UnityEngine.UI;

public class FolderSelector : MonoBehaviour 
{
    public Text folderPath; // dicom folder
    public Text holdOn;
    public Button launchButton;
    public Button browseButton;
    private FileBrowser fileBrowser;
    private bool drawFileBrowser = false;

    // init function
	void Start () 
    {
        this.folderPath.text = "No folder selected";
        this.launchButton.gameObject.SetActive(false);
        this.browseButton.gameObject.SetActive(true);
        this.fileBrowser = new FileBrowser(Application.dataPath);
        this.holdOn.gameObject.SetActive(false);
	}
	

    // allow the user to select a folder
    void OnGUI()
    {
        if (this.drawFileBrowser && this.fileBrowser.draw())
        {
            // no file selected
            if (this.fileBrowser.outputFile == null)
            {
                this.folderPath.text = "No folder selected";
                this.browseButton.gameObject.SetActive(true);
                this.launchButton.gameObject.SetActive(false);
            }
            
            // file selected
            else
            {
                this.folderPath.text = this.fileBrowser.outputFile.Directory.ToString();
                this.launchButton.gameObject.SetActive(true);
                this.browseButton.gameObject.SetActive(true);
            }
            this.drawFileBrowser = false;
        }
    }

    // show the browser
    public void ActiveBrowser()
    {
        this.drawFileBrowser = true;
        this.launchButton.gameObject.SetActive(false);
        this.browseButton.gameObject.SetActive(false);
    }

    /// <summary>
    /// Launch the 3D scene
    /// </summary>
    public void LaunchScene()
    {
        this.holdOn.gameObject.SetActive(true);
        PlayerPrefs.SetString("DICOMFolder", this.folderPath.text);
        Application.LoadLevel(1);
    }
}
