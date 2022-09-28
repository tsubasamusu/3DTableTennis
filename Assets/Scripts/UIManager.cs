using System.Collections;
using System.Collections.Generic;//���X�g���g�p
using UnityEngine;
using System;//Serializable�������g�p
using UnityEngine.UI;//UI���g�p

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
        Title,GameOver,GameClear//�񋓎q
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
    private CanvasGroup cvScore;//���_�̃L�����o�X�O���[�v

    [SerializeField]
    private Text txtScore;//���_�̃e�L�X�g

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
}
