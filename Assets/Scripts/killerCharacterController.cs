using UnityEngine;
using System.Collections;

// エネミーキャラのアニメーションコントロール
public class killerCharacterController : MonoBehaviour {

	private Vector3 currentPos; // 現在のエネミー位置
	private Vector3 previousPos; // 前フレームでのエネミー位置
	private Vector3 enemyDir; // エネミーの向きベクトル

	// 初期化
	void Awake(){
	    currentPos = previousPos = this.transform.position; // 位置情報の初期化
		enemyDir = new Vector3(0.0f, 0.0f, -1.0f); // カメラ側に向かせる
	}

	// 更新
	void Update () {
		currentPos = this.transform.position; // 現在位置の更新
		if(currentPos != previousPos){ // 静止状態でなければ…
			enemyDir = currentPos - previousPos; // 向きベクトル算出
			this.gameObject.GetComponent<Animation>().Play("runForward"); // 走りモーションを再生
		}
		else {
			this.gameObject.GetComponent<Animation>().Play("idle");
		}

		// 移動方向に向かせる
		Vector3 dir = Vector3.Slerp(this.transform.forward, enemyDir, 6.0f*Time.deltaTime);
		this.transform.forward = new Vector3(dir.x, 0.0f, dir.z); // 傾き防止

		previousPos = currentPos; // 現在位置を記録
		// プレイヤーとのコリジョンチェックを行う
		GameObject.Find("Player").SendMessage("EnemyCollisionCheck",currentPos);
	
	}
}
