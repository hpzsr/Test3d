using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallMapScript : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
        m_isStart = false;
    }
	
	// Update is called once per frame
	void Update ()
    {
	    if(Input.GetMouseButtonDown(0))
        {
            if(UtilsScripts.CheckPointInGameObject(gameObject,Input.mousePosition))
            {
                m_isStart = true;
                Camera.main.GetComponent<TrackGameObjScript>().setTrackEnable(false);

                m_beforePoint = Input.mousePosition;
            }
        }
        else if (Input.GetMouseButton(0))
        {
            if (m_isStart)
            {
                Vector3 nowPoint = Input.mousePosition;
                Vector3 moveVec3 = nowPoint - m_beforePoint;
                Camera.main.transform.position += (new Vector3(moveVec3.x, 0, moveVec3.y)) / 50;                

                m_beforePoint = nowPoint;
            }
        }
        else if(Input.GetMouseButtonUp(0))
        {
            if (m_isStart)
            {
                m_isStart = false;
                Camera.main.GetComponent<TrackGameObjScript>().setTrackEnable(true);
            }
        }
    }

    //-----------------------------------------------------------------------------------
    bool m_isStart;
    Vector3 m_beforePoint;
}
