using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerScript : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
        m_gameScript = Camera.main.GetComponent<GameScript>();

        m_attackCD = 2.0f;
        m_canAttack = true;
        m_attack = 2;
        m_attackRange = 1;
        m_allHp = 20;
        m_curHp = m_allHp;

        // 我方塔
        if (gameObject.name[0] == 'M')
        {
            m_isMyTower = true;
            m_gameScript.getMyTowerList().Add(gameObject);
        }
        // 敌方塔
        else
        {
            m_isMyTower = false;
            m_gameScript.getEnemyTowerList().Add(gameObject);
        }

        // 添加血条
        {
            GameObject obj = Resources.Load("Prefabs/ProgressBar") as GameObject;
            m_bloodProgressBar = MonoBehaviour.Instantiate(obj);
            m_bloodProgressBar.transform.SetParent(GameObject.Find("Canvas").transform);
            //m_bloodProgressBar.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);
            m_bloodProgressBar.transform.position = Camera.main.WorldToScreenPoint(gameObject.transform.position) + new Vector3(0, 150, 0);

            if (m_isMyTower)
            {
                m_bloodProgressBar.GetComponent<ProgressScript>().setBarColor(Color.green);
            }
            else
            {
                m_bloodProgressBar.GetComponent<ProgressScript>().setBarColor(Color.red);
            }
        }
    }

    // Update is called once per frame
    void Update ()
    {
        // 更新血条位置
        m_bloodProgressBar.transform.position = Camera.main.WorldToScreenPoint(gameObject.transform.position) + new Vector3(0, 150, 0);

        // 在攻击范围内寻找攻击的对象
        {
            // 优先攻击小兵
            {
                List<GameObject> enemyList;
                if (!m_isMyTower)
                {
                    enemyList = m_gameScript.getMySoldierList();
                }
                else
                {
                    enemyList = m_gameScript.getEnemySoldierList();
                }

                GameObject attackTarget = UtilsScripts.minDistance(gameObject, enemyList);
                if (attackTarget != null)
                {
                    if (UtilsScripts.TwoPointDistance3D(gameObject.transform.position, attackTarget.transform.position) <= m_attackRange)
                    {
                        if (m_canAttack)
                        {
                            // 发射子弹
                            {
                                GameObject prefabs = Resources.Load("Prefabs/TowerBullet") as GameObject;
                                GameObject obj = MonoBehaviour.Instantiate(prefabs);
                                obj.transform.position = gameObject.transform.position + new Vector3(0, 1, 0);
                                obj.GetComponent<TowerBulletScript>().initData(attackTarget, m_isMyTower);
                            }

                            Invoke("attackCDEnd", m_attackCD);
                            m_canAttack = false;
                        }

                        return;
                    }
                }
            }

            // 其次攻击英雄
            {
                List<GameObject> enemyList;
                if (!m_isMyTower)
                {
                    enemyList = m_gameScript.getMyHeroList();
                }
                else
                {
                    enemyList = m_gameScript.getEnemyHeroList();
                }

                GameObject attackTarget = UtilsScripts.minDistance(gameObject, enemyList);
                if (attackTarget != null)
                {
                    if (UtilsScripts.TwoPointDistance3D(gameObject.transform.position, attackTarget.transform.position) <= m_attackRange)
                    {
                        if (m_canAttack)
                        {
                            // 发射子弹
                            {
                                GameObject prefabs = Resources.Load("Prefabs/TowerBullet") as GameObject;
                                GameObject obj = MonoBehaviour.Instantiate(prefabs);
                                obj.transform.position = gameObject.transform.position + new Vector3(0, 1, 0);
                                obj.GetComponent<TowerBulletScript>().initData(attackTarget, m_isMyTower);
                            }

                            Invoke("attackCDEnd", m_attackCD);
                            m_canAttack = false;
                        }

                        return;
                    }
                }
            }
        }
    }

    void attackCDEnd()
    {
        m_canAttack = true;
    }

    public bool hurtAndIsDie(float hurt)
    {
        m_curHp -= hurt;
        int percent = (int)((m_curHp / m_allHp) * 100);
        m_bloodProgressBar.GetComponent<ProgressScript>().setProgress(percent);
        if (m_curHp <= 0)
        {
            AudioScript.getAudioScript().playSound_DestroyTower();

            Destroy(m_bloodProgressBar);
            Destroy(gameObject);

            return true;
        }

        return false;
    }

    //---------------------------------------------------------
    GameScript m_gameScript;
    GameObject m_bloodProgressBar;

    bool m_canAttack;
    bool m_isMyTower;
    float m_attackCD;
    float m_attack;
    float m_attackRange;
    float m_allHp;
    float m_curHp;
}
