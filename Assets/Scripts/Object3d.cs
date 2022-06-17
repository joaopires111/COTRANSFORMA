using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Firebase.Database;
using Firebase.Extensions;

public class Object3d : MonoBehaviour
{
    private float x1, y1, z1, x2, y2, z2, x3, y3, z3, x4, y4, z4, w1, w2, w3, w4;
    public float countdown, countdown2;
    private string eixo;
    private bool startanimation = false, esperar;
    private string Jog;
    Sala sala;



    public InputField inputx1, inputy1, inputz1, inputx2, inputy2, inputz2, inputx3, inputy3, inputz3;
    public InputField inputx4, inputy4, inputz4, inputw1, inputw2, inputw3, inputw4;

    public InputField Valores;
    public TextMeshProUGUI textboxValores;
    public TextMeshProUGUI Pilha;
    public Button ButtonScale, ButtonTranslate, ButtonRotateX, ButtonRotateY, ButtonRotateZ, ButtonEnviar, ButtonAplicarTransform;
    public GameObject CuboProposto;

    private float angulo, angulodividido;
    private Vector3 axis;
    private ArrayList PilhaMatriz;
    private Matrix4x4 matrixfinal;
    public Matrix4x4 localToWorldMatrix;
    private Vector4 column0, column1, column2, column3;
    private MatrixFirebase readDB;
    private Vector3 scale, position, scale2, position2, scaleDividido, positionDividido, scaleant, posant;
    private Quaternion rotateant, rotate2;
    private bool modoadivinha;

    // Start is called before the first frame update
    void Start()
    {
        esperar = false;
        sala = new Sala();
        sala.Ronda = 0;
        Jog = ManagerBotoes.Jog;
        Debug.Log(Jog);

        textboxValores.text = "";
        countdown = 1.0f;
        countdown2 = 3.0f;
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 30;
        

        PilhaMatriz = new ArrayList();
        resetmatriz();
        deactivateMatriz();

        if (Jog == "jog2")
        {
            sala.proporjog2[sala.Ronda] = false;
            esperar = true;
            Pilha.text = "espere pelo jogador1";
            deactivateButtons();
        }
        else
        {
            esperar = false;
            sala.proporjog1[sala.Ronda] = true;
            Pilha.text = "Envie a proposta ao jogador2";

            string json = JsonUtility.ToJson(sala);
            DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
            reference.Child("sala2").SetRawJsonValueAsync(json);

        }
    }

