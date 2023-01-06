using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonHandler : MonoBehaviour
{
    
    public void StartGame(){
        SceneManager.LoadScene("Grid Scene");
    }

    public void Encyclopedia(){
        SceneManager.LoadScene("Encyclopedia Scene");
    }

    public void Credits(){
        SceneManager.LoadScene("Credits Scene");
    }

    public void Menu(){
        SceneManager.LoadScene("HomeScreen Scene");
    }
    
}
