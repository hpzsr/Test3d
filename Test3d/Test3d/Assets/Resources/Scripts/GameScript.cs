using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameScript : MonoBehaviour {

	// Use this for initialization
	void Awake()
    {
        m_playerHero = null;

        m_skeletonList = new List<GameObject>();
        m_mySoldierList = new List<GameObject>();
        m_enemySoldierList = new List<GameObject>();
        m_myTowerList = new List<GameObject>();
        m_enemyTowerList = new List<GameObject>();
        m_allMyList = new List<GameObject>();
        m_allEnemyList = new List<GameObject>();
        m_myHeroList = new List<GameObject>();
        m_enemyHeroList = new List<GameObject>();

        //// 添加skeleton
        //{
        //    GameObject obj = Resources.Load("Prefabs/Skeleton") as GameObject;
        //    GameObject skeleton = MonoBehaviour.Instantiate(obj);
        //    skeleton.transform.position = new Vector3(0, 0.29f, -2.62f);
        //    skeleton.transform.localRotation = Quaternion.EulerAngles(0,-90,0);
        //    skeleton.transform.localScale = new Vector3(3,3,3);

        //    m_skeletonList.Add(skeleton);
        //}

        // 添加我方英雄
        if (true)
        {
            {
                GameObject prefabs = Resources.Load("Prefabs/wq") as GameObject;
                GameObject obj = MonoBehaviour.Instantiate(prefabs);
                obj.transform.position = new Vector3(-7.93f, 0.39f, -2.5f);
                obj.transform.localRotation = Quaternion.Euler(0, 70, 0);
                //obj.transform.Rotate(new Vector3(0, 70, 0));
                
                m_myHeroList.Add(obj);

                // 设为玩家控制的英雄
                m_playerHero = obj;
            }
        }

        // 添加我方soldier
        if (true)
        {
            {
                GameObject prefabs = Resources.Load("Prefabs/Soldier") as GameObject;
                GameObject obj = MonoBehaviour.Instantiate(prefabs);
                obj.transform.position = new Vector3(-7.71f, 0.39f, -2.4f);
                obj.transform.localRotation = Quaternion.Euler(0, 70, 0);
                obj.GetComponent<SoldierScript>().initData(false);

                m_mySoldierList.Add(obj);
            }

            {
                GameObject prefabs = Resources.Load("Prefabs/Soldier") as GameObject;
                GameObject obj = MonoBehaviour.Instantiate(prefabs);
                obj.transform.position = new Vector3(-7.71f, 0.39f, -2.78f);
                obj.transform.localRotation = Quaternion.Euler(0, 70, 0);
                obj.GetComponent<SoldierScript>().initData(false);

                m_mySoldierList.Add(obj);
            }
        }

        // 添加敌方soldier
        if(true)
        {
            {
                GameObject prefabs = Resources.Load("Prefabs/Soldier") as GameObject;
                GameObject obj = MonoBehaviour.Instantiate(prefabs);
                obj.transform.position = new Vector3(5.0f, 0.39f, 2.29f);
                obj.transform.localRotation = Quaternion.Euler(0, -110, 0);
                obj.GetComponent<SoldierScript>().initData(true);
                m_enemySoldierList.Add(obj);
            }

            {
                GameObject prefabs = Resources.Load("Prefabs/Soldier") as GameObject;
                GameObject obj = MonoBehaviour.Instantiate(prefabs);
                obj.transform.position = new Vector3(5.32f, 0.39f, 1.92f);
                obj.transform.localRotation = Quaternion.Euler(0, -110, 0);
                obj.GetComponent<SoldierScript>().initData(true);
                m_enemySoldierList.Add(obj);
            }
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
        // 返回键处理
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //Application.Quit();
            SceneManager.LoadScene("MainScene");
        }
    }

    public List<GameObject> getSkeletonList()
    {
        return m_skeletonList;
    }

    public List<GameObject> getMySoldierList()
    {
        return m_mySoldierList;
    }

    public List<GameObject> getEnemySoldierList()
    {
        return m_enemySoldierList;
    }

    public List<GameObject> getMyTowerList()
    {
        return m_myTowerList;
    }

    public List<GameObject> getEnemyTowerList()
    {
        return m_enemyTowerList;
    }

    public List<GameObject> getMyHeroList()
    {
        return m_myHeroList;
    }

    public List<GameObject> getEnemyHeroList()
    {
        return m_enemyHeroList;
    }

    public List<GameObject> getAllMyList()
    {
        m_allMyList.Clear();

        // 我小兵
        for (int i = 0; i < m_mySoldierList.Count; i++)
        {
            m_allMyList.Add(m_mySoldierList[i]);
        }

        // 我方防御塔
        for (int i = 0; i < m_myTowerList.Count; i++)
        {
            m_allMyList.Add(m_myTowerList[i]);
        }

        // 我方英雄
        for (int i = 0; i < m_myHeroList.Count; i++)
        {
            m_allMyList.Add(m_myHeroList[i]);
        }

        return m_allMyList;
    }

    public List<GameObject> getAllEnemyList()
    {
        m_allEnemyList.Clear();

        // 敌方小兵
        for (int i = 0; i < m_enemySoldierList.Count; i++)
        {
            m_allEnemyList.Add(m_enemySoldierList[i]);
        }

        // 敌方防御塔
        for (int i = 0; i < m_enemyTowerList.Count; i++)
        {
            m_allEnemyList.Add(m_enemyTowerList[i]);
        }

        // 敌方英雄
        for (int i = 0; i < m_enemyHeroList.Count; i++)
        {
            m_allMyList.Add(m_enemyHeroList[i]);
        }

        return m_allEnemyList;
    }

    public GameObject getPlayerHero()
    {
        return m_playerHero;
    }

    public void setLog(string str)
    {
        m_textLog.text = str;
    }
    
    //--------------------------------------------
    Text m_textLog;
    GameObject m_playerHero;

    List<GameObject> m_skeletonList;

    List<GameObject> m_mySoldierList;
    List<GameObject> m_enemySoldierList;

    List<GameObject> m_myTowerList;
    List<GameObject> m_enemyTowerList;

    List<GameObject> m_myHeroList;
    List<GameObject> m_enemyHeroList;

    List<GameObject> m_allMyList;
    List<GameObject> m_allEnemyList;
}
