using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PopupText : MonoBehaviour
{
    private TMP_Text myText;

    [SerializeField] private float speed;   
    [SerializeField] private float desapearingSpeed;   
    [SerializeField] private float colorDesapearingSpeed;   
    [SerializeField] private float lifeTime;

    private float timer;

    private void Awake()
    {
        myText = GetComponent<TMP_Text>();
        timer = lifeTime;
    }

    private void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x, transform.position.y + 1),
            speed * Time.deltaTime);

        timer -= Time.deltaTime;
        if (timer < 0)
        {
            float alpha = myText.color.a - colorDesapearingSpeed * Time.deltaTime;
            myText.color = new Color(myText.color.r, myText.color.g, myText.color.b, alpha);

            if (myText.color.a < 50)
                speed = desapearingSpeed;

            if (myText.color.a <= 0)
                Destroy(gameObject);
        }
    }
}
