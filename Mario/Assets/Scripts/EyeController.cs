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

    public int factor = 20;

    // Start is called before the first frame update
    void Awake()
    {
        blinkWait = blinkTime;
        defaultRotation = col.transform.localRotation;
    }

    // Update is called once per frame
    //Human blinks around a wink each 4 seconds.
    // At 50FPS, a blink each 200 frames.
    //Also, takes around 0.1 seconds
    void Update()
    {
        if (Time.frameCount % 200 == 0)
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
        transform.GetChild(0).transform.Rotate(new Vector3(0, 0, saccadeMag / 3.14f));
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
        if (blinkWait < blinkTime && transform.localScale.y >= 1.5f)
        {
            isBlinking = false;
            closing = true;
            blinkWait = blinkTime;
        }
    }

    public void Movement(List<string> l, List<string> lb, int side)
    {
        int index;
        string[] coord = l.ToArray();
        string[] coordb = lb.ToArray();
        float[] cx = new float[7];
        float[] cy = new float[7];
        float[] cxb = new float[7];
        float[] cyb = new float[7];

        for (index = 0; index < 7; index++)
        {
            cx[index] = float.Parse(coord[index].Split(',')[0]);
            cy[index] = float.Parse(coord[index].Split(',')[1]);

            cxb[index] = float.Parse(coordb[index].Split(',')[0]);
            cyb[index] = float.Parse(coordb[index].Split(',')[1]);
        }

        // Pontos
        // 0 = 42
        // 1 = 43
        // 2 = 44
        // 3 = 45
        // 4 = 46
        // 5 = 47
        // 6 = 69


        float upperMid, upperMidB, bottomMid, bottomMidB;
        upperMid = (cy[2] + cy[1])/2;
        upperMidB = (cyb[2] + cyb[1]) / 2;
        bottomMid = (cy[5] + cy[4])/2;
        bottomMidB = (cyb[5] + cyb[4]) / 2;
        //Debug.Log("upperMid: "+upperMid+" - bottomMid: "+bottomMid);

        float propWorld, propWorldY, nextPropWorld, nextPropWorldY;
        
        propWorld = 225 - ((90 * (cx[6] - cx[0])) / (cx[3] - cx[0]));
        propWorldY = 15 - (((cy[6]-upperMid)*30)/(bottomMid-upperMid));
        nextPropWorld = 225 - ((90 * (cxb[6] - cxb[0])) / (cxb[3] - cxb[0]));
        nextPropWorldY = 15 - (((cyb[6] - upperMidB) * 30) / (bottomMidB - upperMidB));
        //float velX = 0.0f, velY = 0.0f;
        //float newPositionX = Mathf.SmoothDamp(nextPropWorld, propWorld, ref velX, 0.3f);
        //float newPositionY = Mathf.SmoothDamp(nextPropWorldY, propWorldY, ref velY, 0.3f);
        //transform.position = new Vector3(transform.position.x, newPosition, transform.position.z);
        float countAnglesX = propWorld, countAnglesY = propWorldY;
        Debug.Log(countAnglesX);
        for(int i = 0; i < factor; i++)
        {
            float angleXT = 0, angleYT = 0, testX = 0, testY = 0;
        //    Vector3 resultPropWorld = new Vector3(0, 0, 0), resultPropWorldY = new Vector3(0, 0, 0);
      //      Vector3 propWorldVector = new Vector3(0, 0, 0);
    //        Vector3 nextpropWorldVector = new Vector3(0, 0, 0);
  //          Vector3 propWorldYVector = new Vector3(0, 0, 0);
//            Vector3 nextpropWorlYdVector = new Vector3(0, 0, 0);

           // float aX, bX, cY, dY;


            if (i < factor-1)
            {
                angleXT = Mathf.LerpAngle((((nextPropWorld - propWorld) / factor) * i) + countAnglesX, (((nextPropWorld - propWorld) / factor) * (i + 1)) + countAnglesX, 0.005f);
                angleYT = Mathf.LerpAngle((((nextPropWorldY - propWorldY) / factor) * i) + countAnglesY, (((nextPropWorldY - propWorldY) / factor) * (i + 1)) + countAnglesY, 0.005f);

                testX = ((((nextPropWorld - propWorld) / factor) * (i + 1)) + countAnglesX) - ((((nextPropWorld - propWorld) / factor) * i) + countAnglesX);
                testY = ((((nextPropWorldY - propWorldY) / factor) * (i + 1)) + countAnglesY) - ((((nextPropWorldY - propWorldY) / factor) * i) + countAnglesY);
                //propWorldVector.y = (((nextPropWorld - propWorld) / factor) * i) + countAnglesX;
                //nextpropWorldVector.y = (((nextPropWorld - propWorld) / factor) * (i + 1)) + countAnglesX;
                //propWorldYVector.y = (((nextPropWorldY - propWorldY) / factor) * i) + countAnglesY;
                //nextpropWorlYdVector.y = (((nextPropWorldY - propWorldY) / factor) * (i + 1)) + countAnglesY;

                // aX = (((nextPropWorld - propWorld) / factor) * i) + countAnglesX;
                // bX = (((nextPropWorld - propWorld) / factor) * (i + 1)) + countAnglesX;
                // cY = (((nextPropWorldY - propWorldY) / factor) * i) + countAnglesY;
                // dY = (((nextPropWorldY - propWorldY) / factor) * (i + 1)) + countAnglesY;

                countAnglesX = countAnglesX + (((nextPropWorld - propWorld) / factor) * i);
                countAnglesY = countAnglesY + (((nextPropWorldY - propWorldY) / factor) * i);

                //Debug.Log(angleYT);
                //Debug.Log(transform.name + " Current - " + propWorld + " Next - " + nextPropWorld + " Angle - " + angle);
                //col.transform.localEulerAngles = new Vector3(angleYT, angleXT, 0f);
            }
            else
            {
                angleXT = Mathf.LerpAngle((((nextPropWorld - propWorld) / factor) * i) + countAnglesX, nextPropWorld + countAnglesX, 0.005f);
                angleYT = Mathf.LerpAngle((((nextPropWorldY - propWorldY) / factor) * i) + countAnglesY, nextPropWorldY + countAnglesY, 0.005f);


                testX = (nextPropWorld + countAnglesX )- ((((nextPropWorld - propWorld) / factor) * i) + countAnglesX);
                testY = (nextPropWorldY + countAnglesY) - ((((nextPropWorldY - propWorldY) / factor) * i) + countAnglesY);
                //propWorldVector.y = (((nextPropWorld - propWorld) / factor) * i) + countAnglesX;
                //nextpropWorldVector.y = nextPropWorld + countAnglesX;
                //propWorldYVector.y = (((nextPropWorldY - propWorldY) / factor) * i) + countAnglesY;
                //nextpropWorlYdVector.y = nextPropWorldY + countAnglesY;

                // aX = (((nextPropWorld - propWorld) / factor) * i) + countAnglesX;
                // bX = nextPropWorld + countAnglesX;
                // cY = (((nextPropWorldY - propWorldY) / factor) * i) + countAnglesY;
                //  dY = nextPropWorldY + countAnglesY;

                //countAnglesX = countAnglesX + ((nextPropWorld - propWorld) / 10);

                //countAnglesX = countAnglesX + (((nextPropWorld - propWorld) / 10) * i);


                //Debug.Log(transform.name + " Current - " + propWorld + " Next - " + nextPropWorld + " Angle - " + angle);

            }

            //col.transform.localRotation = Quaternion.Slerp(Quaternion., to.rotation, timeCount);
            //timeCount = timeCount + Time.deltaTime;

            //Quaternion rotationAgent = Quaternion.LookRotation(relativePos, Vector3.up);
            //col.transform.rotation = Quaternion.Lerp(new Quaternion(cY, 0, aX, 0), new Quaternion(dY,0,bX, 0), Mathf.SmoothStep(0.0f, 1.5f, Time.deltaTime));

            //resultPropWorld = Vector3.Lerp(propWorldVector, nextpropWorldVector, 0.05f);
            //resultPropWorldY = Vector3.Lerp(propWorldYVector, nextpropWorlYdVector, 0.05f);

            Debug.Log(angleXT);
            if(testY < -30)
            {
                testY = -30;
            }else if (testY > 30)
            {
                testY = 30;
            }
            col.transform.localEulerAngles = new Vector3(testY, testX, 0);

        }

        //float angleX = Mathf.LerpAngle(propWorld, nextPropWorld, 0.0000000000003f);
        //float angleY = (Mathf.LerpAngle(propWorldY, nextPropWorldY, 0.0000000000003f))%180f;


        //Debug.Log(angleY);
        //Debug.Log(transform.name + " Current - " + propWorld + " Next - " + nextPropWorld + " Angle - " + angle);
        //col.transform.localEulerAngles = new Vector3(angleY, angleX, 0f);
        //col.transform.localRotation = Quaternion.EulerAngles(new Vector3(0f, propWorld, 0f));
    }
}
