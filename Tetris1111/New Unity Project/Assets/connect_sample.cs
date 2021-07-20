using Photon.Pun;
using Photon.Realtime;
using UnityEngine;


// PUNのコールバックを受け取れるようにする
public class SampleScene : MonoBehaviourPunCallbacks
{
    private void start()
    {
        //マスターサーバーに接続
        PhotonNetwork.ConnectUsingSettings();
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
        Debug.Log("ルームへ参加しました");
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


    /*
    // ゲームサーバーへの接続が成功した時に呼ばれるコールバック
    public override void OnJoinedRoom() 
    {
        //ここに同期させたいオブジェクトなどを書いていく
        //自身のアバター(ネットワークオブジェクト)を呼び出す
        //動かしているミノをアバターとして設定する

    }
    //ルームを退出する処理を書く
    //ゲームボードを右にもうひとつ増やして対戦相手を表示させる必要がある

    */
}

