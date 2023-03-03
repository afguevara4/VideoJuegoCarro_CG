using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using Unity.Barracuda;
using System;

public class agenteML : Agent
{
    [SerializeField]
    private float _fuerzaMovimiento = 200;

    [SerializeField]
    private Transform _target;
    public bool _training = true;
    private Rigidbody _rb;

    public override void Initialize()
    {
        _rb = GetComponent<Rigidbody>();
        //MaxStep forma parte de la clase Agente
        if (!_training) MaxStep = 0;
    }

    public override void OnEpisodeBegin()
    {
        _rb.velocity = Vector3.zero;
        _rb.angularVelocity = Vector3.zero;
        MoverPosicionInicial();
    }

    private void MoverPosicionInicial()
    {
        bool PosicionEncontrada = false;
        int intentos = 100;
        Vector3 posicionPotencial = Vector3.zero;
        while (!PosicionEncontrada || intentos > 0)
        {
            intentos--;
            posicionPotencial = new Vector3(transform.parent.position.x + UnityEngine.Random.Range(-4f, 4f), 0.555f, transform.parent.position.z + UnityEngine.Random.Range(-4f, 4f));
            Collider[] colliders = Physics.OverlapSphere(posicionPotencial, 0.05f);
            if (colliders.Length == 0)
            {
                transform.position = posicionPotencial;
                PosicionEncontrada |= true;
            }
        }
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        //Construir el vector con el vector recibido
        Vector3 movimiento = new Vector3(actions.ContinuousActions[0], 0f, actions.ContinuousActions[1]);
        //Sumamos el vector construido al rigidbody como fuerza
        _rb.AddForce(movimiento * _fuerzaMovimiento * Time.deltaTime);
    }

    public override void CollectObservations(VectorSensor sensor) {
        //Calcular canto nos queda hasta el objetivo
        Vector3 alObjetivo = _target.position - transform.position;

        //Un vector ocupa 3 observaciones
        sensor.AddObservation(alObjetivo.normalized);
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0f, moveVertical);
        ActionSegment<float> continuousActionsOut = actionsOut.ContinuousActions;
        continuousActionsOut[0] = movement.x;
        continuousActionsOut[1] = movement.z;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("target"))
        {
            if (_training)
            {
                AddReward(1f);
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("target"))
        {
            if (_training)
            {
                AddReward(0.5f);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("border"))
        {
            if (_training)
            {
                AddReward(-0.1f);
            }
        }
    }

}
