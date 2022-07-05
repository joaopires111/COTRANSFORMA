using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Extensions;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class JuntarSala : MonoBehaviour
{

    public static Sala sala;
    public static string Jog;
    public Button juntar;
    public TMP_InputField NumSala;
    public TextMeshProUGUI warningsala;

    // Start is called before the first frame update
    void Start()
    {
        //juntar.interactable = true;
        //juntar.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "Aguarde";

        Jog = "jog2";
        AuthManager.Jog = Jog;

        Debug.Log(AuthManager.Jog = Jog);
    }

    private void Update()
    {
        
    }

    public void ComecarJogo()
    {
        readFirebaseSala(NumSala.text);
    }

    public void readFirebaseSala(string NumSala)
    {
        DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
        FirebaseDatabase.DefaultInstance
        .RootReference
        .Child("Salas")
        .Child(NumSala)
        .GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                warningsala.text = "Codigo errado!";
                // Handle the error...
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                sala = JsonUtility.FromJson<Sala>(snapshot.GetRawJsonValue());
                warningsala.text = "Codigo Correto!";

                if (!sala.jog2entrou) {

                sala.jog2entrou = true;
                sala.nomejog2 = AuthManager.Utilizador;
                string json = JsonUtility.ToJson(sala);
                DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
                reference.Child("Salas").Child(NumSala.ToString()).SetRawJsonValueAsync(json);

                SceneManager.LoadScene("SampleScene");
                }

            }
        });
    }
}
