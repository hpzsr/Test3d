using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierScript : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
    }

    public void initData(bool isEnemy)
    {
        m_gameScript = Camera.main.GetComponent<GameScript>();

        m_character = gameObject.GetComponent<CharacterController>();
        m_animator = gameObject.GetComponent<Animator>();

        m_attack = 1.0f;
        m_canAttack = true;
        m_speed = 0.006f;
        m_attackRange = 0.7f;

        m_allHp = 10;
        m_curHp = m_allHp;

        m_animator.SetTrigger("run");

        m_isEnemy = isEnemy;

        // 添加血条
        {
            GameObject obj = Resources.Load("Prefabs/ProgressBar") as GameObject;
            m_bloodProgressBar = MonoBehaviour.Instantiate(obj);
            m_bloodProgressBar.transform.SetParent(GameObject.Find("Canvas").transform);
            m_bloodProgressBar.transform.localScale = new Vector3(0.6f,0.6f,0.6f);
            m_bloodProgressBar.transform.position = Camera.main.WorldToScreenPoint(gameObject.transform.position) + new Vector3(0, 90, 0);

            if(m_isEnemy)
            {
                m_bloodProgressBar.GetComponent<ProgressScript>().setBarColor(Color.red);
            }
            else
            {
                m_bloodProgressBar.GetComponent<ProgressScript>().setBarColor(Color.green);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!m_character.isGrounded)
        {
            m_character.Move(Vector3.down * 10.0f * Time.deltaTime);
        }

        // 更新血条位置
        m_bloodProgressBar.transform.position = Camera.main.WorldToScreenPoint(gameObject.transform.position) + new Vector3(0, 90, 0);

        {
            if (m_animator.GetCurrentAnimatorStateInfo(0).IsName("attack"))
            {
                m_animator.speed = 1.5f;
            }
            else
            {
                m_animator.speed = 1.0f;
            }
        }

        bool canMove = true;
        List<GameObject> enemyList;
        if (m_isEnemy)
        {
            enemyList = m_gameScript.getAllMyList();
        }
        else
        {
            enemyList = m_gameScript.getAllEnemyList();
        }

        if (enemyList.Count == 0)
        {
            if (m_isEnemy)
            {
                gameObject.transform.localRotation = Quaternion.Euler(0, -90, 0);
            }
            else
            {
                gameObject.transform.localRotation = Quaternion.Euler(0, 90, 0);
            }

            m_character.Move(gameObject.transform.forward * (Time.deltaTime * m_speed / (1.0f / 60.0f)));

            return;
        }

        // 在攻击范围内寻找攻击的对象
        {
            GameObject attackTarget = UtilsScripts.minDistance(gameObject, enemyList);
            if (attackTarget != null)
            {
                if (UtilsScripts.TwoPointDistance3D(gameObject.transform.position, attackTarget.transform.position) <= m_attackRange)
                {
                    canMove = false;

                    if (m_canAttack)
                    {
                        m_animator.SetTrigger("attack");

                        // 让英雄面向敌人
                        {
                            float angle = UtilsScripts.TwoPointAngle(new Vector2(gameObject.transform.position.x, gameObject.transform.position.z), new Vector2(attackTarget.transform.position.x, attackTarget.transform.position.z));
                            gameObject.transform.localRotation = Quaternion.Euler(0, angle, 0);
                        }

                        if (attackTarget.tag.CompareTo("Tower") == 0)
                        {
                            if (attackTarget.GetComponent<TowerScript>().hurtAndIsDie(m_attack))
                            {
                                if (m_isEnemy)
                                {
                                    m_gameScript.getMyTowerList().Remove(attackTarget);
                                    ToastScript.createToast("我方防御塔被摧毁");
                                }
                                else
                                {
                                    m_gameScript.getEnemyTowerList().Remove(attackTarget);
                                    ToastScript.createToast("摧毁地方防御塔");
                                }
                            }
                        }
                        else if (attackTarget.tag.CompareTo("Soldier") == 0)
                        {
                            if (attackTarget.GetComponent<SoldierScript>().hurtAndIsDie(m_attack))
                            {
                                if (m_isEnemy)
                                {
                                    m_gameScript.getMySoldierList().Remove(attackTarget);
                                }
                                else
                                {
                                    m_gameScript.getEnemySoldierList().Remove(attackTarget);
                                }
                            }
                        }
                        else if (attackTarget.tag.CompareTo("Hero") == 0)
                        {
                            if (attackTarget.GetComponent<Hero01Script>().hurtAndIsDie(m_attack))
                            {
                                m_gameScript.getMyHeroList().Remove(attackTarget);
                                ToastScript.createToast("您已阵亡");
                            }
                        }

                        m_canAttack = false;
                    }
                }
                // 如果有敌方小兵距离自己3倍攻击范围之内的，则主动向这个小兵方向移动
                else if (UtilsScripts.TwoPointDistance3D(gameObject.transform.position, attackTarget.transform.position) <= (m_attackRange * 3))
                {
                    // 让小兵面向敌人移动
                    {
                        float angle = UtilsScripts.TwoPointAngle(new Vector2(gameObject.transform.position.x, gameObject.transform.position.z), new Vector2(attackTarget.transform.position.x, attackTarget.transform.position.z));
                        gameObject.transform.localRotation = Quaternion.Euler(0, angle, 0);
                    }
                }
                else
                {
                    if (m_isEnemy)
                    {
                        gameObject.transform.localRotation = Quaternion.Euler(0, -110, 0);
                    }
                    else
                    {
                        gameObject.transform.localRotation = Quaternion.Euler(0, 70, 0);
                    }
                }
            }
        }

        if (canMove)
        {
            m_character.Move(gameObject.transform.forward * (Time.deltaTime * m_speed / (1.0f / 60.0f)));
        }
    }

    public void onAttackEnd()
    {
        m_canAttack = true;
        m_animator.SetTrigger("run");
    }

    public bool hurtAndIsDie(float hurt)
    {
        m_curHp -= hurt;
        int percent = (int)((m_curHp / m_allHp) * 100);
        m_bloodProgressBar.GetComponent<ProgressScript>().setProgress(percent);

        if (m_curHp <= 0)
        {
            Destroy(m_bloodProgressBar);
            Destroy(gameObject);

            return true;
        }

        return false;
    }

    //-------------------------------------------------
    GameScript m_gameScript;

    CharacterController m_character;
    Animator m_animator;
    GameObject m_bloodProgressBar;

    Vector3 m_targetPos;

    bool m_canAttack;
    float m_attack;
    float m_attackRange;
    float m_speed;
    bool m_isEnemy;

    float m_allHp;
    float m_curHp;
}
