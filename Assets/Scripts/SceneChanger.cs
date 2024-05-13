using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    static string nextScene = "Stage1-1";
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GameStart()
    {
        nextScene = "Stage1-2";
        SceneManager.LoadScene("Stage1-1");
    }

    public void GoNextStage()
    {
        string GoScene = nextScene;
        switch(GoScene)
        {
            case "Stage1-2":
                nextScene = "Stage1-3";
                break;
            case "Stage1-3":
                nextScene = "Stage1-4";
                break;
        }

        SceneManager.LoadScene(GoScene);
    }
}
