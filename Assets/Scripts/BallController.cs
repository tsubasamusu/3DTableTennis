using System.Collections;//IEnumeratorを使用
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

    private OwnerType currentOwner = OwnerType.Enemy;//現在の弾の所有者

    private Vector3 currentBoundPos;//現在の跳ねる位置

    private bool inCourt;//コートに入るかどうか

    private bool stopMove;//ボールの動きを止めるかどうか

    private bool playedBoundSE;//ボールが卓球台で跳ねる音を再生したかどうか

    /// <summary>
    /// 「コートに入るかどうか」の取得用
    /// </summary>
    public bool InCourt { get => inCourt; }

    /// <summary>
    /// 「ボールの現在の所有者」の取得用
    /// </summary>
    public OwnerType CurrentOwner { get => currentOwner; }

    /// <summary>
    /// BallControllerの初期設定を行う
    /// </summary>
    public void SetUpBallController()
    {
        //ボールを初期位置に移動させる
        transform.position = new Vector3(0f, 1f, -3f);
    }

    /// <summary>
    /// ボールを打つ
    /// </summary>
    public void ShotBall()
    {
        //効果音を再生していない状態に切り替える
        playedBoundSE = false;

        //効果音を再生
        SoundManager.instance.PlaySound(SoundDataSO.SoundName.RacketSE);

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
        if (!inCourt && transform.position.y <= 0.8f)
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
            if (stopMove)
            {
                //繰り返し処理を終了する
                break;
            }

            //ボールの向きに移動させる
            transform.position += transform.forward * GameData.instance.BallSpeed * Time.deltaTime;

            //y座標を更新する
            transform.position = new Vector3(transform.position.x, Mathf.Clamp(GetAppropriatePosY(), 0.25f, 10f), transform.position.z);

            //効果音再生後なら
            if (playedBoundSE)
            {
                //1フレーム待つ（実質、Updateメソッド）
                yield return null;

                //次の繰り返し処理に移る
                continue;
            }

            //コートに入らないなら
            if (!InCourt)
            {
                //1フレーム待つ（実質、Updateメソッド）
                yield return null;

                //次の繰り返し処理に移る
                continue;
            }

            //ボールの高さが一定以下になったら
            if (transform.position.y <= 0.8f)
            {
                //効果音を再生
                SoundManager.instance.PlaySound(SoundDataSO.SoundName.BoundSE);

                //効果音再生後に切り替える
                playedBoundSE = true;
            }

            //1フレーム待つ（実質、Updateメソッド）
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
            if (currentOwner == racketController.OwnerType)
            {
                //以降の処理を行わない
                return;
            }

            //停止命令を解除
            stopMove = false;

            //ボールの所有者を登録
            currentOwner = racketController.OwnerType;

            //ボールの向きを設定
            transform.eulerAngles = new Vector3(0f, racketController.transform.root.transform.eulerAngles.y, 0f);

            //ボールの所有者に応じて取得する跳ねる位置を変更
            currentBoundPos = (currentOwner == OwnerType.Player ? playerBoundPoint : enemyBoundPoint)

                //仮想の跳ねる位置を取得
                .GetVirtualBoundPointPos(transform, racketController.transform.root.transform.eulerAngles.y);

            //ボールを打つ
            ShotBall();
        }
    }

    /// <summary>
    /// サーブから再スタートするための準備を行う
    /// </summary>
    /// <param name="server">誰がサーブをするか</param>
    /// <param name="playerController">PlayerController</param>
    public void PrepareRestartGame(OwnerType server,PlayerController playerController)
    {
        //サーブから再スタートする
        StartCoroutine(RestartGame(server,playerController));
    }

    /// <summary>
    /// サーブから再スタートする
    /// </summary>
    /// <param name="server">誰がサーブをするか</param>
    /// <param name="playerController">PlayerController</param>
    /// <returns>待ち時間</returns>
    private IEnumerator RestartGame(OwnerType server,PlayerController playerController)
    {
        //ボールの動きを止める
        stopMove = true;

        //コートに入らない状態にする（ボールの状態を初期化）
        inCourt = false;

        //サーブをする人に応じてボールの位置を変更
        transform.position = new Vector3(0f, 1f, server == OwnerType.Player ? -3f : 3f);

        //現在のボールの所有者を設定
        currentOwner = server == OwnerType.Player ? OwnerType.Enemy : OwnerType.Player;

        //プレイヤーの位置を初期化
        playerController.ResetPlayerPos();

        //サーバーがエネミーなら
        if (server == OwnerType.Enemy)
        {
            //一定時間待つ（エネミーがサーブを打つまでの時間を設ける）
            yield return new WaitForSeconds(GameData.instance.EnemyServeTime);
        }

        //コートに入る状態にする（エネミーに動いてもらうため）
        inCourt = true;
    }
}
