using UnityEngine;
using UnityEngine.SceneManagement;

public class Ladder : Entity
{
    public override int EntityID => 4;

    protected override void OnCollision(Collision2D other)
    {
        if (other.gameObject.GetComponent<PlayerMovement>())
            SceneManager.LoadScene("Sewer_2");
    }
}