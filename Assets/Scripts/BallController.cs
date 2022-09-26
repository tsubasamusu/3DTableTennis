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
    private BoundPoint playerBoundPoint;//プレイヤー用の跳ねる位置

    [SerializeField]
    private BoundPoint enemyBoundPoint;//エネミー用の跳ねる位置

    private OwnerType currentOwner=OwnerType.Enemy;//現在の弾の所有者

    private Vector3 currentBoundPos;//現在の跳ねる位置

    private bool inCourt;//コートに入るかどうか

    private bool stopMove;//ボールの動きを止めるかどうか

    /// <summary>
    /// 「コートに入るかどうか」の取得用
    /// </summary>
    public bool InCourt { get=>inCourt;}

    /// <summary>
    /// ボールの現在の所有者の取得用
    /// </summary>
    public OwnerType CurrentOwner { get => currentOwner; }

    /// <summary>
    /// ボールを打つ
    /// </summary>
    public void ShotBall()
    {
        //光線を発射する高さを取得
        float posY = currentOwner == OwnerType.Player ? 0.25f : 0.75f;

        //光線を作成 
        Ray ray = new(new Vector3(transform.position.x, posY, transform.position.z), transform.forward);

        //コートに入らない状態で仮に登録する
        inCourt = false;

        //光線が他のコライダーに触れなかったら
        if (!Physics.Raycast(ray, out RaycastHit hit, 10f))
        {
            //ボールを移動させる準備を行う
            PrepareMoveBall();

            //以降の処理を行わない
            return;
        }

        //接触相手がコートではないなら
        if (!hit.transform.TryGetComponent(out BoundPoint boundPoint))
        {
            //ボールを移動させる準備を行う
            PrepareMoveBall();

            //以降の処理を行わない
            return;
        }

        //現在のボールの所有者のコートに触れたら
        if (boundPoint.GetOwnerTypeOfCourt() == currentOwner)
        {
            //コートに入る状態で登録する
            inCourt = true;

            //ボールを移動させる準備を行う
            PrepareMoveBall();
        }
    }

    /// <summary>
    /// 適切なy座標を取得する
    /// </summary>
    /// <returns>適切なy座標</returns>
    private float GetAppropriatePosY()
    {
        //跳ねる位置との距離を取得
        float length = Mathf.Abs((Vector3.Scale(transform.position, new Vector3(1f, 0f, 1f)) - Vector3.Scale(currentBoundPos, new Vector3(1f, 0f, 1f))).magnitude);

        //コートに入らず、一定以下の低さになったら
        if (!inCourt && transform.position.y<=0.8f)
        {
            //距離を負にする（跳ねさせず、落下させる）
            length *= -1f;
        }

        //適切なy座標を返す
        return -(0.75f / 25f) * (length - 5f) * (length - 5f) + 1.5f;
    }

    /// <summary>
    /// ボールを移動させる準備を行う
    /// </summary>
    private void PrepareMoveBall()
    {
        //ボールを移動させる
        StartCoroutine(MoveBall());
    }

    /// <summary>
    /// ボールを移動させる
    /// </summary>
    /// <returns>待ち時間</returns>
    private IEnumerator MoveBall()
    {
        //ボールの所有者を保持
        OwnerType ownerType = currentOwner;

        //ボールの所有者が変わらない（返球されていない）間、繰り返す
        while (ownerType == currentOwner)
        {
            //ボールの動きを止める指示が出たら
            if(stopMove)
            {
                //繰り返し処理を終了する
                break;
            }

            //ボールの向きに移動させる
            transform.position += transform.forward * GameData.instance.BallSpeed*Time.deltaTime;

            //y座標を更新する
            transform.position = new Vector3(transform.position.x,Mathf.Clamp(GetAppropriatePosY(),0.25f,10f), transform.position.z);

            //次のフレームへ飛ばす（実質、Updateメソッド）
            yield return null;
        }
    }

    /// <summary>
    /// 他のコライダーに接触した際に呼び出される
    /// </summary>
    /// <param name="other">接触相手</param>
    private void OnTriggerEnter(Collider other)
    {
        //ラケットに触れたら
        if (other.TryGetComponent(out RacketController racketController))
        {
            //現在のボールの所有者と、ボールを打った者人が同じなら（二度打ちされたら）
            if(currentOwner==racketController.OwnerType)
            {
                //以降の処理を行わない
                return;
            }

            //停止命令を解除
            stopMove = false;

            //ボールの所有者を登録
            currentOwner = racketController.OwnerType;

            //ボールの向きを設定
            transform.eulerAngles=new Vector3(0f,racketController.transform.root.transform.eulerAngles.y,0f);

            //ボールの所有者に応じて取得する跳ねる位置を変更
            currentBoundPos = (currentOwner == OwnerType.Player ? playerBoundPoint : enemyBoundPoint)

                //仮想の跳ねる位置を取得
                .GetVirtualBoundPointPos(transform, racketController.transform.root.transform.eulerAngles.y);

            //ボールを打つ
            ShotBall();
        }
    }

    /// <summary>
    /// ボールを止め、サーブから再スタートする
    /// </summary>
    /// <param name="server">誰がサーブをするか</param>
    public void StopBall(OwnerType server)
    {
        //ボールの動きを止める
        stopMove = true;

        //サーブをする人に応じてボールの位置を変更
        transform.position = new Vector3(0f, 1f, server == OwnerType.Player ? -3f : 3f);

        //現在のボールの所有者を設定
        currentOwner=server==OwnerType.Player?OwnerType.Enemy:OwnerType.Player;

        //コートに入る状態にする（エネミーに動いてもらうため）
        inCourt = true;
    }
}
