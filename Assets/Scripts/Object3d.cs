using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Firebase.Database;
using Firebase.Extensions;
using UnityEngine.EventSystems;

public class Object3d : MonoBehaviour
{
    private float x1, y1, z1, x2, y2, z2, x3, y3, z3, x4, y4, z4, w1, w2, w3, w4;
    public float countdown, countdown2;
    private string eixo;
    private bool startanimation = false, esperar;
    private string Jog, Utilizador;
    Sala sala;



    public InputField inputx1, inputy1, inputz1, inputx2, inputy2, inputz2, inputx3, inputy3, inputz3;
    public InputField inputx4, inputy4, inputz4, inputw1, inputw2, inputw3, inputw4;

    public InputField Valores;
    public TextMeshProUGUI textboxValores;
    public TextMeshProUGUI Pilha;
    public Button ButtonScale, ButtonTranslate, ButtonRotateX, ButtonRotateY, ButtonRotateZ, ButtonEnviar, ButtonAplicarTransform;
    public GameObject CuboProposto, world2;



    private int indexbotao1, indexbotao2;
    private int[] posicaoBotaoPlay;
    private float angulo, angulodividido;
    private Vector3 axis;
    private ArrayList PilhaMatriz;
    private Matrix4x4 matrixfinal;
    private Vector4 column0, column1, column2, column3;
    private MatrixFirebase readDB;
    private Vector3 scale, position, scaleDividido, positionDividido;
    private Vector3[] scale2, position2, axis2;
    public float[] angulo2;
    public string nomebotao;
    private string nomepilha;

    //prefab dos botoes
    public GameObject ButtonPilha, PlayButton;
    //lista de prefab dos botoes
    private GameObject[] buttonT, buttonP, WORLDprefab;



    private bool modoadivinha;

    public TextMeshProUGUI Tempo;

    public float timeValue = 301;
    public int pontuacao, contador;
    public TextMeshProUGUI pontuacaoText;
    private bool ActivateAnimacao;



