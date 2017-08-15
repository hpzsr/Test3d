using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonCommonScript : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
        m_allHp = 10;
        m_curHp = m_allHp;
    }

    public void hurt(int hurt)
    {
        m_curHp -= hurt;
        if(m_curHp < 0)
        {
            m_curHp = 0;
        }
    }

    public int getCurHp()
    {
        return m_curHp;
    }

    public int getHpPercent()
    {
        float temp = ((float)m_curHp / (float)m_allHp);
        return Convert.ToInt32(temp * 100);
    }

    //-------------------------------------------
    int m_allHp;
    int m_curHp;
}
