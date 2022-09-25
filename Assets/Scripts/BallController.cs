using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

/// <summary>
/// �{�[���̓����𐧌䂷��
/// </summary>
public class BallController : MonoBehaviour
{
    [SerializeField]
    private Transform playerBoundTran;//�v���C���[�p�̒��˂�ʒu

    [SerializeField]
    private Transform enemyBoundTran;//�G�l�~�[�p�̒��˂�ʒu

    private bool isPlayerTurn;//�v���[���[�̃^�[�����ǂ���

    private bool isBounded;//���Œ��˂����ǂ���

    /// <summary>
    /// �u���Œ��˂����ǂ����v�̐ݒ�p
    /// </summary>
    public bool IsBounded { set=>isBounded=value; }

    /// <summary>
    /// �{�[����ł�
    /// </summary>
    /// <param name="direction">�ł���</param>
    public void ShotBall(Vector3 direction)
    {
        //�܂����˂Ă��Ȃ���Ԃɐ؂�ւ���
        isBounded = false;
    }

    /// <summary>
    /// �K�؂�y���W���擾����
    /// </summary>
    /// <param name="inCourt">�R�[�g�ɓ��邩�ǂ���</param>
    /// <returns>�K�؂�y���W</returns>
    private float GetAppropriatePosY(bool inCourt)
    {
        //���˂�ʒu���擾
        Vector3 boundPos = isPlayerTurn ? playerBoundTran.position : enemyBoundTran.position;

        //���˂�ʒu�Ƃ̋������擾
        float length = (Vector3.Scale(transform.position, new Vector3(1f, 0f, 1f))- Vector3.Scale(boundPos, new Vector3(1f, 0f, 1f))).magnitude;

        //�K�؂�y���W��Ԃ�
        return-(0.75f/25f)*(length - 5f)*(length - 5f) + 1.5f;
    }

    private void Update()
    {
        transform.position = new Vector3(transform.position.x,GetAppropriatePosY(true),transform.position.z);
    }
}
