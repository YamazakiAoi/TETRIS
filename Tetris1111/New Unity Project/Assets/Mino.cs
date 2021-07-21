using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mino : MonoBehaviour
{
    public float previousTime;
    // minoの落ちる時間
    public float fallTime = 1f;

    // ステージの大きさ
    private static int width = 10;
    private static int height = 20;

    public string type{get;set;}

    // mino回転
    public Vector3 rotationPoint;

    // grid
    private static Transform[,] grid = new Transform[width, height];

    
    void start ()
    {
        
    }

    void Update()
    {
        MinoMovememt();
        Debug.Log(this.name);
    }

    private void MinoMovememt()
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
                AddToGrid();
                // 今回の追加
                CheckLines();
                this.enabled = false;
                FindObjectOfType<SpawnMino>().NewMino();
                
            }

            previousTime = Time.time;
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow)) 
        {
            while(ValidMovement())
                transform.position += new Vector3(0, -1, 0);
            
            if (!ValidMovement()) 
            {
                transform.position -= new Vector3(0, -1, 0);
                AddToGrid();
                // 今回の追加
                CheckLines();
                this.enabled = false;
                FindObjectOfType<SpawnMino>().NewMino();
                
            }

        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            if(this.name != "Omino(Clone)")
                {
                    // ブロックの回転
                    RotateLeft();
                    transform.RotateAround(transform.TransformPoint(rotationPoint), new Vector3(0, 0, 1), 90);
                }
            
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            if(this.name != "Omino(Clone)")
                {
                    // ブロックの回転
                    RotateRight();
                    transform.RotateAround(transform.TransformPoint(rotationPoint), new Vector3(0, 0, 1), -90);
                }
            
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
        for (int j = 0; j < width; j++)
        {
            if (grid[j, i] == null)
                return false;
        }
        return true;
    }

    // 今回の追加 ラインを消す
    void DeleteLine(int i)
    {
        for (int j = 0; j < width; j++)
        {
            Destroy(grid[j, i].gameObject);
            grid[j, i] = null;
        }

    }

    // 今回の追加 列を下げる
    public void RowDown(int i)
    {
        for (int y = i; y < height; y++)
        {
            for (int j = 0; j < width; j++)
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

    void AddToGrid() 
    {
        
        foreach (Transform children in transform) 
        {
            int roundX = Mathf.RoundToInt(children.transform.position.x);
            int roundY = Mathf.RoundToInt(children.transform.position.y);

           // Debug.Log(roundX);
           // Debug.Log(Mathf.RoundToInt(transform.position.x));
            
            grid[roundX, roundY] = children;
        }
        
    }

    void RotateRight()
    {
        int count = 0; 
        
        int position_x = Mathf.RoundToInt(transform.position.x);
        int position_y = Mathf.RoundToInt(transform.position.y);
        

        switch(this.name)
        {
            case "Imino(Clone)": 
                foreach(Transform children in transform)
                {
                    if(Mathf.RoundToInt(transform.position.y) < Mathf.RoundToInt(children.transform.position.y))
                    {
                        count ++;
                    }
                }

                switch(count)
                {
                    case 1:
                        if(position_x == width - 1)
                        {
                            transform.position += new Vector3(-1, 1, 0);
                        }else if(position_x == 0)
                        {
                            transform.position += new Vector3( 2, -2, 0);
                        }else if(position_x == 1)
                        {
                            transform.position += new Vector3( 1, -1, 0);
                        }
                    break;

                    case 2:
                        if(position_x == width - 1)
                        {
                            transform.position += new Vector3(-2, 2, 0);
                        }else if(position_x == 0)
                        {
                            transform.position += new Vector3( 1, -1, 0);
                        }else if(position_x == width -2)
                        {
                            transform.position += new Vector3( -1, -1, 0);
                        }
                    break;
                }
            break;

            case "Jmino(Clone)": 
                if(position_x == width - 1){
                    transform.position += new Vector3(-1, 1, 0);
                }else if(position_x == 0)
                {
                    transform.position += new Vector3( 1, -1, 0);
                }
            break;

            case "Lmino(Clone)": 
                if(position_x == width - 1){
                    transform.position += new Vector3( -1, -1, 0);
                }else if(position_x == 0)
                {
                    transform.position += new Vector3( 1, -1, 0);
                }
            break;

            case "Smino(Clone)": 
                if(position_x == width - 1){
                    transform.position += new Vector3(-1, 0, 0);
                }else if(position_x == 0)
                {
                    transform.position += new Vector3( 1, 0, 0);
                }
                
            break;

            case "Tmino(Clone)": 
                if(position_x == width - 1){
                    transform.position += new Vector3(-1, 1, 0);
                }else if(position_x == 0)
                {
                    transform.position += new Vector3( 1, -1, 0);
                }
               
            break;

            case "Zmino(Clone)": 
                if(position_x == width - 1){
                    transform.position += new Vector3(-1, 1, 0);
                }else if(position_x == 0)
                {
                    transform.position += new Vector3( 1, -1, 0);
                }
                
            break;

            default: 
            break;
        }
    }

    void RotateLeft()
    {
        int count = 0; 
        
        int position_x = Mathf.RoundToInt(transform.position.x);
        int position_y = Mathf.RoundToInt(transform.position.y);
        

        switch(this.name)
        {
            case "Imino(Clone)": 
                foreach(Transform children in transform)
                {
                    if(Mathf.RoundToInt(transform.position.y) < Mathf.RoundToInt(children.transform.position.y))
                    {
                        count ++;
                    }
                }

                switch(count)
                {
                    case 1:
                        if(position_x == width - 1)
                        {
                            transform.position += new Vector3(-2, -2, 0);
                        }else if(position_x == 0)
                        {
                            transform.position += new Vector3( 1, 1, 0);
                        }else if(position_x == width - 2)
                        {
                            transform.position += new Vector3( -1, -1, 0);
                        }
                    break;

                    case 2:
                        if(position_x == width - 1)
                        {
                            transform.position += new Vector3(-1, -1, 0);
                        }else if(position_x == 0)
                        {
                            transform.position += new Vector3( 2, 2, 0);
                        }else if(position_x == 1)
                        {
                            transform.position += new Vector3( 1, 1, 0);
                        }
                    break;
                }
            break;

            case "Jmino(Clone)": 
                if(position_x == width - 1){
                    transform.position += new Vector3(-1, -1, 0);
                }else if(position_x == 0)
                {
                    transform.position += new Vector3( 1, 1, 0);
                }
            break;

            case "Lmino(Clone)": 
                if(position_x == width - 1){
                    transform.position += new Vector3( -1, -1, 0);
                }else if(position_x == 0)
                {
                    transform.position += new Vector3( 1, 1, 0);
                }
            break;

            case "Smino(Clone)": 
                if(position_x == width - 1){
                    transform.position += new Vector3(-1, -1, 0);
                }else if(position_x == 0)
                {
                    transform.position += new Vector3( 1, 1, 0);
                }
                
            break;

            case "Tmino(Clone)": 
                if(position_x == width - 1){
                    transform.position += new Vector3(-1, -1, 0);
                }else if(position_x == 0)
                {
                    transform.position += new Vector3( 1, 1, 0);
                }
               
            break;

            case "Zmino(Clone)": 
                if(position_x == width - 1){
                    transform.position += new Vector3(-1, 0, 0);
                }else if(position_x == 0)
                {
                    transform.position += new Vector3( 1, 0, 0);
                }
                
            break;

            default: 
            break;
        }
    }

    // minoの移動範囲の制御
    bool ValidMovement()
    {

        foreach (Transform children in transform)
        {
            int roundX = Mathf.RoundToInt(children.transform.position.x);
            int roundY = Mathf.RoundToInt(children.transform.position.y);

            // minoがステージよりはみ出さないように制御
            if (roundX < 0 || roundX >= width || roundY < 0 || roundY >= height)
            {
                return false;
            }
            if (grid[roundX, roundY] != null)
            {
                return false;
            }
            

        }
        return true;
    }
}