    private void resetmatriz()
    {
        inputx1.text = "1";
        inputy1.text = "0";
        inputz1.text = "0";
        inputx2.text = "0";
        inputy2.text = "1";
        inputz2.text = "0";
        inputx3.text = "0";
        inputy3.text = "0";
        inputz3.text = "1";

        inputx4.text = "0";
        inputy4.text = "0";
        inputz4.text = "0";
        inputw1.text = "0";
        inputw2.text = "0";
        inputw3.text = "0";
        inputw4.text = "1";

        Valores.interactable = false;
        Valores.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        if (esperar == true)
        {
            countdown2 -= Time.deltaTime;
            if (countdown2 <= 0.0f)
            {
                readFirebaseSala();
                countdown2 = 3.0f;


                Debug.Log(Jog == "jog2" && sala.matrixjog1[sala.Ronda] != Matrix4x4.zero && sala.proporjog1[sala.Ronda]);

                if (Jog == "jog2" && sala.matrixjog1[sala.Ronda] != Matrix4x4.zero && sala.proporjog1[sala.Ronda])
                {
                    aplicarTransformCuboProposto(sala.matrixjog1[sala.Ronda], sala.rotacaojog1[sala.Ronda]);
                    esperar = false;
                }

                if (Jog == "jog1" && sala.matrixjog2[sala.Ronda] != Matrix4x4.zero && sala.proporjog2[sala.Ronda])
                {
                    aplicarTransformCuboProposto(sala.matrixjog2[sala.Ronda], sala.rotacaojog2[sala.Ronda]);
                    esperar = false;
                }


            }
        }
        else
        {
            if (startanimation == true)
            {
                countdown -= Time.deltaTime;

                if (countdown >= 0.0f)
                {

                    if (angulo == 0) { 
                    transform.localScale += scaleDividido;
                    }
                    transform.RotateAround(Vector3.zero, axis, angulodividido);
                    transform.Translate(positionDividido, Space.World);



                }
                else
                {
                    //transform.localScale = scale;
                    //transform.RotateAround(Vector3.zero, axis, angulo);
                    //transform.Translate(position, Space.World);




                    angulo = 0;

                    startanimation = false;
                    countdown = 1.0f;
                }
            }


        }

    }
    private void deactivateButtons()
    {
        ButtonScale.interactable = false;
        ButtonTranslate.interactable = false;
        ButtonRotateX.interactable = false;
        ButtonRotateY.interactable = false;
        ButtonRotateZ.interactable = false;
        ButtonEnviar.interactable = false;
        ButtonAplicarTransform.interactable = false;
        deactivateMatriz();
    }
    private void activateButtons()
    {
        ButtonScale.interactable = true;
        ButtonTranslate.interactable = true;
        ButtonRotateX.interactable = true;
        ButtonRotateY.interactable = true;
        ButtonRotateZ.interactable = true;
        ButtonEnviar.interactable = true;
        ButtonAplicarTransform.interactable = true;
    }
    private void deactivateMatriz()
    {
        inputx1.interactable = false;
        inputy1.interactable = false;
        inputz1.interactable = false;
        inputx2.interactable = false;
        inputy2.interactable = false;
        inputz2.interactable = false;
        inputx3.interactable = false;
        inputy3.interactable = false;
        inputz3.interactable = false;

        inputx4.interactable = false;
        inputy4.interactable = false;
        inputz4.interactable = false;
        inputw1.interactable = false;
        inputw2.interactable = false;
        inputw3.interactable = false;
        inputw4.interactable = false;

        Valores.interactable = false;
    }
    public void ButtonScaleaActive()
    {
        deactivateMatriz();
        inputx1.interactable = true;
        inputy2.interactable = true;
        inputz3.interactable = true;


        Color newColor = new Color(0.62f, 0.25f, 0.6f, 1f);
        Color highlighted = new Color(0.48f, 0.2f, 0.47f, 1f);


        ColorBlock cb = inputx1.colors;
        cb.normalColor = newColor;
        cb.selectedColor = newColor;
        cb.highlightedColor = highlighted;
        cb.pressedColor = highlighted;
        inputx1.colors = cb;
        inputy2.colors = cb;
        inputz3.colors = cb;
    }


    public void ButtonTranslateActive()
    {
        deactivateMatriz();
        inputx4.interactable = true;
        inputy4.interactable = true;
        inputz4.interactable = true;

        Color newColor = new Color(0.16f, 0.62f, 0.56f, 1f);
        Color highlighted = new Color(0.13f, 0.48f, 0.43f, 1f);



        ColorBlock cb = inputx4.colors;
        cb.normalColor = newColor;
        cb.selectedColor = newColor;
        cb.highlightedColor = highlighted;
        cb.pressedColor = highlighted;
        inputx4.colors = cb;
        inputy4.colors = cb;
        inputz4.colors = cb;
    }
    public void ButtonRotateXActive()
    {
        deactivateMatriz();
        Valores.interactable = true;
        inputy2.interactable = true;
        inputy3.interactable = true;
        inputz2.interactable = true;
        inputz3.interactable = true;
        textboxValores.text = "Rotate X";
        eixo = "x";

        Color newColor = new Color(0.95f, 0.64f, 0.38f, 1f);
        Color highlighted = new Color(0.68f, 0.44f, 0.27f, 1f);



        ColorBlock cb = inputy2.colors;
        cb.normalColor = newColor;
        cb.selectedColor = newColor;
        cb.highlightedColor = highlighted;
        cb.pressedColor = highlighted;
        inputy2.colors = cb;
        inputy3.colors = cb;
        inputz2.colors = cb;
        inputz3.colors = cb;

    }
    public void ButtonRotateYActive()
    {

        deactivateMatriz();
        Valores.interactable = true;
        inputx1.interactable = true;
        inputx3.interactable = true;
        inputz1.interactable = true;
        inputz3.interactable = true;
        eixo = "y";

        Color newColor = new Color(0.91f, 0.44f, 0.32f, 1f);
        Color highlighted = new Color(0.62f, 0.29f, 0.22f, 1f);



        ColorBlock cb = inputx3.colors;
        cb.normalColor = newColor;
        cb.selectedColor = newColor;
        cb.highlightedColor = highlighted;
        cb.pressedColor = highlighted;
        inputx1.colors = cb;
        inputx3.colors = cb;
        inputz1.colors = cb;
        inputz3.colors = cb;
    }
    public void ButtonRotateZActive()
    {

        deactivateMatriz();
        Valores.interactable = true;
        inputx1.interactable = true;
        inputx2.interactable = true;
        inputy1.interactable = true;
        inputy2.interactable = true;
        eixo = "z";

        Color newColor = new Color(0.36f, 0.73f, 0.31f, 1f);
        Color highlighted = new Color(0.28f, 0.56f, 0.24f, 1f);


        ColorBlock cb = inputy1.colors;
        cb.normalColor = newColor;
        cb.selectedColor = newColor;
        cb.highlightedColor = highlighted;
        cb.pressedColor = highlighted;
        inputx1.colors = cb;
        inputx2.colors = cb;
        inputy1.colors = cb;
        inputy2.colors = cb;
    }






