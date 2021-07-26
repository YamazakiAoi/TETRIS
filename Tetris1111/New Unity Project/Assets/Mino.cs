using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class Mino : MonoBehaviourPunCallbacks
{
    public float previousTime;
    // minoの落ちる時間
    public float fallTime = 1f;

    // ステージの大きさ
    private static int width = 10;
    private static int height = 20;

    // mino回転
    public Vector3 rotationPoint;

    // grid
    private static Transform[,] grid = new Transform[30, 20];
    

    void Update()
    {
        
        if(photonView.IsMine)
        {

            MinoMovememt();
        }
    }

    void MinoMovememt()
    {
        // 左矢印キーで左に動く
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            transform.position += new Vector3(-1, 0, 0);
            
            if (!ValidMovement()) 
            {
                transform.position -= new Vector3(-1, 0, 0);
            }

        }
        // 右矢印キーで右に動く
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            transform.position += new Vector3(1, 0, 0);
            
            if (!ValidMovement()) 
            {
                transform.position -= new Vector3(1, 0, 0);
            }
        }
        // 自動で下に移動させつつ、下矢印キーでも移動する
        else if (Input.GetKeyDown(KeyCode.DownArrow) || Time.time - previousTime >= fallTime) 
        {
            transform.position += new Vector3(0, -1, 0);
            
            if (!ValidMovement()) 
            {
                transform.position -= new Vector3(0, -1, 0);
                     //Debug.Log("Add to Gridが動きます");
                AddToGrid();
                // 今回の追加
                CheckLines();
                this.enabled = false;
                //書き換えた
                //何故か新しいのが出てこない
                FindObjectOfType<Connect>().NewMino();
            }

            previousTime = Time.time;
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            // ブロックの回転
            transform.RotateAround(transform.TransformPoint(rotationPoint), new Vector3(0, 0, 1), 90);
        }
    }

    // 今回の追加 ラインがあるか？確認
    public void CheckLines()
    {
        for (int i = height - 1; i >= 0; i--)
        {
            if (HasLine(i))
            {
                DeleteLine(i);
                RowDown(i);
            }
        }
    }

    // 今回の追加 列がそろっているか確認
    bool HasLine(int i)
    {
        for (int j = (width-10); j < width; j++)
        {
            if (grid[j, i] == null)
                return false;
        }
        return true;
    }

    // 今回の追加 ラインを消す
    void DeleteLine(int i)
    {
        for (int j = (width-10); j < width; j++)
        {
            Destroy(grid[j, i].gameObject);
            grid[j, i] = null;
        }

    }

    // 今回の追加 列を下げる
    //消したラインのぶん下げる
    public void RowDown(int i)
    {
        for (int y = i; y < height; y++)
        {
            for (int j = (width -10); j < width; j++)
            {
                if (grid[j, y] != null)
                {
                    grid[j, y - 1] = grid[j, y];
                    grid[j, y] = null;
                    grid[j, y - 1].transform.position -= new Vector3(0, 1, 0);
                }
            }
        }
    }

    //お前マジなにしてるの？
    void AddToGrid() 
    {
        
        foreach (Transform children in transform) 
        {
            int roundX = Mathf.RoundToInt(children.transform.position.x);
            int roundY = Mathf.RoundToInt(children.transform.position.y);
            

            Debug.Log(roundX);
            Debug.Log(roundY);
            
            grid[roundX, roundY] = children;
        }
        
    }

    // minoの移動範囲の制御
    bool ValidMovement()
    {
        Photon.Realtime.Player Player = PhotonNetwork.LocalPlayer;

        foreach (Transform children in transform)
        {
            int roundX = Mathf.RoundToInt(children.transform.position.x);
            int roundY = Mathf.RoundToInt(children.transform.position.y);
            

            if(Player.ActorNumber == 1)
            {
                width = 10;
                height = 20;
            }
            else
            {
                width = 30; 
                height = 20;
            }

            if(Player.ActorNumber == 1)
            {
                // minoがステージよりはみ出さないように制御(左側)
                if (roundX < 0 || roundX >= width || roundY < 0 || roundY >= height)
                {
                    return false;
                }
                if (grid[roundX, roundY] != null)
                {
                    return false;
                }
            }
            else
            {
                if(roundX < 20 || roundX >=width || roundY < 0 || roundY >= height)
                {
                    return false;
                }
                if(grid[roundX,roundY] != null)
                {
                    return false;
                }
            }

        }
        return true;
    }
}