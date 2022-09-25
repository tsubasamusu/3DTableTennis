using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// PlayerController�N���X��EnemyController�N���X�̐e�N���X
/// </summary>
public class ControllerBase : MonoBehaviour
{
    protected CharacterController charaController;//CharacterController

    /// <summary>
    /// ControllerBase�̏����ݒ���s��
    /// </summary>
    public void SetUpControllerBase()
    {
        //CharacterController���擾
        charaController = GetComponent<CharacterController>();

        //�eController�̏����ݒ���s��
        SetUpController();
    }

    /// <summary>
    /// ���t���[���Ăяo�����
    /// </summary>
    private void Update()
    {
        //�ړ�����
        Move();
    }

    /// <summary>
    /// �ړ�����
    /// </summary>
    private void Move()
    {
        charaController.Move(GetMoveDir() * Time.deltaTime * GameData.instance.MoveSpeed + (Vector3.down * GameData.instance.Gravity));
    }

    /// <summary>
    /// �ړ��������擾����
    /// </summary>
    /// <returns></returns>
    protected virtual Vector3 GetMoveDir()
    {
        //TODO:�e�q�N���X�ŏ������L�q

        //��
        return Vector3.zero;
    }

    /// <summary>
    /// �eController�̏����ݒ���s��
    /// </summary>
    protected virtual void SetUpController()
    {
        //TODO:�e�q�N���X�ŏ������L�q
    }
}
