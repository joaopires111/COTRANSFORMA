using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ManagerBotoes : MonoBehaviour
{
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

    public void Btn_Confirmar(string confirmar)
    {
        SceneManager.LoadScene(confirmar);
    }

    public void Btn_TerminarSessao(string terminar)
    {
        SceneManager.LoadScene(terminar);
    }

    public void VoltarMenu(string inicio)
    {
        SceneManager.LoadScene(inicio);
    }
}
