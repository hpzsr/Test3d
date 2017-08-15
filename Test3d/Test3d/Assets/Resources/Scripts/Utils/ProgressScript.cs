using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
        m_progress = 100;

        setProgress(m_progress);
    }
	
    // 0 ~ 100
    public void setProgress(int progress)
    {
        m_progressObj.transform.localScale = new Vector3(progress / 100.0f,1,1);
    }

    public void setBarColor(Color color)
    {
        m_progressObj.GetComponent<Image>().color = color;
    }

    //-------------------------------------------
    public GameObject m_progressObj;
    public int m_progress;
}
