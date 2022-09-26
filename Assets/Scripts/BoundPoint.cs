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
    public Vector3 GetVirtualBoundPointPos(Vector3 ballPos, float angleY)
    {
        //�{�[���Ƃ̋������擾
        float length = Mathf.Abs((Vector3.Scale(transform.position, new Vector3(1f, 0f, 1f)) - Vector3.Scale(ballPos, new Vector3(1f, 0f, 1f))).magnitude);

        //�{�[����a�i�uy=ax+b�v�́ua�i�X���j�v�j���擾
        float a = Mathf.Tan(Mathf.Deg2Rad * (90f - angleY));

        //�{�[����b�i�uy=ax+b�v�́ub�i�ؕЁj�v�j���擾
        float b = ballPos.z - a * ballPos.x;

        //�|����l���擾
        float multiplyValue = ballPos.x >= 0f ? -1f : 1f;

        //���z�ʒu��x�������擾
        float virtualX = (2f * (ballPos.x + (a * ballPos.z) - (a * b))
            + Mathf.Sqrt(4f * ((a * b) - (a * ballPos.z) - ballPos.x) * ((a * b) - (a * ballPos.z) - ballPos.x)
            - 4f * ((a * a) + 1f) * ((ballPos.x * ballPos.x) + (2f * b) - (2f * b * ballPos.z) - (2f * ballPos.z) - (length * length))) * multiplyValue)
            / 2f * ((a * a) + 1f);

        //���z�ʒu��z�������擾
        float virtualZ = a * virtualX + b;

        //�쐬�������z�ʒu��Ԃ�
        return new Vector3(virtualX, transform.position.y, virtualZ);
    }
}
