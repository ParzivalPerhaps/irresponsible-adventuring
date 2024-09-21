using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticFaceEntity : MonoBehaviour
{
    public float xOffset;

    private void Start() {
        if (!FindObjectOfType<StaticFaceEntityDirector>().containsFaceEntity(this)){
            FindObjectOfType<StaticFaceEntityDirector>().addFaceEntity(this);
        }
    }
}
