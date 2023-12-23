using UnityEngine;

[CreateAssetMenu(fileName = "HeldInputPredicate", menuName = "ScriptableObjects/Conditional/HeldInput", order = 1)]
public class HeldInputPredicateSO : PredicateSO // I don't like the concrete type reference; TODO abstract?
{
    [SerializeField] private InputType m_inputType;

    protected override bool Eval(IComponentMap map)
    {
        var components = map.GetReference<MonoInputController>(GetComponentType());

        return components.TrueForAll((item) => item.IsInputHeld(m_inputType));
    }

    public override ComponentType GetComponentType() => ComponentType.InputController;
}
