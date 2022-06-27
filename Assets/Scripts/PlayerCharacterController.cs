using UnityEngine;
using System.Collections;

// プレイヤーキャラのアニメーションとSEのコントロール
public class PlayerCharacterController : MonoBehaviour {

	private float hSpeed = 0.0f; // 横方向の移動スピード
	private float vSpeed = 0.0f; // 奥行き方向の移動スピード

    private PlayerController playerController;
	private Animation playerAnimation;
	private SoundManager soundManager;

	void Start() {
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
		playerAnimation = GameObject.Find("eyeChar").GetComponent<Animation>();
		soundManager = GameObject.Find("GameCtrl").GetComponent<SoundManager>();
	}

	void Update () {
		// カーソル操作を検出
		hSpeed = Mathf.Abs(Input.GetAxis("Horizontal"));
		vSpeed = Mathf.Abs(Input.GetAxis("Vertical"));
		// ジャンプボタンが押されていたら、ジャンプアニメーションを再生
		if(Input.GetButton("Jump") && GameObject.Find("eyeChar").GetComponent<Animation>().isPlaying) {
			playerAnimation.Play("jump");
			soundManager.Play(2);
		}
		// カーソル操作が行われていたら、走りのアニメーションを再生
		else if (hSpeed > 0.1 || vSpeed > 0.1) {
            playerAnimation.Play("runForward");

            // SuperSpeed Modeのときはサウンドのピッチを上げる
            if (playerController.GetSpeedMode())
            {
                soundManager.Play(0, 1.5f);
            }
            else
            {
                soundManager.Play(0);
            }
		}
		// それ以外の場合は、待機アニメーションを再生
		else {
			playerAnimation.Play("idle"); // アニメーション再生
			soundManager.Stop(0); // 足音SEをストップ
        }	
	
	}
}
