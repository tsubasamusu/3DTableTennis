using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �싅��Ɋւ��鏈�����s��
/// </summary>
public class Table : MonoBehaviour
{
    /// <summary>
    /// ���̃R���C�_�[�ɐڐG�����ۂɌĂяo�����
    /// </summary>
    /// <param name="other">�ڐG����</param>
    private void OnTriggerEnter(Collider other)
    {
        //�ڐG���肪�{�[���Ȃ�
        if (other.TryGetComponent(out BallController ballController))
        {
            //�{�[�������˂���Ԃɐ؂�ւ���
            ballController.IsBounded = true;
        }
    }
}
