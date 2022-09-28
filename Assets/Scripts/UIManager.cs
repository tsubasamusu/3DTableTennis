using System.Collections;//IEnumerator���g�p
using System.Collections.Generic;//���X�g���g�p
using UnityEngine;
using System;//Serializable�������g�p
using UnityEngine.UI;//UI���g�p
using DG.Tweening;//DOTween���g�p

/// <summary>
/// UI�𐧌䂷��
/// </summary>
public class UIManager : MonoBehaviour
{
    /// <summary>
    /// ���S�̎��
    /// </summary>
    private enum LogoType
    {
        Title, GameOver, GameClear//�񋓎q
    }

    /// <summary>
    /// ���S�̃f�[�^���Ǘ����� 
    /// </summary>
    [Serializable]
    private class LogoData
    {
        public LogoType LogoType;//���S�̎��
        public Sprite sprite;//�X�v���C�g
    }

    [SerializeField]
    private List<LogoData> logoDatasList = new();//���S�̃f�[�^�̃��X�g

    [SerializeField]
    private Image imgLogo;//���S�̃C���[�W

    [SerializeField]
    private Image imgBackground;//�w�i�̃C���[�W

    [SerializeField]
    private CanvasGroup cgScore;//���_�̃L�����o�X�O���[�v

    [SerializeField]
    private Text txtScore;//���_�̃e�L�X�g

    [SerializeField]
    private CanvasGroup cgButton;//�{�^���̃L�����o�X�O���[�v

    [SerializeField]
    private Button button;//�{�^��

    [SerializeField]
    private Image imgButton;//�{�^���̃C���[�W

    [SerializeField]
    private Text txtButton;//�{�^���̃e�L�X�g

    /// <summary>
    /// ���S�̃X�v���C�g���擾����
    /// </summary>
    /// <param name="logoType">���S�̎��</param>
    /// <returns>���S�̃X�v���C�g</returns>
    private Sprite GetLogoSprite(LogoType logoType)
    {
        //���S�̃X�v���C�g��Ԃ�
        return logoDatasList.Find(x => x.LogoType == logoType).sprite;
    }

    /// <summary>
    /// �Q�[���X�^�[�g���o���s��
    /// </summary>
    /// <returns>�҂�����</returns>
    public IEnumerator PlayGameStart()
    {
        //�Q�[���X�^�[�g���o�I������p
        bool end = false;

        //�w�i�𔒐F�ɐݒ�
        imgBackground.color = new Color(Color.white.r, Color.white.g, Color.white.b, 1f);

        //���S���^�C�g���ɐݒ�
        imgLogo.sprite = GetLogoSprite(LogoType.Title);

        //�{�^����F�ɐݒ�
        imgButton.color = new Color(Color.blue.r, Color.blue.g, Color.blue.b, 0f);

        //�{�^���̃e�L�X�g���uStart�v�ɐݒ�
        txtButton.text = "Start";

        //�{�^���������ꂽ�ۂ̏�����ݒ�
        button.onClick.AddListener(() => ClickedButton());

        //�{�^����񊈐�������
        button.interactable = false;

        //���S���\���ɂ���
        imgLogo.DOFade(0f, 0f)

            //���S����莞�Ԃ����ĕ\������
            .OnComplete(() => imgLogo.DOFade(1f, 1f)

            .OnComplete(() =>
            {
                //�{�^���̃C���[�W����莞�Ԃ����ĕ\������
                { imgButton.DOFade(1f, 1f); }

                {
                    //�{�^���̃L�����o�X�O���[�v����莞�Ԃ����ĕ\������
                    cgButton.DOFade(1f, 1f)

                    //�{�^��������������
                    .OnComplete(() => button.interactable = true);
                }

            }));

        //�{�^���������ꂽ�ۂ̏���
        void ClickedButton()
        {
            //�w�i����莞�Ԃ����Ĕ�\���ɂ���
            imgBackground.DOFade(0f, 1f);

            //���S����莞�Ԃ����Ĕ�\���ɂ���
            imgLogo.DOFade(0f, 1f);

            //�{�^���̃L�����o�X�O���[�v����莞�Ԃ����Ĕ�\���ɂ���
            cgButton.DOFade(0f, 1f)

                //�Q�[���X�^�[�g���o���I��������Ԃɐ؂�ւ���
                .OnComplete(()=>end=true);

            //�{�^����񊈐�������
            button.interactable = false;
        }

        //�Q�[���X�^�[�g���o���I���܂ő҂�
        yield return new WaitUntil(() => end == true);
    }

    /// <summary>
    /// �Q�[���I�[�o�[���o���s��
    /// </summary>
    /// <returns>�҂�����</returns>
    public IEnumerator PlayGameOver()
    {
        //�Q�[���I�[�o�[���o�I������p
        bool end = false;

        //�w�i�����F�ɐݒ�
        imgBackground.color = new Color(Color.black.r,Color.black.g,Color.black.b,0f);

        //���S���Q�[���I�[�o�[�ɐݒ�
        imgLogo.sprite = GetLogoSprite(LogoType.GameOver);

        //�{�^����ԐF�ɐݒ�
        imgButton.color = new Color(Color.red.r, Color.red.g, Color.red.b, 0f);

        //�{�^���̃e�L�X�g���uRestart�v�ɐݒ�
        txtButton.text = "Restart";

        //�{�^���������ꂽ�ۂ̏�����ݒ�
        button.onClick.AddListener(() => ClickedButton());

        //�{�^����񊈐�������
        button.interactable = false;

        //�w�i����莞�Ԃ����ĕ\������
        imgBackground.DOFade(1f,1f)

        //���S���\���ɂ���
        .OnComplete(()=> imgLogo.DOFade(0f, 0f)

            //���S����莞�Ԃ����ĕ\������
            .OnComplete(() => imgLogo.DOFade(1f, 1f)

            .OnComplete(() =>
            {
                //�{�^���̃C���[�W����莞�Ԃ����ĕ\������
                { imgButton.DOFade(1f, 1f); }

                {
                    //�{�^���̃L�����o�X�O���[�v����莞�Ԃ����ĕ\������
                    cgButton.DOFade(1f, 1f)

                    //�{�^��������������
                    .OnComplete(() => button.interactable = true);
                }

            })));

        //�{�^���������ꂽ�ۂ̏���
        void ClickedButton()
        {
            //�w�i����莞�Ԃ����Ĕ��F�ɂ���
            imgBackground.DOColor(Color.white, 1f);

            //���S����莞�Ԃ����Ĕ�\���ɂ���
            imgLogo.DOFade(0f, 1f);

            //�{�^���̃L�����o�X�O���[�v����莞�Ԃ����Ĕ�\���ɂ���
            cgButton.DOFade(0f, 1f)

                //�Q�[���X�^�[�g���o���I��������Ԃɐ؂�ւ���
                .OnComplete(() => end = true);

            //�{�^����񊈐�������
            button.interactable = false;
        }

        //�Q�[���I�[�o�[���o���I���܂ő҂�
        yield return new WaitUntil(() => end == true);
    }
}
