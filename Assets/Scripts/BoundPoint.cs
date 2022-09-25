using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 跳ねる地点に関する処理を行う
/// </summary>
public class BoundPoint : MonoBehaviour
{
    [SerializeField,Header("誰のコートか")]
    private OwnerType ownerType;//所有者の種類

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
    /// 跳ねる位置の仮想位置を取得する
    /// </summary>
    /// <param name="ballPos">ボールの位置</param>
    /// <param name="direction">ボールの進行方向</param>
    /// <returns>跳ねる位置の仮想位置/returns>
    public Vector3 GetBoundPointPos(Vector3 ballPos,Vector3 direction)
    {
        //（仮）
        return Vector3.zero;
    }
}
