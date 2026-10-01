using UnityEngine;

public class HelperScript : MonoBehaviour

{

    public void FlipSprite(bool flip)
    {
        SpriteRenderer sr = gameObject.GetComponent<SpriteRenderer>();

        if (flip == true)
        {
            sr.flipX = true;
        }
        else
        {
            sr.flipX = false;
        }
    }

    public void DestroyObject()
    {
        Destroy(gameObject);
    }

}
