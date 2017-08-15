using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockerScript : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
        m_gameScript = Camera.main.GetComponent<GameScript>();

        m_bgRadius = m_bg.GetComponent<RectTransform>().sizeDelta.x / 2;
        m_triggerMoveLength = 20;
        m_rockerInitPos = gameObject.transform.position;

        m_controlObj = m_gameScript.getPlayerHero();
    }

    // Update is called once per frame
    void Update()
    {
        if (m_useMouse)
        { 
            Vector2 touchPos = Input.mousePosition;
            if (Input.GetMouseButtonDown(0))
            {
                if (UtilsScripts.TwoPointDistance2D(touchPos, m_bg.transform.position) <= m_bgRadius)
                {
                    m_isStartMove = true;
                    m_ball.transform.position = touchPos;
                }
                else if (UtilsScripts.TwoPointDistance2D(touchPos, new Vector2(0, 0)) <= 500)
                {
                    gameObject.transform.position = touchPos;

                    m_isStartMove = true;
                    m_ball.transform.position = touchPos;
                }
            }
            else if (Input.GetMouseButtonUp(0))
            {
                if (m_isStartMove)
                {
                    m_ball.transform.localPosition = new Vector2(0, 0);
                    m_isStartMove = false;

                    gameObject.transform.position = m_rockerInitPos;

                    if(m_controlObj != null)
                    {
                        if (m_controlObj.GetComponent<Hero01Script>().m_heroState == Hero01Script.HeroState.HeroState_walk)
                        {
                            m_controlObj.GetComponent<Hero01Script>().idle();
                        }
                    }
                }
            }
            else if (Input.GetMouseButton(0))
            {
                if (m_isStartMove)
                {
                    if (UtilsScripts.TwoPointDistance2D(touchPos, m_bg.transform.position) <= m_bgRadius)
                    {
                        m_ball.transform.position = touchPos;
                    }
                    else
                    {
                        float k = (touchPos.y - m_bg.transform.position.y) / (touchPos.x - m_bg.transform.position.x);
                        float x = Mathf.Cos(Mathf.Atan(k)) * m_bgRadius;
                        float y = Mathf.Sin(Mathf.Atan(k)) * m_bgRadius;

                        x = Mathf.Abs(x);
                        y = Mathf.Abs(y);

                        if ((touchPos.x < m_bg.transform.position.x) && (touchPos.y > m_bg.transform.position.y))
                        {
                            x = -x;
                        }
                        else if ((touchPos.x < m_bg.transform.position.x) && (touchPos.y < m_bg.transform.position.y))
                        {
                            x = -x;
                            y = -y;
                        }
                        else if ((touchPos.x > m_bg.transform.position.x) && (touchPos.y < m_bg.transform.position.y))
                        {
                            y = -y;
                        }

                        m_ball.transform.localPosition = new Vector2(x, y);
                    }

                    {
                        float angle = getRockerAngle();
                        if (m_controlObj != null)
                        {
                            m_controlObj.GetComponent<Hero01Script>().walk(angle);
                        }
                    }
                }
            }

            return;
        }
        //--------------------------------------------------------------------------------------------------------------------------------
        else if (Input.touchCount > 0)
        {
            Vector2 ballPos = m_ball.transform.position;
            int touchId = -1;

            {
                for(int i = 0 ; i < Input.touchCount; i++)
                {
                    if (UtilsScripts.TwoPointDistance2D(Input.GetTouch(i).position, m_bg.transform.position) <= m_bgRadius)
                    {
                        touchId = i;
                        break;
                    }
                    else if (UtilsScripts.TwoPointDistance2D(Input.GetTouch(i).position, new Vector2(0, 0)) <= 500)
                    {
                        touchId = i;
                        break;
                    }
                }
            }

            if(touchId == -1)
            {
                return;
            }

            Vector2 touchPos = Input.GetTouch(touchId).position;
            if (Input.GetTouch(touchId).phase == TouchPhase.Began)
            {
                if (UtilsScripts.TwoPointDistance2D(touchPos, m_bg.transform.position) <= m_bgRadius)
                {
                    m_isStartMove = true;
                    m_ball.transform.position = touchPos;
                }
                else if (UtilsScripts.TwoPointDistance2D(touchPos, new Vector2(0,0)) <= 500)
                {
                    gameObject.transform.position = touchPos;

                    m_isStartMove = true;
                    m_ball.transform.position = touchPos;
                }
            }
            else if ((Input.GetTouch(touchId).phase == TouchPhase.Stationary) || (Input.GetTouch(touchId).phase == TouchPhase.Moved))
            {
                if (m_isStartMove)
                {
                    if (UtilsScripts.TwoPointDistance2D(touchPos, m_bg.transform.position) <= m_bgRadius)
                    {
                        m_ball.transform.position = touchPos;
                    }
                    else
                    {
                        float k = (touchPos.y - m_bg.transform.position.y) / (touchPos.x - m_bg.transform.position.x);
                        float x = Mathf.Cos(Mathf.Atan(k)) * m_bgRadius;
                        float y = Mathf.Sin(Mathf.Atan(k)) * m_bgRadius;

                        x = Mathf.Abs(x);
                        y = Mathf.Abs(y);

                        if ((touchPos.x < m_bg.transform.position.x) && (touchPos.y > m_bg.transform.position.y))
                        {
                            x = -x;
                        }
                        else if ((touchPos.x < m_bg.transform.position.x) && (touchPos.y < m_bg.transform.position.y))
                        {
                            x = -x;
                            y = -y;
                        }
                        else if ((touchPos.x > m_bg.transform.position.x) && (touchPos.y < m_bg.transform.position.y))
                        {
                            y = -y;
                        }

                        m_ball.transform.localPosition = new Vector2(x, y);
                    }

                    {
                        float angle = getRockerAngle();
                        if (m_controlObj != null)
                        {
                            m_controlObj.GetComponent<Hero01Script>().walk(angle);
                        }
                    }
                }
            }
            else if (Input.GetTouch(touchId).phase == TouchPhase.Ended)
            {
                if(m_isStartMove)
                {
                    m_ball.transform.localPosition = new Vector2(0, 0);
                    m_isStartMove = false;
                    
                    gameObject.transform.position = m_rockerInitPos;

                    if (m_controlObj != null)
                    {
                        if (m_controlObj.GetComponent<Hero01Script>().m_heroState == Hero01Script.HeroState.HeroState_walk)
                        {
                            m_controlObj.GetComponent<Hero01Script>().idle();
                        }
                    }
                }
            }
        }
    }

    public float getRockerAngle()
    {
        if (UtilsScripts.TwoPointDistance2D(new Vector2(0, 0), m_ball.transform.localPosition) <= m_triggerMoveLength)
        {
            return 0;
        }

        return UtilsScripts.TwoPointAngle(new Vector2(0, 0), m_ball.transform.localPosition);
    }

    //-----------------------------------------------
    public GameObject m_controlObj; // 摇杆控制的对象
    public bool m_useMouse;

    public GameObject m_bg;
    public GameObject m_ball;

    GameScript m_gameScript;

    Vector3 m_rockerInitPos;
    float m_bgRadius;               // 大圆的半径，这是小圆的最大移动范围
    float m_triggerMoveLength;      // 触发距离，小圆移动超过这个距离，即可生效
    bool m_isStartMove;
}
