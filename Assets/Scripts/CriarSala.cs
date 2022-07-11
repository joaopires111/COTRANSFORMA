using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Extensions;
using UnityEngine.SceneManagement;
using TMPro;

public class CriarSala : MonoBehaviour
{

    public static Sala sala;
    public static string Jog;

    private float countdown;
    private bool esperar;
    private int NumSala;
    public TextMeshProUGUI warningsala, NumeroSAla;
    private Jog currentjog;

    // Start is called before the first frame update
    void Start()
    {

        warningsala.text = "Aguarde que o jogador entre...";
        esperar = true;
        Jog = "jog1";
        AuthManager.Jog = Jog;
        sala = new Sala();
        sala.nomejog1 = AuthManager.Utilizador;


        NumSala = Random.Range(0, 9999);
        sala.propor[sala.Ronda] = "jog1";

        NumeroSAla.text = "Código da sala: " + NumSala.ToString();

        string json = JsonUtility.ToJson(sala);
        DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
        reference.Child("Salas").Child(NumSala.ToString()).SetRawJsonValueAsync(json);

        readFirebaseJog();
        reference = FirebaseDatabase.DefaultInstance.RootReference;
        reference.Child("jogadores").Child(AuthManager.Utilizador).Child("codigosala").Child((0).ToString()).SetValueAsync(NumSala);
    }

    private void Update()
    {
        if (esperar == true)
        {
            countdown -= Time.deltaTime;




            if (countdown <= 0.0f)
            {

                readFirebaseSala();
                countdown = 3.0f;

                if (sala.jog2entrou)
                {

                    esperar = false;
                    SceneManager.LoadScene("SampleScene");
                }
            }
        }
    }

    public void readFirebaseSala()
    {
        DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
        FirebaseDatabase.DefaultInstance
        .RootReference
        .Child("Salas")
        .Child(NumSala.ToString())
        .GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                // Handle the error...
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                sala = JsonUtility.FromJson<Sala>(snapshot.GetRawJsonValue());

            }
        });
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
                Debug.Log(currentjog);
            }
        });
    }


}

public class Sala
{
    public bool jog2entrou;
    public int Ronda;
    public int[] PtsJog1;
    public int[] PtsJog2;

    public string nomejog1, nomejog2;

    public Quaternion[] rotacaojog1;
    public Quaternion[] rotacaojog2;

    public Vector3[] posjog1;
    public Vector3[] posjog2;

    public Vector3[] escalajog1;
    public Vector3[] escalajog2;

    public Matrix4x4[] matrixjog1;
    public Matrix4x4[] matrixjog2;

    public string[] propor;


    public Sala()
    {
        jog2entrou = false;

        rotacaojog1 = new Quaternion[10];
        rotacaojog2 = new Quaternion[10];

        posjog1 = new Vector3[10];
        posjog2 = new Vector3[10];

        escalajog1 = new Vector3[10];
        escalajog2 = new Vector3[10];

        Ronda = 0;

        matrixjog1 = new Matrix4x4[10];
        matrixjog2 = new Matrix4x4[10];

        PtsJog1  = new int[10];
        PtsJog2 = new int[10];

        propor = new string[10];


    }
}


