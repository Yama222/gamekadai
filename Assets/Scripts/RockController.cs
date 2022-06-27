using UnityEngine;
using System.Collections;

public class RockController : MonoBehaviour {

	// 毎フレーム実行
	void Update () {
		this.GetComponent<Rigidbody>().WakeUp(); // Rigidbodyをスリープさせない
	}
}
