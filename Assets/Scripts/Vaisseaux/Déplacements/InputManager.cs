using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
public class InputManager : MonoBehaviour
{
    [SerializeField] private int nPlayers = 1;
    [SerializeField] private MovementController movement ;
    // MovementInputs -> les touches du clavier "physique"
    // "logique" -> abstrait
    // "physique" -> touche précise du clavier
    [SerializeField] private KeyCode[] movementInputs;
    [SerializeField] private UnityEvent<int>[] movementActions;

    private void Update()
    {
      
        var buffer = PollDigitalInputs(movementInputs);
        ProcessDigitalInputs(buffer,movementActions);
         
    }
  
    public static BitArray PollDigitalInputs(KeyCode[] keys)
    {
        BitArray result = new BitArray(keys.Length);
        for (int i = 0; i < keys.Length; i++)
        {
            if (Input.GetKey(keys[i]))
            {
                result[i] = Input.GetKey(keys[i]);
            }
        }

        return result;
    }

    public static void ProcessDigitalInputs(BitArray buffers,UnityEvent<int>[] actions)
    {
        for(int i = 0; i < buffers.Length;i++)
        {
            if (buffers[i])
            {
                actions[i].Invoke(i % MovementController.nDirections);
            }
        }
    }
}