using UnityEngine;
using System.Collections;

public class PhysicsPusher : MonoBehaviour
{
	
	// RigidBodyを押し返す力	
	public float pushPower;
	
	void Start ()
	{

	}
	
	// 対象オブジェクトが相手に衝突したときに呼び出されるイベント関数	
	void OnControllerColliderHit (ControllerColliderHit hitCollider)
	{			
		// 衝突したRigidBodyオブジェクトを取得する
		Rigidbody rBody = hitCollider.collider.attachedRigidbody;

		// rigidbodyが存在しなければリターン		
		if (rBody == null || rBody.isKinematic)
			return;
		
		// 足元にあるRigidBodyは動かさない(リターンする)	
		if (hitCollider.moveDirection.y < -0.3f)
			return;

		// 押し返す方向ベクトルを計算する（y成分は無視する）	
		Vector3 pushDir = new Vector3 (hitCollider.moveDirection.x, 0, hitCollider.moveDirection.z);
		
		// 押し返すRigidBodyに加速度を与える		
		rBody.velocity = pushDir * pushPower;
		
		return;
	}
}
