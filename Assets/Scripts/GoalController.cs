using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

// ゴールに到達したときの処理
public class GoalController : MonoBehaviour {

	void OnTriggerEnter(Collider col) {
		// ゴールに衝突したのがプレイヤーならば、タイマーをリセットしてステージクリア画面に遷移する
		if(col.gameObject.name == "Player"){
			GameObject.Find("GameCtrl").SendMessage("OnTimerReset");
            SceneManager.LoadScene("GPT.stageClear");
        }
	}
}
