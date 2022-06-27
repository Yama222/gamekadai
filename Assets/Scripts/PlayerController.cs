using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Reflection;
using System.Diagnostics;

public class PlayerController : MonoBehaviour {

	private TrailRenderer trailRenderer;
	private bool isTrailRenderer = false; // Trail Rendererの有無チェック用フラグ
	
	public static int playerLife = 3; // プレイヤーの残機
    public GameObject[] playerIcon;
	public static float collisionDistance = 3.0f;
	private bool superSpeedMode;
	public float superSpeedPower = 100.0f;
	public GameObject gaurgeObject;
    private PhysicsPusher ppusher;
    private float pushPower;

    private SoundManager soundManager;
    private ParticleSystem deadFX;

    // 初期化
    void Start () {
	 	trailRenderer = GetComponent<TrailRenderer>();
        soundManager = GameObject.Find("GameCtrl").GetComponent<SoundManager>();
        ppusher = gameObject.GetComponent<PhysicsPusher>();
        pushPower = ppusher.pushPower;

        // TrailRendereコンポーネントが追加されていない場合はリターン		
        try {trailRenderer.enabled = false;}
		catch{
			return;
		}
		isTrailRenderer = true; 
		superSpeedMode = false; // スピードモードのフラグを下げておく

        // 残機アイコンのセットアップ
        for (int i = 0; i < playerIcon.Length; i++)
        {
            playerIcon[i].GetComponent<Image>().enabled = false;
        }
        for (int i=0; i<playerLife; i++)
        {
            playerIcon[i].GetComponent<Image>().enabled = true;
        }
        deadFX = GameObject.Find("DeadFX").GetComponent<ParticleSystem>();
        deadFX.Stop();
    }

    // 衝突したときにコールされる
    void OnHitKiller () {
        GameObject.Find("explosionLight").GetComponent<Animation>().Play();
        if (deadFX)
        {
            deadFX.Play();
        }

        // プレイヤーをフリーズさせる
        superSpeedMode = false;
        gameObject.GetComponent<CharacterController>().enabled = false;
        gameObject.GetComponent<PlatformInputController>().enabled = false;
        gameObject.GetComponent<CharacterMotor>().enabled = false;
        // 死亡処理
        soundManager.Play(5);
        StartCoroutine(OnDead());
	}
    // プレイヤーの死亡処理
    private Vector3 customJumpMove = Vector3.zero;
    private IEnumerator OnDead()
    {
        Vector3 negativeVel = -1.0f*gameObject.GetComponent<CharacterController>().velocity*0.005f;
        customJumpMove.x = negativeVel.x;
        customJumpMove.z = negativeVel.z;
        for (int i=0; i<30; i++)
        {
            yield return null;
            customJumpMove.y = customJumpMove.y + 0.02f - i*0.01f;
            transform.position += customJumpMove;
            if (transform.position.y < 1.0f)
            {
                transform.position = new Vector3(transform.position.x, 1.0f, transform.position.z);
            }
        }
        
        yield return new WaitForSeconds(0.3f);

        if (--playerLife <= 0)
        {  // 残機がゼロになったら死亡 → GAME OVER画面に飛ぶ
            playerLife = 3;
            GameObject.Find("GameCtrl").SendMessage("OnTimerReset"); // 経過時間のリセット
            SceneManager.LoadScene("GPT.gameOver");
        }
        else
        { // 残機が残っている場合,現在のレベルにて再スタート
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
	
	// エネミーとのコリジョンチェック（A*プラグインを入れるとColliderの判定が動作しないため、回避策）
	void EnemyCollisionCheck (Vector3 enemyPos){
		Vector3 distance = this.transform.position - enemyPos;
		float vsize = distance.x*distance.x + distance.y*distance.y + distance.z*distance.z; 
		if(vsize < collisionDistance){
			OnHitKiller();
		}
	}
	
	// スピードモードの取得・設定
	public void SetSpeedMode(bool mode){
		superSpeedMode = mode;
		// パワーが尽きた場合はモードを切る
		if(this.superSpeedPower < 2.0f) {
			superSpeedMode = false;
		}
    }

    public bool GetSpeedMode()
    {
        return superSpeedMode;
    }

    public void OnResetLife()
    {
        playerLife = 3;
    }

	// 毎フレーム更新
	void Update () {

        Vector3 velocity = this.GetComponent<CharacterController>().velocity;

        if(Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))
        {
            SetSpeedMode(true);
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift) || Input.GetKeyUp(KeyCode.RightShift))
        {
            SetSpeedMode(false);
        }
        // 動いていないときはフラグ下げる
        if (velocity.magnitude == 0.0f)
        {
            SetSpeedMode(false);
        }

        // Super Speed Powerの増減
        float symbol = superSpeedMode? -1.0f:0.2f;
		this.superSpeedPower += symbol*40.0f*Time.deltaTime;
		this.superSpeedPower = Mathf.Clamp(this.superSpeedPower, 0.0f, 100.0f);

		// スピードモードに応じてTrailRendererの表示/非表示を切り替え
		if(isTrailRenderer){
			trailRenderer.enabled = superSpeedMode;
            ppusher.pushPower = superSpeedMode ? pushPower * 2.0f : pushPower;
		}

		// スーパースピードパワーのゲージ表示
		gaurgeObject.transform.localScale = new Vector3(superSpeedPower*0.01f, 1.0f, 1.0f);

        this.GetComponent<CharacterMotor>().movement.maxForwardSpeed = superSpeedMode&&superSpeedPower>0.0f ? 15.0f:5.0f;
    }
	
}
