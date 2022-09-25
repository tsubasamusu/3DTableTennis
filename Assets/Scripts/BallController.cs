using System.Collections;//IEnumerator���g�p
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

    private OwnerType currentOwner;//���݂̒e�̏��L��

    /// <summary>
    /// �{�[����ł�
    /// </summary>
    /// <param name="direction">�ł���</param>
    /// <param name="ownerType">�{�[���̏��L��</param>
    public void ShotBall(Vector3 direction, OwnerType ownerType)
    {
        //���݂̃{�[���̏��L�҂��擾
        currentOwner = ownerType;

        //�����𔭎˂��鍂�����擾
        float posY = ownerType == OwnerType.Player ? 0.25f : 0.75f;

        //�������쐬 
        Ray ray = new(new Vector3(transform.position.x, posY, transform.position.z), direction);

        //���������̃R���C�_�[�ɐG��Ȃ�������
        if (!Physics.Raycast(ray, out RaycastHit hit, 10f))
        {
            //�{�[�����ړ�������
            StartCoroutine(MoveBall(false, direction));

            //�ȍ~�̏������s��Ȃ�
            return;
        }

        //�ڐG���肪�R�[�g�ł͂Ȃ��Ȃ�
        if (!hit.transform.TryGetComponent(out BoundPoint boundPoint))
        {
            //�{�[�����ړ�������
            StartCoroutine(MoveBall(false, direction));

            //�ȍ~�̏������s��Ȃ�
            return;
        }

        //���݂̃{�[���̏��L�҂̃R�[�g�ɐG�ꂽ��
        if (boundPoint.GetOwnerTypeOfCourt() == ownerType)
        {
            //�{�[�����ړ�������
            StartCoroutine(MoveBall(true, direction));
        }
    }

    /// <summary>
    /// �K�؂�y���W���擾����
    /// </summary>
    /// <param name="inCourt">�R�[�g�ɓ��邩�ǂ���</param>
    /// <returns>�K�؂�y���W</returns>
    private float GetAppropriatePosY(bool inCourt)
    {
        //���˂�ʒu���擾
        Vector3 boundPos = currentOwner==OwnerType.Player ? playerBoundTran.position : enemyBoundTran.position;

        //���˂�ʒu�Ƃ̋������擾
        float length = Mathf.Abs((Vector3.Scale(transform.position, new Vector3(1f, 0f, 1f))- Vector3.Scale(boundPos, new Vector3(1f, 0f, 1f))).magnitude);

        //�R�[�g�ɓ��炸�A���ȉ��̒Ⴓ�ɂȂ�����
        if(!inCourt&&transform.position.y<=0.8f)
        {
            //�����𕉂ɂ���i����������j
            length *= -1f;
        }

        //�K�؂�y���W��Ԃ�
        return -(0.75f / 25f) * (length - 5f) * (length - 5f) + 1.5f;
    }

    /// <summary>
    /// �{�[�����ړ�������
    /// </summary>
    /// <param name="inCourt">�R�[�g�ɓ��邩�ǂ���</param>
    /// <param name="direction">�{�[���̈ړ�����</param>
    /// <returns>�҂�����</returns>
    private IEnumerator MoveBall(bool inCourt, Vector3 direction)
    {
        //�{�[���̏��L�҂�ێ�
        OwnerType ownerType=currentOwner;

        //�{�[���̏��L�҂��ς��Ȃ��i�ԋ�����Ă��Ȃ��j�ԁA�J��Ԃ�
        while(ownerType==currentOwner)
        {
            //�{�[���̈ړ��������擾
            direction = Vector3.Scale(direction, new Vector3(1f, 0f, 1f)).normalized;

            //�{�[�����ړ�������
            transform.Translate(direction * GameData.instance.BallSpeed * Time.deltaTime);

            //y���W���X�V����
            transform.position = new Vector3(transform.position.x, GetAppropriatePosY(inCourt), transform.position.z);

            //���̃t���[���֔�΂��i�����AUpdate���\�b�h�j
            yield return null;
        }
    }
}
