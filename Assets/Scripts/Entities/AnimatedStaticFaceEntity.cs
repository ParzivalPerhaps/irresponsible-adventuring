using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatedStaticFaceEntity : MonoBehaviour
{
    
    public Sprite[] frames;
    public float frameDelay;

    public bool tracking;
    public float xOffset = 0;

    SpriteRenderer sRenderer;

    int i;

    private IEnumerator Start()
    {
        if (tracking){
            gameObject.AddComponent<StaticFaceEntity>().xOffset = xOffset;
        }



        i = 0;
        sRenderer = GetComponent<SpriteRenderer>();

        while (true){
            sRenderer.sprite = frames[i];

            if (i + 1 == frames.Length){
                i = 0;
            }else{
                i++;   
            }

            yield return new WaitForSeconds(frameDelay);
        }

    }

}
