using UnityEngine;

/// <summary>
/// 跳ねる地点に関する処理を行う
/// </summary>
public class BoundPoint : MonoBehaviour
{
    [SerializeField,Header("誰のコートか")]
    private OwnerType ownerType;//所有者の種類

    [SerializeField]
    private Transform virtualBoundPoint;//仮想位置

    private Transform virtualBoundPointTran;//仮想位置の保持用

    /// <summary>
    /// コートの所有者を取得する
    /// </summary>
    /// <returns>コートの所有者</returns>
    public OwnerType GetOwnerTypeOfCourt()
    {
        //所有者の種類を返す
        return ownerType;
    }

    /// <summary>
    /// 仮想の跳ねる位置を取得する
    /// </summary>
    /// <param name="ballTran">ボールの位置情報</param>
    /// <param name="angleY">ボールのy角度</param>
    /// <returns>跳ねる位置の仮想位置/returns>
    public Vector3 GetVirtualBoundPointPos(Transform ballTran, float angleY)
    {
        //まだ仮想位置を生成していないなら
        if(virtualBoundPointTran==null)
        {
            //仮想位置を生成
            virtualBoundPointTran = Instantiate(virtualBoundPoint,transform.position,Quaternion.identity);
        }

        //仮想位置の親をボールに設定
        virtualBoundPointTran.SetParent(ballTran);

        //仮想位置の向きをボールの向きに合わせる
        virtualBoundPointTran.localEulerAngles= Vector3.zero;

        //仮想位置の座標を初期化
        virtualBoundPointTran.position = transform.position;

        //仮想位置の位置を設定
        virtualBoundPointTran.localPosition = new Vector3(0f,virtualBoundPointTran.localPosition.y,virtualBoundPointTran.localPosition.z);

        //仮想位置の親を自分に設定（親を解除）
        virtualBoundPointTran.SetParent(transform);

        //仮想位置を返す
        return virtualBoundPointTran.position;
    }
}
