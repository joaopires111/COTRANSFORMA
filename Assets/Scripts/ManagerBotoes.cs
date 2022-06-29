using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Firebase;
using Firebase.Auth;
using TMPro;

public class ManagerBotoes : MonoBehaviour
{
    public static string Jog;


    public void Btn_Sair()
    {
        Application.Quit();
    }

    public void Btn_Jogar(string jogar)
    {
        SceneManager.LoadScene(jogar);
    }

    public void Btn_Instrucoes(string instrucoes)
    {
        SceneManager.LoadScene(instrucoes);
    }

    public void Btn_Creditos(string creditos)
    {
        SceneManager.LoadScene(creditos);
    }

    public void Btn_IniciarSessao(string iniciar)
    {
        SceneManager.LoadScene(iniciar);
    }

    public void Btn_Registar(string registo)
    {
        SceneManager.LoadScene(registo);
    }

    public void Btn_TerminarSessao(string terminar)
    {
        SceneManager.LoadScene(terminar);
    }

    public void VoltarMenu(string inicio)
    {
        SceneManager.LoadScene(inicio);
    }

    public void jog1(string jog1)
    {
        Jog = "jog1";

        SceneManager.LoadScene(jog1);
    }
    public void jog2(string jog2)
    {
        Jog = "jog2";
        
        PlayerPrefs.SetString("jogador", "Jog2");

        SceneManager.LoadScene(jog2);
    }
}
