﻿using UnityEngine;
using System.Collections.Generic;

public class AudioScript : MonoBehaviour {

    static public AudioScript getAudioScript ()
    {
        if(!s_audioObj)
        {
            s_audioObj = new GameObject();
            MonoBehaviour.DontDestroyOnLoad(s_audioObj);
            s_audioScript = s_audioObj.AddComponent<AudioScript>();
            s_audioScript.init();
        }

        return s_audioScript;
    }

    public void init ()
    {
        initMusicPlayer();
        initSfxPlayer();
    }

    void initMusicPlayer ()
    {
        GameObject go = new GameObject("musicPlayer");
        go.transform.SetParent(transform, false);
        AudioSource player = go.AddComponent<AudioSource>();
        m_musicPlayer = player;

        player.loop = true;
        player.mute = false;
        player.volume = 0.2f;
        player.pitch = 1.0f;
        player.playOnAwake = false;

        m_musicEnable = getMusicEnable();
    }

    void initSfxPlayer ()
    {
        m_soundPlayer = new List<AudioSource>();

        GameObject go = new GameObject("soundPlayer");
        go.transform.SetParent(transform, false);
        AudioSource player = go.AddComponent<AudioSource>();
        m_soundPlayer.Add(player);

        player.loop = false;
        player.mute = false;
        player.volume = 1.0f;
        player.pitch = 1.0f;
        player.playOnAwake = false;

        m_soundEnable = getSoundEnable();
    }

    void playMusic (string audioPath)
    {
        if(m_musicEnable)
        {
            m_musicPlayer.clip = (AudioClip)Resources.Load(audioPath, typeof(AudioClip));
            m_musicPlayer.Play();
        }
    }

    void playSound (string audioPath)
    {
        if(m_soundEnable)
        {
            for(int i = 0; i < m_soundPlayer.Count; i++)
            {
                if(!m_soundPlayer[i].isPlaying)
                {
                    m_soundPlayer[i].clip = (AudioClip)Resources.Load(audioPath, typeof(AudioClip));
                    m_soundPlayer[i].Play();

                    return;
                }
            }

            // 如果执行到这里，说明暂时没有空余的音效组件使用，需要再新建一个
            {
                GameObject go = new GameObject("soundPlayer");
                go.transform.SetParent(transform, false);
                AudioSource player = go.AddComponent<AudioSource>();
                m_soundPlayer.Add(player);

                player.loop = false;
                player.mute = false;
                player.volume = 1.0f;
                player.pitch = 1.0f;
                player.playOnAwake = false;

                player.clip = (AudioClip)Resources.Load(audioPath, typeof(AudioClip));
                player.Play();
            }
        }
    }

    public void stopMusic ()
    {
        if(m_musicPlayer.isPlaying)
        {
            m_musicPlayer.Stop();
        }
    }

    public void stopSound ()
    {
        for(int i = 0; i < m_soundPlayer.Count; i++)
        {
            if(m_soundPlayer[i].isPlaying)
            {
                m_soundPlayer[i].Stop();
            }
        }
    }

    public void setMusicEnable (bool enable)
    {
        PlayerPrefs.SetInt("music enable", enable ? 1 : 0);
        m_musicEnable = enable;
    }

    public void setSoundEnable (bool enable)
    {
        PlayerPrefs.SetInt("sound enable", enable ? 1 : 0);
        m_soundEnable = enable;
    }

    public bool getMusicEnable ()
    {
        return PlayerPrefs.GetInt("music enable", 1) == 1 ? true : false;
    }

    public bool getSoundEnable ()
    {
        return PlayerPrefs.GetInt("sound enable", 1) == 1 ? true : false;
    }

    //----------------------------------------------------------------------------播放 start

    // 主界面
    public void playMusic_mainScene()
    {
        playMusic("Audio/MainScene");
    }

    // 战斗
    public void playMusic_battle ()
    {
        playMusic("Audio/Battle");
    }

    // 我被杀死
    public void playSound_KillMe ()
    {
        playSound("Audio/KillMe");
    }

    // 摧毁防御塔
    public void playSound_DestroyTower()
    {
        playSound("Audio/DestroyTower");
    }

    // 重攻击
    public void playSound_Attack_High()
    {
        playSound("Audio/Attack_High");
    }

    // 中攻击
    public void playSound_Attack_Middle()
    {
        playSound("Audio/Attack_Middle");
    }

    // 轻攻击
    public void playSound_Attack_Low()
    {
        playSound("Audio/Attack_Low");
    }

    // 普通攻击
    public void playSound_Attack_Common()
    {
        playSound("Audio/Attack_Common");
    }

    //----------------------------------------------------------------------------播放 end

    //---------------------------------------------------
    static GameObject s_audioObj = null;
    static AudioScript s_audioScript;


    //#背景音乐只会有一个;
    AudioSource m_musicPlayer;
    //#音效会同时播放多个，所以用List;
    List<AudioSource> m_soundPlayer;

    private bool m_musicEnable = true;
    private bool m_soundEnable = true;
}
