using UnityEngine;
using System.Collections;

public class KeyItemController : MonoBehaviour {

	void OnTriggerEnter (Collider col) {

		// プレイヤーが触れたとき、キー取得メソッドを呼ぶ
		if (col.gameObject.name == "Player"){
			GameObject.Find("GameCtrl").SendMessage("OnGotKeyItem");
			Destroy(gameObject);
		}
	}
}



