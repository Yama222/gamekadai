using UnityEngine;
using System.Collections;

// ビルボーディング機能
public class Billboarding : MonoBehaviour {
	
	public GameObject targetObject; // ターゲットオブジェクト。カメラ以外のターゲットも想定する
	public bool Y_AxisOnly = false; // Y軸固定するかどうか
	public Vector3 offset; // 回転オフセット値

	private Quaternion trot;	//ターゲットへの回転
	private Quaternion myrot;	//自分自身の回転データ

	// Billboardメソッドの切り替え用
	public enum Mode{
		LookAtCamera,
		FaceCamera,
	}
	public Mode mode = Mode.LookAtCamera;

	// 初期化
	void Start () {
		// エディタでターゲットカメラが指定されていない場合は、メインカメラをターゲットにする
		if(targetObject == null){ 
			targetObject = Camera.main.gameObject;
		}
		myrot = transform.rotation;	//自分自身の回転データを保存
	}

	// 更新
	void Update () {

		switch(mode) {
		case Mode.LookAtCamera: // ターゲット側を向くモード
			// ターゲットに向くためのオブジェクトの向きを算出
			trot = Quaternion.Slerp(transform.rotation,Quaternion.LookRotation(targetObject.transform.position - transform.position),1.0f);
			// Y軸固定モードのとき
			if (Y_AxisOnly) {
				trot.x = myrot.x; // x成分は自身の回転角のものに戻す
				//trot.z = 0.0f; //ｚ成分はゼロにする
				trot.z = myrot.z; //z成分は自身の回転角のものに戻す
			}
			transform.rotation = trot; // 算出した回転値を適用する

			break;

		case Mode.FaceCamera: // カメラ正対モード
			// オブジェクトの向きとして、カメラの逆向きベクトルを得る
			Vector3 lookVector = -targetObject.transform.forward;

			// Y軸固定モードの場合、Y成分の向き要素は０にする
			if(Y_AxisOnly) {
				lookVector = new Vector3(lookVector.x, 0.0f, lookVector.z);
			}
			// 上記で算出した向きにオブジェクトを向ける
			this.transform.LookAt (this.transform.position + lookVector);

			break;
		}

		// オフセット値指定に基づいて回転させる（オイラー角指定）
		transform.localEulerAngles += offset;

        transform.localPosition = new Vector3(0.0f, transform.localPosition.y, 0.0f);
	}
}