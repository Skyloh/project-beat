using System.Collections.Generic;
using UnityEngine;

// a little scuffed of an implementation, but i need this to be generic-less. As such, I needed a method that took a component
// map but also was named clearly. Having "Retrieve" perform the logic was not only confusing naming-wise but also would've led
// to duplication of the guard statement logic.
public abstract class PredicateSO : ScriptableObject, IComponentMapRetriever, IConditional
{
    // if we received two predicate requests from the same player back to back, don't grab the component from the 2nd time onwards.
    // This is because we're already loadede w/ the correct components UNTIL a new player wants to use this predicate.
    private int m_previousPlayerHash;

    private bool m_result;

    public void Retrieve(IComponentMap map) 
    {
        if (AlreadyLoaded(map.GetHashCode())) return; // guard statement to prevent redundant reference requests

        m_result = Eval(map);
    }

    private bool AlreadyLoaded(int hash)
    {
        if (hash == m_previousPlayerHash) return true;
        m_previousPlayerHash = hash;

        return false;
    }

    public bool Check() => m_result;

    public abstract ComponentType GetComponentType();

    protected abstract bool Eval(IComponentMap map);
}
