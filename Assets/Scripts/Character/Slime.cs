using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MSL
{

    public class Slime : Entity
    {

        // Use this for initialization
        void Start()
        {
            m_answer = new Dictionary<QUESTION_STATE, List<string>>();
            LoadData();
        }

        // Update is called once per frame
        void Update()
        {

        }

        public override void LoadDataFromScriptableObject()
        {
            MonsterDataList datas = Resources.Load<MonsterDataList>("MonsterData/Monsters");
            foreach (var data in datas.dataList)
            {
                Debug.Log(data.m_monsterType);
                if (data.m_monsterType == MonsterTools.MONSTER_TYPE.SPHINX)
                {
                    LoadData(data);
                }
                else
                    continue;
            }
        }

        private void LoadData(MonsterData data)
        {
            m_dialog = data.m_questions;
            m_answer.Add(QUESTION_STATE.Q1, data.m_answerQ1);
            m_answer.Add(QUESTION_STATE.Q2, data.m_answerQ2);
            m_answer.Add(QUESTION_STATE.Q3, data.m_answerQ3);
            m_answer.Add(QUESTION_STATE.Q4, data.m_answerQ4);
            m_answer.Add(QUESTION_STATE.Q5, data.m_answerQ5);
        }

        private void LoadData()
        {
            m_dialog = m_monsterData.m_questions;
            m_answer.Add(QUESTION_STATE.Q1, m_monsterData.m_answerQ1);
            m_answer.Add(QUESTION_STATE.Q2, m_monsterData.m_answerQ2);
            m_answer.Add(QUESTION_STATE.Q3, m_monsterData.m_answerQ3);
            m_answer.Add(QUESTION_STATE.Q4, m_monsterData.m_answerQ4);
            m_answer.Add(QUESTION_STATE.Q5, m_monsterData.m_answerQ5);
        }
    }

}