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
    /// ���z�̒��˂�ʒu���擾����
    /// </summary>
    /// <param name="ballPos">�{�[���̈ʒu</param>
    /// <param name="angleY">�{�[����y�p�x</param>
    /// <returns>���˂�ʒu�̉��z�ʒu/returns>
    public Vector3 GetBoundPointPos(Vector3 ballPos,float angleY)
    {
        //�{�[���Ƃ̋������擾
        float length = Mathf.Abs((Vector3.Scale(transform.position, new Vector3(1f, 0f, 1f)) - Vector3.Scale(ballPos, new Vector3(1f, 0f, 1f))).magnitude);

        //�{�[����tan�Ƃ��擾
        float tan=Mathf.Tan(Mathf.Deg2Rad*angleY);

        //�i���j
        return Vector3.zero;
    }
}
