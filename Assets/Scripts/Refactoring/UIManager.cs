using System.Collections;//IEnumerator���g�p
using System.Collections.Generic;//���X�g���g�p
using System;//Serializable�������g�p
using UnityEngine.UI;//UI���g�p
using DG.Tweening;//DOTween���g�p
using UnityEngine;

namespace yamap 
{
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

        private bool isUIEffect;//���o�I������p

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
            //�܂����o���I�����Ă��Ȃ���Ԃɐ؂�ւ���
            isUIEffect = false;

            //���_�̃L�����o�X�O���[�v���\���ɂ���
            cgScore.alpha = 0f;

            //�w�i�𔒐F�ɐݒ�
            imgBackground.color = new Color(Color.white.r, Color.white.g, Color.white.b, 1f);

            //���S���^�C�g���ɐݒ�
            imgLogo.sprite = GetLogoSprite(LogoType.Title);

            //�{�^����F�ɐݒ�
            imgButton.color = new Color(Color.blue.r, Color.blue.g, Color.blue.b, 0f);

            //�{�^���̃e�L�X�g���uStart�v�ɐݒ�
            txtButton.text = "Start";

            //�{�^���������ꂽ�ۂ̏�����ݒ�
            button.onClick.AddListener(() => ClickedButton(SoundDataSO.SoundName.GameStartSE));

            //�{�^����񊈐�������
            button.interactable = false;

            //Sequence���쐬
            Sequence sequence = DOTween.Sequence();

            //���o���s��
            {
                sequence.Append(imgLogo.DOFade(0f, 0f));
                sequence.Append(imgLogo.DOFade(1f, 1f));
                sequence.Append(imgButton.DOFade(1f, 1f));
                sequence.Join(cgButton.DOFade(1f, 1f))
                    .OnComplete(() => button.interactable = true).SetLink(gameObject);
            }

            //���o���I������܂ő҂�
            yield return new WaitUntil(() => isUIEffect == true);

