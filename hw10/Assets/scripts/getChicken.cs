using UnityEngine;
using Vuforia;
using UnityEngine.SceneManagement;

public class getChicken : MonoBehaviour, IVirtualButtonEventHandler
{
    Material m;
    GameObject chicken;
    GameObject button;
    public VirtualButtonBehaviour vb;
    void Start()
    {
        vb = GetComponentInChildren<VirtualButtonBehaviour>();
        vb.RegisterEventHandler(this);
        m = transform.GetChild(2).GetComponent<MeshRenderer>().material;
        chicken = transform.GetChild(0).gameObject;
        button = transform.GetChild(2).gameObject;
        m.color = Color.red;
        Debug.Log("chicken!!!");
    }

    public void OnButtonPressed(VirtualButtonBehaviour vb)
    {
        m.color = Color.green;
    }

    public void OnButtonReleased(VirtualButtonBehaviour vb)
    {
        Destroy(chicken);
        Destroy(button);
        next();
    }

    private void OnGUI()
    {
        GUIStyle gameStyle = new GUIStyle();
        gameStyle.fontSize = 30;
        gameStyle.normal.textColor = new Color(255, 255, 255);
        GUI.Button(new Rect(0, 0, 500, 50), "FIND \"Oswold Hunter\"", gameStyle);
    }

    void next()
    {
        SceneManager.LoadScene("scene4");
    }
}