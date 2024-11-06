using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    public GameObject Sender;
    public GameObject Reciever;

 
    private void OnTriggerEnter(Collider collision)
    {
        var closestPlayer = collision.gameObject;

        if (closestPlayer.layer == 7) {
            Debug.Log("Teleporting player!!!!");
            closestPlayer.transform.position = Reciever.transform.position;
            closestPlayer.transform.Rotate(new Vector3(90, 0, 0));
        }
    }
}