    public void aplicarModoFacil()
    {
        float rotacao = float.Parse(Valores.text) * Mathf.Deg2Rad;

        angulo = float.Parse(Valores.text);

        if (eixo == "x")
        {
            y2 = Mathf.Round(Mathf.Cos(rotacao) * 1000f) / 1000f;
            y3 = Mathf.Round(Mathf.Sin(rotacao) * 1000f) / 1000f;
            z2 = -Mathf.Round(Mathf.Sin(rotacao) * 1000f) / 1000f;
            z3 = Mathf.Round(Mathf.Cos(rotacao) * 1000f) / 1000f;

            inputy2.text = y2.ToString();
            inputy3.text = y3.ToString();
            inputz2.text = z2.ToString();
            inputz3.text = z3.ToString();

            axis = Vector3.right;
           
        }

        if (eixo == "y")
        {

            x1 = Mathf.Round(Mathf.Cos(rotacao) * 1000f) / 1000f;
            x3 = -Mathf.Round(Mathf.Sin(rotacao) * 1000f) / 1000f;
            z1 = Mathf.Round(Mathf.Sin(rotacao) * 1000f) / 1000f;
            z3 = Mathf.Round(Mathf.Cos(rotacao) * 1000f) / 1000f;

            inputx1.text = x1.ToString();
            inputx3.text = x3.ToString();
            inputz1.text = z1.ToString();
            inputz3.text = z3.ToString();

            axis = Vector3.up;
        }

        if (eixo == "z")
        {

            x1 = Mathf.Round(Mathf.Cos(rotacao) * 1000f) / 1000f;
            x2 = -Mathf.Round(Mathf.Sin(rotacao) * 1000f) / 1000f;
            y1 = Mathf.Round(Mathf.Sin(rotacao) * 1000f) / 1000f;
            y2 = Mathf.Round(Mathf.Cos(rotacao) * 1000f) / 1000f;

            inputx1.text = x1.ToString();
            inputx2.text = x2.ToString();
            inputy1.text = y1.ToString();
            inputy2.text = y2.ToString();

            axis = Vector3.forward;

        }


    }

    public void aplicarTransformCuboProposto(Matrix4x4 matriz, Vector3 rotacaojog)
    {
        position2 = matriz.ExtractPosition();
        scale2 = matriz.ExtractScale();
        //rotate2 = matriz.ExtractRotation();

        CuboProposto.transform.localPosition = position2;
        CuboProposto.transform.localScale = scale2;
        CuboProposto.transform.eulerAngles = rotacaojog;

        Pilha.text = "Responda ao jogador1";
        activateButtons();

        if (Jog == "jog2" && sala.proporjog1[sala.Ronda])
        {
            modoadivinha = true;
            Pilha.text = "modoadivinha";
        }
        if (Jog == "jog1" && sala.proporjog2[sala.Ronda])
        {
            modoadivinha = true;
            Pilha.text = "modoadivinha";
        }

    }





