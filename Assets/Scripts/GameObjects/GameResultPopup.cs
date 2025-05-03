using System;
using AppCoreModule.Scripts.UI.Screens;
using Common;
using Cysharp.Threading.Tasks;
using Services;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

namespace GameObjects
{
    public class GameResultPopup : BaseScreen
    {
        [Inject] private GameAudioService _audioService;
        
        [SerializeField] private TextMeshProUGUI _winLabel;
        [SerializeField] private TextMeshProUGUI _gameOverLabel;
        [SerializeField] private Button _buttonAgain;
        [SerializeField] private Button _buttonMainMenu;
        private bool _isWin;

        public void Setup(bool isWin)
        {
            _isWin = isWin;
            _winLabel.gameObject.SetActive(isWin);
            _gameOverLabel.gameObject.SetActive(!isWin);
        }

        private void OnEnable()
        {
            PlaySound().Forget();
        }

        private async UniTask PlaySound()
        {
            await UniTask.WaitForSeconds(0.3f);
            _audioService.PlaySfx(_isWin ? "win" : "gameOver");
        }

        private void Start()
        {
            _buttonAgain.onClick.AddListener(OnAgainClicked);
            _buttonMainMenu.onClick.AddListener(OnMainMenuClicked);
        }

        private void OnDestroy()
        {
            _buttonAgain.onClick.RemoveListener(OnAgainClicked);
            _buttonMainMenu.onClick.RemoveListener(OnMainMenuClicked);
        }

        private void OnMainMenuClicked()
        {
            SceneManager.LoadScene(SceneNames.MainMenu);
        }

        private void OnAgainClicked()
        {
            SceneManager.LoadScene(SceneNames.GameScene);
        }
    }
}