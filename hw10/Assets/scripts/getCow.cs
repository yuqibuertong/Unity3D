using UnityEngine;
using Vuforia;
using UnityEngine.SceneManagement;

public class getCow : MonoBehaviour, IVirtualButtonEventHandler
{
    Material m;
    GameObject cow;
    GameObject button;
    public VirtualButtonBehaviour vb;
    void Start()
    {
        vb = GetComponentInChildren<VirtualButtonBehaviour>();
        vb.RegisterEventHandler(this);
        m = transform.GetChild(2).GetComponent<MeshRenderer>().material;
        cow = transform.GetChild(0).gameObject;
        button = transform.GetChild(2).gameObject;
        m.color = Color.red;
        Debug.Log("cow!!!");
    }

    public void OnButtonPressed(VirtualButtonBehaviour vb)
    {
        m.color = Color.green;
    }

    public void OnButtonReleased(VirtualButtonBehaviour vb)
    {
        Destroy(cow);
        Destroy(button);
        next();
    }

    private void OnGUI()
    {
        GUIStyle gameStyle = new GUIStyle();
        gameStyle.fontSize = 30;
        gameStyle.normal.textColor = new Color(255, 255, 255);
        GUI.Button(new Rect(0, 0, 500, 50), "FIND \"Pulp Fiction\"", gameStyle);
    }

    void next()
    {
        SceneManager.LoadScene("end");
    }
}