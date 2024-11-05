using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuHandler : MonoBehaviour
{

    public Button button;
    public TMP_InputField inputField;
    public TextMeshProUGUI errorText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnValueChanged(string newvalue)
    {
        if (inputField != null)
        {
            errorText.text = "";
        }

    }

    public void StartGame()
    {
        if (inputField.text != "")
        {
            NameManager.Instance.Name = inputField.text;
            SceneManager.LoadScene("main");
        }
        else
        {
            errorText.text = "You must enter a name first";
        }
    }

}
