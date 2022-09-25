using System.Collections;
using System.Collections.Generic;
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

    /// <summary>
    /// �K�؂�y���W���擾����
    /// </summary>
    /// <returns>�K�؂�y���W</returns>
    private float GetAppropriatePosY()
    {
        //���˂�ʒu���擾
        Vector3 boundPos = isPlayerTurn ? playerBoundTran.position : enemyBoundTran.position;

        //���˂�ʒu�Ƃ̋������擾
        float length = Mathf.Abs(Vector3.Scale((transform.position - boundPos), new Vector3(1f, 0f, 1f)).magnitude);

        //�K�؂�y���W��Ԃ�
        return -(2f / 25f)*(length - 5f)*(length - 5f) + 2f;
    }
}
