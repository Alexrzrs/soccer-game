using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.SceneManagement;

public class Balon : MonoBehaviour
{

   public event EventHandler GameOver;

   public event EventHandler GameWin;

   public TextMeshProUGUI countText;

   public GameObject[] vidas;

   private int contador;
   
  [SerializeField] private int vidasCont;

   private bool colisionOcurridaEsteTurno = false;

   private bool anotacion = false;


    [Header("Target")]
    public Transform target;

    [Header("Shoot")]
    public float Force;
    public float maxForce;
    public float distance;

    [Header("UI")]
    public Slider forceUI;
    public Toggle curveToggle;
    public Toggle powerShot;
    
    
    [Header("Goal Keeper")]
    GoalKeeperScript goal;
    public GameObject GoalKeeperScript;

    [Header("Ball Position")]
    Vector3 StartPos;

    [Header("Goal Keeper Position")]
    Vector3 GoalPos;

    [Header("Curve")]
    public float curveStrength;
    Vector3 lateralDirection;


    


    // Start is called before the first frame update
    void Start()
    {
        contador = 0;
        vidasCont = 3;
        StartPos = transform.position;
        GoalPos = GoalKeeperScript.transform.position;
        forceUI.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        // Rellenar slider dependiendo del Force acumulado 
        if(Input.GetKey(KeyCode.Space))
        {
            forceUI.gameObject.SetActive(true);
            Force++;
            slider();
        }

        // Patear al levantar key
        if(Input.GetKeyUp(KeyCode.Space)) 
        {
            StartCoroutine(KickWaitCoroutine());
        }

        if(Force > maxForce)
            Force = maxForce;
    }

    void FixedUpdate() 
    {
        if(curveToggle != null && curveToggle.isOn)
        {
            powerShot.isOn = false;
            distance = 1f;
            curve();
        }

        if(powerShot != null && powerShot.isOn)
        {
            curveToggle.isOn = false;
            distance = 1f;
        }
    }

    // Patear balon
    void shoot()
    {
        Vector3 Shoot = ((target.position * distance)- this.transform.position).normalized;
        GetComponent<Rigidbody>().angularDrag = 1;
        GetComponent<Rigidbody>().AddForce(Shoot * Force + new Vector3(lateralDirection.x, 3f, lateralDirection.z), ForceMode.Impulse);
    }

    void curve() 
    {
        Vector3 velocity = GetComponent<Rigidbody>().velocity;
        Vector3 lateralVelocity = new Vector3(velocity.x, 0, velocity.z);

        float speed = lateralVelocity.magnitude;
        lateralDirection = lateralVelocity.normalized;

        Vector3 curveForce = Vector3.Cross(lateralDirection * 0.6f, Physics.gravity.normalized) * curveStrength * speed;

        if(target.position.x > 2f)
        {
            GetComponent<Rigidbody>().AddForce(-curveForce, ForceMode.Force);
        }

        if(target.position.x < -2f)
        {
            GetComponent<Rigidbody>().AddForce(curveForce, ForceMode.Force);
        }
    }

    void onCollisionEnter(Collision col)
    {
        if(col.gameObject.CompareTag("Pole"))
        {
            curveToggle.isOn = false;
        }

        if(col.gameObject.CompareTag("Net"))
        {
            curveToggle.isOn = false;
        }

       
    }
   void OnTriggerEnter(Collider otro) {
    if (otro.gameObject.CompareTag("Coleccionable") && !colisionOcurridaEsteTurno) {
        contador++;
        countText.text = "Marcador: " + contador.ToString();
        colisionOcurridaEsteTurno = true;
        anotacion = true;

        if (contador % 3 == 0) {
            float nuevaPosicionZ = 39.0f - ((contador / 3) * 2);
            StartCoroutine(MoverBalon(nuevaPosicionZ));
        }
    } 

    if(!colisionOcurridaEsteTurno){
        vidasCont--;
        vidas[vidasCont].SetActive(false);
    }

}

public void slider() {
    forceUI.value = Force;
} 

public void ResetGauge() {
    Force = 0;
    forceUI.value = 0;
}

IEnumerator MoverBalon(float nuevaPosicionZ) {
    Vector3 direccion = Vector3.back; // Vector que representa el movimiento hacia atrás

    while (transform.position.z > nuevaPosicionZ) {
        Vector3 nuevaPosicion = transform.position + direccion * Time.deltaTime; // Mueve el balón hacia atrás
        transform.position = nuevaPosicion;
        yield return null;
    }

    StartPos = transform.position; // Actualiza la nueva posición inicial
}

IEnumerator KickWaitCoroutine() {
    yield return new WaitForSeconds(1.5f);
    forceUI.gameObject.SetActive(false);
    shoot();
    yield return new WaitForSeconds(0.05f);
    FindObjectOfType<GoalKeeperScript>().GoalMove();
    yield return new WaitForSeconds(1.5f);
    ResetGauge(); // Restablecer slider y fuerza

    GetComponent<Rigidbody>().angularDrag = 40;
    yield return new WaitForSeconds(3f);

    GetComponent<Rigidbody>().velocity = Vector3.zero;
    transform.position = StartPos; // Restablecer posicion del balon
    GoalKeeperScript.transform.position = GoalPos; // Restablecer posicion del portero
    curveStrength = 1.5f;

    FindObjectOfType<GoalKeeperScript>().Reset();
    FindObjectOfType<GoalKeeperScript>().Move = 0; // Restablecer index del portero
    colisionOcurridaEsteTurno = false; // Restablecer el turno para el collider

    if (contador == 3) {
        Vector3 nuevaPosicion = transform.position;
        nuevaPosicion.z = 30.0f;
        transform.position = nuevaPosicion;
        StartPos = transform.position;
    }

     if(anotacion){
        anotacion = false;
    } else {
        vidasCont--;
        vidas[vidasCont].SetActive(false);

        if (vidasCont == 0) {
            GameOver?.Invoke(this, EventArgs.Empty);
        }
    }


    if(contador == 5) {
        GameWin?.Invoke(this, EventArgs.Empty);
    }
}
}
