using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MSL
{

    public class credtitButton : MonoBehaviour {

        // Use this for initialization
        void Start() {

        }

        // Update is called once per frame
        void Update() {

        }

        public void onReturnToMenu()
        {
            GameManager.Instance.SceneLoader.LoadScene(SceneLoader.SCENE_ENUM.MAIN_MENU);
        }
    }

}