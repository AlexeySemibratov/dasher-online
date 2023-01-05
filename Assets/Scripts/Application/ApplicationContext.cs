using UnityEngine;

public class ApplicationContext : MonoBehaviour
{
    public static ApplicationContext Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            InitContext();
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitContext()
    {
        Debug.Log("Initialize app context");
    }
}
