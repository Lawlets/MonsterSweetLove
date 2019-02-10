using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MSL
{

    public class GameManager : MonoBehaviour
    {

        private bool m_created = false;

        private SceneLoader m_sceneLoader = null;
        public SceneLoader SceneLoader { get { return m_sceneLoader; } }

        static private GameManager m_instance = null;
        static public GameManager Instance
        {
            get
            {
                if (!m_instance)
                    m_instance = new GameManager();
                return m_instance;
            }
        }

        private void Awake()
        {
            if (!m_created)
            {
                DontDestroyOnLoad(gameObject);
                m_created = true;
            }   
        }

        void Start()
        {
            m_instance = this;
            if (!m_sceneLoader)
            {
                GameObject gao = new GameObject("SceneLoader");
                m_sceneLoader = gao.AddComponent<SceneLoader>();
            }
        }

        void Update()
        {

        }
    }

}
