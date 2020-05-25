using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{

    public GameObject bar;
    private float maxHp;
    private float hp;

    public void Start()
    {
        hp = maxHp;
    }

    public float getHp()
    {
        return hp;
    }

    public void setMaxHp(float maxHp)
    {
        this.maxHp = maxHp;
    }

    public void resetHp()
    {
        hp = maxHp;
    }

    public void changeHealth(float delta)
    {
        hp = hp + delta;
        bar.transform.localScale = new Vector3(hp < 0 ? 0 : (float)(hp/maxHp), 1);
    }
}
