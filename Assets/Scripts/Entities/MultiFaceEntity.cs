using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiFaceEntity : MonoBehaviour
{
    public Sprite[] inputFaces = new Sprite[4];

    public bool relative;

    SpriteRenderer spriteRenderer;

    public Sprite[] faces;

    Sprite activeFace;

    GameObject visObj; // Holds the sprite renderer and points towards the player

    public GameObject player;

    int direction;

    float xDiff;
    float zDiff;

    Transform pTransform;
    Transform sTransform;

    float lastFrameX;
    float lastFrameZ;

    int f = 0;

    // Start is called before the first frame update
    void Start()
    {
        faces = (Sprite[]) inputFaces.Clone();
        activeFace = faces[0];
       
        visObj = new GameObject("VisObject");
        visObj.transform.position = gameObject.transform.position;

        spriteRenderer = visObj.AddComponent<SpriteRenderer>();
        player = null;

        lastFrameX = transform.position.x;
        lastFrameZ = transform.position.z;        
    }

    // Update is called once per frame
    void Update()
    {
        f++;
        if (f == 12){
            player = FindObjectOfType<PlayerController>().gameObject;
        }

        if (player.transform != null){
            visObj.transform.position = gameObject.transform.position;
            ManageFace(player.transform);
            spriteRenderer.sprite = activeFace;

            visObj.transform.LookAt(player.transform);
            visObj.transform.rotation = Quaternion.Euler(0, visObj.transform.rotation.eulerAngles.y, visObj.transform.rotation.eulerAngles.z);
        }

    }

    
    void ManageFace(Transform playerTransform){
        // Right = P-X > E-X
        direction = 1; // 1/2/3/4

        GameObject targetObject = gameObject.GetComponent<GeneralBehavior>().targetActivity.gameObject;
        if (DistanceTo(transform.position.x, targetObject.gameObject.transform.position.x, true) > DistanceTo(transform.position.z, targetObject.gameObject.transform.position.z, true)){
            // X
            if (DistanceTo(transform.position.x, targetObject.gameObject.transform.position.x, false) > 0){
                direction = 4;
            }else{
                direction = 3;
            }
        }else{
            // Y
             if (DistanceTo(transform.position.z, targetObject.gameObject.transform.position.z, false) > 0){
                direction = 2;
            }else{
                direction = 1;
            }
        }

        // Input Faces Key
        // 0 = Front
        // 1 = Back
        // 2 = Left
        // 3 = Right

        if (direction == 1){
            faces[0] = inputFaces[0];
            faces[1] = inputFaces[1];
            faces[2] = inputFaces[2];
            faces[3] = inputFaces[3];
        }else if (direction == 2){
            faces[0] = inputFaces[1];
            faces[1] = inputFaces[0];
            faces[2] = inputFaces[3];
            faces[3] = inputFaces[2];
        }else if (direction == 3){
            faces[0] = inputFaces[3];
            faces[1] = inputFaces[2];
            faces[2] = inputFaces[0];
            faces[3] = inputFaces[1];
        }else if(direction == 4){
            faces[0] = inputFaces[2];
            faces[1] = inputFaces[3];
            faces[2] = inputFaces[1];
            faces[3] = inputFaces[0];
        }

        xDiff = Mathf.Abs(playerTransform.position.x - gameObject.transform.position.x);
        zDiff = Mathf.Abs(playerTransform.position.z - gameObject.transform.position.z);
        if (xDiff > zDiff){
            if (playerTransform.position.x > gameObject.transform.position.x){
                activeFace = faces[2];
            }else{
                activeFace = faces[3];
            }
        }else{
            if (playerTransform.position.z > gameObject.transform.position.z){
                activeFace = faces[0];
            }else{
                activeFace = faces[1];
            }
        }

    }

    float DistanceTo(float pos1, float pos2, bool absolute){
        if (absolute){
            return Mathf.Abs(pos1) - Mathf.Abs(pos2);
        }else{
            return pos1 - pos2;
        }

    }
    
}
