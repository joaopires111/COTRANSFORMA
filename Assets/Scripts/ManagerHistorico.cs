using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using TMPro;
using Firebase.Database;
using Firebase.Extensions;
using System;

public class ManagerHistorico : MonoBehaviour
{
    private int cont;
        //prefab dos botoes
    public GameObject Panelprefab;
    //lista de prefab dos botoes
    private GameObject[] Panelprefablist;
    TextMeshProUGUI[] TEXT;
    private Sala[] salas;
    //public Jog currentjog;
    public Jog currentjog;
    private float countdown;
    private bool esperar;

    //devido a nao conseguir ler o metodo acima este é copiado

    // Start is called before the first frame update
    void postStart()
    {
        bool esperar = true;
        currentjog = new Jog();
        currentjog.codigosala[0] = 0;
        //valor inicial do mousewheel
        cont = 1;
        Panelprefablist = new GameObject[12];
        readFirebaseJog();

    }

    private string verificarvencedor(Sala sala)
    {
        int pontostotalj1 = 0, pontostotalj2 = 0;
        foreach (int pontos in sala.PtsJog1)
        {
            pontostotalj1 += pontos;
        }
        foreach (int pontos in sala.PtsJog2)
        {
            pontostotalj2 += pontos;
        }

        if (pontostotalj1 > pontostotalj2)
        {
            return "Jogador 1";
        }
        else
        {
            return "Jogador 2";
        }


    }

    // Update is called once per frame
    void Update()
    {
        if (esperar == true)
        {
            countdown -= Time.deltaTime;
            if (countdown <= 0.0f)
            {
                countdown = 3.0f;

                Debug.Log("WAAAAAAAAAAAAAAAAAAAAAAAAAAAA" + currentjog.codigosala[0]);

                if (currentjog.codigosala[0] != 0)
                {

                    esperar = false;
                    readFirebaseSalaLista();

                    int i = 0;


                    foreach (Sala sala in salas)
                    {

                        //CRIAR paineis de salas
                        Panelprefablist[i] = Instantiate(Panelprefab, GameObject.FindGameObjectWithTag("Canvas").transform);
                        Panelprefablist[i].transform.localPosition = new Vector3(110, 20 - (i * 100), 0);
                        TEXT = Panelprefablist[i].GetComponentsInChildren<TMPro.TextMeshProUGUI>();

                        TEXT[0].text = "Jogo :" + i;
                        TEXT[1].text = "Codigo :" + currentjog.codigosala[i];
                        TEXT[2].text = "Jogador1 :" + sala.nomejog1;
                        TEXT[3].text = "Jogador2 :" + sala.nomejog2;

                        TEXT[4].text = "Pts j1:" + verificarvencedor(sala);

                        //CRIAR evento de click no botão
                        EventTrigger trigger = Panelprefablist[i].GetComponent<EventTrigger>();
                        EventTrigger.Entry entry = new EventTrigger.Entry();
                        entry.eventID = EventTriggerType.PointerClick;
                        int x = i;
                        entry.callback.AddListener((eventData) => { clicarHistorico(x); });
                        trigger.triggers.Add(entry);
                        i++;
                    }
                }
            }
        }

        else { 
        Vector3 adicao = new Vector3(0, 20, 0);
        //scroll para baixo
        if (Input.GetAxis("Mouse ScrollWheel") > 0 && cont > 0)
        {
            cont--;
            for (int i = 0; i < Panelprefablist.Length; i++)
            {

                Panelprefablist[i].transform.localPosition = new Vector3(110, 20 - (i * 100), 0) + (adicao * cont);
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
    }

    public void clicarHistorico(int x)
    {
        SceneManager.LoadScene("SampleSceneHistorico");
    }

    public void readFirebaseSalaLista()
    {
        DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
        Debug.Log("CURRENTJOGARRRGGG" + currentjog.codigosala[0]);


        for (int i = 0; i < 1; i++) {
            if(currentjog.codigosala[i] != 0) { 
        FirebaseDatabase.DefaultInstance
        .RootReference
        .Child("Salas")
        .Child(currentjog.codigosala[0].ToString())
        .GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                // Handle the error...
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                salas[i] = JsonUtility.FromJson<Sala>(snapshot.GetRawJsonValue());
                i++;

            }
        });

    }
            }
    }

    public void readFirebaseJog()
    {
        DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
        FirebaseDatabase.DefaultInstance
        .RootReference
        .Child("jogadores")
        .Child(AuthManager.Utilizador)
        .GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                // Handle the error...
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                 currentjog = JsonUtility.FromJson<Jog>(snapshot.GetRawJsonValue());
                Debug.Log("CURRENTJOGARRR1111" + currentjog.codigosala[0]);
            }
        });
    }

}
public class Jog
{
    public int[] codigosala;


    public Jog()
    {
        codigosala = new int[10];
    }
}