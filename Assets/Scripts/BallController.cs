using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

/// <summary>
/// ボールの動きを制御する
/// </summary>
public class BallController : MonoBehaviour
{
    [SerializeField]
    private Transform playerBoundTran;//プレイヤー用の跳ねる位置

    [SerializeField]
    private Transform enemyBoundTran;//エネミー用の跳ねる位置

    private bool isPlayerTurn;//プレーヤーのターンかどうか

    private bool isBounded;//台上で跳ねたかどうか

    /// <summary>
    /// 「台上で跳ねたかどうか」の設定用
    /// </summary>
    public bool IsBounded { set=>isBounded=value; }

    /// <summary>
    /// ボールを打つ
    /// </summary>
    /// <param name="direction">打つ方向</param>
    public void ShotBall(Vector3 direction)
    {
        //まだ跳ねていない状態に切り替える
        isBounded = false;
    }

    /// <summary>
    /// 適切なy座標を取得する
    /// </summary>
    /// <param name="inCourt">コートに入るかどうか</param>
    /// <returns>適切なy座標</returns>
    private float GetAppropriatePosY(bool inCourt)
    {
        //跳ねる位置を取得
        Vector3 boundPos = isPlayerTurn ? playerBoundTran.position : enemyBoundTran.position;

        //跳ねる位置との距離を取得
        float length = (Vector3.Scale(transform.position, new Vector3(1f, 0f, 1f))- Vector3.Scale(boundPos, new Vector3(1f, 0f, 1f))).magnitude;

        //適切なy座標を返す
        return-(0.75f/25f)*(length - 5f)*(length - 5f) + 1.5f;
    }

    private void Update()
    {
        transform.position = new Vector3(transform.position.x,GetAppropriatePosY(true),transform.position.z);
    }
}
