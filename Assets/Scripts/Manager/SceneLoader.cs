using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MSL
{

    public class SceneLoader : MonoBehaviour
    {

        private bool m_created = false;

        public enum SCENE_ENUM
        {
            MAIN_MENU,
            IN_GAME,
            END_SCENE
        }

        private void Awake()
        {
            if(!m_created)
            {
                DontDestroyOnLoad(gameObject);
                m_created = true;
            }
        }

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void LoadScene(SCENE_ENUM toLoad)
        {
            string sceneName = GetSceneFromEnum(toLoad);
            // Load Scene
            SceneManager.LoadScene(sceneName);
        }

        private string GetSceneFromEnum(SCENE_ENUM toLoad)
        {
            if (toLoad == SCENE_ENUM.MAIN_MENU)
                return "MainMenu";
            else if (toLoad == SCENE_ENUM.IN_GAME)
                return "InGame";
            else if (toLoad == SCENE_ENUM.END_SCENE)
                return "CreditScene";

            return "";
        }

    }

}
