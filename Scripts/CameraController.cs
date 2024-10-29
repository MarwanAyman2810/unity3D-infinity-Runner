using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player;      public Vector3 offset;    
         void Update()
    {
                 transform.position = new Vector3(transform.position.x, transform.position.y, player.position.z + offset.z);
    }
}
