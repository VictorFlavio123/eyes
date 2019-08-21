using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainController : MonoBehaviour
{
    private GameObject[] eyes;
    private string[] arr;
    private int k, index, kT;
    public GameObject left, right, rightT, leftT;

    private void Awake()
    {
        eyes = GameObject.FindGameObjectsWithTag("Eye");
    }

    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 24;
        List<string> lst = new List<string>();

        // Lê os pontos que precisamos (de todos os frames contido no diretório abaixo)
        foreach (string file in System.IO.Directory.EnumerateFiles("Assets/Data/BrossardEyes", "*.txt"))
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

        // Faz tratamento dos dados lidos (tira todas informações que não são X,Y)
        arr = lst.ToArray();
        for (int i = 0; i < arr.Length; i++)
        {
            string[] str = arr[i].Split(':');
            arr[i] = str[1];
            //Debug.Log(arr[i]);
            string[] aux = arr[i].Split(',');
            arr[i] = aux[0] + "," + aux[1];
        }

        // Garante que iremos acessar o vetor desde o início
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

        /*if (index < arr.Length - 2 * 14)
        {
            arr[index + 12] = ((float.Parse(arr[index + 12].Split(',')[0]) + float.Parse(arr[index + 12 + 14].Split(',')[0]) + float.Parse(arr[index + 12 + 2 * 14].Split(',')[0])) / 3).ToString("0.0000")
                              + ","
                              + ((float.Parse(arr[index + 12].Split(',')[1]) + float.Parse(arr[index + 12 + 14].Split(',')[1]) + float.Parse(arr[index + 12 + 2 * 14].Split(',')[1])) / 3).ToString("0.0000");

            arr[index + 13] = ((float.Parse(arr[index + 13].Split(',')[0]) + float.Parse(arr[index + 13 + 14].Split(',')[0]) + float.Parse(arr[index + 13 + 2 * 14].Split(',')[0])) / 3).ToString("0.0000")
                              + ","
                              + ((float.Parse(arr[index + 13].Split(',')[1]) + float.Parse(arr[index + 13 + 14].Split(',')[1]) + float.Parse(arr[index + 13 + 2 * 14].Split(',')[1])) / 3).ToString("0.0000");
        }*/


        // Pausa entre as rotações
        if (Time.frameCount % 24 == 0)
        {
            if (index < arr.Length - 14)
            {
                /*for (int i = 0; i < 10; i++)
                {
                    List<string> laT = new List<string>();
                    List<string> lbT = new List<string>();
                    List<string> maT = new List<string>();
                    List<string> mbT = new List<string>();

                    // Pega pontos do olho essquerdo
                    for (kT = 0; kT < 6; kT++)
                    {
                        laT.Add(arr[kT + index]); // Frame atual 
                        lbT.Add(arr[kT + index + 14]); // Frame seguinte
                    }
                    laT.Add(arr[12 + index]);
                    lbT.Add(arr[12 + index + 14]);

                    // Pega pontos do olho direito
                    for (kT = 6; kT < 12; kT++)
                    {
                        maT.Add(arr[kT + index]);
                        mbT.Add(arr[kT + index + 14]);
                    }
                    maT.Add(arr[13 + index]);
                    mbT.Add(arr[13 + index + 14]);

                    // Realiza movimento com base no pontos que foram pêgos
                    //rightT.GetComponent<EyeController>().Movement(maT, mbT, 1);
                    //leftT.GetComponent<EyeController>().Movement(maT, mbT, 1);
                }*/

                List<string> la = new List<string>();
                List<string> lb = new List<string>();
                List<string> ma = new List<string>();
                List<string> mb = new List<string>();

                // Pega pontos do olho essquerdo
                for (k = 0; k < 6; k++)
                {
                    la.Add(arr[k + index]); // Frame atual 
                    lb.Add(arr[k + index + 14]); // Frame seguinte
                }
                la.Add(arr[12 + index]);
                lb.Add(arr[12 + index + 14]);

                // Pega pontos do olho direito
                for (k = 6; k < 12; k++)
                {
                    ma.Add(arr[k + index]);
                    mb.Add(arr[k + index + 14]);
                }
                ma.Add(arr[13 + index]);
                mb.Add(arr[13 + index + 14]);

                // Realiza movimento com base no pontos que foram pêgos
                right.GetComponent<EyeController>().Movement(ma, mb, 1);
                left.GetComponent<EyeController>().Movement(ma, mb, 1);
                index += 14; // Incrementa para pegar próximo conjunto de pontos, ou seja, dados do próximo frame
            }
            else
            {
                // Recomeça loop
                index = 0;
            }
        }
        //}
    }

    private float CalculateSaccade()
    {
        return (-6.9f * Mathf.Log(Random.Range(1, 15) / 15.7f));
    }
}
