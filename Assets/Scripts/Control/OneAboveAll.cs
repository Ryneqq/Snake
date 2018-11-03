using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneAboveAll : MonoBehaviour {
    Astar astar;
    NN nn;
    BTree bt;
    Player player;

    void Start ()
    {
        // this.astar  = GameObject.Find("Astar Snake").GetComponent<Astar>();
        // this.player = GameObject.Find("Player Snake").GetComponent<Player>();
        // this.nn     = GameObject.Find("NN Snake").GetComponent<NN>();
        this.bt     = GameObject.Find("BTree Snake").GetComponent<BTree>();

        InvokeRepeating("MoveSnake", 0.3f, 0.3f);
    }

    private void MoveSnake()
    {
        // this.player.MoveSnake();
        // this.astar.MoveSnake();
        // this.nn.MoveSnake();
        this.bt.MoveSnake();
    }
}
