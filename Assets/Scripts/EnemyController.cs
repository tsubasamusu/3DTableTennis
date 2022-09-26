using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �G�l�~�[�̍s���𐧌䂷��
/// </summary>
public class EnemyController : ControllerBase
{
    [SerializeField]
    private BoundPoint enemyBoundPoint;//�G�l�~�[�̒��˂�ʒu

    [SerializeField]
    private BallController ballController;//BallController

    /// <summary>
    /// �ړ��������擾����
    /// </summary>
    /// <returns></returns>
    protected override Vector3 GetMoveDir()
    {
        //�e�q�N���X�ŏ������L�q

        //��
        return Vector3.zero;
    }

    /// <summary>
    /// EnemyController�̏����ݒ���s��
    /// </summary>
    public void SetUpEnemyController(BallController ballController)
    {
        //BallController���擾
        this.ballController = ballController;
    }

    /// <summary>
    /// ���P�b�g�𐧌䂷��iUpdate���\�b�h�ŌĂяo���ꑱ����j
    /// </summary>
    protected override void ControlRacket()
    {
        //�{�[�����U�������ɓ����Ă��Ȃ�������
        if (Mathf.Abs((transform.position - ballController.transform.position).magnitude) > GameData.instance.EnemyShotRange)
        {
            //�ȍ~�̏������s��Ȃ�
            return;
        }

        //���P�b�g��U���Ă���Œ��Ȃ�
        if(!racketController.IsIdle)
        {
            //�ȍ~�̏������s��Ȃ�
            return ;
        }

        //�t�H�A�n���h�h���C�u�ɂ��邩�o�b�N�n���h�h���C�u�ɂ��邩�������_���Ɍ��߂āA�h���C�u������
        racketController.Drive(Random.Range(0, 2) == 0 ? true : false);
    }

    /// <summary>
    /// �L�����N�^�[�̌�����ݒ肷��iUpdate���\�b�h�ŌĂяo���ꑱ����j
    /// </summary>
    protected override void SetCharaDirection()
    {
        //��ɃG�l�~�[�̃R�[�g�̕����Ɍ���
        transform.LookAt(enemyBoundPoint.transform.position);
    }
}