            //�܂����o���I�����Ă��Ȃ���Ԃɐ؂�ւ���
            isUIEffect = false;
        }


        /// <summary>
        /// �{�^���������ꂽ�ۂ̏���
        /// </summary>
        /// <param name="seName">���ʉ��̖��O</param>
        private void ClickedButton(SoundDataSO.SoundName seName) 
        {
            //�{�^����񊈐�������
            button.interactable = false;

            //���ʉ����Đ�
            SoundManager.instance.PlaySound(seName);

            //���ʉ��̖��O�ɉ����ď�����ύX
            switch(seName)
            {
                //�Q�[���J�n���Ȃ�A�w�i�̃C���[�W������莞�Ԃ����Ĕ�\���ɂ���
                case SoundDataSO.SoundName.GameStartSE:imgBackground.DOFade(0f, 1f);break;

                //�Q�[���I�[�o�[���Ȃ�A�w�i�̃C���[�W����莞�Ԃ����Ĕ��F�ɕω�������
                case SoundDataSO.SoundName.GameOverSE: imgBackground.DOColor(Color.white, 1f);break;
            }

            //���S����莞�Ԃ����Ĕ�\���ɂ���
            imgLogo.DOFade(0f, 1f);

            //�{�^���̃L�����o�X�O���[�v����莞�Ԃ����Ĕ�\���ɂ���
            cgButton.DOFade(0f, 1f);

            //���o���I��������Ԃɐ؂�ւ���
            isUIEffect = true;
        }

        /// <summary>
        /// �Q�[���I�[�o�[���o���s��
        /// </summary>
        /// <returns>�҂�����</returns>
        public IEnumerator PlayGameOver() 
        {
            //�܂����o���I�����Ă��Ȃ���Ԃɐ؂�ւ���
            isUIEffect = false;

            //�w�i�����F�ɐݒ�
            imgBackground.color = new Color(Color.black.r, Color.black.g, Color.black.b, 0f);

            //���S���Q�[���I�[�o�[�ɐݒ�
            imgLogo.sprite = GetLogoSprite(LogoType.GameOver);

            //�{�^����ԐF�ɐݒ�
            imgButton.color = new Color(Color.red.r, Color.red.g, Color.red.b, 0f);

            //�{�^���̃e�L�X�g���uRestart�v�ɐݒ�
            txtButton.text = "Restart";

            //�{�^���ɓo�^����Ă��鏈�����폜
            button.onClick.RemoveAllListeners();

            //�{�^���������ꂽ�ۂ̏�����ݒ�
            button.onClick.AddListener(() => ClickedButton(SoundDataSO.SoundName.GameRestartSE));

            //�{�^����񊈐�������
            button.interactable = false;

            ////���_�̃L�����o�X�O���[�v����莞�Ԃ����Ĕ�\���ɂ���
            cgScore.DOFade(0f, 1f);

            //Sequence���쐬
            Sequence sequence = DOTween.Sequence();

            //���o���s��
            {
                sequence.Append(imgLogo.DOFade(0f, 0f));
                sequence.Append(imgBackground.DOFade(1f, 1f));
                sequence.Append(imgLogo.DOFade(1f, 1f));
                sequence.Append(imgButton.DOFade(1f, 1f))
                    .OnComplete(() => button.interactable = true);
            }

            //���o���I���܂ő҂�
            yield return new WaitUntil(() => isUIEffect == true);

            //�܂����o���I�����Ă��Ȃ���Ԃɐ؂�ւ���
            isUIEffect = false;
        }

        /// <summary>
        /// �Q�[���N���A���o���s��
        /// </summary>
        /// <returns>�҂�����</returns>
        public IEnumerator PlayGameClear() 
        {
            //�܂����o���I�����Ă��Ȃ���Ԃɐ؂�ւ���
            isUIEffect = false;

            //�w�i�𔒐F�ɐݒ�
            imgBackground.color = new Color(Color.white.r, Color.white.g, Color.white.b, 0f);

            //���S���Q�[���N���A�ɐݒ�
            imgLogo.sprite = GetLogoSprite(LogoType.GameClear);

            //�{�^�������F�ɐݒ�
            imgButton.color = new Color(Color.yellow.r, Color.yellow.g, Color.yellow.b, 0f);

            //�{�^���̃e�L�X�g���uRestart�v�ɐݒ�
            txtButton.text = "Restart";

            //�{�^���ɓo�^����Ă��鏈�����폜
            button.onClick.RemoveAllListeners();

            //�{�^���������ꂽ�ۂ̏�����ݒ�
            button.onClick.AddListener(() => ClickedButton(SoundDataSO.SoundName.PlayerPointSE));

            //�{�^����񊈐�������
            button.interactable = false;

            //���_�̃e�L�X�g����莞�Ԃ����ĐF�ɕς���
            txtScore.DOColor(Color.blue, 2f);

            //Sequence���쐬
            Sequence sequence = DOTween.Sequence();

            //���o���s��
            {
                sequence.Append(imgLogo.DOFade(0f, 0f));
                sequence.Append(imgBackground.DOFade(1f, 1f));
                sequence.Append(imgLogo.DOFade(1f, 1f));
                sequence.Append(imgButton.DOFade(1f, 1f));
                sequence.Join(cgButton.DOFade(1f, 1f))
                    .OnComplete(() => button.interactable = true);
            }

            //�Q�[���I�[�o�[���o���I���܂ő҂�
            yield return new WaitUntil(() => isUIEffect == true);

            //�܂����o���I�����Ă��Ȃ���Ԃɐ؂�ւ���
            isUIEffect = false;
        }

        /// <summary>
        /// ���_�̕\�����X�V���鏀�����s��
        /// �]���̏���(UI �\�������ƁA���o�����𕪂���)
        /// </summary>
        public void PrepareUpdateTxtScore() 
        {
            //���_�̃e�L�X�g��ݒ肷��
            txtScore.text = GameData.instance.score.playerScore.ToString() + ":" + GameData.instance.score.enemyScore.ToString();

            //���_�̕\�����X�V����
            StartCoroutine(PlayScoreEffect());
        }

        /// <summary>
        /// ���_�̕\�����X�V����iReactiveProperty�̍w�ǂɂ��A�l�X�V���ɃC�x���g�Ƃ��Ď����I�Ɏ��s�����j
        /// </summary>
        /// <param name="playerScore">�v���C���[�̓��_</param>
        /// <param name="enemyScore">�G�l�~�[�̓��_</param>
        public void UpdateDisplayScoreObservable(int playerScore, int enemyScore) 
        {
            //ReactiveProperty�ōw�ǂ��Ă�������󂯎��A�\�����X�V����(View ��)
            txtScore.text = playerScore + " : " + enemyScore;

            //���_�̕\�����X�V����
            StartCoroutine(PlayScoreEffect());
        }

        /// <summary>
        /// ���_�X�V���̉��o���s��
        /// </summary>
        /// <returns>�҂�����</returns>
        private IEnumerator PlayScoreEffect() 
        {    
            //���_�̃L�����o�X�O���[�v����莞�Ԃ����ĕ\������
            cgScore.DOFade(1f, 0.25f);

            //���_����莞�ԁA�\����������
            yield return new WaitForSeconds(0.25f + GameData.instance.DisplayScoreTime);

            //�v���C���[������������
            if (GameData.instance.score.playerScore == GameData.instance.MaxScore) 
            {
                //�ȍ~�̏������s��Ȃ�
                yield break;
            }

            //���_�̃L�����o�X�O���[�v����莞�Ԃ����Ĕ�\���ɂ���
            cgScore.DOFade(0f, 0.25f);
        }
    }
}