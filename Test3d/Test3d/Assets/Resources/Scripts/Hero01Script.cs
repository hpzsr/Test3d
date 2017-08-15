using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hero01Script : MonoBehaviour {

    public enum HeroState
    {
        HeroState_idle,
        HeroState_walk,
        HeroState_attack,
        HeroState_skill1,
        HeroState_skill2,
        HeroState_skill3,
    };

    // Use this for initialization
    void Start ()
    {
        m_animator = gameObject.GetComponent<Animator>();
        m_character = gameObject.GetComponent<CharacterController>();
        
        m_gameScript = Camera.main.GetComponent<GameScript>();
        
        m_heroState = HeroState.HeroState_idle;

        m_speed = 0.003f;
        m_attackRange = 0.7f;
        m_skillRange = 1.0f;

        m_allHp = 30;
        m_curHp = m_allHp;

        m_isDoubleSpeed = false;

        // 添加血条
        {
            GameObject obj = Resources.Load("Prefabs/ProgressBar") as GameObject;
            m_bloodProgressBar = MonoBehaviour.Instantiate(obj);
            m_bloodProgressBar.transform.SetParent(GameObject.Find("Canvas").transform);
            m_bloodProgressBar.transform.position = Camera.main.WorldToScreenPoint(gameObject.transform.position) + new Vector3(0,120,0);
            m_bloodProgressBar.GetComponent<ProgressScript>().setBarColor(Color.green);
            m_bloodProgressBar.transform.localScale *= 0.8f;
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
        // 显示人物状态
        // setLog(m_heroState.ToString());

        if (!m_character.isGrounded)
        {
            m_character.Move(Vector3.down * 10.0f * Time.deltaTime);
        }

        // 更新血条位置
        m_bloodProgressBar.transform.position = Camera.main.WorldToScreenPoint(gameObject.transform.position) + new Vector3(0, 120, 0);

        // 攻击动画加速
        {
            if((m_animator.GetCurrentAnimatorStateInfo(0).IsName("paobu")) || (m_animator.GetCurrentAnimatorStateInfo(0).IsName("daiji")))
            {
                m_animator.speed = 1;
            }
            else
            {
                m_animator.speed = 1.5f;
            }
        }

        if(m_isDoubleSpeed)
        {
            if(m_animator.GetCurrentAnimatorStateInfo(0).IsName("paobu"))
            {
                m_animator.speed = 2;
            }
        }
        else
        {
            if (m_animator.GetCurrentAnimatorStateInfo(0).IsName("paobu"))
            {
                m_animator.speed = 1;
            }
        }
    }
    
    public void setRotate(float angle)
    {
        gameObject.transform.localRotation = Quaternion.Euler(0, angle, 0);
    }

    public void idle()
    {
        //m_animator.Play("daiji");
        m_heroState = HeroState.HeroState_idle;
        m_animator.SetTrigger("daiji");
    }

    public void walk(float angle)
    {
        switch (m_heroState)
        {
        case HeroState.HeroState_idle:
        case HeroState.HeroState_walk:
        {
            m_heroState = HeroState.HeroState_walk;

            setRotate(angle);

            if (angle != 0)
            {
                // run
                if (true)
                {
                    //m_animator.SetTrigger("paobu");
                    m_animator.Play("paobu");

                    if(m_isDoubleSpeed)
                    {
                        m_character.Move(gameObject.transform.forward * (Time.deltaTime * m_speed * 2 / (1.0f / 60.0f)) * 4);
                    }
                    else
                    {
                        m_character.Move(gameObject.transform.forward * (Time.deltaTime * m_speed / (1.0f / 60.0f)) * 4);
                    }
                }
            }
        }
        break;
        }
    }

    void attackEnemy(float attackRange,float hurt)
    {
        // 在攻击范围内寻找攻击的对象
        {
            GameObject attackTarget = UtilsScripts.minDistance(gameObject, m_gameScript.getAllEnemyList());
            if (attackTarget != null)
            {
                if (UtilsScripts.TwoPointDistance3D(gameObject.transform.position, attackTarget.transform.position) <= m_attackRange)
                {
                    // 让英雄面向敌人
                    {
                        float angle = UtilsScripts.TwoPointAngle(new Vector2(gameObject.transform.position.x, gameObject.transform.position.z), new Vector2(attackTarget.transform.position.x, attackTarget.transform.position.z));
                        gameObject.transform.localRotation = Quaternion.Euler(0, angle, 0);
                    }

                    if (attackTarget.tag.CompareTo("Tower") == 0)
                    {
                        if (attackTarget.GetComponent<TowerScript>().hurtAndIsDie(hurt))
                        {
                            m_gameScript.getEnemyTowerList().Remove(attackTarget);
                            ToastScript.createToast("摧毁地方防御塔");
                        }
                    }
                    else if (attackTarget.tag.CompareTo("Soldier") == 0)
                    {
                        //// 添加攻击特效
                        //{
                        //    GameObject prefabs = Resources.Load("Prefabs/shouji") as GameObject;
                        //    GameObject obj = MonoBehaviour.Instantiate(prefabs);
                        //    obj.transform.position = attackTarget.transform.position + new Vector3(0, 0.2f, 0);
                        //}

                        if (attackTarget.GetComponent<SoldierScript>().hurtAndIsDie(hurt))
                        {
                            m_gameScript.getEnemySoldierList().Remove(attackTarget);
                        }
                    }
                }
            }
        }
    }

    public void attack()
    {
        switch (m_heroState)
        {
        case HeroState.HeroState_idle:
        case HeroState.HeroState_walk:
        {
            m_heroState = HeroState.HeroState_attack;
            //m_animator.Play("gongji");
            m_animator.SetTrigger("gongji1");

            attackEnemy(m_attackRange,1.0f);
            AudioScript.getAudioScript().playSound_Attack_Common();
        }
        break;

        }
    }

    public void skill1()
    {
        switch (m_heroState)
        {
            case HeroState.HeroState_idle:
            case HeroState.HeroState_walk:
            case HeroState.HeroState_attack:
            {
                m_heroState = HeroState.HeroState_skill1;
                //m_animator.Play("gongji");
                m_animator.SetTrigger("gongji2");
                showCircleProcessBar(new Vector2(385.2f, -295.6f), 3000,115);

                attackEnemy(m_skillRange, 2.0f);

                AudioScript.getAudioScript().playSound_Attack_Low();
            }
            break;

        }
    }

    public void skill2()
    {
        switch (m_heroState)
        {
            case HeroState.HeroState_idle:
            case HeroState.HeroState_walk:
            case HeroState.HeroState_attack:
            {
                m_heroState = HeroState.HeroState_skill2;
                //m_animator.Play("gongji");
                m_animator.SetTrigger("gongji3");
                showCircleProcessBar(new Vector2(428.3f, -145.01f), 5000, 115);

                attackEnemy(m_skillRange, 3.0f);

                AudioScript.getAudioScript().playSound_Attack_Middle();
            }
            break;

        }
    }

    public void skill3()
    {
        switch (m_heroState)
        {
            case HeroState.HeroState_idle:
            case HeroState.HeroState_walk:
            case HeroState.HeroState_attack:
            {
                m_heroState = HeroState.HeroState_skill3;
                //m_animator.Play("gongji");
                m_animator.SetTrigger("gongji4");
                showCircleProcessBar(new Vector2(559.9f, -89.8f), 9000, 115);

                attackEnemy(m_skillRange, 4.0f);

                AudioScript.getAudioScript().playSound_Attack_High();
            }
            break;

        }
    }

    public void Attack1End()
    {
        idle();
    }

    public void Attack2End()
    {
        idle();
    }

    public void Attack3End()
    {
        idle();
    }

    public void Attack4End()
    {
        idle();
    }

    public void Attack5End()
    {
        idle();
    }

    public bool hurtAndIsDie(float hurt)
    {
        m_curHp -= hurt;
        int percent = (int)((m_curHp / m_allHp) * 100);
        m_bloodProgressBar.GetComponent<ProgressScript>().setProgress(percent);

        if (m_curHp <= 0)
        {
            AudioScript.getAudioScript().playSound_KillMe();

            Destroy(m_bloodProgressBar);
            Destroy(gameObject);

            return true;
        }

        return false;
    }

    public void startAddBlood()
    {
        float cd = 5000.0f;
        float effectTime = 5.0f;
        showCircleProcessBar(new Vector2(150, -300), cd,80);
        InvokeRepeating("addBlood",0.1f,0.1f);
        Invoke("stopAddBlood", effectTime);
    }

    void addBlood()
    {
        if(m_curHp <= m_allHp)
        {
            m_curHp += 0.2f;
            if(m_curHp > m_allHp)
            {
                m_curHp = m_allHp;
            }

            int percent = (int)((m_curHp / m_allHp) * 100);
            m_bloodProgressBar.GetComponent<ProgressScript>().setProgress(percent);
        }
    }

    void stopAddBlood()
    {
        CancelInvoke("addBlood");
    }

    public void startAddSpeed()
    {
        float cd = 7000.0f;
        float effectTime = 2.5f;
        showCircleProcessBar(new Vector2(250, -300),cd,80);
        m_isDoubleSpeed = true;
        Invoke("stopAddSpeed", effectTime);
    }

    void stopAddSpeed()
    {
        m_isDoubleSpeed = false;
    }

    void showCircleProcessBar(Vector2 pos, float millisecond,float width)
    {
        GameObject prefabs = Resources.Load("Prefabs/CircleProcessBar") as GameObject;
        GameObject obj = MonoBehaviour.Instantiate(prefabs);
        obj.transform.SetParent(GameObject.Find("Canvas").transform);
        obj.transform.localPosition = pos;
        obj.GetComponent<CircleProcessBarScript>().init(width, millisecond);
    }

    //void OnControllerColliderHit(ControllerColliderHit hit)
    //{
    //    Debug.Log(hit.gameObject.name);
    //    if(hit.gameObject.transform.tag.CompareTo("TowerBullet") == 0)
    //    {
    //        Destroy(hit.gameObject);
    //    }
    //}

    //-----------------------------------------------------------------------
    CharacterController m_character;
    public GameObject m_rockerObj;
    GameObject m_bloodProgressBar;
    
    GameScript m_gameScript;

    public HeroState m_heroState;

    Animator m_animator;

    //--------------------------------------------英雄属性start
    float m_speed;
    float m_attackRange;        // 普攻范围
    float m_skillRange;         // 技能范围

    float m_allHp;
    float m_curHp;

    bool m_isDoubleSpeed;
    //--------------------------------------------英雄属性end
}
