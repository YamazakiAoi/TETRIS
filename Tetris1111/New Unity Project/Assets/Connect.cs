using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;


// PUNのコールバックを受け取れるようにする
public class Connect : MonoBehaviourPunCallbacks
{

    public string[] Minos={"Imino","Jmino","Lmino","Omino","Smino","Tmino","Zmino"};
    public GameObject[] Mino;
    //public GameObject wall;
    public int MinoShape = 0;

    // シャッフルするもとの配列
    private int[] ary1 = new int[] { 0, 1, 2, 3, 4, 5, 6};

    // ランダムな順にソートされた配列
    private int[] ary2 = new int[7];//ary1.OrderBy(i => Guid.NewGuid()).ToArray();
    private void Start()
    {
        //マスターサーバーに接続
        PhotonNetwork.ConnectUsingSettings();
        Debug.Log("マスターサーバーに接続しました");
    }

    //マスターサーバーに接続できたときに呼ばれるコールバック
    public override void OnConnectedToMaster()
    {
        //ランダムなルームに参加
        PhotonNetwork.JoinRandomRoom();

    }

        // ルームへの参加が成功した時に呼ばれるコールバック
    public override void OnJoinedRoom() 
    {
        Photon.Realtime.Player Player = PhotonNetwork.LocalPlayer;
        int x,y;
        Debug.Log("ルームへ参加しました");


        //p1かp2で座標を変える
        if(Player.ActorNumber == 1)
        {
            x = 5;
            y = 18;

        }else
        {
            x = 25;
            y = 18;
        }

        Shuffle();
        NewMino(x,y);

    }

    //ルームの参加に失敗したときに呼ばれるコールバック
    public override void OnJoinRandomFailed(short returnCode, string message) 
    {
        //つくるルームの設定
        var roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 2;

        // ランダムで参加できるルームが存在しないなら、新規でルームを作成する
        PhotonNetwork.CreateRoom(null,roomOptions);
    }

    public void NewMino(int x,int y)
    {
        var position = new Vector3(x,y);
        var obj = PhotonNetwork.Instantiate(Minos[ary2[MinoShape]], position, Quaternion.identity);
        obj.GetComponent<Mino>().type = Mino[ary2[MinoShape]].name;

        //Debug.Log(MinoShape);
        //Debug.Log(ary2[MinoShape]);
        //Debug.Log(Minos[ary2[MinoShape]].name);

        MinoShape ++;
        if(MinoShape >= 7)
        {
            Shuffle();
            MinoShape = 0;
        }
        
    }

        public void Shuffle()
    {
        ary2 = ary1.OrderBy(i => Guid.NewGuid()).ToArray();
    }

}

