public class InputControl
{
    private Controls m_instance;
    public Controls Instance
    {
        get { return m_instance; }
        private set { m_instance = value; }
    }

    public InputControl() => m_instance = new Controls();

    public void SubscribePerformed(IPerformedInputFeatures features)
    {
        m_instance.PlayerControls.Move.performed += features.PerformedMove;
        m_instance.PlayerControls.Jump.performed += features.PerformedJump;
        m_instance.PlayerControls.Button1.performed += features.PerformedButton1;
        m_instance.PlayerControls.Button2.performed += features.PerformedButton2;
        m_instance.PlayerControls.Button3.performed += features.PerformedButton3;
        m_instance.PlayerControls.Button4.performed += features.PerformedButton4;
    }

    public void DesubscribePerformed(IPerformedInputFeatures features)
    {
        m_instance.PlayerControls.Move.performed -= features.PerformedMove;
        m_instance.PlayerControls.Jump.performed -= features.PerformedJump;
        m_instance.PlayerControls.Button1.performed -= features.PerformedButton1;
        m_instance.PlayerControls.Button2.performed -= features.PerformedButton2;
        m_instance.PlayerControls.Button3.performed -= features.PerformedButton3;
        m_instance.PlayerControls.Button4.performed -= features.PerformedButton4;
    }

    public void SubscribeCanceled(ICanceledInputFeatures features)
    {
        m_instance.PlayerControls.Move.canceled += features.CanceledMove;
        m_instance.PlayerControls.Jump.canceled += features.CanceledJump;
        m_instance.PlayerControls.Button1.canceled += features.CanceledButton1;
        m_instance.PlayerControls.Button2.canceled += features.CanceledButton2;
        m_instance.PlayerControls.Button3.canceled += features.CanceledButton3;
        m_instance.PlayerControls.Button4.canceled += features.CanceledButton4;
    }

    public void DesubscribeCanceled(ICanceledInputFeatures features)
    {
        m_instance.PlayerControls.Move.canceled -= features.CanceledMove;
        m_instance.PlayerControls.Jump.canceled -= features.CanceledJump;
        m_instance.PlayerControls.Button1.canceled -= features.CanceledButton1;
        m_instance.PlayerControls.Button2.canceled -= features.CanceledButton2;
        m_instance.PlayerControls.Button3.canceled -= features.CanceledButton3;
        m_instance.PlayerControls.Button4.canceled -= features.CanceledButton4;
    }
}
