using UnityEngine;
using TMPro;

public class OverheadHP : MonoBehaviour
{
    public Camera cam;
    public Unit unit;
    public TMP_Text healthNumberText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(cam == null)
        {
            cam = Camera.main;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(unit != null && healthNumberText != null)
        {
            healthNumberText.text = unit.health + " / " + unit.maxHealth;
        }

        transform.LookAt(cam.transform);
    }
}
