using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticFaceEntityDirector : MonoBehaviour
{
    public List<StaticFaceEntity> faceEntities = new List<StaticFaceEntity>();
    private Transform plrTransform;

    private PlayerController plr;
    int y;
    // Start is called before the first frame update
    void Start()
    {
        plr = FindObjectOfType<PlayerController>();
        plrTransform = plr.transform;
        y = 0;
    }

    // Update is called once per frame
    void Update()
    {

        if (y == 12){
            foreach (StaticFaceEntity s in FindObjectsOfType<StaticFaceEntity>()){
                faceEntities.Add(s);
            }
        
        }

        plrTransform = plr.transform;

        foreach (StaticFaceEntity f in faceEntities){
            if (f == null){
                continue;
            }else{
                f.gameObject.transform.LookAt(plrTransform);
                f.gameObject.transform.rotation = Quaternion.Euler(f.xOffset, f.transform.rotation.eulerAngles.y, f.transform.rotation.eulerAngles.z);
            }
        }
        y++;
    }

    public void addFaceEntity(StaticFaceEntity f){
        faceEntities.Add(f);
    }

    public bool containsFaceEntity(StaticFaceEntity f){
        return faceEntities.Contains(f);
    }
}
