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

    /// <summary>
    /// ボールを打つ
    /// </summary>
    /// <param name="direction">打つ方向</param>
    public void ShotBall(Vector3 direction)
    {

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
        float length = Mathf.Abs((Vector3.Scale(transform.position, new Vector3(1f, 0f, 1f))- Vector3.Scale(boundPos, new Vector3(1f, 0f, 1f))).magnitude);

        //コートに入らず、一定以下の低さになったら
        if(!inCourt&&transform.position.y<=0.8f)
        {
            //距離を負にする（落下させる）
            length *= -1f;
        }

        //適切なy座標を返す
        return -(0.75f / 25f) * (length - 5f) * (length - 5f) + 1.5f;
    }

    private void Update()
    {
        transform.position = new Vector3(transform.position.x,GetAppropriatePosY(false),transform.position.z);
    }
}
