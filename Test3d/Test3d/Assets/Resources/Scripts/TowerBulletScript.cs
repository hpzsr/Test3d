using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBulletScript : MonoBehaviour {	
    public void initData(GameObject attackTarget, bool isMyTowerBullet)
    {
        m_gameScript = Camera.main.GetComponent<GameScript>();
        m_attackTarget = attackTarget;
        m_start = true;
        m_isMyTowerBullet = isMyTowerBullet;
    }

	// Update is called once per frame
	void Update ()
    {
        if(m_start)
        {
            if (m_attackTarget != null)
            {
                gameObject.transform.LookAt(m_attackTarget.transform);
                MoveTowardsTarget(m_attackTarget.transform.position + new Vector3(0,0.1f,0));
            }
            else
            {
                Debug.Log("攻击目标不在了");
                Destroy(gameObject);
            }
        }
    }

    void MoveTowardsTarget(Vector3 targetPosition)
    {
        Vector3 currentPosition = this.transform.position;
        //first, check to see if we're close enough to the target
        if (Vector3.Distance(currentPosition, targetPosition) > .1f)
        {
            Vector3 directionOfTravel = targetPosition - currentPosition;
            //now normalize the direction, since we only want the direction information
            directionOfTravel.Normalize();
            //scale the movement on each axis by the directionOfTravel vector components

            this.transform.Translate(
                (directionOfTravel.x * m_speed * Time.deltaTime),
                (directionOfTravel.y * m_speed * Time.deltaTime),
                (directionOfTravel.z * m_speed * Time.deltaTime),
                Space.World);
        }
    }

    void OnTriggerEnter(Collider collidedObject)
    {
        if (collidedObject.gameObject.transform.tag.CompareTo("Hero") == 0)
        {
            if (collidedObject.gameObject.GetComponent<Hero01Script>().hurtAndIsDie(m_attack))
            {
                if (m_isMyTowerBullet)
                {
                    m_gameScript.getEnemyHeroList().Remove(collidedObject.gameObject);
                }
                else
                {
                    m_gameScript.getMyHeroList().Remove(collidedObject.gameObject);
                    ToastScript.createToast("您已阵亡");
                }
            }

            Destroy(gameObject);
        }
        else if (collidedObject.gameObject.transform.tag.CompareTo("Soldier") == 0)
        {
            if (collidedObject.gameObject.GetComponent<SoldierScript>().hurtAndIsDie(m_attack))
            {
                if(m_isMyTowerBullet)
                {
                    m_gameScript.getEnemySoldierList().Remove(collidedObject.gameObject);
                }
                else
                {
                    m_gameScript.getMySoldierList().Remove(collidedObject.gameObject);
                }
            }

            Destroy(gameObject);
        }
    }

    //void OnTriggerStay(Collider collidedObject)
    //{

    //}

    //void OnTriggerExit(Collider collidedObject)
    //{

    //}

    //---------------------------------------------
    GameScript m_gameScript;

    GameObject m_attackTarget = null;
    bool m_isMyTowerBullet;
    bool m_start = false;
    float m_speed = 1;
    float m_attack = 2;
}
