using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int direction;
    float x, y;

    public Bullet(int _direction)
    {
        direction = _direction;
    }

    public void FrameEvent(int inMoveX, int inMoveY, Player _player)
    {
        x = x + (direction * 10);
    }
}
