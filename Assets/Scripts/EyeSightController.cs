using UnityEngine;
using System.Collections;
using Pathfinding;

public class EyeSightController : MonoBehaviour {
	
	public float sight_angle = 180.0f; // 視野角
	public float sight_minDistance = 20.0f; // 方角に関係なく探知する距離
	public float sight_maxDistance = 200.0f; // 見渡せる距離

	private bool check;
	private GameObject player;
	private KillerController killerController;

    private Vector3 aimVector;
	
	// 初期化
	void Start () {
		check = false;
		player = GameObject.Find("Player");
		killerController = this.gameObject.GetComponent<KillerController>();
	}

	// 更新
	void FixedUpdate () {
		check = EyeSightCheck(player);
		if(check){ // プレイヤーが視野に入っているとき、巡回アニメーションをオフ、A*をオンにする
			killerController.SetNextAIMode(1);
		}
		else{ // 視野にいなくなったときは、巡回モードに戻る
			if(killerController.aiMode == 1){
				killerController.SetNextAIMode(2);
			}
			else if(anchorDistance < 3.0f){
				killerController.SetNextAIMode(0);
			}
		}
	}

	private float playerDistance;
	private float anchorDistance;

    private Vector3 killerDirection;
    // 視野チェック
    private bool EyeSightCheck(GameObject target) {
		// プレイヤーとの距離を算出
		playerDistance = (transform.position - target.transform.position).magnitude;
        // プレイヤーのいる方向とエネミーの向きとの角度差を算出
        aimVector = target.transform.position - transform.position;
        killerDirection = killerController.killerCharacterTransform.forward;
        float viewAngle = Vector3.Angle(aimVector, killerDirection);

		anchorDistance = (transform.position - killerController.anchorObj.transform.position).magnitude;

        // プレイヤーに近いか、視野内にプレイヤーがいるかどうかを判定
        if (playerDistance < sight_minDistance || (playerDistance < sight_maxDistance && viewAngle < (sight_angle * 0.5)))
        {
            // 壁等の遮蔽物があるかどうか判定
            RaycastHit hit;
            if (Physics.Raycast(transform.position, aimVector, out hit, sight_maxDistance) && hit.transform.gameObject.name=="Player")
            {
                return true;
            }
            else
                return false;
        }
        else
            return false;
	}

    // ギズモ描画
    void OnDrawGizmos()
    {
#if UNITY_EDITOR
        //Debug.DrawRay(transform.position, aimVector, Color.red);
        UnityEditor.Handles.color = Color.yellow;
        UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.up, sight_minDistance);
        UnityEditor.Handles.color = Color.green;
        int steps = 5;
        for (int i = 0; i < steps; i++)
        {
            float d = sight_minDistance + (sight_maxDistance - sight_minDistance) / steps*i;
            UnityEditor.Handles.DrawWireArc(transform.position, Vector3.up, killerDirection, sight_angle * -0.5f, d);
            UnityEditor.Handles.DrawWireArc(transform.position, Vector3.up, killerDirection, sight_angle * 0.5f, d);
        }
#endif
    }
}
