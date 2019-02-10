using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace MSL
{

    public class EditorGui : EditorWindow
    {
        public MonsterDataList m_monsterDataList;
        private CreateDataListPopup m_CreateDataListPopup;

        private int m_elementIndex;
        private int m_questionIdx;
        private int m_answerIdx;
        private int m_questionEnumIdx;
        private int m_questionListIdx;
        private MonsterTools.QUESTION_NUMBER m_currentQuestion;

        private Editor m_animEditor;
        private Vector2 m_scrollVector;

        [MenuItem("Utility/AnimationEditor Config %#e")]
        static void Init()
        {
            EditorWindow.GetWindow(typeof(EditorGui));
        }

        void OnEnable()
        {
            m_elementIndex = -1;
            m_questionIdx = -1;
            m_answerIdx = -1;
            m_questionEnumIdx = -1;
            m_questionListIdx = -1;
            m_monsterDataList = null;
            m_scrollVector = Vector2.right;
        }

        void OnGUI()
        {
            GUILayout.BeginVertical();
            GUILayout.Label("AnimationEditor Config", EditorStyles.boldLabel);
            GUILayout.EndVertical();

            GUILayout.Space(5);

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("LoadEntityConfig"))
            {
                LoadEntityConfigButton();
            }

            if (GUILayout.Button("Create"))
            {
                CreateDataListButton();
            }

            GUILayout.EndHorizontal();
            GUILayout.Label("", GUI.skin.horizontalSlider);
            GUILayout.Space(20);

            if (m_monsterDataList)
                DisplayDataList();
        }

        void DisplayDataList()
        {
            m_scrollVector = GUILayout.BeginScrollView(m_scrollVector);
            GUILayout.BeginHorizontal();
            GUILayout.Label("List size: " + m_monsterDataList.dataList.Count, EditorStyles.boldLabel);

            if (GUILayout.Button("Add Element"))
            {
                MonsterData toAdd = new MonsterData();
                toAdd.m_questions = new List<string>();
                toAdd.m_answerQ1 = new List<string>();
                toAdd.m_answerQ2 = new List<string>();
                toAdd.m_answerQ3 = new List<string>();
                toAdd.m_answerQ4 = new List<string>();
                toAdd.m_answerQ5 = new List<string>();
                m_monsterDataList.dataList.Add(toAdd);
                m_elementIndex = m_monsterDataList.dataList.Count - 1;
                m_questionIdx = -1;
            }
            if (GUILayout.Button("Remove Element"))
            {
                if (m_monsterDataList.dataList.Count > 0)
                {
                    m_monsterDataList.dataList.RemoveAt(m_elementIndex);
                    m_questionIdx = -1;
                }
            }
            GUILayout.EndHorizontal();

            GUILayout.Label("Current element: " + m_elementIndex);

            GUILayout.BeginHorizontal();

            if (GUILayout.Button("Prev"))
            {
                if (m_elementIndex > 0)
                {
                    m_elementIndex--;
                    m_questionIdx = -1;
                }
            }

            if (GUILayout.Button("Next"))
            {
                if (m_elementIndex < m_monsterDataList.dataList.Count - 1)
                {
                    m_elementIndex++;
                    m_questionIdx = -1;
                }
            }
            GUILayout.EndHorizontal();

            UpdateElementIndex();

            GUILayout.Space(20);

            if (m_elementIndex > -1)
                DisplayData();

            GUILayout.EndScrollView();
        }

        void DisplayData()
        {
            GUILayout.BeginVertical("box");
            m_monsterDataList.dataList[m_elementIndex].m_monsterType= (MonsterTools.MONSTER_TYPE)EditorGUILayout.EnumPopup(m_monsterDataList.dataList[m_elementIndex].m_monsterType);
            DisplayQuestionList();
            GUILayout.Space(5);
            GUILayout.EndVertical();
            GUILayout.BeginVertical("box");
            DisplayAnswer();
            GUILayout.EndVertical();
        }

        

        void DisplayAnswer()
        {
            GUILayout.BeginVertical();
            m_currentQuestion = (MonsterTools.QUESTION_NUMBER)EditorGUILayout.EnumPopup(m_currentQuestion);
            GUILayout.Label("Question: " + Tools.getStringFromEnum(m_currentQuestion));
            List<string> _answer = Tools.getListFromEnum(m_currentQuestion, m_monsterDataList.dataList[m_elementIndex]);

            if(m_answerIdx >= _answer.Count)
            {
                m_answerIdx = -1;
            }
            GUILayout.BeginVertical();

            GUILayout.Label("Answer index: "+m_answerIdx);

            GUILayout.BeginHorizontal();

            if (GUILayout.Button("Prev"))
            {
                if (m_answerIdx >= 0)
                    m_answerIdx--;
                if (m_answerIdx == 0)
                    m_answerIdx = 0;
            }

            if (GUILayout.Button("Next"))
            {
                m_answerIdx++;
                if (m_answerIdx >= _answer.Count)
                    m_answerIdx = _answer.Count - 1;
            }

            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();

            if (m_answerIdx > -1)
            {
                if (GUILayout.Button("Remove Answer"))
                {
                    _answer.RemoveAt(m_answerIdx);  
                    m_answerIdx--;
                }
            }

            if (GUILayout.Button("Add Answer"))
            {
                m_answerIdx++;
                _answer.Add("");
            }

            GUILayout.EndHorizontal();

            if (m_answerIdx > -1)
            {
                _answer[m_answerIdx] =
                    EditorGUILayout.TextField(_answer[m_answerIdx]) as string;
                EditorGUILayout.TextArea(_answer[m_answerIdx]);
            }


            GUILayout.EndVertical();

            GUILayout.EndVertical();

        }

        void DisplayQuestionList()
        {
            List<string> _question = m_monsterDataList.dataList[m_elementIndex].m_questions;
            GUILayout.BeginHorizontal();
            GUILayout.Label("Question Count: " + m_monsterDataList.dataList[m_elementIndex].m_questions.Count);
            if (GUILayout.Button("Add question"))
            {
                _question.Add("");
                m_questionIdx = _question.Count - 1;
            }
            if (m_questionIdx != -1 && GUILayout.Button("Remove question"))
            {
                _question.RemoveAt(m_questionIdx);
                if (m_questionIdx - 1 > -1)
                    m_questionIdx--;
                else if (m_questionIdx >= _question.Count)
                    m_questionIdx = -1;
            }
            GUILayout.EndHorizontal();
            
            GUILayout.Label("current animation index: " + m_questionIdx);
            
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Previous Question"))
            {
                if (m_questionIdx > 0)
                    m_questionIdx--;
            }
            
            if (GUILayout.Button("Next Question"))
            {
                if (m_questionIdx < _question.Count - 1)
                    m_questionIdx++;
            }
            GUILayout.EndHorizontal();
            
            GUILayout.BeginVertical();
            
            if (m_questionIdx!= -1)
            {
                _question[m_questionIdx] =
                    EditorGUILayout.TextField(_question[m_questionIdx]) as string;
                EditorGUILayout.TextArea(_question[m_questionIdx]);
            }
            
            GUILayout.EndVertical();
        }

        void UpdateElementIndex()
        {
            if (m_elementIndex >= m_monsterDataList.dataList.Count)
                m_elementIndex = m_monsterDataList.dataList.Count - 1;

            if (m_elementIndex <= 0 && m_monsterDataList.dataList.Count > 0)
                m_elementIndex = 0;

            if (m_monsterDataList.dataList.Count == 0)
                m_elementIndex = -1;


        }

        void CreateDataListButton()
        {
            if (!m_CreateDataListPopup)
            {
                m_CreateDataListPopup = CreateInstance<CreateDataListPopup>();
            }

            m_CreateDataListPopup.m_mainEditorWindow = this;
            m_CreateDataListPopup.Show();

        }

        public void CreateDataList(string assetPath, string assetName)
        {
            m_monsterDataList = EditorManager.Create(assetPath, assetName);
        }

        void LoadEntityConfigButton()
        {
            string path = EditorUtility.OpenFilePanel("Load config", "", "asset");
            m_monsterDataList = AssetDatabase.LoadAssetAtPath<MonsterDataList>(Tools.GetAssetsPathFromRoot(path));
        }
    }

    public class CreateDataListPopup : EditorWindow
    {
        public EditorGui m_mainEditorWindow;
        private string m_assetPath;
        private string m_assetName;

        static void Init()
        {
            EditorWindow.GetWindow(typeof(CreateAssetMenuAttribute));
        }

        void OnEnable()
        {
            m_assetName = "";
            m_assetPath = "";
        }

        void OnGUI()
        {
            GUILayout.BeginVertical();
            GUILayout.Label("Create DataList", EditorStyles.boldLabel);
            GUILayout.EndVertical();
            GUILayout.Space(5);

            GUILayout.BeginVertical();
            m_assetName = EditorGUILayout.TextField("Asset name: ", m_assetName);
            GUILayout.Label("Path: " + m_assetPath);

            if (GUILayout.Button("Choose path"))
            {
                m_assetPath = EditorUtility.OpenFolderPanel("Asset path: ", m_assetPath, "");
            }

            GUILayout.EndVertical();
            GUILayout.Space(60);

            GUILayout.BeginHorizontal();

            if (GUILayout.Button("Create"))
            {
                string newAssetPath = Tools.GetAssetsPathFromRoot(m_assetPath) + "/";
                m_mainEditorWindow.CreateDataList(newAssetPath, m_assetName.Trim() + ".asset");

                this.Close();
            }

            if (GUILayout.Button("Close"))
            {
                this.Close();
            }
            GUILayout.EndHorizontal();
        }
    }

    public class Tools
    {
        public static string GetAssetsPathFromRoot(string rootPath, bool keepAssetsInPath = true)
        {
            string searchString = "Assets/";
            int startIndex = rootPath.IndexOf(searchString);
            if (keepAssetsInPath)
                return rootPath.Substring(startIndex);
            return rootPath.Substring(startIndex + searchString.Length);
        }

        public static string getStringFromEnum(MonsterTools.QUESTION_NUMBER idx)
        {
            if (idx == MonsterTools.QUESTION_NUMBER.Q1)
                return "Q1";
            if (idx == MonsterTools.QUESTION_NUMBER.Q2)
                return "Q2";
            if (idx == MonsterTools.QUESTION_NUMBER.Q3)
                return "Q3";
            if (idx == MonsterTools.QUESTION_NUMBER.Q4)
                return "Q4";
            if (idx == MonsterTools.QUESTION_NUMBER.Q5)
                return "Q5";

            return "";
        }

        public static List<string> getListFromEnum(MonsterTools.QUESTION_NUMBER idx, MonsterData monsterData)
        {
            if (idx == MonsterTools.QUESTION_NUMBER.Q1)
                return monsterData.m_answerQ1;
            if (idx == MonsterTools.QUESTION_NUMBER.Q2)
                return monsterData.m_answerQ2;
            if (idx == MonsterTools.QUESTION_NUMBER.Q3)
                return monsterData.m_answerQ3;
            if (idx == MonsterTools.QUESTION_NUMBER.Q4)
                return monsterData.m_answerQ4;
            if (idx == MonsterTools.QUESTION_NUMBER.Q5)
                return monsterData.m_answerQ5;

            return monsterData.m_answerQ1;
        }
    }

}