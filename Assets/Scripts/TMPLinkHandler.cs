using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class TMPLinkHandler : MonoBehaviour, IPointerClickHandler
{
    public TMP_Text tmpText;  // Reference to the TMP_Text component with your links

    void Start()
    {
        Debug.Log("TMPLinkHandler started");
        // Make sure the TMP_Text component is assigned
        if (tmpText == null)
        {
            tmpText = GetComponent<TMP_Text>();
        }
    }

    // Implement the IPointerClickHandler interface to handle clicks on the text
    public void OnPointerClick(PointerEventData eventData)
    {
        if (tmpText == null)
        {
            Debug.LogError("TMP_Text is not assigned.");
            return;
        }

        // Convert the screen position to local position in the TMP_Text's space
        Vector3 mousePosition = new Vector3(eventData.position.x, eventData.position.y, 0);
        Debug.Log("Pointer clicked at local position: " + mousePosition);

        // Check if the clicked area is a link
        int linkIndex = TMP_TextUtilities.FindIntersectingLink(tmpText, mousePosition, Camera.main);
        Debug.Log("tmpText: " + tmpText.text);
        if (linkIndex != -1)
        {
            // Get the link's string (URL or identifier)
            string link = tmpText.textInfo.linkInfo[linkIndex].GetLinkID();
            Debug.Log("Link clicked: " + link);

            // Open the link in a browser
            Application.OpenURL(link);
        }
        else
        {
            Debug.Log("Pointer clicked, but no link was clicked.");
        }
    }
}
