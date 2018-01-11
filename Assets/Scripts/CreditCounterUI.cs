using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class CreditCounterUI : MonoBehaviour
{
    private Text creditAmt;

    void Awake()
    {
        creditAmt = GetComponent<Text>();
    }

    void Update()
    {
        creditAmt.text = "Credits: " + MasterControlProgram.Credits.ToString();
    }
}
