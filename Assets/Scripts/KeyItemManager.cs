using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Reflection;

// キーアイテムの管理
public class KeyItemManager : MonoBehaviour {

	public GameObject[] keyItemStatObj;
	public GameObject[] wallObj;
	private int gotItem = 0;

	// 初期化
	void Start () {
		// HUD上のキーアイテム表示を消しておく
		foreach(GameObject obj in keyItemStatObj) {
			obj.GetComponent<Image>().enabled = false;
		}
	}

	public void OnGotKeyItem() {
		// SE再生
		GameObject.Find("GameCtrl").GetComponent<SoundManager>().Play(4);

		Destroy (wallObj[gotItem]);
		keyItemStatObj[gotItem].GetComponent<Image>().enabled = true;

		gotItem++;
	}
}
