using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SetCreditsText : MonoBehaviour
{
    [SerializeField]
    private TextAsset creditsTextFile;
    [SerializeField]
    private TextAsset contactTextFile;
    private Text text;
    [SerializeField]
    private Text contactText;

    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<Text>();
        text.text = creditsTextFile.text;
        contactText.text = contactTextFile.text;
    }

    public void OpenContact()
    {
        Application.OpenURL("https://www.instagram.com/ziadalgendi");
    }
}
