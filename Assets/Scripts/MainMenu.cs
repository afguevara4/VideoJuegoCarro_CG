using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SceneLogin()
    {
        SceneManager.LoadScene("Login");
    }

    public void SceneRegister()
    {
        SceneManager.LoadScene("Registro");
    }

    public void SceneMain()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
