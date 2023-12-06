using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class GameWinScreen : MonoBehaviour
{
    [SerializeField] private GameObject menuGameWin;
    
    private Balon balon;

    private void Start() {
        balon = GameObject.FindGameObjectWithTag("Ball").GetComponent<Balon>();
        balon.GameWin += ActivarMenu;
    }

    private void ActivarMenu(object sender, EventArgs e) {
        menuGameWin.SetActive(true);
    }

    public void Reniciar() {
        SceneManager.LoadScene(0);
    }
}
