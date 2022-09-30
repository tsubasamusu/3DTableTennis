using System.Collections;//IEnumerator���g�p
using UnityEngine;

/// <summary>
/// �{�[���̓����𐧌䂷��
/// </summary>
public class BallController : MonoBehaviour
{
    [SerializeField]
    private BoundPoint playerBoundPoint;//�v���C���[�p�̒��˂�ʒu

    [SerializeField]
    private BoundPoint enemyBoundPoint;//�G�l�~�[�p�̒��˂�ʒu

    private OwnerType currentOwner = OwnerType.Enemy;//���݂̒e�̏��L��

    private Vector3 currentBoundPos;//���݂̒��˂�ʒu

    private bool inCourt;//�R�[�g�ɓ��邩�ǂ���

    private bool stopMove;//�{�[���̓������~�߂邩�ǂ���

    private bool playedBoundSE;//�{�[�����싅��Œ��˂鉹���Đ��������ǂ���

    /// <summary>
    /// �u�R�[�g�ɓ��邩�ǂ����v�̎擾�p
    /// </summary>
    public bool InCourt { get => inCourt; }

    /// <summary>
    /// �u�{�[���̌��݂̏��L�ҁv�̎擾�p
    /// </summary>
    public OwnerType CurrentOwner { get => currentOwner; }

    /// <summary>
    /// BallController�̏����ݒ���s��
    /// </summary>
    public void SetUpBallController()
    {
        //�{�[���������ʒu�Ɉړ�������
        transform.position = new Vector3(0f, 1f, -3f);
    }

    /// <summary>
    /// �{�[����ł�
    /// </summary>
    public void ShotBall()
    {
        //���ʉ����Đ����Ă��Ȃ���Ԃɐ؂�ւ���
        playedBoundSE = false;

        //���ʉ����Đ�
        SoundManager.instance.PlaySound(SoundDataSO.SoundName.RacketSE);

        //�����𔭎˂��鍂�����擾
        float posY = currentOwner == OwnerType.Player ? 0.25f : 0.75f;

        //�������쐬 
        Ray ray = new(new Vector3(transform.position.x, posY, transform.position.z), transform.forward);

        //�R�[�g�ɓ���Ȃ���Ԃŉ��ɓo�^����
        inCourt = false;

        //���������̃R���C�_�[�ɐG��Ȃ�������
        if (!Physics.Raycast(ray, out RaycastHit hit, 10f))
        {
            //�{�[�����ړ������鏀�����s��
            PrepareMoveBall();

            //�ȍ~�̏������s��Ȃ�
            return;
        }

        //�ڐG���肪�R�[�g�ł͂Ȃ��Ȃ�
        if (!hit.transform.TryGetComponent(out BoundPoint boundPoint))
        {
            //�{�[�����ړ������鏀�����s��
            PrepareMoveBall();

            //�ȍ~�̏������s��Ȃ�
            return;
        }

        //���݂̃{�[���̏��L�҂̃R�[�g�ɐG�ꂽ��
        if (boundPoint.GetOwnerTypeOfCourt() == currentOwner)
        {
            //�R�[�g�ɓ����Ԃœo�^����
            inCourt = true;

            //�{�[�����ړ������鏀�����s��
            PrepareMoveBall();
        }
    }

    /// <summary>
    /// �K�؂�y���W���擾����
    /// </summary>
    /// <returns>�K�؂�y���W</returns>
    private float GetAppropriatePosY()
    {
        //���˂�ʒu�Ƃ̋������擾
        float length = Mathf.Abs((Vector3.Scale(transform.position, new Vector3(1f, 0f, 1f)) - Vector3.Scale(currentBoundPos, new Vector3(1f, 0f, 1f))).magnitude);

        //�R�[�g�ɓ��炸�A���ȉ��̒Ⴓ�ɂȂ�����
        if (!inCourt && transform.position.y <= 0.8f)
        {
            //�����𕉂ɂ���i���˂������A����������j
            length *= -1f;
        }

        //�K�؂�y���W��Ԃ�
        return -(0.75f / 25f) * (length - 5f) * (length - 5f) + 1.5f;
    }

    /// <summary>
    /// �{�[�����ړ������鏀�����s��
    /// </summary>
    private void PrepareMoveBall()
    {
        //�{�[�����ړ�������
        StartCoroutine(MoveBall());
    }

