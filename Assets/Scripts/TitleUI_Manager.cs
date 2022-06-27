using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Diagnostics;

public class TitleUI_Manager : MonoBehaviour {

    public GameObject[] stageButtons;

    void Awake() {

        // いったん消す
        foreach (GameObject obj in stageButtons)
            obj.SetActive(false);

        // Scenes in Buildに含まれているシーンを検索
        int sceneCount = UnityEngine.SceneManagement.SceneManager.sceneCountInBuildSettings;
        string[] scenes = new string[sceneCount];
        for (int i = 0; i < sceneCount; i++)
        {
            scenes[i] = System.IO.Path.GetFileNameWithoutExtension(UnityEngine.SceneManagement.SceneUtility.GetScenePathByBuildIndex(i));
            for (int j = 0; j < 4; j++)
            {
                // ステージファイルが見つかったらボタンをアクティベート
                if (scenes[i] ==  "GPT.stage0" + (j + 1).ToString())
                {
                    stageButtons[j].SetActive(true);
                }
            }
        }



	}

}
