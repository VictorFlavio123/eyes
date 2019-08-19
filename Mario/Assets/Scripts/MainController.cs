using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainController : MonoBehaviour
{
    private GameObject[] eyes;
    private string[] arr;
    private int k, index;
    public GameObject left, right;

    private void Awake()
    {
        eyes = GameObject.FindGameObjectsWithTag("Eye");

        
    }

    // Start is called before the first frame update
    void Start()
    {
        List<string> lst = new List<string>();
        foreach (string file in System.IO.Directory.EnumerateFiles("Assets/Data/angry_out_txt", "*.txt"))
        {
            string[] lines = System.IO.File.ReadAllLines(file);
            // Left eye
            lst.Add(lines[35]); lst.Add(lines[36]); lst.Add(lines[37]);
            lst.Add(lines[38]); lst.Add(lines[39]); lst.Add(lines[40]);
            // Right eye
            lst.Add(lines[41]); lst.Add(lines[42]); lst.Add(lines[43]);
            lst.Add(lines[44]); lst.Add(lines[45]); lst.Add(lines[46]);
            // Pupils
            lst.Add(lines[67]); lst.Add(lines[68]);      
        }
        arr = lst.ToArray();
        for (int i = 0; i < arr.Length; i++)
        {
            string[] str = arr[i].Split(':');
            arr[i] = str[1];
            string[] aux = arr[i].Split(',');
            arr[i] = aux[0] + "," + aux[1];
        }

        
        index = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //for each eye, generate a saccade behavior
        //float saccadeMag = CalculateSaccade();

        //foreach(GameObject eye in eyes)
        //{
        //eye.GetComponent<EyeController>().SaccadeBehavior(saccadeMag);
        //Debug.Log(arr.Length);

        if (index < arr.Length - 2 * 14)
        {
            arr[index + 12] = ((float.Parse(arr[index + 12].Split(',')[0]) + float.Parse(arr[index + 12 + 14].Split(',')[0]) + float.Parse(arr[index + 12 + 2 * 14].Split(',')[0])) / 3).ToString("0.0000")
                              + ","
                              + ((float.Parse(arr[index + 12].Split(',')[1]) + float.Parse(arr[index + 12 + 14].Split(',')[1]) + float.Parse(arr[index + 12 + 2 * 14].Split(',')[1])) / 3).ToString("0.0000");

            arr[index + 13] = ((float.Parse(arr[index + 13].Split(',')[0]) + float.Parse(arr[index + 13 + 14].Split(',')[0]) + float.Parse(arr[index + 13 + 2 * 14].Split(',')[0])) / 3).ToString("0.0000")
                              + ","
                              + ((float.Parse(arr[index + 13].Split(',')[1]) + float.Parse(arr[index + 13 + 14].Split(',')[1]) + float.Parse(arr[index + 13 + 2 * 14].Split(',')[1])) / 3).ToString("0.0000");
        }

        if (index < arr.Length)
        {
            List<string> l = new List<string>();
            List<string> m = new List<string>();
            for (k = 0; k < 6; k++)
                l.Add(arr[k+index]);
            l.Add(arr[12+index]);
            for (k = 6; k < 12; k++)
                m.Add(arr[k + index]);
            m.Add(arr[13 + index]);
            right.GetComponent<EyeController>().Movement(m, 1);
            left.GetComponent<EyeController>().Movement(l, 0);
            index += 14;
        }
        else
        {
            index = 0;
        }
        //}
    }

    private float CalculateSaccade()
    {
        return (-6.9f * Mathf.Log(Random.Range(1, 15) / 15.7f));
    }
}
