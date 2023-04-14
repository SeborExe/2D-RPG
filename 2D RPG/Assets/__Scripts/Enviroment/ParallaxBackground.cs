using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    private GameObject Cam;

    [SerializeField] private float parallaxEffect;

    private float xPosition;
    private float length;

    private void Awake()
    {
        Cam = Camera.main.gameObject;
    }

    private void Start()
    {
        xPosition = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    private void Update()
    {
        float distanceMoved = Cam.transform.position.x * (1 - parallaxEffect);
        float distanceToMove = Cam.transform.position.x * parallaxEffect;

        transform.position = new Vector3(xPosition + distanceToMove, transform.position.y);

        if (distanceMoved > xPosition + length) 
        {
            xPosition += length;
        }
        else if (distanceMoved < xPosition - length) 
        { 
            xPosition -= length;
        }
    }
}
