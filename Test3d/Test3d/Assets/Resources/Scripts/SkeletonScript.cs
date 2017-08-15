using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkeletonScript : MonoBehaviour {

    public enum HeroState
    {
        HeroState_idle,
        HeroState_walk,
        HeroState_attack,
        HeroState_skill1,
        HeroState_skill2,
        HeroState_skill3,
    };

    public enum AvoidDirection
    {
        AvoidDirection_none,
        AvoidDirection_add,
        AvoidDirection_sub,
    };

    // Use this for initialization
    void Start ()
    {
        m_skeletonCommonScript = gameObject.GetComponent<SkeletonCommonScript>();
        m_animator = gameObject.transform.Find ("Skeleton@Skin").GetComponent<Animator> ();
        m_character = gameObject.GetComponent<CharacterController>();

        m_speed = 0.03f;
        m_heroState = HeroState.HeroState_idle;

        // 添加血条
        {
            GameObject obj = Resources.Load("Prefabs/ProgressBar") as GameObject;
            m_bloodProgressBar = MonoBehaviour.Instantiate(obj);
            m_bloodProgressBar.transform.SetParent(GameObject.Find("Canvas").transform);
            m_bloodProgressBar.GetComponent<TrackGameObjScript>().setTrackEnable(false);
            m_bloodProgressBar.GetComponent<TrackGameObjScript>().m_target = gameObject;
            m_bloodProgressBar.transform.position = Camera.main.WorldToScreenPoint(gameObject.transform.position) + new Vector3(0, 80, 0);

            m_progressScript = m_bloodProgressBar.GetComponent<ProgressScript>();
        }
    }
	
	// Update is called once per frame
	void Update () 
	{
        if(!m_character.isGrounded)
        {
            m_character.Move(Vector3.down * 10.0f * Time.deltaTime);
        }

        // 更新血条位置
        m_bloodProgressBar.transform.position = Camera.main.WorldToScreenPoint(gameObject.transform.position) + new Vector3(0, 80, 0);
    }

    void FixedUpdate()
    {
    }

    public void setRotate(float angle)
	{
		gameObject.transform.localRotation = Quaternion.Euler(0, angle, 0);
    }

    public void idle()
    {
        m_animator.speed = 1;
        m_animator.Play("Idle");

        m_heroState = HeroState.HeroState_idle;
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
                    // walk
                    if (false)
                    {
                        m_animator.Play("Walk");
                        gameObject.transform.Translate(Vector3.forward * m_speed);
                    }

                    // run
                    if(true)
                    {
                        m_animator.Play("Run");
                        //gameObject.transform.Translate(Vector3.forward * m_speed * 3);
                        m_character.Move(gameObject.transform.forward * m_speed * 4);
                    }
                }
            }
            break;
        }
    }

    public void attack()
    {
        switch (m_heroState)
        {
        case HeroState.HeroState_idle:
        case HeroState.HeroState_walk:
        {
            m_animator.speed = 1.5f;
            m_animator.Play("Attack");

            m_heroState = HeroState.HeroState_attack;
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
                m_animator.Play("Skill");

                m_heroState = HeroState.HeroState_skill1;
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
                m_animator.Play("Skill");

                m_heroState = HeroState.HeroState_skill2;
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
                m_animator.Play("Skill");

                m_heroState = HeroState.HeroState_skill3;
            }
            break;

        }
    }

    public void setLog(string str)
    {
        m_textLog.text = str;
    }

    AvoidDirection avoidDirection(float angle)
    {
        if((angle > 0) && (angle <= 90))
        {
            return AvoidDirection.AvoidDirection_sub;
        }
        else if ((angle > 90) && (angle <= 180))
        {
            return AvoidDirection.AvoidDirection_add;
        }
        else if ((angle > 180) && (angle <= 270))
        {
            return AvoidDirection.AvoidDirection_sub;
        }
        else if ((angle > 270) && (angle <= 360))
        {
            return AvoidDirection.AvoidDirection_add;
        }

        return AvoidDirection.AvoidDirection_none;
    }

    Vector3 angleToVector(float angle)
    {
        float x = Mathf.Cos(Mathf.Atan(Mathf.Deg2Rad * angle)) * 1;
        float y = Mathf.Sin(Mathf.Atan(Mathf.Deg2Rad * angle)) * 1;

        x = Mathf.Abs(x);
        y = Mathf.Abs(y);

        if ((angle >= 270) && (angle <= 360))
        {
            x = -x;
        }
        else if ((angle >= 180) && (angle <= 270))
        {
            x = -x;
            y = -y;
        }
        else if ((angle >= 90) && (angle <= 180))
        {
            y = -y;
        }
        
        return new Vector3(x,0,y);
    }

    //void OnTriggerEnter(Collider collidedObject)
    //{
    
    //}

    //void OnTriggerStay(Collider collidedObject)
    //{
        
    //}

    //void OnTriggerExit(Collider collidedObject)
    //{

    //}

    //void OnCollisionEnter(Collision collisionInfo)
    //{
    //    Debug.Log("OnCollisionEnter:"+collisionInfo.transform.name);
    //}

    //void OnCollisionStay(Collision collisionInfo)
    //{
    //    Debug.Log("OnCollisionStay");
    //}

    //void OnCollisionExit(Collision collisionInfo)
    //{
    //    Debug.Log("OnCollisionExit");
    //}

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        //Debug.Log("OnControllerColliderHit:"+ hit.gameObject.name);
    }

    public void hurt(int hurt)
    {
        m_skeletonCommonScript.hurt(hurt);
        int percent = m_skeletonCommonScript.getHpPercent();
        m_progressScript.setProgress(percent);

        if(percent <= 0)
        {
            Destroy(m_bloodProgressBar);
            Destroy(gameObject);
        }
    }

    //-------------------------------------------
    SkeletonCommonScript m_skeletonCommonScript;
    ProgressScript m_progressScript;
    CharacterController m_character;
    public GameObject m_rockerObj;
    GameObject m_bloodProgressBar;
    public Text m_textLog;
    public HeroState m_heroState;

    Animator m_animator;
    float m_speed;
}
