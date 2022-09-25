using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �v���[���[�̍s���𐧌䂷��
/// </summary>
public class PlayerController : ControllerBase
{
    private Transform mainCameraTran;//���C���J�����̈ʒu���

    /// <summary>
    /// PlayerController�̏����ݒ���s��
    /// </summary>
    protected override void SetUpController()
    {
        //���C���J�����̈ʒu�����擾
        mainCameraTran = Camera.main.transform;
    }

    /// <summary>
    /// �ړ��������擾����
    /// </summary>
    protected override Vector3 GetMoveDir()
    {
        //���E�ړ��̓��͂��擾
        float moveH = Input.GetAxis("Horizontal");

        //�O��ړ��̓��͂��擾
        float moveV = Input.GetAxis("Vertical");

        //���̈ړ�������ݒ�
        Vector3 movement = new Vector3(moveH, 0, moveV);

        //�ړ��������擾���A�Ԃ�
        return mainCameraTran.forward * movement.z + mainCameraTran.right * movement.x;
    }

    /// <summary>
    /// ���P�b�g�𐧌䂷��iUpdate���\�b�h�ŌĂяo���ꑱ����j
    /// </summary>
    protected override void ControlRacket()
    {
        //���N���b�N���ꂽ��
        if (Input.GetMouseButtonDown(0))
        {
            //�o�b�N�n���h�h���C�u������
            racketController.Drive(false);
        }
        //�E�N���b�N���ꂽ��
        else if (Input.GetMouseButtonDown(1))
        {
            //�t�H�A�n���h�h���C�u������
            racketController.Drive(true);
        }
    }

    /// <summary>
    /// �L�����N�^�[�̌�����ݒ肷��iUpdate���\�b�h�ŌĂяo���ꑱ����j
    /// </summary>
    protected override void SetCharaDirection()
    {
        //�L�����N�^�[�̌�����ݒ肷��
        transform.eulerAngles = new Vector3(0f, mainCameraTran.eulerAngles.y, 0f);
    }
}
