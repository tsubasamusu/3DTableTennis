using System.Collections;//IEnumeratorを使用
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

    private OwnerType currentOwner;//現在の弾の所有者

    /// <summary>
    /// ボールを打つ
    /// </summary>
    /// <param name="direction">打つ方向</param>
    /// <param name="ownerType">ボールの所有者</param>
    public void ShotBall(Vector3 direction, OwnerType ownerType)
    {
        //現在のボールの所有者を取得
        currentOwner = ownerType;

        //光線を発射する高さを取得
        float posY = ownerType == OwnerType.Player ? 0.25f : 0.75f;

        //光線を作成 
        Ray ray = new(new Vector3(transform.position.x, posY, transform.position.z), direction);

        //光線が他のコライダーに触れなかったら
        if (!Physics.Raycast(ray, out RaycastHit hit, 10f))
        {
            //ボールを移動させる
            StartCoroutine(MoveBall(false, direction));

            //以降の処理を行わない
            return;
        }

        //接触相手がコートではないなら
        if (!hit.transform.TryGetComponent(out BoundPoint boundPoint))
        {
            //ボールを移動させる
            StartCoroutine(MoveBall(false, direction));

            //以降の処理を行わない
            return;
        }

        //現在のボールの所有者のコートに触れたら
        if (boundPoint.GetOwnerTypeOfCourt() == ownerType)
        {
            //ボールを移動させる
            StartCoroutine(MoveBall(true, direction));
        }
    }

    /// <summary>
    /// 適切なy座標を取得する
    /// </summary>
    /// <param name="inCourt">コートに入るかどうか</param>
    /// <returns>適切なy座標</returns>
    private float GetAppropriatePosY(bool inCourt)
    {
        //跳ねる位置を取得
        Vector3 boundPos = currentOwner==OwnerType.Player ? playerBoundTran.position : enemyBoundTran.position;

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

    /// <summary>
    /// ボールを移動させる
    /// </summary>
    /// <param name="inCourt">コートに入るかどうか</param>
    /// <param name="direction">ボールの移動方向</param>
    /// <returns>待ち時間</returns>
    private IEnumerator MoveBall(bool inCourt, Vector3 direction)
    {
        //ボールの所有者を保持
        OwnerType ownerType=currentOwner;

        //ボールの所有者が変わらない（返球されていない）間、繰り返す
        while(ownerType==currentOwner)
        {
            //ボールの移動方向を取得
            direction = Vector3.Scale(direction, new Vector3(1f, 0f, 1f)).normalized;

            //ボールを移動させる
            transform.Translate(direction * GameData.instance.BallSpeed * Time.deltaTime);

            //y座標を更新する
            transform.position = new Vector3(transform.position.x, GetAppropriatePosY(inCourt), transform.position.z);

            //次のフレームへ飛ばす（実質、Updateメソッド）
            yield return null;
        }
    }
}
