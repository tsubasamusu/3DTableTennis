using System.Collections.Generic;//���X�g���g�p 
using System;//Serializable�������g�p
using UnityEngine; 

//�A�Z�b�g���j���[�ŁuCreate SoundDataSO�v��I������ƁuSoundDataSO�v���쐬�ł��� 
[CreateAssetMenu(fileName = "SoundDataSO", menuName = "Create SoundDataSO")]
public class SoundDataSO : ScriptableObject
{
    /// <summary>  
    /// ���̖��O  
    /// </summary>  
    public enum SoundName
    {
        MainBGM,//���C��BGM
        GameStartSE,//�Q�[���X�^�[�g��
        GameOverSE,//�Q�[���I�[�o�[��
        GameClearSE,//�Q�[���N���A��
        PlayerPointSE,//�v���C���[�����_�������̉�
        EnemyPointSE,//�G�l�~�[�����_�������̉�
        RacketSE,//�{�[�������P�b�g�ɓ����������̉�
        BoundSE,//�{�[�����싅��Ńo�E���h�������̉�
    }

    /// <summary>  
    /// ���̃f�[�^���Ǘ�����
    /// </summary>  
    [Serializable]
    public class SoundData
    {
        public SoundName name;//���O 
        public AudioClip clip;//�N���b�v
    }

    public List<SoundData> soundDataList = new();//���̃f�[�^�̃��X�g 
}