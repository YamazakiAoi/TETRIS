using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;


// PUNのコールバックを受け取れるようにする
public class Connect : MonoBehaviourPunCallbacks
{
    public string[] Minos={"Imino","Jmino","Lmino","Omino","Smino","Tmino","Zmino"};
     int position_x = 0;
     int position_y = 0; 

     
    public void Start()
    {
        Debug.Log("startが動きます");
        //マスターサーバーに接続
        PhotonNetwork.ConnectUsingSettings();
    }

    //マスターサーバーに接続できたときに呼ばれるコールバック
    public override void OnConnectedToMaster()
    {
        Debug.Log("マスターサーバーに接続しました");
        //ランダムなルームに参加
        PhotonNetwork.JoinRandomRoom();

    }

        // ルームへの参加が成功した時に呼ばれるコールバック
    public override void OnJoinedRoom() 
    {
        Photon.Realtime.Player Player = PhotonNetwork.LocalPlayer;
        Debug.Log("ルームへ参加しました");
         //ここに同期させたいオブジェクトなどを書いていく
        //自身のアバター(ネットワークオブジェクト)を呼び出す
        //動かしているミノをアバターとして設定する


        if(Player.ActorNumber == 1)
        {
            position_x = 5;
            position_y = 17;
        }
        else
        {
            position_x = 25;
            position_y = 17;
        }

        NewMino();
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

    public void NewMino() 
    {
        
        var position = new Vector3(position_x,position_y);
        
        
        PhotonNetwork.Instantiate(Minos[Random.Range(0,Minos.Length)], position, Quaternion.identity);
    }
    //ルームを退出する処理を書く
    //ゲームボードを右にもうひとつ増やして対戦相手を表示させる必要がある

    
}

