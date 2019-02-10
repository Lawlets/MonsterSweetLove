using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MSL
{

    public class GUIManager : MonoBehaviour
    {

        static private GUIManager m_instance = null;
        static public GUIManager Instance { get
            {
                if (!m_instance)
                    m_instance = new GUIManager();
                return m_instance;
            } }

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }


}