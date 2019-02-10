using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {

    private static SoundManager m_instance = null;
    public static SoundManager Instance {
        get
        {
            if (!m_instance)
                m_instance = new SoundManager();
            return m_instance;
        }
    }

    public enum SOUND_STATE
    {
        EXPLORATION,
        FIGHT,
    }

    public SOUND_STATE m_currentSoundState = SOUND_STATE.EXPLORATION;

    public AudioClip m_exploration;
    public AudioClip m_fight;
    public AudioClip m_beachSound;

    public AudioSource m_backgroundAudio;
    public AudioSource m_themeAudio;
    public AudioSource m_monsterAudio;

	// Use this for initialization
	void Start () {

        m_instance = this;

        m_backgroundAudio = gameObject.AddComponent<AudioSource>();
        m_backgroundAudio.loop = true;
        m_backgroundAudio.volume = 0.3f;
        m_backgroundAudio.clip = m_beachSound;
        m_backgroundAudio.Play();

        m_themeAudio = gameObject.AddComponent<AudioSource>();
        m_backgroundAudio.loop = true;
        m_themeAudio.volume = 0.38f;
        m_themeAudio.clip = m_exploration;
        m_themeAudio.Play();

        m_monsterAudio = gameObject.AddComponent<AudioSource>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
