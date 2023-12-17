using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A Scriptable Object to store listings of priorities for inputs.
/// </summary>
[CreateAssetMenu(fileName = "PriorityListing", menuName = "ScriptableObjects/PriorityListing", order = 1)]
public class PriorityListingSO : ScriptableObject
{
    [SerializeField] private List<InputType> m_inputPriorities;

    private void Awake()
    {
        // God, I hate editor SOs. Why do the values appear, then vanish?
        Debug.Log("Remember to press the '+' then '-' sign under the empty list to get default values.");
    }

    private void OnValidate()
    {
        if (m_inputPriorities == null || m_inputPriorities.Count == 0)
        {
            m_inputPriorities = new List<InputType>
            {
                InputType.Button4,
                InputType.Button3,
                InputType.Button2,
                InputType.Button1,
                InputType.Jump,
                InputType.Movement
            };
        }

        // setting dirty here and then immediately saving doesn't work. The values still disappear.
    }

    public IReadOnlyList<InputType> GetPriorities() => m_inputPriorities;

    public int GetPriorityOf(InputType type) => m_inputPriorities.IndexOf(type);
}
