using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace Controller
{
    public class UIManager : MonoBehaviour
    {
        public Text displayMaxCount;
        public Text displayKilledCount;
        public GameObject gameOverPanel;
        public Button btnReStar;

        private void Awake()
        {
            gameOverPanel.SetActive(false);
            btnReStar.onClick.AddListener(OnReStarClick);
            DontDestroyOnLoad(this);
            GameSessionController.GetInstance().Init();
            GameSessionController.GetInstance().OnSessionComplete += (complete, level) =>
            {
                if (!complete)
                {
                    //展示gameOver界面与重开按钮
                    gameOverPanel.SetActive(true);
                }
                else
                {
                    //NextLevel
                }
            };
        }

        // Update is called once per frame
        void Update()
        {
            displayKilledCount.text = GameSessionController.GetInstance().GetKilledEnemyCount().ToString();
            displayMaxCount.text = GameSessionController.GetInstance().GetMaxEnemyCount().ToString();
        }

        private void OnReStarClick()
        {
            Debug.Log("OnReStarClick");
            GameSessionController.GetInstance().ReStar();
            gameOverPanel.SetActive(false);
        }
    }
}