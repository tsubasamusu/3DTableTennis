using System.Collections;//IEnumerator���g�p
using System.Collections.Generic;//���X�g���g�p
using System;//Serializable�������g�p
using UnityEngine.UI;//UI���g�p
using DG.Tweening;//DOTween���g�p
using UnityEngine;

namespace yamap {

    /// <summary>
    /// UI�𐧌䂷��
    /// </summary>
    public class UIManager : MonoBehaviour {
        /// <summary>
        /// ���S�̎��
        /// </summary>
        private enum LogoType {
            Title, GameOver, GameClear//�񋓎q
        }

        /// <summary>
        /// ���S�̃f�[�^���Ǘ����� 
        /// </summary>
        [Serializable]
        private class LogoData {
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

        private bool isUIEffect;

        /// <summary>
        /// ���S�̃X�v���C�g���擾����
        /// </summary>
        /// <param name="logoType">���S�̎��</param>
        /// <returns>���S�̃X�v���C�g</returns>
        private Sprite GetLogoSprite(LogoType logoType) {
            //���S�̃X�v���C�g��Ԃ�
            return logoDatasList.Find(x => x.LogoType == logoType).sprite;
        }

        /// <summary>
        /// �Q�[���X�^�[�g���o���s��
        /// </summary>
        /// <returns>�҂�����</returns>
        public IEnumerator PlayGameStart() {
            //�Q�[���X�^�[�g���o�I������p
            //bool end = false;

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

            // Sequence �ō��  ->  �l�X�g���Ȃ��Ȃ�A�ǐ��������Ȃ�
            Sequence sequence = DOTween.Sequence();

            sequence.Append(imgLogo.DOFade(0f, 0f).SetEase(Ease.Linear));
            sequence.Append(imgLogo.DOFade(1f, 1f).SetEase(Ease.Linear));
            sequence.Append(imgButton.DOFade(1f, 1f));
            sequence.Join(cgButton.DOFade(1f, 1f))
                .OnComplete(() => button.interactable = true).SetLink(gameObject);

            // �Q�[���X�^�[�g���o���I���܂ő҂�  ���[�J���֐��́A���\�b�h���ł���΂ǂ��ɏ����Ă����Ȃ��̂ŁA������ǂ݂₷������悤�ɍl���Ă݂Ă��������B
            // �P�[�X�o�C�P�[�X�ł����A����̂悤�ɑҋ@�����̑O�Ƀ��[�J���֐�������Ă��܂��ƁA�����̑S�̗̂��ꂪ�ǂ݂ɂ����Ȃ�܂��B
            // ���̂��߁A�܂��̓��\�b�h�S�̂̏����������Ă��烍�[�J���֐�������Ă݂܂��傤�B

            yield return new WaitUntil(() => isUIEffect == true);
            isUIEffect = false;

            ////���S���\���ɂ���
            //imgLogo.DOFade(0f, 0f)

            //    //���S����莞�Ԃ����ĕ\������
            //    .OnComplete(() => imgLogo.DOFade(1f, 1f)

            //    .OnComplete(() => {
            //        //�{�^���̃C���[�W����莞�Ԃ����ĕ\������
            //        { imgButton.DOFade(1f, 1f); }

            //        {
            //            //�{�^���̃L�����o�X�O���[�v����莞�Ԃ����ĕ\������
            //            cgButton.DOFade(1f, 1f)

            //            //�{�^��������������
            //            .OnComplete(() => button.interactable = true);
            //        }

            //    }));

            ////�{�^���������ꂽ�ۂ̏���
            //void ClickedButton(SoundDataSO.SoundName seName) {
            //    //���ʉ����Đ�
            //    SoundManager.instance.PlaySound(SoundDataSO.SoundName.GameStartSE);

            //    //�w�i����莞�Ԃ����Ĕ�\���ɂ���
            //    imgBackground.DOFade(0f, 1f);

            //    //���S����莞�Ԃ����Ĕ�\���ɂ���
            //    imgLogo.DOFade(0f, 1f);

            //    //�{�^���̃L�����o�X�O���[�v����莞�Ԃ����Ĕ�\���ɂ���
            //    cgButton.DOFade(0f, 1f)

            //        //�Q�[���X�^�[�g���o���I��������Ԃɐ؂�ւ���
            //        .OnComplete(() => end = true);

            //    //�{�^����񊈐�������
            //    button.interactable = false;
            //}

            ////�Q�[���X�^�[�g���o���I���܂ő҂�
            //yield return new WaitUntil(() => end == true);
        }


