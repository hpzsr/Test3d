using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CircleProcessBarScript : MonoBehaviour {

    // Use this for initialization
    private void Awake()
    {
        m_start = false;
        m_millisecond = 0;
        m_passMillisecond = 0;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (m_start)
        {
            m_passMillisecond += Time.deltaTime * 1000;
            float percent = (m_millisecond - m_passMillisecond) / m_millisecond;
            gameObject.GetComponent<Image>().fillAmount = percent;
            
            int showTime = (int)((m_millisecond - m_passMillisecond) / 1000) + 1;
            m_secondText.text = showTime.ToString();

            if(m_passMillisecond >= m_millisecond)
            {
                m_start = false;
                Destroy(gameObject);
            }
        }
	}

    /*
     * width:进度条宽度
     * time:倒计时时间（毫秒）
     */
    public void init(float width,float millisecond)
    {
        float scale = width / gameObject.GetComponent<RectTransform>().sizeDelta.x;
        gameObject.transform.localScale *= scale;
        m_millisecond = millisecond;
        m_secondText.text = int.Parse((millisecond / 1000).ToString()).ToString();

        m_start = true;
    }

    //-----------------------------------------------------
    bool m_start;
    float m_millisecond;
    float m_passMillisecond;

    public Text m_secondText;
}
