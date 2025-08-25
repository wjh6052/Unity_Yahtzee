using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SystemMessages : MonoBehaviour
{
    public TMP_Text Messages_Text;

    public void SetSystemMessages(string inMessages, float inRemovalTime = 2.0f)
    {
        Messages_Text.text = inMessages;

        Destroy(this.gameObject, inRemovalTime);
    }
}
