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
        //�e�q�N���X�ŏ������L�q
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
