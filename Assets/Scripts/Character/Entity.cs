using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MSL
{

    public abstract class Entity : MonoBehaviour
    {

        [SerializeField]
        public MonsterData m_monsterData;

        public enum QUESTION_STATE : int
        {
            Q1 = 0,
            Q2 = 1,
            Q3 = 2,
            Q4 = 3,
            Q5 = 4
        }

        public AudioClip m_win;
        public AudioClip m_loose;
        public AudioClip m_happy;
        public AudioClip m_fail;
        public AudioClip m_interraction;
      
        public Sprite m_BasicPoseSprite;
        public Sprite m_HappyPoseSprite;
        public Sprite m_AngryPoseSprite;
        public Sprite m_gameOverSprite;
        public Sprite m_gameOverBackground;
        public Sprite m_ahegaoSprite;

        public Sprite m_textBoxSprite;
        public Sprite m_nameBoxSprite;

        public string m_intro;
        public List<string> m_dialog;
        public Dictionary<QUESTION_STATE, List<string>> m_answer;
        public List<string> m_quote;
        public List<string> m_end;

        public abstract void LoadDataFromScriptableObject();

    }
}
