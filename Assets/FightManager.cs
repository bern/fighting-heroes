using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightManager : MonoBehaviour
{

    public PlayerController p1;
    public PlayerController p2;
    public GameObject p1Wins;
    public GameObject p2Wins;
    public GameObject draw;
    private bool fightIsOver;

    private void Start()
    {
        fightIsOver = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (p1.IsDead() && !fightIsOver)
        {
            if (p2.IsDead() && !fightIsOver)
            {
                Instantiate(draw, transform);
                fightIsOver = true;
            } else if (!p2.IsDead() && !fightIsOver)
            {
                Instantiate(p2Wins, transform);
                fightIsOver = true;
            }
        }
        else if (p2.IsDead() && !fightIsOver)
        {
            Instantiate(p1Wins, transform);
            fightIsOver = true;
        }
    }
}
