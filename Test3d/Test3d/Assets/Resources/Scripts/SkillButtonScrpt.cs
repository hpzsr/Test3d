using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillButtonScrpt : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
        m_gameScript = Camera.main.GetComponent<GameScript>();
        m_hero = m_gameScript.getPlayerHero();
        m_hero01Script = null;

        if (m_hero)
        {
            m_hero01Script = m_hero.GetComponent<Hero01Script>();
        }
    }

    public void onClickAttack()
    {
        if (m_hero01Script)
        {
            m_hero01Script.attack();
        } 
    }

    public void onClickSkill1()
    {
        if (m_hero01Script)
        {
            m_hero01Script.skill1();
        }
    }

    public void onClickSkill2()
    {
        if (m_hero01Script)
        {
            m_hero01Script.skill2();
        }
    }

    public void onClickSkill3()
    {
        if (m_hero01Script)
        {
            m_hero01Script.skill3();
        }
    }

    // 加血
    public void onClickAddBlood()
    {
        if (m_hero01Script)
        {
            m_hero01Script.startAddBlood();
        }

    }

    // 加速
    public void onClickAddSpeed()
    {
        if (m_hero01Script)
        {
            m_hero01Script.startAddSpeed();
        }
    }

    //------------------------------------
    GameObject m_hero;
    Hero01Script m_hero01Script;
    GameScript m_gameScript;
}
