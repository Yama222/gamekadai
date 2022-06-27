using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class TimeController : MonoBehaviour {
	
	private static float elapsedTime = 0.0f; // ゲーム経過時間
	public float timeLimit = 90.0f; // 制限時間 (インスペクタ側で定義する)
	private AudioSource BGM_source;
	private GameObject timeTextObject;
    private GameObject player;

    void Start() {
		BGM_source = GameObject.Find("Main Camera").GetComponent<AudioSource>();
		timeTextObject = GameObject.Find("TXT_time");
        player = GameObject.Find("Player");
    }

    float t1=0.0f, t2=0.0f;
	void Update () {
		
		elapsedTime += Time.deltaTime; // 経過時間を累積
	
		if(elapsedTime > timeLimit) { // 制限時間に達した場合
            if(t1==0.0f)
                t1 = Time.time;
            BGM_source.pitch = Mathf.Lerp(1.0f, 0.01f, 0.1f * (Time.time-t1));
            StartCoroutine(OnTimeUp());
        }
        else
        { // 画面上に残り時間を表示
            int remainTime = (int)(this.timeLimit - elapsedTime);
            timeTextObject.GetComponent<Text>().text = "残り時間：" + remainTime.ToString() + " 秒";
        }
        if (timeLimit - elapsedTime < 15.0f && elapsedTime <= timeLimit) {
            if (t2 == 0.0f)
                t2 = Time.time;
            BGM_source.pitch = Mathf.Lerp(1.0f, 2.0f, 0.05f * (Time.time - t2));
            timeTextObject.GetComponent<Animator>().Play("TimeText_warning");
		}
    }

    // タイムアップ処理
    private IEnumerator OnTimeUp()
    {
        // プレイヤーをフリーズさせる
        player.GetComponent<PlayerController>().enabled = false;
        player.GetComponent<CharacterController>().enabled = false;
        player.GetComponent<PlatformInputController>().enabled = false;
        player.GetComponent<CharacterMotor>().enabled = false;

        yield return new WaitForSeconds(2.0f);

        OnTimerReset(); // 経過時間をリセット	
        SceneManager.LoadScene("GPT.gameOver");// ゲームオーバー画面に遷移

    }

    void OnTimerReset () {

		elapsedTime = 0.0f; // 経過時間をリセット	

	}

}




