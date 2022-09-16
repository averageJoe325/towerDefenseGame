using UnityEngine;

//  Manages the rope for the claw.
public class ClawRope : MonoBehaviour
{
    // Move the rope with the claw.
    private void Update()
    {
        Vector2 towerToClaw = (Vector2)transform.parent.GetComponent<RectTransform>().localPosition
            - transform.GetComponentInParent<ClawProjectile>().startLocation;
        GetComponent<RectTransform>().localPosition = -towerToClaw / 2 / transform.parent.GetComponent<RectTransform>().localScale.x;
        GetComponent<RectTransform>().localRotation
            = Quaternion.Euler(new Vector3(0, 0, Mathf.Rad2Deg * Mathf.Atan2(towerToClaw.y, towerToClaw.x)));
        GetComponent<RectTransform>().localScale = new Vector3(towerToClaw.magnitude / transform.parent.GetComponent<RectTransform>().localScale.x, .5f);
    }
}
