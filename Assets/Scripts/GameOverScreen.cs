using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class GameOverScreen : MonoBehaviour
{
    [SerializeField] private GameObject menuGameOver;
    
    private Balon balon;

    private void Start() {
        balon = GameObject.FindGameObjectWithTag("Ball").GetComponent<Balon>();
        balon.GameOver += ActivarMenu;
    }

    private void ActivarMenu(object sender, EventArgs e) {
        menuGameOver.SetActive(true);
    }

    public void Reniciar() {
        SceneManager.LoadScene(0);
    }
}
