using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private ControllerBase controllerBase;//ControllerBase

    /// <summary>
    /// ゲーム開始直後に呼び出される
    /// </summary>
    void Start()
    {
        //ControllerBaseの初期設定を行う
        controllerBase.SetUpControllerBase();
    }
}
