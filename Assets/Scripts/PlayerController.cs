using UnityEngine;

/// <summary>
/// プレーヤーの行動を制御する
/// </summary>
public class PlayerController : ControllerBase
{
    private Vector3 firstPos;//プレイヤーの初期位置

    /// <summary>
    /// PlayerControllerの初期設定を行う
    /// </summary>
    public void SetUpPlayerController()
    {
        //プレイヤーの初期位置を取得
        firstPos = transform.position;
    }

    /// <summary>
    /// 移動方向を取得する
    /// </summary>
    protected override Vector3 GetMoveDir()
    {
        //何も押されていないなら
        if(!Input.anyKey)
        {
            //以降の処理を行わない（無駄な処理を防止・無操作での移動防止）
            return Vector3.zero;
        }

        //左右移動の入力を取得
        float moveH = Input.GetAxis("Horizontal");

        //前後移動の入力を取得
        float moveV = Input.GetAxis("Vertical");

        //仮の移動方向を設定
        Vector3 movement = new Vector3(moveH, 0, moveV);

        //移動方向を取得し、返す
        return Camera.main.transform.forward * movement.z + Camera.main.transform.right * movement.x;
    }

    /// <summary>
    /// ラケットを制御する（Updateメソッドで呼び出され続ける）
    /// </summary>
    protected override void ControlRacket()
    {
        //左クリックされたら
        if (Input.GetMouseButtonDown(0))
        {
            //バックハンドドライブをする
            racketController.Drive(false);
        }
        //右クリックされたら
        else if (Input.GetMouseButtonDown(1))
        {
            //フォアハンドドライブをする
            racketController.Drive(true);
        }
    }

    /// <summary>
    /// キャラクターの向きを設定する（Updateメソッドで呼び出され続ける）
    /// </summary>
    protected override void SetCharaDirection()
    {
        //キャラクターの向きを設定する
        transform.eulerAngles = new Vector3(0f, Camera.main.transform.eulerAngles.y, 0f);
    }

    /// <summary>
    /// プレイヤーを初期位置に移動させる
    /// </summary>
    public void ResetPlayerPos()
    {
        //CharacterControllerを非活性化する
        charaController.enabled = false;

        //プレーヤーを初期位置に移動させる
        transform.position = firstPos;

        //CharacterControllerを活性化する
        charaController.enabled = true;
    }
}
