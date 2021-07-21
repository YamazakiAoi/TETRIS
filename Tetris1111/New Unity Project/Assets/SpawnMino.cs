using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class SpawnMino : MonoBehaviour
{
    public GameObject[] Minos;
    public int MinoShape = 0;

    // シャッフルするもとの配列
    private int[] ary1 = new int[] { 0, 1, 2, 3, 4, 5, 6};

    // ランダムな順にソートされた配列
    private int[] ary2 = new int[7];//ary1.OrderBy(i => Guid.NewGuid()).ToArray();

    // Start is called before the first frame update
    void Start()
    {
        
        Shuffle();
        NewMino();
        
    }

    public void NewMino()
    {
        
        //var obj = Instantiate(Minos[ary2[MinoShape]], transform.position, Quaternion.identity);
        var obj = Instantiate(Minos[0], transform.position, Quaternion.identity);
        obj.GetComponent<Mino>().type = Minos[ary2[MinoShape]].name;

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