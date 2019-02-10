using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MSL
{

    public class MonsterTools
    {
        public enum MONSTER_TYPE
        {
            WENDIGO,
            SLIME,
            SPHINX,
            SIRENE,
            BOSS,
        }

        public enum QUESTION_NUMBER
        {
            Q1 = 0,
            Q2 = 1,
            Q3 = 2,
            Q4 = 3,
            Q5 = 4
        }
    }

    [System.Serializable]
    public class MonsterData
    {
        public MonsterTools.MONSTER_TYPE m_monsterType;
        public List<string> m_questions;
        public List<string> m_answerQ1;
        public List<string> m_answerQ2;
        public List<string> m_answerQ3;
        public List<string> m_answerQ4;
        public List<string> m_answerQ5;
    }

}