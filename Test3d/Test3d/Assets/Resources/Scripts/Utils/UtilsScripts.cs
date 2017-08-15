using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UtilsScripts : MonoBehaviour
{
    // 3D中两点之间的距离
    static public float TwoPointDistance3D(Vector3 point1, Vector3 point2)
    {
        float i = Mathf.Sqrt((point1.x - point2.x) * (point1.x - point2.x)
                           + (point1.y - point2.y) * (point1.y - point2.y)
                           + (point1.z - point2.z) * (point1.z - point2.z));

        return i;
    }

    // 2D中两点之间的距离
    static public float TwoPointDistance2D(Vector3 point1, Vector3 point2)
    {
        float i = Mathf.Sqrt((point1.x - point2.x) * (point1.x - point2.x)
                           + (point1.y - point2.y) * (point1.y - point2.y));

        return i;
    }

    // 返回值：0~360,从正Y轴开始
    static public float TwoPointAngle(Vector2 point1, Vector2 point2)
    {
        float angle = 0;
        float k = (point2.y - point1.y) / (point2.x - point1.x);
        float tempAngle = Mathf.Atan(k) * Mathf.Rad2Deg;

        // 第一象限
        if ((point2.x > point1.x) && (point2.y > point1.y))
        {
            angle = 90 - tempAngle;
        }
        // 第二象限
        else if ((point2.x < point1.x) && (point2.y > point1.y))
        {
            angle = 360 - (90 + tempAngle);
        }
        // 第三象限
        else if ((point2.x < point1.x) && (point2.y < point1.y))
        {
            angle = 180 + (90 - tempAngle);
        }
        // 第四象限
        else if ((point2.x > point1.x) && (point2.y < point1.y))
        {
            angle = 90 + (-tempAngle);
        }

        return angle;
    }

    static public bool CheckPointInGameObject(GameObject obj,Vector3 vec3)
    {
        Vector3 pos = obj.transform.position;
        float width = obj.GetComponent<RectTransform>().sizeDelta.x;
        float height = obj.GetComponent<RectTransform>().sizeDelta.y;

        if((vec3.x >= (pos.x - width / 2)) &&
            (vec3.x <= (pos.x + width / 2)) &&
            (vec3.y >= (pos.y - height / 2)) &&
            (vec3.y <= (pos.y + height / 2)))
        {
            return true;
        }

        return false;
    }

    static public GameObject minDistance(GameObject obj,List<GameObject> listObj)
    {
        GameObject backObj = null;
        float distance = 10000;

        for (int i = 0; i < listObj.Count; i++)
        {
            float temp = TwoPointDistance3D(obj.transform.position, listObj[i].transform.position);
            if (temp < distance)
            {
                distance = temp;
                backObj = listObj[i];
            }
        }

        return backObj;
    }
}
