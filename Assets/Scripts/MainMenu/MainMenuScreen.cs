using System;
using AppCoreModule.Scripts.UI.Screens;
using Common;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace MainMenu
{
    public class MainMenuScreen : BaseScreen
    {
        [SerializeField] private Button _buttonExit;
        [SerializeField] private Button _startGame;

        private void Awake()
        {
            _buttonExit.onClick.AddListener(OnExit);
            _startGame.onClick.AddListener(OnStart);
        }

        private void OnDestroy()
        {
            _buttonExit.onClick.RemoveListener(OnExit);
            _startGame.onClick.RemoveListener(OnStart);
        }

        private void OnStart()
        {
            SceneManager.LoadScene(SceneNames.GameScene);
        }

        private void OnExit()
        {
            Application.Quit();
        }
    }
}