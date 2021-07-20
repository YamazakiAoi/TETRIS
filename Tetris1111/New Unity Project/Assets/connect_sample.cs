using Photon.Pun;
using Photon.Realtime;
using UnityEngine;


// PUNのコールバックを受け取れるようにする
public class connect_sample : MonoBehaviourPunCallbacks
{
    public string[] Minos={"Imino","Jmino","Lmino","Omino","Smino","Tmino","Zmino"};
    public void start()
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
        Debug.Log("ルームへ参加しました");
         //ここに同期させたいオブジェクトなどを書いていく
        //自身のアバター(ネットワークオブジェクト)を呼び出す
        //動かしているミノをアバターとして設定する

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
        PhotonNetwork.Instantiate(Minos[Random.Range(0, Minos.Length)], vector3(5,17,0), Quaternion.identity);
    }
    //ルームを退出する処理を書く
    //ゲームボードを右にもうひとつ増やして対戦相手を表示させる必要がある

    
}

