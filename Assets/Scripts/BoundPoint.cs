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
}
