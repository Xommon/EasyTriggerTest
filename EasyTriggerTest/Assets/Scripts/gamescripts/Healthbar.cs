using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Healthbar : MonoBehaviour
{
    public RectTransform[] hitpoints;
    public Player player;

    private void Update()
    {
        UpdateScore();
    }

    public void UpdateScore()
    {
        for (int i = 6; i > 0; i--)
        {
            if (i > player.health)
            {
                hitpoints[i - 1].sizeDelta = new Vector3(7, 3, 0);
            }
            else
            {
                hitpoints[i - 1].sizeDelta = new Vector3(7, 22, 0);
            }
        }
    }
}
