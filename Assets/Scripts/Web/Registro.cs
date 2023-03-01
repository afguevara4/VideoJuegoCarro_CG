using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Registro : MonoBehaviour
{
    public Servidor servidor;

    public InputField InUsuario;
    public InputField InPass;
    public InputField InJugador;
    public DBUsuario usuario;

    public void RegistroSesion()
    {
        StartCoroutine(Registrar());
    }
    IEnumerator Registrar()
    {
        string[] datos = new string[3];
        datos[0] = InUsuario.text;
        datos[1] = InPass.text;
        datos[2] = InJugador.text;
        StartCoroutine(servidor.ConsumirServicio("Registro", datos, PosCargar));
        yield return new WaitForSeconds(0.5f);
        yield return new WaitUntil(() => !servidor.ocupado);

    }

    void PosCargar()
    {
        switch (servidor.respuesta.codigo)
        {
            case 401: //Error intentando crear el usuario
                print(servidor.respuesta.mensaje);
                break;
            case 201: //Inicio de Sesi�n Correcto
                SceneManager.LoadScene("Login");
                usuario = JsonUtility.FromJson<DBUsuario>(servidor.respuesta.respuesta);
                break;
            case 402: //Faltan datos para ejecutar la accion solicitada 
                print(servidor.respuesta.mensaje);
                break;
            case 400: //Error 
                print("Erro, no se puede conectar con el servidor");
                break;
            default:
                break;
        }
    }
}