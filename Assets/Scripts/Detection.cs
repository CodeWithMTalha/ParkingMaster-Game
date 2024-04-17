using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detection : MonoBehaviour
{

    GamePlaySc gameplay;


    //public Color startColor = Color.red;
    //public Color endColor = Color.clear;
    //public float speed = 1;
    //Renderer ren;


    public float blinkDuration = 20f;
    public Color blinkColor = Color.red;

    private Material barrierMaterial;
    private Color originalColor = Color.yellow;
    private bool isBlinking = false;

    Renderer barrierRenderer;
    private void Awake()
    {
       // ren = GetComponent<Renderer>();
    }

    // Start is called before the first frame update
    void Start()
    {
        gameplay = GameObject.FindGameObjectWithTag("gamecontrol").GetComponent<GamePlaySc>();

      /*  // Assuming the barrier has a Renderer component
        barrierRenderer = GetComponent<Renderer>();

        if (barrierRenderer != null)
        {
            barrierMaterial = barrierRenderer.material;
            originalColor = barrierMaterial.color;
        }
        else
        {
            Debug.LogError("Renderer component not found on the barrier.");
        }*/
    }

    // Update is called once per frame
    void Update()
    {
       // ren.material.color = Color.Lerp(startColor, endColor, Mathf.PingPong(Time.time * speed, 1));
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "checkpoint")
        {
            other.gameObject.SetActive(false);
            //gameObject.GetComponent<Renderer>().material.SetColor("Color_", Color.green);
        }

        if (other.gameObject.tag == "finishpoint")
        {
            gameplay.LevelComplete();
        }

      /*  if (other.gameObject.tag == "barrier")
        {
            //other.gameObject.GetComponent<Material>().color = Color.red;
            //other.gameObject.GetComponent<Material>().color = Color.red;
        }*/
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag =="barrier")
        {
            if (!isBlinking)
            {
               collision.gameObject.GetComponent<Renderer>().material.color = blinkColor;
                
                //barrierRenderer.material.color = blinkColor;
                StartCoroutine(BlinkEffect());
            }
        }
    }

    IEnumerator BlinkEffect()
    {
        //isBlinking = true;
        yield return new WaitForSeconds(2);
        gameplay.LevelFail();
        /* // Blink between red and green colors
         //barrierMaterial.color = blinkColor;
         gameObject.GetComponent<Renderer>().material.color = blinkColor;
         yield return new WaitForSeconds(blinkDuration);
         Debug.Log("original");
         barrierMaterial.color = originalColor;
         yield return new WaitForSeconds(blinkDuration);
         Debug.Log("false");
         isBlinking = false;*/
    }

}
