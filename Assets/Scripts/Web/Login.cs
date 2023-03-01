using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Login : MonoBehaviour
{
    public Servidor servidor;

    public InputField InUsuario;
    public InputField InPass;
    public GameObject ImLoading;
    public DBUsuario usuario;


    public void IniciarSesion()
    {
        StartCoroutine(Iniciar());
    }
    IEnumerator Iniciar()
    {
        //Puntaje p = new Puntaje();
        ImLoading.SetActive(true);
        string[] datos = new string[2];
        datos[0] = InUsuario.text;
        datos[1] = InPass.text;
        //p.nombre(InUsuario.text);
        StartCoroutine(servidor.ConsumirServicio("Login", datos, PosCargar));
        yield return new WaitForSeconds(0.5f);
        yield return new WaitUntil(() => !servidor.ocupado);
        ImLoading.SetActive(false);

    }

    void PosCargar()
    {
        switch (servidor.respuesta.codigo)
        {
            case 204: //El usuario o la contra son incorrecto
                print(servidor.respuesta.mensaje);
                break;
            case 205: //Inicio sesion correcto
                SceneManager.LoadScene("Game");
                usuario = JsonUtility.FromJson<DBUsuario>(servidor.respuesta.respuesta);
                servidor.usuario = usuario.usuario;
                break;
            case 402: //Faltan datos para ejecutar la accion solicitada
                print(servidor.respuesta.mensaje);
                break;
            case 404: //Error
                print("Error, no se puede conectar con el servidor");
                break;
            default:
                break;
        }
    }

}
