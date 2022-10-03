using UnityEngine;

/// <summary>
/// ���˂�n�_�Ɋւ��鏈�����s��
/// </summary>
public class BoundPoint : MonoBehaviour
{
    [SerializeField,Header("�N�̃R�[�g��")]
    private OwnerType ownerType;//���L�҂̎��

    [SerializeField,Header("���z�ʒu")]
    private Transform virtualBoundPointTran;//���z�ʒu

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
    /// <param name="ballTran">�{�[���̈ʒu���</param>
    /// <returns>���˂�ʒu�̉��z�ʒu/returns>
    public Vector3 GetVirtualBoundPointPos(Transform ballTran)
    {
        //���z�ʒu�̐e���{�[���ɐݒ�
        virtualBoundPointTran.SetParent(ballTran);

        //���z�ʒu�̌������{�[���̌����ɍ��킹��
        virtualBoundPointTran.localEulerAngles= Vector3.zero;

        //���z�ʒu�̍��W��������
        virtualBoundPointTran.position = transform.position;

        //���z�ʒu�̈ʒu��ݒ�
        virtualBoundPointTran.localPosition = new Vector3(0f,virtualBoundPointTran.localPosition.y,virtualBoundPointTran.localPosition.z);

        //���z�ʒu�̐e�������ɐݒ�i�e�������j
        virtualBoundPointTran.SetParent(transform);

        //���z�ʒu��Ԃ�
        return virtualBoundPointTran.position;
    }
}
