using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private ControllerBase controllerBase;//ControllerBase

    /// <summary>
    /// �Q�[���J�n����ɌĂяo�����
    /// </summary>
    void Start()
    {
        //ControllerBase�̏����ݒ���s��
        controllerBase.SetUpControllerBase();
    }
}
