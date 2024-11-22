using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressResetter : MonoBehaviour
{
    public void ResetProgress()
    {
        PlayerPrefs.DeleteAll();
        LevelManager.LoadLevel("Menu");
    }
}
