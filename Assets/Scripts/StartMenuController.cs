using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace SkillBoxFinal
{
    public class StartMenuController : MonoBehaviour
    {
        public int PlayerTypeNum = 0;
        [SerializeField] private InputField PlayerNameInput;
        [SerializeField] private Button StartGameButton;
        [SerializeField] private Button[] PlayerTypeButtons;
        [SerializeField] private Text ConnectionStatusText;

        [Inject] private GameController _gameController;
        [Inject] private NetworkController _networkController;

        void Start()
        {
            for (int i = 0; i < PlayerTypeButtons.Length; i++)
            {
                int currentIndex = i;
                PlayerTypeButtons[i].onClick.AddListener(() => OnPlayerTypeClick(currentIndex));
            }
            SelectPlayerTypeImage();
            PlayerNameInput.text = "Игрок " + Mathf.RoundToInt(Random.Range(1000, 9999));
            _networkController.OnConnected += OnServerConnected;
        }

        public void OnPlayerTypeClick(int playerTypeNum)
        {
            PlayerTypeNum = playerTypeNum;
            SelectPlayerTypeImage();
        }

        void SelectPlayerTypeImage()
        {
            for (int i = 0; i < PlayerTypeButtons.Length; i++)
            {
                if (PlayerTypeButtons[i].gameObject.TryGetComponent(out Image buttonImage))
                {
                    if (i == PlayerTypeNum)
                        buttonImage.color = Color.green;
                    else
                        buttonImage.color = Color.white;
                }
            }
        }

        public void OnStartGameClick()
        {
            _gameController.StartGame(PlayerTypeNum, PlayerNameInput.text);
        }

        public void OnServerConnected(bool isServer)
        {
            if (isServer)
                ConnectionStatusText.text = "Вы подключены как СЕРВЕР";
            else
                ConnectionStatusText.text = "Вы подключены как КЛИЕНТ";
            StartGameButton.interactable = true;
        }
    }
}
