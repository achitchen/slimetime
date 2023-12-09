using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressScript : MonoBehaviour
{
    public bool isKeyCollected1;
    public bool isKeyCollected2;
    public bool isKeyCollected3;
    public bool isKeyCollected4;
    public bool isKeyCollected5;
    public bool isKeyCollected6;
    public bool isKeyCollected7;
    public bool isKeyCollected8;
    public bool isLockOpened1;
    public bool isLockOpened2;
    public bool isLockOpened3;
    public bool isLockOpened4;
    public bool isLockOpened5;
    public bool isLockOpened6;
    public bool isLockOpened7;
    public bool isLockOpened8;
    public bool isSwitchPressed1;
    public bool isSwitchPressed2;
    public bool isSwitchPressed3;
    public bool isSwitchPressed4;
    public bool isSwitchPressed5;
    public bool isSwitchPressed6;
    public bool isSwitchPressed7;
    public bool isSwitchPressed8;
    public bool isSwitchPressed9;
    public bool isSwitchPressed10;
    public bool isSwitchPressed11;
    public bool isSwitchPressed12;
    public bool isSwitchPressed13;
    public List<bool> areKeysCollected;
    public List<bool> areLocksOpened;
    public List<bool> areSwitchesPressed;
    public List<GameObject> keys;
    public List<GameObject> keyLocks;
    public List<GameObject> switches;
    public int keysPossessed = 0;

    private void Awake()
    {
        areKeysCollected = new List<bool>();
        if (areKeysCollected.Count == 0)
        {
            areKeysCollected.Add(isKeyCollected1);
            areKeysCollected.Add(isKeyCollected2);
            areKeysCollected.Add(isKeyCollected3);
            areKeysCollected.Add(isKeyCollected4);
            areKeysCollected.Add(isKeyCollected5);
            areKeysCollected.Add(isKeyCollected6);
            areKeysCollected.Add(isKeyCollected7);
            areKeysCollected.Add(isKeyCollected8);
        }
        areLocksOpened = new List<bool>();
        if (areLocksOpened.Count == 0)
        {
            areLocksOpened.Add(isLockOpened1);
            areLocksOpened.Add(isLockOpened2);
            areLocksOpened.Add(isLockOpened3);
            areLocksOpened.Add(isLockOpened4);
            areLocksOpened.Add(isLockOpened5);
            areLocksOpened.Add(isLockOpened6);
            areLocksOpened.Add(isLockOpened7);
            areLocksOpened.Add(isLockOpened8);
        }
        areSwitchesPressed = new List<bool>();
        if (areSwitchesPressed.Count == 0)
        {
            areSwitchesPressed.Add(isSwitchPressed1);
            areSwitchesPressed.Add(isSwitchPressed2);
            areSwitchesPressed.Add(isSwitchPressed3);
            areSwitchesPressed.Add(isSwitchPressed4);
            areSwitchesPressed.Add(isSwitchPressed5);
            areSwitchesPressed.Add(isSwitchPressed6);
            areSwitchesPressed.Add(isSwitchPressed7);
            areSwitchesPressed.Add(isSwitchPressed8);
            areSwitchesPressed.Add(isSwitchPressed9);
            areSwitchesPressed.Add(isSwitchPressed10);
            areSwitchesPressed.Add(isSwitchPressed11);
            areSwitchesPressed.Add(isSwitchPressed12);
            areSwitchesPressed.Add(isSwitchPressed13);
        }
    }

    public void UpdateBools()
    {
        isKeyCollected1 = areKeysCollected[0];
        isKeyCollected2 = areKeysCollected[1];
        isKeyCollected3 = areKeysCollected[2];
        isKeyCollected4 = areKeysCollected[3];
        isKeyCollected5 = areKeysCollected[4];
        isKeyCollected6 = areKeysCollected[5];
        isKeyCollected7 = areKeysCollected[6];
        isKeyCollected8 = areKeysCollected[7];

        isLockOpened1 = areLocksOpened[0];
        isLockOpened2 = areLocksOpened[1];
        isLockOpened3 = areLocksOpened[2];
        isLockOpened4 = areLocksOpened[3];
        isLockOpened5 = areLocksOpened[4];
        isLockOpened6 = areLocksOpened[5];
        isLockOpened7 = areLocksOpened[6];
        isLockOpened8 = areLocksOpened[7];

        isSwitchPressed1 = areSwitchesPressed[0];
        isSwitchPressed2 = areSwitchesPressed[1];
        isSwitchPressed3 = areSwitchesPressed[2];
        isSwitchPressed4 = areSwitchesPressed[3];
        isSwitchPressed5 = areSwitchesPressed[4];
        isSwitchPressed6 = areSwitchesPressed[5];
        isSwitchPressed7 = areSwitchesPressed[6];
        isSwitchPressed8 = areSwitchesPressed[7];
        isSwitchPressed9 = areSwitchesPressed[8];
        isSwitchPressed10 = areSwitchesPressed[9];
        isSwitchPressed11 = areSwitchesPressed[10];
        isSwitchPressed12 = areSwitchesPressed[11];
        isSwitchPressed13 = areSwitchesPressed[12];
    }
}
