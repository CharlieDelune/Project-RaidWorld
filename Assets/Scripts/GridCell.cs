using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCell : MonoBehaviour
{
    private Renderer rend;
    [SerializeField]
    private bool passable;
    private Color color;
    public int x, z, gCost, hCost, fCost;
    public GridCell previousCell;
    // Start is called before the first frame update
    void Start()
    {
        rend =GetComponent<Renderer>();
        color = Color.white;
        passable = true;
        this.x = (int)this.gameObject.transform.localPosition.x;
        this.z = (int)this.gameObject.transform.localPosition.z;
    }

    // Update is called once per frame
    void Update()
    {
    }

    void OnMouseEnter()
    {
        rend.material.color = Color.green;
    }

    void OnMouseExit()
    {
        rend.material.color = color;
    }

    void OnMouseDown()
    {
        this.passable = !this.passable;
        this.color = this.passable ? Color.white : Color.black;
        rend.material.color = color;
    }

    public void CalculateFCost()
    {
        fCost = gCost + hCost;
    }

    public void SetColor(Color inputColor)
    {
        color = inputColor;
        rend.material.color = color;
    }

    public bool IsPassable()
    {
        return this.passable;
    }

    public void ResetCell()
    {
        this.SetColor(Color.white);
        this.passable = true;
    }

}
