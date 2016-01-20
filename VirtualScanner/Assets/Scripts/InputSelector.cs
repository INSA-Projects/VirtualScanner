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

    public void SetRazerInput()
    {
        PlayerPrefs.SetInt("InputMethod", InputMethod.RAZER);
        this.letsGo();
    }

    public void SetKeyboardInput()
    {
        PlayerPrefs.SetInt("InputMethod", InputMethod.KEYBOARD);
        this.letsGo();
    }

    private void letsGo()
    {
        Application.LoadLevel(2);
    }
}