        /// <summary>
        /// �����悤�ȏ����͏����Ȃ��悤�ɐS�|���܂��傤�B���[�J���֐��ɂقڂ��Ȃ����e�̂��̂�����܂����B
        /// ���\�b�h�̈����𗘗p���邱�ƂłP�ɂ܂Ƃ߂邱�Ƃ��ł��A�d�����\�b�h������ł��܂��B
        /// �����ł͂Q�����܂Ƃ߂Ă܂��B�c��̂P�́A�������C�����Ȃ���l���Ă݂Ă��������B
        /// ������ SoundName �^���� LogoType �^�ɕύX���āA��������� SE ���擾������A��������ƂR�̏��������̃��\�b�h�ɂ܂Ƃ߂��܂��B
        /// </summary>
        /// <param name="seName"></param>
        private bool ClickedButton(SoundDataSO.SoundName seName) {
            //�{�^����񊈐�������
            button.interactable = false;

            //���ʉ����Đ�
            SoundManager.instance.PlaySound(seName);

            // �Q�[���X�^�[�g��
            if (seName == SoundDataSO.SoundName.GameStartSE) {
                //�w�i����莞�Ԃ����Ĕ�\���ɂ���
                imgBackground.DOFade(0f, 1f);
            } else {
                // �Q�[���I�[�o�[�E���X�^�[�g��
                //�w�i����莞�Ԃ����Ĕ��F�ɂ���
                imgBackground.DOColor(Color.white, 1f);
            }
            // �Q�[���N���A�͂܂�����܂���B


            //���S����莞�Ԃ����Ĕ�\���ɂ���
            imgLogo.DOFade(0f, 1f);

            //�{�^���̃L�����o�X�O���[�v����莞�Ԃ����Ĕ�\���ɂ���
            cgButton.DOFade(0f, 1f);
            isUIEffect = true;

            return isUIEffect;
        }

        /// <summary>
        /// �Q�[���I�[�o�[���o���s��
        /// </summary>
        /// <returns>�҂�����</returns>
        public IEnumerator PlayGameOver() {
            //�Q�[���I�[�o�[���o�I������p
            //bool end = false;

            //Debug.Log("Game Over");

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


            // �����́A������ Sequence ������Ă݂Ă��������B�ǐ������܂�܂��B

            ////���_�̃L�����o�X�O���[�v����莞�Ԃ����Ĕ�\���ɂ���
            //cgScore.DOFade(0f, 1f);

            ////���S���\���ɂ���
            //imgLogo.DOFade(0f, 0f)

            ////�w�i����莞�Ԃ����ĕ\������
            //.OnComplete(() => imgBackground.DOFade(1f, 1f)

            //    //���S����莞�Ԃ����ĕ\������
            //    .OnComplete(() => imgLogo.DOFade(1f, 1f)

            //    .OnComplete(() => {
            //        //�{�^���̃C���[�W����莞�Ԃ����ĕ\������
            //        { imgButton.DOFade(1f, 1f); }

            //        {
            //            //�{�^���̃L�����o�X�O���[�v����莞�Ԃ����ĕ\������
            //            cgButton.DOFade(1f, 1f)

            //            //�{�^��������������
            //            .OnComplete(() => button.interactable = true);
            //        }

            //    })));


            //�Q�[���I�[�o�[���o���I���܂ő҂�
            yield return new WaitUntil(() => isUIEffect == true);
            isUIEffect = false;

            ////�{�^���������ꂽ�ۂ̏���
            //void ClickedButton() {
            //    //���ʉ����Đ�
            //    SoundManager.instance.PlaySound(SoundDataSO.SoundName.GameRestartSE);

            //    //�w�i����莞�Ԃ����Ĕ��F�ɂ���
            //    imgBackground.DOColor(Color.white, 1f);

            //    //���S����莞�Ԃ����Ĕ�\���ɂ���
            //    imgLogo.DOFade(0f, 1f);

            //    //�{�^���̃L�����o�X�O���[�v����莞�Ԃ����Ĕ�\���ɂ���
            //    cgButton.DOFade(0f, 1f)

            //        //�Q�[���I�[�o�[���o���I��������Ԃɐ؂�ւ���
            //        .OnComplete(() => end = true);

            //    //�{�^����񊈐�������
            //    button.interactable = false;
            //}

            ////�Q�[���I�[�o�[���o���I���܂ő҂�
            //yield return new WaitUntil(() => end == true);
        }


