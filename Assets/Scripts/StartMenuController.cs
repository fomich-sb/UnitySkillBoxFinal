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
        public int LocationTypeNum = 0;
        [SerializeField] private InputField PlayerNameInput;
        [SerializeField] private Button StartGameButton;
        [SerializeField] private Button[] PlayerTypeButtons;
        [SerializeField] private Button[] LocationTypeButtons;
        [SerializeField] private Text ConnectionStatusText;

        [Inject] private GameController _gameController;
        [Inject] private NetworkController _networkController;

        void Start()
        {
            PlayerTypeNum = PlayerPrefs.GetInt("PlayerTypeNum", 0);
            LocationTypeNum = PlayerPrefs.GetInt("LocationTypeNum", 0);

            for (int i = 0; i < PlayerTypeButtons.Length; i++)
            {
                int currentIndex = i;
                PlayerTypeButtons[i].onClick.AddListener(() => OnPlayerTypeClick(currentIndex));
            }
            SelectPlayerTypeImage();
            for (int i = 0; i < LocationTypeButtons.Length; i++)
            {
                int currentIndex = i;
                LocationTypeButtons[i].onClick.AddListener(() => OnLocationTypeClick(currentIndex));
            }
            SelectLocationTypeImage();

            
            PlayerNameInput.text = PlayerPrefs.GetString("PlayerName", "Игрок " + Mathf.RoundToInt(Random.Range(1000, 9999))); ;
            _networkController.OnConnected += OnServerConnected;
        }

        public void OnPlayerTypeClick(int playerTypeNum)
        {
            PlayerTypeNum = playerTypeNum;
            SelectPlayerTypeImage();
        }
        public void OnLocationTypeClick(int locationTypeNum)
        {
            LocationTypeNum = locationTypeNum;
            SelectLocationTypeImage();
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

        void SelectLocationTypeImage()
        {
            for (int i = 0; i < LocationTypeButtons.Length; i++)
            {
                if (LocationTypeButtons[i].gameObject.TryGetComponent(out Image buttonImage))
                {
                    if (i == LocationTypeNum)
                        buttonImage.color = Color.green;
                    else
                        buttonImage.color = Color.white;
                }
            }
        }

        public void OnStartGameClick()
        {
            PlayerPrefs.SetString("PlayerName", PlayerNameInput.text);
            PlayerPrefs.SetInt("PlayerTypeNum", PlayerTypeNum);
            PlayerPrefs.SetInt("LocationTypeNum", LocationTypeNum);
            _gameController.StartGame(PlayerTypeNum, LocationTypeNum, PlayerNameInput.text);
        }

        public void OnServerConnected(bool isServer)
        {
            /*       if (isServer)
                       ConnectionStatusText.text = "Вы подключены как СЕРВЕР";
                   else
                       ConnectionStatusText.text = "Вы подключены как КЛИЕНТ";
                   StartGameButton.interactable = true;*/
        }
    }
}
