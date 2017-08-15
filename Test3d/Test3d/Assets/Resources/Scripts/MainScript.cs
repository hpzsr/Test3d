using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
        AudioScript.getAudioScript().playMusic_mainScene();
    }
	
	// Update is called once per frame
	void Update ()
    {
        // 返回键处理
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    public void onClickEnterGame()
    {
        SceneManager.LoadScene("GameScene");
    }
}
