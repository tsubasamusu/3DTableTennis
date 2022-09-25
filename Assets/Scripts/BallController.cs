using System.Collections;
using System.Collections.Generic;
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
    /// 適切なy座標を取得する
    /// </summary>
    /// <returns>適切なy座標</returns>
    private float GetAppropriatePosY()
    {
        //跳ねる位置を取得
        Vector3 boundPos = isPlayerTurn ? playerBoundTran.position : enemyBoundTran.position;

        //跳ねる位置との距離を取得
        float length = Mathf.Abs(Vector3.Scale((transform.position - boundPos), new Vector3(1f, 0f, 1f)).magnitude);

        //適切なy座標を返す
        return -(2f / 25f)*(length - 5f)*(length - 5f) + 2f;
    }
}
