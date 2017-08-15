using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneLogScript : MonoBehaviour {

    public static Text m_text;

	// Use this for initialization
	void Start () {
        m_text = gameObject.GetComponent<Text>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public static void setLog(string str)
    {
        m_text.text = str;
    }

    public static void addLog(string str)
    {
        m_text.text += ("\n" + str);
    }
}
