using UnityEngine;

public class Resource : MonoBehaviour
{
    private bool isChecked = false;
    public bool checkedResources()
    {
        return isChecked;
    }

    public void takedResources()
    {
        isChecked = true;
    }


}
