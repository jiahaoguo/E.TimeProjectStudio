using UnityEngine;
using UnityEngine.Events;

public static class GameEvents
{
    // Event that sends the variable name and its new value
    public static UnityEvent<string, int> OnVariableUpdated = new UnityEvent<string, int>();
}
