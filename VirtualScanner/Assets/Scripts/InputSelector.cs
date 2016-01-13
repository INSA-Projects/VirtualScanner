using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public static class InputMethod
{
    public static readonly int KEYBOARD = 0;
    public static readonly int RAZER = 1;
}

public class InputSelector : MonoBehaviour 
{
    private int inputMethod = InputMethod.KEYBOARD;
    public Text goWithButtonText;
    public Button goWithButton;
    
    void Start()
    {
        this.goWithButton.gameObject.SetActive(false);
    }

    public void SetRazerInput()
    {
        this.goWithButton.gameObject.SetActive(true);
        this.inputMethod = InputMethod.RAZER;
        this.goWithButtonText.text = "Go with Razer!";
    }

    public void SetKeyboardInput()
    {
        this.goWithButton.gameObject.SetActive(true);
        this.inputMethod = InputMethod.KEYBOARD;
        this.goWithButtonText.text = "Go with Keyboard!";
    }

    public void LetsGo()
    {
        PlayerPrefs.SetInt("InputMethod", this.inputMethod);
        Application.LoadLevel(3);
    }
}
