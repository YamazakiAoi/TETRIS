using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Mino : MonoBehaviourPunCallbacks
{
    public float previousTime;
    // minoの落ちる時間
    public float fallTime = 1f;

    private bool Gameover_flag = false;
    // ステージの大きさ
    private static int width = 10;
    private static int height = 20;

    public Transform wall;
    public string type{get;set;}

    // mino回転
    public Vector3 rotationPoint;
    private int rotationType = 0;

    // grid
    private static Transform[,] grid = new Transform[30 + 4, height + 2];

    
    void Start ()
    {
        Photon.Realtime.Player Player = PhotonNetwork.LocalPlayer;
        if(Player.ActorNumber == 2)
        {
            width=30;
        }

        //壁の判定を創る
        for(int i = (width-10); i < width + 4; i ++)
        {
            for (int j = 0; j < height + 2;j++)
            {
                if(i == (width-10) || i ==(width-9) ||j == 0 || j ==1 || i == width + 2 || i == width +3)
                {
                    grid[i,j] = wall;
                }
            }
        }
    }

    void Update()
    {
        if(photonView.IsMine)
        {
            MinoMovememt();
            //Debug.Log(this.name);
        }
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
                if(!Gameover_flag)
                FindObjectOfType<Connect>().NewMino(width-4,height);
                
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
                if(!Gameover_flag)
                FindObjectOfType<Connect>().NewMino(width-4,height);
                
            }

        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            if(this.name != "Omino(Clone)")
                {
                    // ブロックの回転
                    transform.RotateAround(transform.TransformPoint(rotationPoint), new Vector3(0, 0, 1), 90);
                    Rotate(false);
                     Debug.Log(rotationType);
                }
            
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            if(this.name != "Omino(Clone)")
                {
                    // ブロックの回転
                    transform.RotateAround(transform.TransformPoint(rotationPoint), new Vector3(0, 0, 1), -90);
                    Rotate(true);
                    Debug.Log(rotationType);
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
                //PhotonView.RPC(nameof(DeleteLine_enemy),RpcTarget.Others,i);
                //PhotonView.RPC(nameof(RowDown_enemy),RpcTarget.Others,i);
            }
        }
    }

    // 今回の追加 列がそろっているか確認
    bool HasLine(int i)
    {
        for (int j = (width-8); j < width + 2; j++)
        {
            if (grid[j, i] == null)
                return false;
        }
        return true;
    }

    // 今回の追加 ラインを消す
    void DeleteLine(int i)
    {
        for (int j = (width-8); j < width + 2; j++)
        {
            PhotonNetwork.Destroy(grid[j, i].gameObject);
            grid[j, i] = null;
        }

    }

    // 今回の追加 列を下げる
    public void RowDown(int i)
    {
        for (int y = i; y < height + 1; y++)
        {
            for (int j = (width-8); j < width + 2; j++)
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



 /*
    [PunRPC]
    private void DeleteLine_enemy(int i)
    {
        
    }

    [PunRPC]
    private void RowDown_enemy(int i)
    {

    }

*/
    void AddToGrid() 
    {
        
        foreach (Transform children in transform) 
        {
            int roundX = Mathf.RoundToInt(children.transform.position.x);
            int roundY = Mathf.RoundToInt(children.transform.position.y);

           // Debug.Log(roundX);
            Debug.Log(Mathf.RoundToInt(transform.position.x));

            if(roundY == height)
            {
                Gameover_flag = true;
                return;
            }
            
            grid[roundX, roundY] = children;
        }
        
    }


    void Rotate(bool Rotatefrag)
    {
        Vector3 Reset =  this.transform.position;
        int Reset_type = rotationType;

        if(Rotatefrag)
        {
            rotationType = Mathf.Abs(rotationType + 1) % 4;
        }
        else
        {
            if(rotationType == 0)
            {
                rotationType = 3;
                
            }else
            {
                rotationType = Mathf.Abs(rotationType - 1) % 4;
            }
            
        }
        switch(this.name)
        {
            case "Imino(Clone)": 

                switch(rotationType)
                {
                    case 0:
                        if(Rotatefrag)
                            transform.position += new Vector3(0, 1, 0);
                        else
                            transform.position += new Vector3(-1, 0, 0);
                    break;

                    case 1:
                        if(Rotatefrag)
                            transform.position += new Vector3(1, 0, 0);
                        else
                            transform.position += new Vector3(0, 1, 0);
                    break;

                    case 2:
                        if(Rotatefrag)
                            transform.position += new Vector3(0, -1, 0);
                        else
                            transform.position += new Vector3(1, 0, 0);
                    break;

                    case 3:
                        if(Rotatefrag)
                            transform.position += new Vector3(-1, 0, 0);
                        else
                            transform.position += new Vector3(0, -1, 0);
                    break;
                }
                
                if(ValidMovement())
                {
                    break;
                }else
                {
                    if(!RotateMino(Rotatefrag))
                    {
                        transform.position = Reset;
                    }
                }

            break;

            default: 
                if(ValidMovement())
                {
                    break;
                }else
                {
                    if(!RotateMino(Rotatefrag))
                    {
                        transform.position = Reset;
                        rotationType = Reset_type;
                    }
                        
                }
            break;
        }
    }

    bool RotateMino(bool rotateType)//右回転が真、左回転が偽
    {
        switch(this.name)
        {
            case "Imino(Clone)": 
            {
                if(rotationType == 3 && !rotateType)
                {
                    transform.position += new Vector3(-1, 0, 0);
                }else if(rotationType == 1 && rotateType)
                {
                    transform.position += new Vector3(-2, 0, 0);
                }else if(rotationType == 0 && !rotateType)
                {
                    transform.position += new Vector3(2, 0, 0);
                }else if(rotationType == 2 && rotateType)
                {
                    transform.position += new Vector3(-1, 0, 0);
                }else if(rotationType == 1 && !rotateType)
                {
                    transform.position += new Vector3(1, 0, 0);
                }else if(rotationType == 3 && rotateType)
                {
                    transform.position += new Vector3(2, 0, 0);
                }else if(rotationType == 2 && !rotateType)
                {
                    transform.position += new Vector3(1, 0, 0);
                }else if(rotationType == 0 && rotateType)
                {
                    transform.position += new Vector3(-2, 0, 0);
                }

                if(ValidMovement())
                {
                    return true;
                }

                if(rotationType == 3 && !rotateType)
                {
                    transform.position += new Vector3(3, 0, 0);
                }else if(rotationType == 1 && rotateType)
                {
                    transform.position += new Vector3(3, 0, 0);
                }else if(rotationType == 0 && !rotateType)
                {
                    transform.position += new Vector3(-3, 0, 0);
                }else if(rotationType == 2 && rotateType)
                {
                    transform.position += new Vector3(3, 0, 0);
                }else if(rotationType == 1 && !rotateType)
                {
                    transform.position += new Vector3(-3, 0, 0);
                }else if(rotationType == 3 && rotateType)
                {
                    transform.position += new Vector3(-3, 0, 0);
                }else if(rotationType == 2 && !rotateType)
                {
                    transform.position += new Vector3(-3, 0, 0);
                }else if(rotationType == 0 && rotateType)
                {
                    transform.position += new Vector3(3, 0, 0);
                }

                if(ValidMovement())
                {
                    return true;
                }

                if(rotationType == 3 && !rotateType)
                {
                    transform.position += new Vector3(-3, 2, 0);
                }else if(rotationType == 1 && rotateType)
                {
                    transform.position += new Vector3(-3, -1, 0);
                }else if(rotationType == 0 && !rotateType)
                {
                    transform.position += new Vector3(3, 1, 0);
                }else if(rotationType == 2 && rotateType)
                {
                    transform.position += new Vector3(3, 2, 0);
                }else if(rotationType == 1 && !rotateType)
                {
                    transform.position += new Vector3(3, -2, 0);
                }else if(rotationType == 3 && rotateType)
                {
                    transform.position += new Vector3(3, 1, 0);
                }else if(rotationType == 2 && !rotateType)
                {
                    transform.position += new Vector3(0, -1, 0);
                }else if(rotationType == 0 && rotateType)
                {
                    transform.position += new Vector3(0, 2, 0);
                }

                if(ValidMovement())
                {
                    return true;
                }

                if(rotationType == 3 && !rotateType)
                {
                    transform.position += new Vector3(3, -3, 0);
                }else if(rotationType == 1 && rotateType)
                {
                    transform.position += new Vector3(3, 3, 0);
                }else if(rotationType == 0 && !rotateType)
                {
                    transform.position += new Vector3(3, -3, 0);
                }else if(rotationType == 2 && rotateType)
                {
                    transform.position += new Vector3(3, -3, 0);
                }else if(rotationType == 1 && !rotateType)
                {
                    transform.position += new Vector3(-3, 3, 0);
                }else if(rotationType == 3 && rotateType)
                {
                    transform.position += new Vector3(-3, -3, 0);
                }else if(rotationType == 2 && !rotateType)
                {
                    transform.position += new Vector3(3, 3, 0);
                }else if(rotationType == 0 && rotateType)
                {
                    transform.position += new Vector3(-3, 3, 0);
                }

                if(!ValidMovement())
                {
                    if(rotateType)
                    {
                        transform.RotateAround(transform.TransformPoint(rotationPoint), new Vector3(0, 0, 1), 90);
                    }else{
                        transform.RotateAround(transform.TransformPoint(rotationPoint), new Vector3(0, 0, 1), -90);
                    }

                    return false;
                }
            }
            break;

            default: 
                if(rotationType == 3 && !rotateType)
                {
                    transform.position += new Vector3(1, 0, 0);
                }else if(rotationType == 1 && rotateType)
                {
                    transform.position += new Vector3(-1, 0, 0);
                }else if(rotationType == 0 && !rotateType)
                {
                    transform.position += new Vector3(1, 0, 0);
                }else if(rotationType == 2 && rotateType)
                {
                    transform.position += new Vector3(1, 0, 0);
                }else if(rotationType == 1 && !rotateType)
                {
                    transform.position += new Vector3(-1, 0, 0);
                }else if(rotationType == 3 && rotateType)
                {
                    transform.position += new Vector3(1, 0, 0);
                }else if(rotationType == 2 && !rotateType)
                {
                    transform.position += new Vector3(-1, 0, 0);
                }else if(rotationType == 0 && rotateType)
                {
                    transform.position += new Vector3(-1, 0, 0);
                }

                if(ValidMovement())
                {
                    return true;
                }

                if(rotationType == 3 && !rotateType)
                {
                    transform.position += new Vector3(0, 1, 0);
                }else if(rotationType == 1 && rotateType)
                {
                    transform.position += new Vector3(0, 1, 0);
                }else if(rotationType == 0 && !rotateType)
                {
                    transform.position += new Vector3(0, -1, 0);
                }else if(rotationType == 2 && rotateType)
                {
                    transform.position += new Vector3(0, -1, 0);
                }else if(rotationType == 1 && !rotateType)
                {
                    transform.position += new Vector3(0, 1, 0);
                }else if(rotationType == 3 && rotateType)
                {
                    transform.position += new Vector3(0, 1, 0);
                }else if(rotationType == 2 && !rotateType)
                {
                    transform.position += new Vector3(0, -1, 0);
                }else if(rotationType == 0 && rotateType)
                {
                    transform.position += new Vector3(0, -1, 0);
                }

                if(ValidMovement())
                {
                    return true;
                }

                if(rotationType == 3 && !rotateType)
                {
                    transform.position += new Vector3(-1, -3, 0);
                }else if(rotationType == 1 && rotateType)
                {
                    transform.position += new Vector3(1, -3, 0);
                }else if(rotationType == 0 && !rotateType)
                {
                    transform.position += new Vector3(-1, 3, 0);
                }else if(rotationType == 2 && rotateType)
                {
                    transform.position += new Vector3(1, 3, 0);
                }else if(rotationType == 1 && !rotateType)
                {
                    transform.position += new Vector3(1, -3, 0);
                }else if(rotationType == 3 && rotateType)
                {
                    transform.position += new Vector3(-1, 3, 0);
                }else if(rotationType == 2 && !rotateType)
                {
                    transform.position += new Vector3(1, 3, 0);
                }else if(rotationType == 0 && rotateType)
                {
                    transform.position += new Vector3(1, 3, 0);
                }

                if(ValidMovement())
                {
                    return true;
                }

                if(rotationType == 3 && !rotateType)
                {
                    transform.position += new Vector3(1, 0, 0);
                }else if(rotationType == 1 && rotateType)
                {
                    transform.position += new Vector3(-1, 0, 0);
                }else if(rotationType == 0 && !rotateType)
                {
                    transform.position += new Vector3(1, 0, 0);
                }else if(rotationType == 2 && rotateType)
                {
                    transform.position += new Vector3(-1, 0, 0);
                }else if(rotationType == 1 && !rotateType)
                {
                    transform.position += new Vector3(1, 0, 0);
                }else if(rotationType == 3 && rotateType)
                {
                    transform.position += new Vector3(1, 0, 0);
                }else if(rotationType == 2 && !rotateType)
                {
                    transform.position += new Vector3(-1, 0, 0);
                }else if(rotationType == 0 && rotateType)
                {
                    transform.position += new Vector3(-1, 0, 0);
                }

                if(!ValidMovement())
                {
                    if(rotateType)
                    {
                        transform.RotateAround(transform.TransformPoint(rotationPoint), new Vector3(0, 0, 1), 90);
                    }else{
                        transform.RotateAround(transform.TransformPoint(rotationPoint), new Vector3(0, 0, 1), -90);
                    }

                    return false;
                }
            break;
        }
        return false;
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
            if (roundX < (width-8) || roundX > width + 1 || roundY < 2 || roundY >= height + 2 )
            {
                //Debug.Log("はみだしてます！");
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