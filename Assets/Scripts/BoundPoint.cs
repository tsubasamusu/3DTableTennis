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
    /// 仮想の跳ねる位置を取得する
    /// </summary>
    /// <param name="ballPos">ボールの位置</param>
    /// <param name="angleY">ボールのy角度</param>
    /// <returns>跳ねる位置の仮想位置/returns>
    public Vector3 GetVirtualBoundPointPos(Vector3 ballPos,float angleY)
    {
        //ボールとの距離を取得
        float length = Mathf.Abs((Vector3.Scale(transform.position, new Vector3(1f, 0f, 1f)) - Vector3.Scale(ballPos, new Vector3(1f, 0f, 1f))).magnitude);

        //ボールのa（「y=ax+b」の「a（傾き）」）を取得
        float a=Mathf.Tan(Mathf.Deg2Rad*angleY);

        //ボールのb（「y=ax+b」の「b（切片）」）を取得
        float b = ballPos.z - a * ballPos.x;

        //仮想位置のx成分
        float virtualX=0f;

        //仮想位置のz成分
        float virtualZ=0f;

        virtualZ = a * virtualX + b;

        //作成した仮想位置を返す
        return new Vector3(virtualX,transform.position.y,virtualZ);
    }
}
