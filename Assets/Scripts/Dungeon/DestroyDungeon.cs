using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyDungeon : MonoBehaviour
{

    public GameObject player;
    private string tagRoom = "room";

    public GameObject prefabRoom;
    public Vector3 spawnPosition;

    public CameraControl cameraControl;

    void Update()
    {
       if(cameraControl.Dungeon == true && Input.GetKeyDown(KeyCode.C)){
        GameObject[] items = GameObject.FindGameObjectsWithTag(tagRoom);

            foreach (GameObject obj in items)
        {
            Destroy(obj);
        }
    
       }else if(cameraControl.Dungeon == false && Input.GetKeyDown(KeyCode.C)){
         Instantiate(prefabRoom, spawnPosition, Quaternion.identity);

         player.transform.position = new Vector3(0, 0, 0);
       }
    }




}
