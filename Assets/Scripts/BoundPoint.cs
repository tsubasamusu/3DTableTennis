using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���˂�n�_�Ɋւ��鏈�����s��
/// </summary>
public class BoundPoint : MonoBehaviour
{
    [SerializeField,Header("�N�̃R�[�g��")]
    private OwnerType ownerType;//���L�҂̎��

    /// <summary>
    /// �R�[�g�̏��L�҂��擾����
    /// </summary>
    /// <returns>�R�[�g�̏��L��</returns>
    public OwnerType GetOwnerTypeOfCourt()
    {
        //���L�҂̎�ނ�Ԃ�
        return ownerType;
    }

    /// <summary>
    /// ���˂�ʒu�̉��z�ʒu���擾����
    /// </summary>
    /// <param name="ballPos">�{�[���̈ʒu</param>
    /// <param name="direction">�{�[���̐i�s����</param>
    /// <returns>���˂�ʒu�̉��z�ʒu/returns>
    public Vector3 GetBoundPointPos(Vector3 ballPos,Vector3 direction)
    {
        //�i���j
        return Vector3.zero;
    }
}