    /// <summary>
    /// �{�[�����ړ�������
    /// </summary>
    /// <returns>�҂�����</returns>
    private IEnumerator MoveBall()
    {
        //�{�[���̏��L�҂�ێ�
        OwnerType ownerType = currentOwner;

        //�{�[���̏��L�҂��ς��Ȃ��i�ԋ�����Ă��Ȃ��j�ԁA�J��Ԃ�
        while (ownerType == currentOwner)
        {
            //�{�[���̓������~�߂�w�����o����
            if (stopMove)
            {
                //�J��Ԃ��������I������
                break;
            }

            //�{�[���̌����Ɉړ�������
            transform.position += transform.forward * GameData.instance.BallSpeed * Time.deltaTime;

            //y���W���X�V����
            transform.position = new Vector3(transform.position.x, Mathf.Clamp(GetAppropriatePosY(), 0.25f, 10f), transform.position.z);

            //���ʉ��Đ���Ȃ�
            if (playedBoundSE)
            {
                //1�t���[���҂i�����AUpdate���\�b�h�j
                yield return null;

                //���̌J��Ԃ������Ɉڂ�
                continue;
            }

            //�R�[�g�ɓ���Ȃ��Ȃ�
            if (!InCourt)
            {
                //1�t���[���҂i�����AUpdate���\�b�h�j
                yield return null;

                //���̌J��Ԃ������Ɉڂ�
                continue;
            }

            //�{�[���̍��������ȉ��ɂȂ�����
            if (transform.position.y <= 0.8f)
            {
                //���ʉ����Đ�
                SoundManager.instance.PlaySound(SoundDataSO.SoundName.BoundSE);

                //���ʉ��Đ���ɐ؂�ւ���
                playedBoundSE = true;
            }

            //1�t���[���҂i�����AUpdate���\�b�h�j
            yield return null;
        }
    }

    /// <summary>
    /// ���̃R���C�_�[�ɐڐG�����ۂɌĂяo�����
    /// </summary>
    /// <param name="other">�ڐG����</param>
    private void OnTriggerEnter(Collider other)
    {
        //���P�b�g�ɐG�ꂽ��
        if (other.TryGetComponent(out RacketController racketController))
        {
            //���݂̃{�[���̏��L�҂ƁA�{�[����ł����Ґl�������Ȃ�i��x�ł����ꂽ��j
            if (currentOwner == racketController.OwnerType)
            {
                //�ȍ~�̏������s��Ȃ�
                return;
            }

            //��~���߂�����
            stopMove = false;

            //�{�[���̏��L�҂�o�^
            currentOwner = racketController.OwnerType;

            //�{�[���̌�����ݒ�
            transform.eulerAngles = new Vector3(0f, racketController.transform.root.transform.eulerAngles.y, 0f);

            //�{�[���̏��L�҂ɉ����Ď擾���钵�˂�ʒu��ύX
            currentBoundPos = (currentOwner == OwnerType.Player ? playerBoundPoint : enemyBoundPoint)

                //���z�̒��˂�ʒu���擾
                .GetVirtualBoundPointPos(transform, racketController.transform.root.transform.eulerAngles.y);

            //�{�[����ł�
            ShotBall();
        }
    }

    /// <summary>
    /// �T�[�u����ăX�^�[�g���邽�߂̏������s��
    /// </summary>
    /// <param name="server">�N���T�[�u�����邩</param>
    /// <param name="playerController">PlayerController</param>
    public void PrepareRestartGame(OwnerType server,PlayerController playerController)
    {
        //�T�[�u����ăX�^�[�g����
        StartCoroutine(RestartGame(server,playerController));
    }

    /// <summary>
    /// �T�[�u����ăX�^�[�g����
    /// </summary>
    /// <param name="server">�N���T�[�u�����邩</param>
    /// <param name="playerController">PlayerController</param>
    /// <returns>�҂�����</returns>
    private IEnumerator RestartGame(OwnerType server,PlayerController playerController)
    {
        //�{�[���̓������~�߂�
        stopMove = true;

        //�R�[�g�ɓ���Ȃ���Ԃɂ���i�{�[���̏�Ԃ��������j
        inCourt = false;

        //�T�[�u������l�ɉ����ă{�[���̈ʒu��ύX
        transform.position = new Vector3(0f, 1f, server == OwnerType.Player ? -3f : 3f);

        //���݂̃{�[���̏��L�҂�ݒ�
        currentOwner = server == OwnerType.Player ? OwnerType.Enemy : OwnerType.Player;

        //�v���C���[�̈ʒu��������
        playerController.ResetPlayerPos();

        //�T�[�o�[���G�l�~�[�Ȃ�
        if (server == OwnerType.Enemy)
        {
            //��莞�ԑ҂i�G�l�~�[���T�[�u��ł܂ł̎��Ԃ�݂���j
            yield return new WaitForSeconds(GameData.instance.EnemyServeTime);
        }

        //�R�[�g�ɓ����Ԃɂ���i�G�l�~�[�ɓ����Ă��炤���߁j
        inCourt = true;
    }
}
