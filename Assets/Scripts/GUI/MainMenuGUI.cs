using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MSL
{

    public class MainMenuGUI : MonoBehaviour
    {

        public Button m_playButton;
        public Button m_exitButton;

        // Use this for initialization
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void onPlayButton()
        {
            GameManager.Instance.SceneLoader.LoadScene(SceneLoader.SCENE_ENUM.IN_GAME);

        }

        public void onExitButton()
        {
            Application.Quit();
        }

        public void onCreditButton()
        {
            GameManager.Instance.SceneLoader.LoadScene(SceneLoader.SCENE_ENUM.END_SCENE);
        }
    }

}