    // Start is called before the first frame update
    void Start()
    {
        indexbotao1 = -1;
        indexbotao2 = -1;
        nomebotao = "??";
        scale2 = new Vector3[10];
        position2 = new Vector3[10];
        angulo2 = new float[10];
        axis2 = new Vector3[10];

        buttonT = new GameObject[10];
        buttonP = new GameObject[10];
        WORLDprefab = new GameObject[10];

        posicaoBotaoPlay = new int[10];
        Utilizador = AuthManager.Utilizador;
        contador = 0;

        esperar = false;

        //sala = new Sala();

        
        Jog = CriarSala.Jog;

        Debug.Log(string.IsNullOrEmpty(Jog));
        Debug.Log(Jog);

        if (string.IsNullOrEmpty(Jog))
        {
            Jog = JuntarSala.Jog;
        }

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
            sala = JuntarSala.sala;

            esperar = true;
            Pilha.text = "espere pelo jogador1";
            deactivateButtons();
        }
        else
        {
            sala = CriarSala.sala;

            esperar = false;
            Pilha.text = "Envie a proposta ao jogador2";
        }
    }

    private void resetobjeto()
    {

        transform.parent = null;

        transform.localScale = Vector3.one;
        transform.localRotation = Quaternion.identity;
        transform.localPosition = Vector3.zero;

        System.Array.Clear(WORLDprefab, 0, WORLDprefab.Length);

        foreach(GameObject prefab in GameObject.FindGameObjectsWithTag("worldprefabtag")) Destroy(prefab);
        //acerto para final de ronda
        //contador = 0;

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
        Valores.text = "0";
    }

    // Update is called once per frame
    void Update()
    {

        if (pontuacao < 0)
        {
            Color newColor = new Color(1f, 0f, 0f, 1);

            pontuacaoText.color = newColor;
        }

        if (timeValue > 0)
        {
            timeValue -= Time.deltaTime;
        }
        else
        {
            timeValue = 0;
        }

        DisplayTime(timeValue);


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
                    aplicarTransformCuboProposto(sala.matrixjog1[sala.Ronda], sala.rotacaojog1[sala.Ronda], sala.escalajog1[sala.Ronda], sala.posjog1[sala.Ronda]);
                    esperar = false;
                }

                if (Jog == "jog1" && sala.matrixjog2[sala.Ronda] != Matrix4x4.zero && sala.proporjog2[sala.Ronda])
                {
                    aplicarTransformCuboProposto(sala.matrixjog2[sala.Ronda], sala.rotacaojog2[sala.Ronda], sala.escalajog1[sala.Ronda], sala.posjog1[sala.Ronda]);
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

                    if (angulo == 0)
                    {
                        WORLDprefab[contador - 1].transform.localScale += scaleDividido;
                    }
                    WORLDprefab[contador - 1].transform.RotateAround(Vector3.zero, axis, angulodividido);
                    WORLDprefab[contador - 1].transform.Translate(positionDividido, Space.World);




                }
                else
                {
                    //acerto final da transformação
                    if (angulo == 0)
                    {
                        WORLDprefab[contador - 1].transform.localScale = scale;
                    }

                    WORLDprefab[contador - 1].transform.localPosition = Vector3.zero;
                    WORLDprefab[contador - 1].transform.localRotation = Quaternion.identity;

                    WORLDprefab[contador - 1].transform.RotateAround(Vector3.zero, axis, angulo);
                    WORLDprefab[contador - 1].transform.Translate(position, Space.World);

                    angulo = 0;
                    startanimation = false;
                    countdown = 1.0f;
                }
            }

        }

    }

    void DisplayTime(float timeToDisplay)
    {
        if (timeToDisplay < 0)
        {
            timeToDisplay = 0;
        }

        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        if (minutes == 0 && seconds <= 10)
        {
            Color newColor = new Color(0.69f, 0f, 0f, 1f);
            Tempo.color = newColor;
        }

        Tempo.text = string.Format("{0:00}:{1:00}", minutes, seconds);

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
        resetmatriz();
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
        nomebotao = "Escala";

    }


    public void ButtonTranslateActive()
    {
        deactivateMatriz();
        resetmatriz();
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

        nomebotao = "Translação";
    }
    public void ButtonRotateXActive()
    {
        deactivateMatriz();
        resetmatriz();
        Valores.interactable = true;
        inputy2.interactable = true;
        inputy3.interactable = true;
        inputz2.interactable = true;
        inputz3.interactable = true;
        textboxValores.text = "Rotação em X: ";
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

        nomebotao = "Rotação X";
    }
    public void ButtonRotateYActive()       
    {

        deactivateMatriz();
        resetmatriz();
        Valores.interactable = true;
        inputx1.interactable = true;
        inputx3.interactable = true;
        inputz1.interactable = true;
        inputz3.interactable = true;
        textboxValores.text = "Rotação em Y: ";
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

        nomebotao = "Rotação Y";
    }
    public void ButtonRotateZActive()
    {

        deactivateMatriz();
        resetmatriz();
        Valores.interactable = true;
        inputx1.interactable = true;
        inputx2.interactable = true;
        inputy1.interactable = true;
        inputy2.interactable = true;
        textboxValores.text = "Rotação em Z: ";
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

        nomebotao = "Rotação Z";
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

    public void aplicarTransformCuboProposto(Matrix4x4 matriz, Quaternion rotacaojog, Vector3 escalajog, Vector3 posjog)
    {
        //position2 = matriz.ExtractPosition();
        //scale2 = matriz.ExtractScale();
        //rotate2 = matriz.ExtractRotation();

        CuboProposto.transform.localPosition = posjog;
        CuboProposto.transform.localScale = escalajog;
        CuboProposto.transform.localRotation = rotacaojog;

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
        //modifica a posição do objeto para o ultimo play
        if (contador > 0)
        {
           // PlayPilha(contador - 1);
        }

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

        matrixfinal = Matrix4x4.identity;

        foreach (Matrix4x4 pilha in PilhaMatriz)
            {
                matrixfinal *= pilha;
            }

        //CRIAR WORLD EMPTY OBJECT
        WORLDprefab[contador] = Instantiate(world2);

        if (contador == 0)
        {
            transform.parent = WORLDprefab[contador].transform;
        }
        else
        {
            WORLDprefab[contador - 1].transform.parent = WORLDprefab[contador].transform;
        }



        //scale = new Vector3 (matrix.ExtractScale().x * world.transform.localScale.x, matrix.ExtractScale().y * world.transform.localScale.y, matrix.ExtractScale().z * world.transform.localScale.z);
        scale = matrix.ExtractScale();
        position = matrix.ExtractPosition();
        //rotate = matrixfinal.ExtractRotation();


        //diferença entre a transformação atual do objeto e a transformação da matriz final dividida pelo framerate
        //escala
        scaleDividido = (scale - Vector3.one) / 30;

        //rotação (conversão de quartenion para eulerangle (vector3))
        angulodividido = angulo / 30;

        //posição
        positionDividido = position / 30;

        //guardar as transformaçoes para trocar a ordem
        scale2[contador] = matrix.ExtractScale();
        position2[contador] = position;
        angulo2[contador] = angulo;
        axis2[contador] = axis;

        //CRIAR BOTÃO da pilha de transformação
        buttonT[contador] = Instantiate(ButtonPilha, GameObject.FindGameObjectWithTag("Canvas").transform);
        buttonT[contador].transform.localPosition = new Vector3(44 + (contador * 140), 188, 0);
        buttonT[contador].GetComponentInChildren<TMPro.TextMeshProUGUI>().text = nomebotao;

        //CRIAR evento de click no botão
        EventTrigger trigger = buttonT[contador].GetComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerClick;
        int x = contador;
        entry.callback.AddListener((eventData) => { clicarpilha(x); });
        trigger.triggers.Add(entry);


        //CRIAR botão para correr a animação da transformação
        buttonP[contador] = Instantiate(PlayButton, GameObject.FindGameObjectWithTag("Canvas").transform);
        buttonP[contador].transform.localPosition = new Vector3(44 + (contador * 140), 140, 0);
        buttonP[contador].GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "Play " + contador;

        //CRIAR evento de click no botão play
        EventTrigger trigger2 = buttonP[contador].GetComponent<EventTrigger>();
        EventTrigger.Entry entry2 = new EventTrigger.Entry();
        entry2.eventID = EventTriggerType.PointerClick;
        entry2.callback.AddListener((eventData) => { PlayPilha(x); });
        posicaoBotaoPlay[contador] = x;
        trigger2.triggers.Add(entry2);






        resetmatriz();

        startanimation = true;

        pontuacaoText.text = "Pontuação: " + pontuacao-- + " pontos";

        if (pontuacao < 0)
        {
            ButtonAplicarTransform.interactable = false;
        }

        contador++;
    }

    public void PlayPilha(int idx_)
    {
        Debug.Log(idx_);

        //ActivateAnimacao = true;
        //aplicar as transformações na nova ordem

        resetobjeto();


        for (int i = 0; i <= idx_; i++)
        {

            //EMPTY WORLD
            WORLDprefab[i] = Instantiate(world2);
            if (i == 0)
            {
                transform.parent = WORLDprefab[i].transform;
            }
            else
            {
                WORLDprefab[i - 1].transform.parent = WORLDprefab[i].transform;
            }

            WORLDprefab[i].transform.RotateAround(Vector3.zero, axis2[i], angulo2[i]);
            WORLDprefab[i].transform.Translate(position2[i], Space.World);

            if (angulo2[i] == 0)
            {
                WORLDprefab[i].transform.localScale = scale2[i];
            }
        }
    }


     void clicarpilha(int idx_)
    {
        Debug.Log(posicaoBotaoPlay[idx_]);
        if (indexbotao1 == -1)
        {
            indexbotao1 = idx_;

        }   
        else if(indexbotao2 == -1)
        {
            indexbotao2 = idx_;

            string STRINGtemp = buttonT[indexbotao1].GetComponentInChildren<TMPro.TextMeshProUGUI>().text;

            Vector3 POStempPlay = buttonP[indexbotao1].transform.position;

            Vector3 SCALEtemp = scale2[indexbotao1];
            Vector3 POSITIONtemp = position2[indexbotao1];
            float ANGtemp = angulo2[indexbotao1];
            Vector3 AXIStemp = axis2[indexbotao1];
            Matrix4x4 MATRIXtemp = (Matrix4x4)PilhaMatriz[indexbotao1];

            //Trocar posicao dos  botoes
            buttonT[indexbotao1].GetComponentInChildren<TMPro.TextMeshProUGUI>().text = buttonT[indexbotao2].GetComponentInChildren<TMPro.TextMeshProUGUI>().text;
            buttonT[indexbotao2].GetComponentInChildren<TMPro.TextMeshProUGUI>().text = STRINGtemp;
            //Trocar posicao dos  botoes Play
            //buttonP[indexbotao1].transform.position = buttonP[indexbotao2].transform.position;
            //buttonP[indexbotao2].transform.position = POStempPlay;

            //trocar ordem das transformações no array
            scale2[indexbotao1] = scale2[indexbotao2];
            position2[indexbotao1] = position2[indexbotao2];
            angulo2[indexbotao1] = angulo2[indexbotao2];
            axis2[indexbotao1] = axis2[indexbotao2];

            scale2[indexbotao2] = SCALEtemp;
            position2[indexbotao2] = POSITIONtemp;
            angulo2[indexbotao2] = ANGtemp;
            axis2[indexbotao2] = AXIStemp;

            PilhaMatriz[indexbotao1] = PilhaMatriz[indexbotao2];
            PilhaMatriz[indexbotao2] = MATRIXtemp;

            //troca o index da animaçao associada ao botao
            int POSPLAYtemp = posicaoBotaoPlay[indexbotao1];
            posicaoBotaoPlay[indexbotao1] = posicaoBotaoPlay[indexbotao2];
            posicaoBotaoPlay[indexbotao2] = POSPLAYtemp;


            //aplicar as transformações na nova ordem
            resetobjeto();
            matrixfinal = Matrix4x4.identity;
            Vector3 escalareal = new Vector3(1, 1, 1);

            for (int i = 0; i < contador; i++)
            {
                //EMPTY WORLD
                WORLDprefab[i] = Instantiate(world2);
                if (i == 0)
                {
                    transform.parent = WORLDprefab[i].transform;
                }
                else
                {
                    WORLDprefab[i - 1].transform.parent = WORLDprefab[i].transform;
                }

                matrixfinal *= (Matrix4x4)PilhaMatriz[i];

                WORLDprefab[i].transform.RotateAround(Vector3.zero, axis2[i], angulo2[i]);
                WORLDprefab[i].transform.Translate(position2[i], Space.World);

                if(angulo2[i] == 0)
                {
                escalareal = new Vector3(scale2[i].x * escalareal.x, scale2[i].y * escalareal.y , scale2[i].z * escalareal.z);
                    WORLDprefab[i].transform.localScale = escalareal;

                }
            }

            //Reset dos indices dos botoes a serem trocados
            indexbotao1 = -1;
            indexbotao2 = -1;

            
        }
          

    }



    public void enviar()
    {

        if (!modoadivinha)
        {
            deactivateButtons();
            Pilha.text = "espere pela resposta";
            if (Jog == "jog1")
            {
                sala.matrixjog1[sala.Ronda] = matrixfinal;
                sala.rotacaojog1[sala.Ronda] = WORLDprefab[contador - 1].transform.rotation;
                sala.escalajog1[sala.Ronda] = WORLDprefab[contador - 1].transform.lossyScale;
                sala.posjog1[sala.Ronda] = WORLDprefab[contador - 1].transform.position;

            }
            else
            {
                sala.matrixjog2[sala.Ronda] = matrixfinal;
                sala.rotacaojog2[sala.Ronda] = WORLDprefab[contador - 1].transform.rotation;
                sala.escalajog2[sala.Ronda] = WORLDprefab[contador - 1].transform.lossyScale;
                sala.posjog2[sala.Ronda] = WORLDprefab[contador - 1].transform.position;
            }


            if (sala.proporjog2[sala.Ronda])
            {
                sala.Ronda++;
            }
            string json = JsonUtility.ToJson(sala);
            DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
            reference.Child("sala2").SetRawJsonValueAsync(json);

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
                    PilhaMatriz.Clear();
                    resetobjeto();
                }
                else
                {
                    //falhou
                    Pilha.text = "modo adivinha - falhaste";
                    PilhaMatriz.Clear();
                    resetobjeto();


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
                    PilhaMatriz.Clear();
                    resetobjeto();


                }
                else
                {
                    //falhou
                    Pilha.text = "modo adivinha - falhaste";
                    PilhaMatriz.Clear();
                    resetobjeto();

                }
            }
        }
        resetmatriz();
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

public class jogador
{
    public string nomejog;
    public int[] codigoSala;

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