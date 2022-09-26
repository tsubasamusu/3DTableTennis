using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// PlayerController�N���X��EnemyController�N���X�̐e�N���X
/// </summary>
public class ControllerBase : MonoBehaviour
{
    protected CharacterController charaController;//CharacterController

    protected RacketController racketController;//RacketController

    /// <summary>
    /// ControllerBase�̏����ݒ���s��
    /// </summary>
    public void SetUpControllerBase()
    {
        //CharacterController���擾
        charaController = GetComponent<CharacterController>();

        //RacketController���擾
        racketController = transform.GetChild(1).GetComponent<RacketController>();

        //RacketController�̏����ݒ���s��
        racketController.SetUpRacketController();
    }

    /// <summary>
    /// ���t���[���Ăяo�����
    /// </summary>
    private void Update()
    {
        //�L�����N�^�[�̌�����ݒ肷��
        SetCharaDirection();

        //���P�b�g��U���Ă���Œ��Ȃ�
        if(!racketController.IsIdle)
        {
            //�ȍ~�̏������s��Ȃ�
            return;
        }

        //�ړ�����
        Move();

        //���P�b�g�𐧌䂷��
        ControlRacket();
    }

    /// <summary>
    /// �ړ�����
    /// </summary>
    private void Move()
    {
        //�ړ������s����
        charaController.Move(GetMoveDir() * Time.deltaTime * GameData.instance.MoveSpeed + (Vector3.down * GameData.instance.Gravity));
    }

    /// <summary>
    /// �ړ��������擾����
    /// </summary>
    /// <returns></returns>
    protected virtual Vector3 GetMoveDir()
    {
        //�e�q�N���X�ŏ������L�q

        //��
        return Vector3.zero;
    }

    /// <summary>
    /// ���P�b�g�𐧌䂷��iUpdate���\�b�h�ŌĂяo���ꑱ����j
    /// </summary>
    protected virtual void ControlRacket()
    {
        //�e�q�N���X�ŏ������L�q
    }

    /// <summary>
    /// �L�����N�^�[�̌�����ݒ肷��iUpdate���\�b�h�ŌĂяo���ꑱ����j
    /// </summary>
    protected virtual void SetCharaDirection()
    {
        //�e�q�N���X�ŏ������L�q
    }
}
