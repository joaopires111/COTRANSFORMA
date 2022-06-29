using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NomeUtilizador_MenuJogo : MonoBehaviour
{
    private string Utilizador;
    public TMP_Text NomeUtilizador;

    void Start()
    {

        Utilizador = AuthManager.Utilizador;
        NomeUtilizador.text = Utilizador;

    }
}