        // ���̃��\�b�h���ɂ� Click �p�̃��\�b�h������̂ŁA�P�̃��\�b�h�Ƃ��ċ@�\����悤�ɂ��Ă݂Ă��������B

        /// <summary>
        /// �Q�[���N���A���o���s��
        /// </summary>
        /// <returns>�҂�����</returns>
        public IEnumerator PlayGameClear() {
            //�Q�[���N���A���o�I������p
            //bool end = false;
            //Debug.Log("Game Clear");

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
            //button.onClick.AddListener(() => ClickedButton(LogoType.GameClear));   // �����̃q���g�ł��B

            //�{�^����񊈐�������
            button.interactable = false;


            // Sequence ����Ă݂Ă��������B


            ////���_�̃e�L�X�g����莞�Ԃ����ĐF�ɕς���
            //txtScore.DOColor(Color.blue, 2f);

            ////���S���\���ɂ���
            //imgLogo.DOFade(0f, 0f)

            ////�w�i����莞�Ԃ����ĕ\������
            //.OnComplete(() => imgBackground.DOFade(1f, 1f)

            //    //���S����莞�Ԃ����ĕ\������
            //    .OnComplete(() => imgLogo.DOFade(1f, 1f)

            //    .OnComplete(() => {
            //        //�{�^���̃C���[�W����莞�Ԃ����ĕ\������
            //        { imgButton.DOFade(1f, 1f); }

            //        {
            //            //�{�^���̃L�����o�X�O���[�v����莞�Ԃ����ĕ\������
            //            cgButton.DOFade(1f, 1f)

            //            //�{�^��������������
            //            .OnComplete(() => button.interactable = true);
            //        }

            //    })));


            // ��ɍ�������\�b�h�Ə������܂Ƃ߂āA�P�̃��\�b�h�ɂ��Ă݂Ă��������B


            ////�{�^���������ꂽ�ۂ̏���
            //void ClickedButton() {
            //    //���ʉ����Đ�
            //    SoundManager.instance.PlaySound(SoundDataSO.SoundName.GameRestartSE);

            //    //���_�̃L�����o�X�O���[�v����莞�Ԃ����Ĕ�\���ɂ���
            //    cgScore.DOFade(0f, 1f);

            //    //���S����莞�Ԃ����Ĕ�\���ɂ���
            //    imgLogo.DOFade(0f, 1f);

            //    //�{�^���̃L�����o�X�O���[�v����莞�Ԃ����Ĕ�\���ɂ���
            //    cgButton.DOFade(0f, 1f)

            //        //�Q�[���N���A���o���I��������Ԃɐ؂�ւ���
            //        .OnComplete(() => end = true);

            //    //�{�^����񊈐�������
            //    button.interactable = false;
            //}

            //�Q�[���N���A���o���I���܂ő҂�
            //yield return new WaitUntil(() => end == true);

            //�Q�[���I�[�o�[���o���I���܂ő҂�
            yield return new WaitUntil(() => isUIEffect == true);
            isUIEffect = false;
        }

        /// <summary>
        /// ���_�̕\�����X�V���鏀�����s��
        /// </summary>
        public void PrepareUpdateTxtScore() {
            //���_�̕\�����X�V����
            StartCoroutine(UpDateTxtScore());
        }

        /// <summary>
        /// ���_�̕\�����X�V����
        /// </summary>
        /// <returns>�҂�����</returns>
        private IEnumerator UpDateTxtScore() {
            //���_�̃e�L�X�g��ݒ肷��
            txtScore.text = GameData.instance.score.playerScore.ToString() + ":" + GameData.instance.score.enemyScore.ToString();

            //���_�̃L�����o�X�O���[�v����莞�Ԃ����ĕ\������
            cgScore.DOFade(1f, 0.25f);

            //���_����莞�ԁA�\����������
            yield return new WaitForSeconds(0.25f + GameData.instance.DisplayScoreTime);

            //�v���C���[������������
            if (GameData.instance.score.playerScore == GameData.instance.MaxScore) {
                //�ȍ~�̏������s��Ȃ�
                yield break;
            }

            //���_�̃L�����o�X�O���[�v����莞�Ԃ����Ĕ�\���ɂ���
            cgScore.DOFade(0f, 0.25f);
        }
    }
}