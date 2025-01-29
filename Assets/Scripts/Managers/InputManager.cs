using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public float GetHorizontalAxis()
    {
        return Input.GetAxis("Horizontal");
    }

    public float GetVerticalAxis()
    {
        return Input.GetAxis("Vertical");
    }

    public bool GetJumpButtonDown()
    {
        return Input.GetButtonDown("Jump");
    }
}