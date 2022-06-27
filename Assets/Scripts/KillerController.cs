using UnityEngine;
using System.Collections;

// Killerに触れたら呼び出される, プレイヤーライフを減らす
public class KillerController : MonoBehaviour {

	public int aiMode = 0; // AIモード（0:巡回 , 1:A*, 2:巡回地点に移動中）
	private int preAiMode;
	public GameObject anchorObj; // 巡回地点の記憶用
	public GameObject exObj; // !!マーク
    public Transform killerCharacterTransform; // EyeSightControllerのために管理

    private SoundManager soundManager;

    // 初期化
    void Start() {
        soundManager = GameObject.Find("GameCtrl").GetComponent<SoundManager>();

        // アニメーション巡回を開始する
        try
        {
			gameObject.GetComponent<Animator>().enabled = true;
		}
		catch{

		}
		// 巡回位置記憶用のオブジェクトに座標を記録させる
		anchorObj.transform.position = this.gameObject.transform.position;
	}

	// AIモードのセット
	public void SetNextAIMode(int mode) {
		aiMode = mode;
	}

	// AIモードが切り替わったときにコールする
	private void OnAIChange() {
		if(aiMode == 0) { //  巡回モードに移行
			gameObject.GetComponent<AIFollow>().enabled = false;
			gameObject.GetComponent<Animator>().enabled = true;
			gameObject.GetComponent<Animator>().speed = 1.0f;

			if(preAiMode == 2){
				gameObject.GetComponent<AIFollow>().target = GameObject.Find("Player").transform;
			}
		}
		else if(aiMode == 1) { // A＊モードに移行
			if(preAiMode == 0){
				// 巡回位置記憶用のオブジェクトに座標を記録させる
				anchorObj.transform.position = this.gameObject.transform.position;
			}
			// プレイヤーにA*のターゲットを定める
			gameObject.GetComponent<AIFollow>().target = GameObject.Find("Player").transform;

			// AI Followコンポーネントをアクティベート
			gameObject.GetComponent<Animator>().speed = 0.0f;
			gameObject.GetComponent<Animator>().enabled = false;
			gameObject.GetComponent<AIFollow>().enabled = true;
			aiMode = 1;

			// !!マーク出現アニメーションを再生
			exObj.GetComponent<Animator>().Play("ex_Up");
            soundManager.Play(6);
        }
		else if(aiMode == 2 && preAiMode == 1){ // 巡回地点に戻る
			gameObject.GetComponent<AIFollow>().target = anchorObj.transform;
		}
	}

	// 衝突時にコールされる
	void OnTriggerEnter (Collider col) {
	
		// Killerに衝突したのがプレイヤーだった場合、プレイヤーの"OnHitKiller"メソッドを呼ぶ
		if(col.gameObject.name == "Player"){
			col.gameObject.SendMessage("OnHitKiller");
		}
		
		// 岩に当たっているときは、アニメーション巡回とA*を外す
		else if(col.gameObject.name == "PhysicsRock"){
			gameObject.GetComponent<AIFollow>().enabled = false;
			gameObject.GetComponent<Animator>().enabled = false;
		}

	}

	// 更新
	void Update() {

		if(this.aiMode != this.preAiMode){
			this.OnAIChange();
		}

		//　現在のAIモードを記憶
		this.preAiMode = this.aiMode;
	}

}



