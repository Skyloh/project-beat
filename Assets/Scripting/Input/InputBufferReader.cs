using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputBufferReader : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        PriorityQueue<int, int> queue = new (Comparer<int>.Create((int a, int b) => { return a-b; }));

        queue.Enqueue(1, 1);
        Debug.Log(queue.Peek());
        queue.Enqueue(2, 2);
        queue.Enqueue(3, 3);
        queue.Enqueue(-1, -1);
        Debug.Log(queue);
        Debug.Log(queue.Dequeue());
        Debug.Log(queue.Peek());
        Debug.Log(queue.Dequeue());
        Debug.Log(queue.Peek());
    }
}

/*
 * Input / Input Buffer Wishlist:
 * 
 * an input buffer for every player
 * emitting signals about buffered inputs to listeners
 * priority queue of inputs (w/ an SO to store priority lists of inputs, so it can be unique per character)
 * means to externally extend extinction window of inputs
 *  e.g. if a move has a long end period, we should be able to make an event flag that extends extinction period for any input registered in it
 *  player-unique, ofc
 * custom yield instruction to wait for input/while input held/etc.
 * 
 * 
 */ 
