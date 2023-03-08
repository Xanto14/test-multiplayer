using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
public class PlayerInput : MonoBehaviour
{
    [SerializeField] private int nPlayers = 1;
    [SerializeField] private PlayerMovement movement ;
    // MovementInputs -> les touches du clavier "physique"
    // "logique" -> abstrait
    // "physique" -> touche précise du clavier
    [SerializeField] private KeyCode[] movementInputs;
    [SerializeField] private UnityEvent<int>[] movementActions;
    private void Awake()
    {
        Debug.Assert(movement != null);
        Debug.Assert(movementInputs.Length == PlayerMovement.nDirections * nPlayers);
    }

    private void Update()
    {
        /* ---- CETTE MÉTHODE EST MOINS BONNE MAIS DÉMONTRE BIEN LA LOGIQUE SIMPLEMENT ----*/
        // for (int i = 0; i < PlayerMovement.nDirections; i++)
        // {
        //     if (Input.GetKey(movementInputs[i])) // GetKey est vrai tant que c'est appuyé,
        //                         // GetKeyDown et GetKeyUp c'est juste au moment que l'on clique que c'est vrai
        //     {
        //         movementActions[i].Invoke(i);
        //     }
        // }
        /* ---------------------------------------------------------------------------------*/
        var buffer = PollDigitalInputs(movementInputs);
        ProcessDigitalInputs(buffer,movementActions);
         
    }
    // Cette méthode fait un "sondage" de nos intrants numériques
    // et sauvegarde le résultat du sondage dans une structure de données
    
    // BitArray -> c'est comme un tableau de bool, mais plus compact en mémoire
    // chaque bit représente ne touche du clavier, ici
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
                actions[i].Invoke(i % PlayerMovement.nDirections);
            }
        }
    }
}