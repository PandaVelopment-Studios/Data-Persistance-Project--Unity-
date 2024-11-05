using UnityEngine;

public class NameManager : MonoBehaviour
{
    
    public static NameManager Instance;

    public string Name;

    private void Awake()
    {
       if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        else{
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void NameEntered(string name)
    {
        NameManager.Instance.Name = name;
    }


}
