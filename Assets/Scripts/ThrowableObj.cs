using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "throwable", menuName = "Throwable/New Throwable", order = 0)]
public class ThrowableObj : ScriptableObject
{
    public float soundArea;
    public AudioClip audio;
    public bool attracts;
}