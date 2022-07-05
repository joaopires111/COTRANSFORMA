using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using TMPro;

public class ManagerHistorico : MonoBehaviour
{
    private int cont;
        //prefab dos botoes
    public GameObject Panelprefab;
    //lista de prefab dos botoes
    private GameObject[] Panelprefablist;
    TextMeshProUGUI[] TEXT;

    // Start is called before the first frame update
    void Start()
    {
        cont = 1;
        Panelprefablist = new GameObject[12];

        for (int i = 0; i < Panelprefablist.Length; i++) { 
            //CRIAR paineis de salas
            Panelprefablist[i] = Instantiate(Panelprefab, GameObject.FindGameObjectWithTag("Canvas").transform);
            Panelprefablist[i].transform.localPosition = new Vector3(110, 20 - (i * 100), 0);
            TEXT = Panelprefablist[i].GetComponentsInChildren<TMPro.TextMeshProUGUI>();

            TEXT[0].text= "Sala :" + i;

            //CRIAR evento de click no botão
            EventTrigger trigger = Panelprefablist[i].GetComponent<EventTrigger>();
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerClick;
            int x = i;
            entry.callback.AddListener((eventData) => { clicarHistorico(x); });
            trigger.triggers.Add(entry);
        }
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(cont);
        Vector3 adicao = new Vector3(0, 20, 0);
        //scroll para baixo
        if (Input.GetAxis("Mouse ScrollWheel") > 0 && cont > 0)
        {
            cont--;
            for (int i = 0; i < Panelprefablist.Length; i++)
            {

                Panelprefablist[i].transform.localPosition = new Vector3(110, 20 - (i * 100), 0) + (adicao * cont) ;
            }
        }
        //scroll para cima
        if (Input.GetAxis("Mouse ScrollWheel") < 0 && cont < Panelprefablist.Length * 4.5)

        {
            cont++;
            for (int i = 0; i < Panelprefablist.Length; i++)
            {
                Panelprefablist[i].transform.localPosition = new Vector3(110, 20 - (i * 100), 0) + (adicao * cont);
            }
        }
        // this C# script would go on your camra
    }

    public void clicarHistorico(int x)
    {
        SceneManager.LoadScene("SampleSceneHistorico");
    }

}
