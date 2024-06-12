using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    internal static string nowScene {get; private set;}
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
        nowScene = "Stage1-1";
        SceneManager.LoadScene("Stage1-1");
    }

    public void GoNextStage()
    {
        string GoScene = nextScene;
        nowScene = GoScene;
        switch(GoScene)
        {
            case "Stage1-2":
                nextScene = "Stage1-3";
                break;
            case "Stage1-3":
                nextScene = "Stage1-4";
                break;
            case "Stage1-4":
                nextScene = "Stage1-5";
                break;
            case "Stage1-5":
                nextScene = "Stage2-1";
                break;
            case "Stage2-1":
                nextScene = "Stage2-2";
                break;
            case "Stage2-2":
                nextScene = "Stage2-3";
                break;
            case "Stage2-3":
                nextScene = "Stage2-4";
                break;
            case "Stage2-4":
                nextScene = "Stage2-5";
                break;
            case "Stage2-5":
                nextScene = "Stage3-1";
                break;
            case "Stage3-1":
                nextScene = "Stage3-2";
                break;
            case "Stage3-2":
                nextScene = "Stage3-3";
                break;
            case "Stage3-3":
                nextScene = "Stage3-4";
                break;
            case "Stage3-4":
                nextScene = "Stage3-5";
                break;
        }

        SceneManager.LoadScene(GoScene);
    }
}
