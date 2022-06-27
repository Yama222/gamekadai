using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

// ステージジャンプコントロール
public class LevelJumpController : MonoBehaviour {

    void Awake()
    {
        Screen.SetResolution(854, 480, false);
    }

    void FixedUpdate()
    {
        // Returnでタイトルに戻る
        if (Input.GetKeyUp(KeyCode.Return))
        {
            GameObject.Find("TimeText").SendMessage("OnTimerReset"); // 経過時間をリセット	
            GameObject.Find("Player").SendMessage("OnResetLife");
            Jump2Stage("GPT.title");
        }
        // Escで終了
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

	public void Jump2Stage(string levelID){
        if (Application.CanStreamedLevelBeLoaded(levelID)){
            SceneManager.LoadScene(levelID);
		}
	}

}




