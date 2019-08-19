using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeController : MonoBehaviour
{
    //blinking time (frames) and steps (how much the eye close/open when blink)
    public int blinkTime = 5;
    public float blinkStep = 0.1f;

    //while it is blinking
    public bool isBlinking = false;
    public int blinkWait;
    public bool closing = true;

    //default transform rotation of the eye color
    public Quaternion defaultRotation;

    public GameObject col;
        
    // Start is called before the first frame update
    void Awake()
    {
        blinkWait = blinkTime;
        defaultRotation = transform.GetChild(0).transform.rotation;
    }

    // Update is called once per frame
    //Human blinks around a wink each 4 seconds.
    // At 50FPS, a blink each 200 frames.
    //Also, takes around 0.1 seconds
    void Update()
    {
        if(Time.frameCount % 200 == 0)
        {
            isBlinking = true;
        }

        if (isBlinking)
        {
            Blink();
        }

        //follow mouse
        //FollowMouse();
    }

    //eye follows the mouse
    public void FollowMouse()
    {
        Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        //mouseRay = new Ray(new Vector3(mouseRay.origin.x, mouseRay.origin.y, 0), 
        //new Vector3(mouseRay.direction.x, mouseRay.direction.y, 0.1f));
        float midPoint = (transform.GetChild(0).transform.position - Camera.main.transform.position).magnitude * 0.5f;
        Vector3 lookThere = mouseRay.origin + mouseRay.direction * midPoint;
        transform.GetChild(0).transform.LookAt(lookThere);
    }

    //saccade behavior
    public void SaccadeBehavior(float saccadeMag)
    {
        //saccade
        transform.GetChild(0).transform.rotation = defaultRotation;
        transform.GetChild(0).transform.Rotate(new Vector3(0, 0, saccadeMag/3.14f));
    }

    //bliiink mothafucka!
    public void Blink()
    {
        //if it is closing, eye diminish
        if (closing)
        {
            transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y - blinkStep, 
                transform.localScale.z);
        }
            
        //if the eye is closed, wait a bit
        if (transform.localScale.y <= 0 && blinkWait > 0)
        {
            closing = false;
            blinkWait--;
        }

        //if it is not closing, eye open
        if (!closing)
        {
            transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y + blinkStep,
                transform.localScale.z);
        }

        //donezzo
        if(blinkWait < blinkTime && transform.localScale.y >= 1.5f)
        {
            isBlinking = false;
            closing = true;
            blinkWait = blinkTime;
        }
    }

    public void Movement(List<string> l, int side)
    {
        int index;
        string[] coord = l.ToArray();
        float[] cx = new float[7];
        float[] cy = new float[7];

        for (index = 0; index < 7; index++)
        {
            cx[index] = float.Parse(coord[index].Split(',')[0]);
            cy[index] = float.Parse(coord[index].Split(',')[1]);
        }

        Vector3[] worldC = new Vector3[6];

        /*worldC[0] = transform.Find("A").transform.localPosition;
        worldC[1] = transform.Find("B").transform.localPosition;
        worldC[2] = transform.Find("C").transform.localPosition;
        worldC[3] = transform.Find("D").transform.localPosition;
        worldC[4] = transform.Find("E").transform.localPosition;
        worldC[5] = transform.Find("F").transform.localPosition;*/

        //Debug.Log("42: "+cx[6]+" , "+cy[6]+" ; 45: "+cx[9]+" , "+cy[9]+" ; 69: "+cx[13]+" , "+cy[13]);

        float propImg;
        float propWorld;
        if (side == 1)
        {
            /*propImg = (cx[6] - cx[3]) / (cx[3] - cx[0]);
            propWorld = propImg * (10 - 20) + 20;*/
            propWorld = (315 * cx[0] - 225 * cx[3] - 90 * cx[6]) / (cx[3] - cx[0]);
            //Debug.Log("Right: pImg: "+propImg+" pWorld: "+propWorld);
        }
        else
        {
            //propImg = (cx[6] - cx[3]) / (cx[0] - cx[3]);
            //propWorld = cx[0]
            propWorld = (315*cx[0]-225*cx[3]-90*cx[6]) / (cx[3] - cx[0]);
            
         //   Debug.Log("Left: pImg: " + propImg );
        }

        //float propImg = (cx[6] - cx[0]) / (cx[3] - cx[0]);
        //float propWorld = propImg * (10 - 35) + 35;
        //Debug.Log(propWorld);

        /*Vector3 relativePos = (new Vector3(propWorld, 0, 0)) - col.transform.position;

        // the second argument, upwards, defaults to Vector3.up
        Quaternion rotation = Quaternion.LookRotation(relativePos, Vector3.up);
        col.transform.rotation = rotation;
        */

        col.transform.rotation = defaultRotation;
        col.transform.Rotate(new Vector3(0, propWorld * Time.deltaTime * 0.1f, 0));
        /*if (propWorld >= 0.5f)
        {
            Debug.Log(propWorld + " >>>>>>>>> 0.5 ");
            col.transform.Rotate(new Vector3(0, -propWorld / 55, 0));
        }
        else
        {
            col.transform.Rotate(new Vector3(0, propWorld / 55, 0));
            Debug.Log(" <<<<<<<<< 0.5 ");
        }*/

        //Debug.Log(transform.name + "NOT NORMALIZED-----" + new Vector3(0, propWorld, 0));
        //transform.GetChild(0).transform

        //Debug.Log(worldC[0].x);


        /*float cx = (p42x + p45x)/2;
        float cy = (p42y + p45y)/2;
        float vx = cx - p69x; 
        float vy = cy - p69y;*/

        /*Vector2 norm = new Vector2(vx, vy).normalized;
        Vector3 v = new Vector3(Mathf.Asin(norm.y), Mathf.Sin(norm.x), 0f);
        transform.GetChild(0).localEulerAngles = v;
        Debug.Log(cx + " " + cy + " " + vx + " " + vy+ " " + norm + " " + v);*/
    }
}