    public void aplicarTransform()
    {
        Matrix4x4 matrix;

        x1 = float.Parse(inputx1.text);
        x2 = float.Parse(inputx2.text);
        x3 = float.Parse(inputx3.text);
        y1 = float.Parse(inputy1.text);
        y2 = float.Parse(inputy2.text);
        y3 = float.Parse(inputy3.text);
        z1 = float.Parse(inputz1.text);
        z2 = float.Parse(inputz2.text);
        z3 = float.Parse(inputz3.text);

        x4 = float.Parse(inputx4.text);
        y4 = float.Parse(inputy4.text);
        z4 = float.Parse(inputz4.text);
        w1 = float.Parse(inputw1.text);
        w2 = float.Parse(inputw2.text);
        w3 = float.Parse(inputw3.text);
        w4 = float.Parse(inputw4.text);

        
        column0 = new Vector4(x1, y1, z1, w1);
        column1 = new Vector4(x2, y2, z2, w2);
        column2 = new Vector4(x3, y3, z3, w3);
        column3 = new Vector4(x4, y4, z4, w4);

        matrix = new Matrix4x4(column0, column1, column2, column3);

        PilhaMatriz.Add(matrix);

        matrixfinal = (Matrix4x4)PilhaMatriz[0];



        if (PilhaMatriz.Count > 1)
        {
            foreach (Matrix4x4 pilha in PilhaMatriz)
            {
                matrixfinal *= pilha;
            }
        }

        scale = matrixfinal.ExtractScale();
        position = matrix.ExtractPosition();
        //rotate = matrixfinal.ExtractRotation();


        //diferença entre a transformação atual do objeto e a transformação da matriz final dividida pelo framerate
        //escala
        scaleDividido = (scale - transform.localScale) / 30;
        //rotação (conversão de quartenion para eulerangle (vector3))
        //rotateDividido = (rotate.eulerAngles - transform.localEulerAngles) / 30;
        angulodividido = angulo / 30;

        //posição
        positionDividido = position / 30;



        //Valores.text = rotate.x.ToString();
        /*textboxValores.text = "Position " + position.ToString();
        textboxValores.text = "Rotate " + rotate.ToString();*/


        resetmatriz();
        startanimation = true;
    }

    public void enviar()
    {

        if(!modoadivinha)
        {
            deactivateButtons();
            Pilha.text = "espere pela resposta";
            if (Jog == "jog1")
            {
                sala.matrixjog1[sala.Ronda] = matrixfinal;
                sala.rotacaojog1[sala.Ronda] = transform.eulerAngles;
            }
            else
            {
                sala.matrixjog2[sala.Ronda] = matrixfinal;
                sala.rotacaojog2[sala.Ronda] = transform.eulerAngles;
            }
            string json = JsonUtility.ToJson(sala);
            DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
            reference.Child("sala2").SetRawJsonValueAsync(json);

            if (sala.proporjog2[sala.Ronda])
            {
                sala.Ronda++;
            }
            esperar = true;

        }
        else
        {
            if (Jog == "jog1")
            {
                if (matrixfinal == sala.matrixjog2[sala.Ronda])
                {
                    //acertou
                    Pilha.text = "acertaste agora faz a proposta";
                    modoadivinha = false;
                    sala.proporjog1[sala.Ronda] = true;
                    sala.proporjog2[sala.Ronda] = false;
                }
                else
                {
                    //falhou
                    Pilha.text = "modo adivinha - falhaste";
                    matrixfinal = Matrix4x4.identity;
                    PilhaMatriz.Clear();

                }
            }
            if (Jog == "jog2")
            {
                if (matrixfinal == sala.matrixjog1[sala.Ronda])
                {
                    //acertou
                    Pilha.text = "acertaste agora faz a proposta";
                    modoadivinha = false;
                    sala.proporjog2[sala.Ronda] = true;
                    sala.proporjog1[sala.Ronda] = false;

                }
                else
                {
                    //falhou
                    Pilha.text = "modo adivinha - falhaste";
                    matrixfinal = Matrix4x4.identity;
                    PilhaMatriz.Clear();
                }
            }
        }
    }

