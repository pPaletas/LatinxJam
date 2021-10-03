using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KarmaManager : MonoBehaviour
{
    #region Singleton
    public KarmaManager Instance{get; private set;}

    private void Awake() {
        Instance = this;
    }
    #endregion

    int karmaLevel = 0;

    public int GetKarma() => karmaLevel;
    public void AddKarma(int karmaToAdd) => karmaLevel += karmaToAdd;
    public void DecreaseKarma(int karmaToDec) => karmaLevel -= karmaToDec;
}