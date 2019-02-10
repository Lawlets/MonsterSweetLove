using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MSL
{

    public class MonsterManager : MonoBehaviour
    {

        private static MonsterManager m_instance = null;
        public static MonsterManager Instance
        {
            get{
                if (!m_instance)
                    m_instance = new MonsterManager();
                return m_instance;
            }
        }

        public List<Entity> m_monsters;
        private List<Entity> m_monstersMet;
        public Entity m_choosenMonster;

        // Use this for initialization
        void Start()
        {
            m_instance = this;
            m_monstersMet = new List<Entity>();
        }

        // Update is called once per frame
        void Update()
        {

        }

        public Vector3 getNextMonsterCoord()
        {
            foreach(Entity monster in m_monsters)
            {
                m_monstersMet.Add(monster);
                m_choosenMonster = monster;
                m_monsters.Remove(monster);
                return monster.transform.position;
            }
            m_choosenMonster = null;
            return Vector3.zero;
        }

    }

}