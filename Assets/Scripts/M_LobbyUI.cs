using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;


public class M_LobbyUI : MonoBehaviour
{
    void Start()
    {
        
    }

    public void NextScene()
    {
        SceneManager.LoadScene(1);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();

#elif UNITY_STANDALONE
        //2. 어플리케이션일 경우
        Application.Quit();

#endif 
    }

}
