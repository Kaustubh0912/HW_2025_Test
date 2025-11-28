using UnityEngine;

public class GameInput : MonoBehaviour
{
    public static GameInput Instance { get; private set; }
    
    private InputActions inputActions;

    private void Awake()
    {
        Instance = this;
        inputActions = new InputActions();
        inputActions.Enable();
    }
    private void OnDestroy()
    {
        inputActions.Disable();
    }

    public bool IsUpActionPressed()
    {
        return inputActions.Doofus.Up.IsPressed();
    }
    public bool IsDownActionPressed()
    {
        return inputActions.Doofus.Down.IsPressed();
    }
    public bool IsLeftActionPressed()
    {
        return inputActions.Doofus.Left.IsPressed();
    }
    public bool IsRightActionPressed()
    {
        return inputActions.Doofus.Right.IsPressed();
    }
}
