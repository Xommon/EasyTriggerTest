using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Healthbar : MonoBehaviour
{
    RectTransform[] hitpoints;
    public Player player;

    private void Start()
    {
        hitpoints = GetComponentsInChildren<RectTransform>();
    }

    private void Update()
    {
        UpdateScore();
    }

    public void UpdateScore()
    {
        for (int i = 6; i >= 0; i--)
        {
            if (i > player.health)
            {
                hitpoints[i].sizeDelta = new Vector3(7, 3, 0);
            }
            else
            {
                hitpoints[i].sizeDelta = new Vector3(7, 22, 0);
            }
        }
    }
}
