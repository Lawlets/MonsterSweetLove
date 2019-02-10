using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MSL
{

    public class Player : MonoBehaviour
    {
        [SerializeField] private int OFFSET = 160;

        public enum FIGHT_STATE
        {
            INTRO,
            READ,
            ANSWER,
            LAST_ANSWER,
            END,
            CONTINUE,
            WIN,
            NONE
        }

        private bool m_allQuestionAnswered = false;

        public FIGHT_STATE m_currentFightState = FIGHT_STATE.NONE;

        public GameObject m_dialogBox;
        public GameObject newBtn;
        public GameObject newBtn1;
        public GameObject newBtn2;
        public GameObject newBtn3;
        public GameObject menuButton;
        public Text m_textZone;
        public GameObject m_backGround;
        public Sprite m_WinSprite;
        public Text m_monsterName;

        private int m_idxQuestion = -1;

        public Canvas m_sceneCanvas;

        private CharacterController m_characterController;
        private SoundManager m_soundManager;

        private int m_life = 30;
        public int Life { get { return m_life; } set { m_life = value; } }

        // Use this for initialization
        void Start()
        {
            m_characterController = GetComponent<CharacterController>();
            m_currentFightState = FIGHT_STATE.NONE;
        }

        // Update is called once per frame
        void Update()
        {
            if(m_currentFightState == FIGHT_STATE.INTRO)
                if (Input.GetMouseButtonDown(0))
                    AnswerQ1();

            if(m_currentFightState == FIGHT_STATE.READ)
                if (Input.GetMouseButtonDown(0))
                    AnswerNextQuestion();

            if (m_currentFightState == FIGHT_STATE.END)
                End();

            if(m_currentFightState == FIGHT_STATE.CONTINUE)
                if (Input.GetMouseButtonDown(0))
                    Explore();

            if (m_currentFightState != FIGHT_STATE.ANSWER && newBtn.active)
                DesactivateButton();

            if (m_currentFightState == FIGHT_STATE.NONE && m_dialogBox.active)
                m_dialogBox.active = false;

            if(m_currentFightState == FIGHT_STATE.LAST_ANSWER)
                if (Input.GetMouseButtonDown(0))
                    m_currentFightState = FIGHT_STATE.END;

            if (m_currentFightState == FIGHT_STATE.NONE)
                if (Input.GetMouseButtonDown(0))
                    GoToMonster();

            //if (m_currentFightState == FIGHT_STATE.WIN)
            //    if (Input.GetMouseButtonDown(0))
            //        WinGame();

        }

        public void onNewBtnClick()
        {
            Entity monster = MonsterManager.Instance.m_choosenMonster;
            m_textZone.text = monster.m_quote[(m_idxQuestion * 4)];
        }
        public void onNewBtn1Click()
        {
            Entity monster = MonsterManager.Instance.m_choosenMonster;
            m_textZone.text = monster.m_quote[1+(m_idxQuestion * 4)];
        }
        public void onNewBtn2Click()
        {
            Entity monster = MonsterManager.Instance.m_choosenMonster;
            m_textZone.text = monster.m_quote[2+(m_idxQuestion * 4)];
        }
        public void onNewBtn3Click()
        {
            Entity monster = MonsterManager.Instance.m_choosenMonster;
            m_textZone.text = monster.m_quote[3+(m_idxQuestion * 4)];
        }

        private void AnswerNextQuestion()
        {
            if (m_idxQuestion == 0)
                AnswerQ2();
            else if (m_idxQuestion == 1)
                AnswerQ3();
            else if (m_idxQuestion == 2)
                AnswerQ4();
            else if (m_idxQuestion == 3)
                AnswerQ5();
        }

        public void WinFight()
        {
            m_soundManager.m_themeAudio.Stop();
            m_soundManager.m_themeAudio.clip = m_soundManager.m_exploration;
            m_soundManager.m_themeAudio.volume = 0.38f;
            m_soundManager.m_backgroundAudio.Play();

            Entity monster = MonsterManager.Instance.m_choosenMonster;

            if (monster.m_win)
            {
                m_soundManager.m_monsterAudio.clip = monster.m_win;
                m_soundManager.m_monsterAudio.Play();
            }

            monster.GetComponentInChildren<SpriteRenderer>().sprite = monster.m_HappyPoseSprite;

            m_textZone.text = m_textZone.text = monster.m_end[0];
            m_currentFightState = FIGHT_STATE.CONTINUE;
            m_idxQuestion = -1;
            m_allQuestionAnswered = false;
        }

        public void LooseFight()
        {
            m_soundManager.m_themeAudio.Stop();
            m_soundManager.m_backgroundAudio.Play();


            Entity monster = MonsterManager.Instance.m_choosenMonster;
            if (monster.m_loose)
            {
                m_soundManager.m_monsterAudio.clip = monster.m_loose;
                m_soundManager.m_monsterAudio.Play();
            }

            monster.GetComponentInChildren<SpriteRenderer>().sprite = monster.m_gameOverSprite;

            m_textZone.text = monster.m_end[1];
            m_currentFightState = FIGHT_STATE.CONTINUE;
            m_idxQuestion = -1;
            m_allQuestionAnswered = false;
        }

        private void Explore()
        {
            if (IsAlive())
            {
                m_soundManager.m_themeAudio.Play();
                if (MonsterManager.Instance.m_choosenMonster.m_monsterData.m_monsterType == MonsterTools.MONSTER_TYPE.BOSS)
                {
                    m_currentFightState = FIGHT_STATE.WIN;
                    m_backGround.GetComponent<SpriteRenderer>().sprite = m_WinSprite;
                    m_dialogBox.active = false;
                    MonsterManager.Instance.m_choosenMonster.gameObject.active = false;
                    menuButton.active = true;
                    //WinGame();
                    return;
                }
                m_currentFightState = FIGHT_STATE.NONE;
            }
            else
            {
                Entity monster = MonsterManager.Instance.m_choosenMonster;
                monster.gameObject.active = false;
                m_backGround.GetComponent<SpriteRenderer>().sprite = monster.m_gameOverBackground;
                m_dialogBox.active = false;

                menuButton.active = true;
            }
        }

        private void CloseFight()
        {
            WinFight();
        }

        public void GoToMonster()
        {
            Vector3 res = MonsterManager.Instance.getNextMonsterCoord();

            m_characterController.transform.position = res;
            m_backGround.transform.position = transform.position;

            StartFightWithMonster();
        }

        private void StartFightWithMonster()
        {
            if (m_soundManager == null)
                m_soundManager = SoundManager.Instance;
            if (!MonsterManager.Instance.m_choosenMonster)
                return;

            m_soundManager.m_backgroundAudio.Stop();
            m_soundManager.m_themeAudio.Stop();
            m_soundManager.m_themeAudio.clip = m_soundManager.m_fight;
            m_soundManager.m_themeAudio.volume = 0.2f;
            m_soundManager.m_themeAudio.Play();

            m_currentFightState = FIGHT_STATE.INTRO;
            Intro();

            m_idxQuestion = 0;
            Entity monster = MonsterManager.Instance.m_choosenMonster;
            m_dialogBox.GetComponent<Image>().sprite = monster.m_textBoxSprite;
            //m_dialogBox.transform.GetChild(1).GetComponent<Image>().sprite = monster.m_nameBoxSprite;
            m_monsterName.text = monster.name;
            m_dialogBox.transform.GetChild(1).gameObject.active = false;
            monster.gameObject.active = false;
            m_dialogBox.active = true;

            Life = 30;

        }

        private void BestAnswer()
        {
            DesactivateButton();
            m_currentFightState = FIGHT_STATE.READ;
            if (m_allQuestionAnswered)
                m_currentFightState = FIGHT_STATE.LAST_ANSWER;
            Life += 20;


            Entity monster = MonsterManager.Instance.m_choosenMonster;
            
            if (monster.m_happy)
            {
                m_soundManager.m_monsterAudio.clip = monster.m_happy;
                m_soundManager.m_monsterAudio.Play();
            }

            if (monster.m_ahegaoSprite)
            {
                monster.GetComponentInChildren<SpriteRenderer>().sprite = monster.m_ahegaoSprite;
            }
            else
                monster.GetComponentInChildren<SpriteRenderer>().sprite = monster.m_HappyPoseSprite;
            
            ClearButtonEvent();
        } 
        private void GoodAnswer()
        {
            DesactivateButton();
            m_currentFightState = FIGHT_STATE.READ;
            if (m_allQuestionAnswered)
                m_currentFightState = FIGHT_STATE.LAST_ANSWER;
            Life += 10;

            Entity monster = MonsterManager.Instance.m_choosenMonster;
            if (monster.m_happy)
            {
                m_soundManager.m_monsterAudio.clip = monster.m_happy;
                m_soundManager.m_monsterAudio.Play();
            }
            monster.GetComponentInChildren<SpriteRenderer>().sprite = monster.m_HappyPoseSprite;
            ClearButtonEvent();
        }
        private void NeutralAnswer()
        {
            DesactivateButton();
            m_currentFightState = FIGHT_STATE.READ;
            if (m_allQuestionAnswered)
                m_currentFightState = FIGHT_STATE.LAST_ANSWER;
        }
        private void BadAnswer()
        {
            DesactivateButton();
            m_currentFightState = FIGHT_STATE.READ;
            if (m_allQuestionAnswered)
                m_currentFightState = FIGHT_STATE.LAST_ANSWER;
            Life -= 10;

            Entity monster = MonsterManager.Instance.m_choosenMonster;
            if (monster.m_fail)
            {
                m_soundManager.m_monsterAudio.clip = monster.m_fail;
                m_soundManager.m_monsterAudio.Play();
            }

            monster.GetComponentInChildren<SpriteRenderer>().sprite = monster.m_AngryPoseSprite;
            ClearButtonEvent();
        }

        private void WorstAnswer()
        {
            DesactivateButton();
            m_currentFightState = FIGHT_STATE.READ;
            if (m_allQuestionAnswered)
                m_currentFightState = FIGHT_STATE.LAST_ANSWER;
            Life -= 20;

            Entity monster = MonsterManager.Instance.m_choosenMonster;
            if (MonsterManager.Instance.m_choosenMonster.m_fail)
            {
                m_soundManager.m_monsterAudio.clip = MonsterManager.Instance.m_choosenMonster.m_fail;
                m_soundManager.m_monsterAudio.Play();
            }
            monster.GetComponentInChildren<SpriteRenderer>().sprite = monster.m_AngryPoseSprite;
            ClearButtonEvent();
        }

        private void DesactivateButton()
        {
            newBtn.active = false;
            newBtn1.active = false;
            newBtn2.active = false;
            newBtn3.active = false;
        }

        private void EnableButton()
        {
            newBtn.active = true;
            newBtn1.active = true;
            newBtn2.active = true;
            newBtn3.active = true;
        }

        private void ClearButtonEvent()
        {
            newBtn.GetComponent<Button>().onClick.RemoveAllListeners();
            newBtn1.GetComponent<Button>().onClick.RemoveAllListeners();
            newBtn2.GetComponent<Button>().onClick.RemoveAllListeners();
            newBtn3.GetComponent<Button>().onClick.RemoveAllListeners();
        }

        private void Intro()
        {
            Entity monster = MonsterManager.Instance.m_choosenMonster;
            m_textZone.text = monster.m_intro;
        }

        private void AnswerQ1()
        {
            Entity monster = MonsterManager.Instance.m_choosenMonster;
            m_currentFightState = FIGHT_STATE.ANSWER;

            m_dialogBox.transform.GetChild(1).gameObject.active = true;
            monster.gameObject.active = true;

            if (monster.m_dialog.Count > 0)
            {
                m_textZone.text = monster.m_dialog[0];

                Debug.Log(newBtn.GetComponentInChildren<Text>().text);
                newBtn.GetComponentInChildren<Text>().text = monster.m_answer[Entity.QUESTION_STATE.Q1][0];
                newBtn1.GetComponentInChildren<Text>().text = monster.m_answer[Entity.QUESTION_STATE.Q1][1];
                newBtn2.GetComponentInChildren<Text>().text = monster.m_answer[Entity.QUESTION_STATE.Q1][2];
                newBtn3.GetComponentInChildren<Text>().text = monster.m_answer[Entity.QUESTION_STATE.Q1][3];
            }

            if (monster.m_monsterData.m_monsterType == MonsterTools.MONSTER_TYPE.SLIME)
            {
                newBtn.GetComponent<Button>().onClick.AddListener(GoodAnswer);
                newBtn1.GetComponent<Button>().onClick.AddListener(NeutralAnswer);
                newBtn2.GetComponent<Button>().onClick.AddListener(BestAnswer);
                newBtn3.GetComponent<Button>().onClick.AddListener(BadAnswer);
            }
            else if (monster.m_monsterData.m_monsterType == MonsterTools.MONSTER_TYPE.SIRENE)
            {
                newBtn.GetComponent<Button>().onClick.AddListener(GoodAnswer);
                newBtn1.GetComponent<Button>().onClick.AddListener(BestAnswer);
                newBtn2.GetComponent<Button>().onClick.AddListener(NeutralAnswer);
                newBtn3.GetComponent<Button>().onClick.AddListener(BadAnswer);
            }
            else if (monster.m_monsterData.m_monsterType == MonsterTools.MONSTER_TYPE.WENDIGO)
            {
                newBtn.GetComponent<Button>().onClick.AddListener(NeutralAnswer);
                newBtn1.GetComponent<Button>().onClick.AddListener(BadAnswer);
                newBtn2.GetComponent<Button>().onClick.AddListener(GoodAnswer);
                newBtn3.GetComponent<Button>().onClick.AddListener(BadAnswer);
            }
            else if (monster.m_monsterData.m_monsterType == MonsterTools.MONSTER_TYPE.SPHINX)
            {
                newBtn.GetComponent<Button>().onClick.AddListener(BadAnswer);
                newBtn1.GetComponent<Button>().onClick.AddListener(BadAnswer);
                newBtn2.GetComponent<Button>().onClick.AddListener(BadAnswer);
                newBtn3.GetComponent<Button>().onClick.AddListener(GoodAnswer);
            }
            else if (monster.m_monsterData.m_monsterType == MonsterTools.MONSTER_TYPE.BOSS)
            {
                newBtn.GetComponent<Button>().onClick.AddListener(WorstAnswer);
                newBtn1.GetComponent<Button>().onClick.AddListener(GoodAnswer);
                newBtn2.GetComponent<Button>().onClick.AddListener(WorstAnswer);
                newBtn3.GetComponent<Button>().onClick.AddListener(BestAnswer);
            }


            EnableButton();
            m_idxQuestion = 0;
            monster.GetComponentInChildren<SpriteRenderer>().sprite = monster.m_BasicPoseSprite;
        }

        private void AnswerQ2()
        {
            Entity monster = MonsterManager.Instance.m_choosenMonster;
            m_currentFightState = FIGHT_STATE.ANSWER;


            if (monster.m_dialog.Count > 0)
            {
                m_textZone.text = monster.m_dialog[1];

                newBtn.GetComponentInChildren<Text>().text = monster.m_answer[Entity.QUESTION_STATE.Q2][0];
                newBtn1.GetComponentInChildren<Text>().text = monster.m_answer[Entity.QUESTION_STATE.Q2][1];
                newBtn2.GetComponentInChildren<Text>().text = monster.m_answer[Entity.QUESTION_STATE.Q2][2];
                newBtn3.GetComponentInChildren<Text>().text = monster.m_answer[Entity.QUESTION_STATE.Q2][3];

            }

            if (monster.m_monsterData.m_monsterType == MonsterTools.MONSTER_TYPE.SLIME)
            {
                newBtn.GetComponent<Button>().onClick.AddListener(NeutralAnswer);
                newBtn1.GetComponent<Button>().onClick.AddListener(GoodAnswer);
                newBtn2.GetComponent<Button>().onClick.AddListener(BestAnswer);
                newBtn3.GetComponent<Button>().onClick.AddListener(BadAnswer);
            }
            else if (monster.m_monsterData.m_monsterType == MonsterTools.MONSTER_TYPE.SIRENE)
            {
                newBtn.GetComponent<Button>().onClick.AddListener(BestAnswer);
                newBtn1.GetComponent<Button>().onClick.AddListener(NeutralAnswer);
                newBtn2.GetComponent<Button>().onClick.AddListener(BadAnswer);
                newBtn3.GetComponent<Button>().onClick.AddListener(GoodAnswer);
            }
            else if (monster.m_monsterData.m_monsterType == MonsterTools.MONSTER_TYPE.WENDIGO)
            {
                newBtn.GetComponent<Button>().onClick.AddListener(WorstAnswer);
                newBtn1.GetComponent<Button>().onClick.AddListener(NeutralAnswer);
                newBtn2.GetComponent<Button>().onClick.AddListener(GoodAnswer);
                newBtn3.GetComponent<Button>().onClick.AddListener(NeutralAnswer);
            }
            else if (monster.m_monsterData.m_monsterType == MonsterTools.MONSTER_TYPE.SPHINX)
            {
                newBtn.GetComponent<Button>().onClick.AddListener(BestAnswer);
                newBtn1.GetComponent<Button>().onClick.AddListener(NeutralAnswer);
                newBtn2.GetComponent<Button>().onClick.AddListener(GoodAnswer);
                newBtn3.GetComponent<Button>().onClick.AddListener(WorstAnswer);
            }
            else if (monster.m_monsterData.m_monsterType == MonsterTools.MONSTER_TYPE.BOSS)
            {
                newBtn.GetComponent<Button>().onClick.AddListener(NeutralAnswer);
                newBtn1.GetComponent<Button>().onClick.AddListener(BestAnswer);
                newBtn2.GetComponent<Button>().onClick.AddListener(WorstAnswer);
                newBtn3.GetComponent<Button>().onClick.AddListener(GoodAnswer);
            }

            EnableButton();
            m_idxQuestion = 1;
            monster.GetComponentInChildren<SpriteRenderer>().sprite = monster.m_BasicPoseSprite;
        }

        private void AnswerQ3()
        {
            Entity monster = MonsterManager.Instance.m_choosenMonster;
            m_currentFightState = FIGHT_STATE.ANSWER;

            if (monster.m_dialog.Count > 0)
            {

                m_textZone.text = monster.m_dialog[2];

                newBtn.GetComponentInChildren<Text>().text = monster.m_answer[Entity.QUESTION_STATE.Q3][0];
                newBtn1.GetComponentInChildren<Text>().text = monster.m_answer[Entity.QUESTION_STATE.Q3][1];
                newBtn2.GetComponentInChildren<Text>().text = monster.m_answer[Entity.QUESTION_STATE.Q3][2];
                newBtn3.GetComponentInChildren<Text>().text = monster.m_answer[Entity.QUESTION_STATE.Q3][3];

            }


            if (monster.m_monsterData.m_monsterType == MonsterTools.MONSTER_TYPE.SLIME)
            {
                newBtn.GetComponent<Button>().onClick.AddListener(BadAnswer);
                newBtn1.GetComponent<Button>().onClick.AddListener(NeutralAnswer);
                newBtn2.GetComponent<Button>().onClick.AddListener(BestAnswer);
                newBtn3.GetComponent<Button>().onClick.AddListener(GoodAnswer);
            }
            else if (monster.m_monsterData.m_monsterType == MonsterTools.MONSTER_TYPE.SIRENE)
            {
                newBtn.GetComponent<Button>().onClick.AddListener(NeutralAnswer);
                newBtn1.GetComponent<Button>().onClick.AddListener(BadAnswer);
                newBtn2.GetComponent<Button>().onClick.AddListener(WorstAnswer);
                newBtn3.GetComponent<Button>().onClick.AddListener(GoodAnswer);
            }
            else if (monster.m_monsterData.m_monsterType == MonsterTools.MONSTER_TYPE.WENDIGO)
            {
                newBtn.GetComponent<Button>().onClick.AddListener(BadAnswer);
                newBtn1.GetComponent<Button>().onClick.AddListener(BestAnswer);
                newBtn2.GetComponent<Button>().onClick.AddListener(NeutralAnswer);
                newBtn3.GetComponent<Button>().onClick.AddListener(BadAnswer);
            }
            else if (monster.m_monsterData.m_monsterType == MonsterTools.MONSTER_TYPE.SPHINX)
            {
                newBtn.GetComponent<Button>().onClick.AddListener(WorstAnswer);
                newBtn1.GetComponent<Button>().onClick.AddListener(GoodAnswer);
                newBtn2.GetComponent<Button>().onClick.AddListener(NeutralAnswer);
                newBtn3.GetComponent<Button>().onClick.AddListener(BadAnswer);
            }
            else if (monster.m_monsterData.m_monsterType == MonsterTools.MONSTER_TYPE.BOSS)
            {
                newBtn.GetComponent<Button>().onClick.AddListener(BestAnswer);
                newBtn1.GetComponent<Button>().onClick.AddListener(GoodAnswer);
                newBtn2.GetComponent<Button>().onClick.AddListener(WorstAnswer);
                newBtn3.GetComponent<Button>().onClick.AddListener(BadAnswer);
            }

            EnableButton();
            m_idxQuestion = 2;
            monster.GetComponentInChildren<SpriteRenderer>().sprite = monster.m_BasicPoseSprite;
        }

        private void AnswerQ4()
        {
            Entity monster = MonsterManager.Instance.m_choosenMonster;
            m_currentFightState = FIGHT_STATE.ANSWER;

            if (monster.m_dialog.Count > 0)
            {

                m_textZone.text = monster.m_dialog[3];

                newBtn.GetComponentInChildren<Text>().text = monster.m_answer[Entity.QUESTION_STATE.Q4][0];
                newBtn1.GetComponentInChildren<Text>().text = monster.m_answer[Entity.QUESTION_STATE.Q4][1];
                newBtn2.GetComponentInChildren<Text>().text = monster.m_answer[Entity.QUESTION_STATE.Q4][2];
                newBtn3.GetComponentInChildren<Text>().text = monster.m_answer[Entity.QUESTION_STATE.Q4][3];

            }

            if (monster.m_monsterData.m_monsterType == MonsterTools.MONSTER_TYPE.SLIME)
            {
                newBtn.GetComponent<Button>().onClick.AddListener(BestAnswer);
                newBtn1.GetComponent<Button>().onClick.AddListener(BadAnswer);
                newBtn2.GetComponent<Button>().onClick.AddListener(GoodAnswer);
                newBtn3.GetComponent<Button>().onClick.AddListener(NeutralAnswer);
            }
            else if (monster.m_monsterData.m_monsterType == MonsterTools.MONSTER_TYPE.SIRENE)
            {
                newBtn.GetComponent<Button>().onClick.AddListener(WorstAnswer);
                newBtn1.GetComponent<Button>().onClick.AddListener(BadAnswer);
                newBtn2.GetComponent<Button>().onClick.AddListener(GoodAnswer);
                newBtn3.GetComponent<Button>().onClick.AddListener(BestAnswer);
            }
            else if (monster.m_monsterData.m_monsterType == MonsterTools.MONSTER_TYPE.WENDIGO)
            {
                newBtn.GetComponent<Button>().onClick.AddListener(GoodAnswer);
                newBtn1.GetComponent<Button>().onClick.AddListener(BestAnswer);
                newBtn2.GetComponent<Button>().onClick.AddListener(WorstAnswer);
                newBtn3.GetComponent<Button>().onClick.AddListener(BadAnswer);
            }
            else if (monster.m_monsterData.m_monsterType == MonsterTools.MONSTER_TYPE.SPHINX)
            {
                newBtn.GetComponent<Button>().onClick.AddListener(GoodAnswer);
                newBtn1.GetComponent<Button>().onClick.AddListener(BadAnswer);
                newBtn2.GetComponent<Button>().onClick.AddListener(BadAnswer);
                newBtn3.GetComponent<Button>().onClick.AddListener(NeutralAnswer);
            }
            else if (monster.m_monsterData.m_monsterType == MonsterTools.MONSTER_TYPE.BOSS)
            {
                newBtn.GetComponent<Button>().onClick.AddListener(GoodAnswer);
                newBtn1.GetComponent<Button>().onClick.AddListener(BestAnswer);
                newBtn2.GetComponent<Button>().onClick.AddListener(BadAnswer);
                newBtn3.GetComponent<Button>().onClick.AddListener(WorstAnswer);
            }


            EnableButton();
            m_idxQuestion = 3;
            monster.GetComponentInChildren<SpriteRenderer>().sprite = monster.m_BasicPoseSprite;
        }

        private void AnswerQ5()
        {
            Entity monster = MonsterManager.Instance.m_choosenMonster;
            m_currentFightState = FIGHT_STATE.ANSWER;

            if (monster.m_dialog.Count > 0)
            {

                m_textZone.text = monster.m_dialog[4];

                newBtn.GetComponentInChildren<Text>().text = monster.m_answer[Entity.QUESTION_STATE.Q5][0];
                newBtn1.GetComponentInChildren<Text>().text = monster.m_answer[Entity.QUESTION_STATE.Q5][1];
                newBtn2.GetComponentInChildren<Text>().text = monster.m_answer[Entity.QUESTION_STATE.Q5][2];
                newBtn3.GetComponentInChildren<Text>().text = monster.m_answer[Entity.QUESTION_STATE.Q5][3];

            }

            if (monster.m_monsterData.m_monsterType == MonsterTools.MONSTER_TYPE.SLIME)
            {
                newBtn.GetComponent<Button>().onClick.AddListener(BestAnswer);
                newBtn1.GetComponent<Button>().onClick.AddListener(NeutralAnswer);
                newBtn2.GetComponent<Button>().onClick.AddListener(BadAnswer);
                newBtn3.GetComponent<Button>().onClick.AddListener(GoodAnswer);
            }
            else if (monster.m_monsterData.m_monsterType == MonsterTools.MONSTER_TYPE.SIRENE)
            {
                newBtn.GetComponent<Button>().onClick.AddListener(GoodAnswer);
                newBtn1.GetComponent<Button>().onClick.AddListener(NeutralAnswer);
                newBtn2.GetComponent<Button>().onClick.AddListener(BadAnswer);
                newBtn3.GetComponent<Button>().onClick.AddListener(WorstAnswer);
            }
            else if (monster.m_monsterData.m_monsterType == MonsterTools.MONSTER_TYPE.WENDIGO)
            {
                newBtn.GetComponent<Button>().onClick.AddListener(WorstAnswer);
                newBtn1.GetComponent<Button>().onClick.AddListener(NeutralAnswer);
                newBtn2.GetComponent<Button>().onClick.AddListener(NeutralAnswer);
                newBtn3.GetComponent<Button>().onClick.AddListener(BestAnswer);
            }
            else if (monster.m_monsterData.m_monsterType == MonsterTools.MONSTER_TYPE.SPHINX)
            {
                newBtn.GetComponent<Button>().onClick.AddListener(GoodAnswer);
                newBtn1.GetComponent<Button>().onClick.AddListener(BadAnswer);
                newBtn2.GetComponent<Button>().onClick.AddListener(WorstAnswer);
                newBtn3.GetComponent<Button>().onClick.AddListener(NeutralAnswer);
            }
            else if (monster.m_monsterData.m_monsterType == MonsterTools.MONSTER_TYPE.BOSS)
            {
                newBtn.GetComponent<Button>().onClick.AddListener(BestAnswer);
                newBtn1.GetComponent<Button>().onClick.AddListener(GoodAnswer);
                newBtn2.GetComponent<Button>().onClick.AddListener(WorstAnswer);
                newBtn3.GetComponent<Button>().onClick.AddListener(BadAnswer);
            }

            EnableButton();
            m_idxQuestion = 4;
            m_allQuestionAnswered = true;
            monster.GetComponentInChildren<SpriteRenderer>().sprite = monster.m_BasicPoseSprite;
        }

        private void End()
        {
            Entity monster = MonsterManager.Instance.m_choosenMonster;
            if (IsAlive())
                WinFight();
            else
                LooseFight();
        }
        
        private bool IsAlive()
        {
            if (Life >= 80)
                return true;
            return false;
        }

        public void GameOver()
        {
            GameManager.Instance.SceneLoader.LoadScene(SceneLoader.SCENE_ENUM.MAIN_MENU);
        }

        public void WinGame()
        {
            GameManager.Instance.SceneLoader.LoadScene(SceneLoader.SCENE_ENUM.END_SCENE);
        }
    }

}