    public void writeFirebase()
    {
        MatrixFirebase DBfire = new MatrixFirebase(x1, y1, z1, w1, x2, y2, z2, w2, x3, y3, z3, w3, x4, y4, z4, w4);
        string json = JsonUtility.ToJson(DBfire);
        DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
        reference.Child("matrix").SetRawJsonValueAsync(json);
    }
    public void readFirebase()
    {
        DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
        FirebaseDatabase.DefaultInstance
        .RootReference
        .Child("matrix")
        .GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                // Handle the error...
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                readDB = JsonUtility.FromJson<MatrixFirebase>(snapshot.GetRawJsonValue());
                inputx1.text = readDB.x1.ToString();
                inputy1.text = readDB.y1.ToString();
                inputz1.text = readDB.z1.ToString();
                inputw1.text = readDB.w1.ToString();
                inputx2.text = readDB.x2.ToString();
                inputy2.text = readDB.y2.ToString();
                inputz2.text = readDB.z2.ToString();
                inputw2.text = readDB.w2.ToString();

                inputx3.text = readDB.x3.ToString();
                inputy3.text = readDB.y3.ToString();
                inputz3.text = readDB.z3.ToString();
                inputw3.text = readDB.w3.ToString();

                inputx4.text = readDB.x4.ToString();
                inputy4.text = readDB.y4.ToString();
                inputz4.text = readDB.z4.ToString();
                inputw4.text = readDB.w4.ToString();
            }
        });
    }

    public void readFirebaseSala()
    {
        DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
        FirebaseDatabase.DefaultInstance
        .RootReference
        .Child("sala2")
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


}


public static class MatrixExtensions
{
    public static Quaternion ExtractRotation(this Matrix4x4 matrix)
    {
        Vector3 forward;
        forward.x = matrix.m02;
        forward.y = matrix.m12;
        forward.z = matrix.m22;

        Vector3 upwards;
        upwards.x = matrix.m01;
        upwards.y = matrix.m11;
        upwards.z = matrix.m21;

        return Quaternion.LookRotation(forward, upwards);
    }

    public static Vector3 ExtractPosition(this Matrix4x4 matrix)
    {
        Vector3 position;
        position.x = matrix.m03;
        position.y = matrix.m13;
        position.z = matrix.m23;
        return position;
    }

    public static Vector3 ExtractScale(this Matrix4x4 matrix)
    {
        Vector3 scale;
        scale.x = new Vector4(matrix.m00, matrix.m10, matrix.m20, matrix.m30).magnitude;
        scale.y = new Vector4(matrix.m01, matrix.m11, matrix.m21, matrix.m31).magnitude;
        scale.z = new Vector4(matrix.m02, matrix.m12, matrix.m22, matrix.m32).magnitude;
        return scale;
    }
}



public class Sala
{
    public int CodigoSala = 0000;
    public int Ronda;

    public Vector3[] rotacaojog1;
    public Vector3[] rotacaojog2;

    public Matrix4x4[] matrixjog1;
    public bool[] proporjog1;

    public Matrix4x4[] matrixjog2;
    public bool[] proporjog2;


    public Sala()
    {
        rotacaojog2 = new Vector3[10];
        rotacaojog2 = new Vector3[10];

        Ronda = 0;
        CodigoSala = 0;

        matrixjog1 = new Matrix4x4[10];
        proporjog1 = new bool[10];

        matrixjog2 = new Matrix4x4[10];
        proporjog2 = new bool[10];

    }
}
public class jogador
{
    public Matrix4x4[] matrixjog1;
    public bool[] proporjog1;

    public jogador()
    {
        matrixjog1 = new Matrix4x4[10];
        proporjog1 = new bool[10];
    }

}


public class MatrixFirebase
{
    public float x1, y1, z1, w1, x2, y2, z2, w2, x3, y3, z3, w3, x4, y4, z4, w4;
    public MatrixFirebase()
    {
        x1 = 0;
    }
    public MatrixFirebase(float x1, float y1, float z1, float w1, float x2, float y2, float z2, float w2, float x3, float y3, float z3, float w3, float x4, float y4, float z4, float w4)
    {
        this.x1 = x1;
        this.y1 = y1;
        this.z1 = z1;
        this.w1 = w1;
        this.x2 = x2;
        this.y2 = y2;
        this.z2 = z2;
        this.w2 = w2;
        this.x3 = x3;
        this.y3 = y3;
        this.z3 = z3;
        this.w3 = w3;
        this.w3 = w3;
        this.x4 = x4;
        this.y4 = y4;
        this.z4 = z4;
        this.w4 = w4;
    }
}