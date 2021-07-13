using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mino : MonoBehaviour
{
    public float previousTime;

    //ミノが落ちる時間
    public float fallTime = 1f;

    // mino回転
    public Vector3 rotationPoint;

    //ステージの大きさ
    private int width = 10;
    private int height = 20;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        MinoMovement();
    }

    private void MinoMovement()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            transform.position += new Vector3(-1, 0, 0);
            if(!ValidMovement())
            {
                transform.position -= new Vector3(-1, 0, 0);
            }
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            transform.position += new Vector3(1, 0, 0);
            if(!ValidMovement())
            {
                transform.position -= new Vector3(1, 0, 0);
            }
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow) || Time.time-previousTime >= fallTime)
        {
            transform.position += new Vector3(0 ,-1, 0);
            if(!ValidMovement())
            {
                transform.position -= new Vector3(0, -1, 0);
            }
            previousTime = Time.time;
        }
        else if(Input.GetKeyDown(KeyCode.A))
        {
            transform.RotateAround(transform.TransformPoint(rotationPoint), new Vector3(0, 0, 1), -90);
        }
        
        else if(Input.GetKeyDown(KeyCode.D))
        {
            transform.RotateAround(transform.TransformPoint(rotationPoint), new Vector3(0, 0, 1), 90);
        }
        
    }

    bool ValidMovement()
    {
        foreach(Transform children in transform)
        {
            int roundX = Mathf.RoundToInt(children.transform.position.x);
            int roundY = Mathf.RoundToInt(children.transform.position.y);

            if(roundX < 0 || roundX >= width || roundY < 0 || roundY >= height)
            {
                return false;
            }
        }

        return true;
    }
}
