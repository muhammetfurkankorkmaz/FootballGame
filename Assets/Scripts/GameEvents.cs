using System;
using UnityEngine;

public class GameEvents : MonoBehaviour
{
    public static GameEvents Instance { get; private set; }

    // In this awake method, we check if there's an instance of this object.
    private void Awake()
    {
        // If there isn't, that means its the first one so we save a reference to it in "Instance"
        if (Instance == null)
        {
            Instance = this;
            // We use this method to make sure the object doesn't get destroyed even if we switch scenes
            DontDestroyOnLoad(gameObject);
        }
        // If there is, that means it's a duplicate so we delete it
        else
        {
            Destroy(gameObject);
        }
    }


    // This is the declaration of the event, any method that subscribes to it needs an "int" as a parameter
    public event Action<int> OnButtonPressed;


    // This is a function that we wrote so we can use it to trigger the event
    public void ButtonPressed(int triggerID)
    {
        OnButtonPressed?.Invoke(triggerID);
    }

}
