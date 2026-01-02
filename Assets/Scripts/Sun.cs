using UnityEngine;

public class Sun : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision) {
        GameManager.Instance.CompleteLevel();
    }


    /*
   public float radius;




  private void Start() {
      StartCoroutine(CheckIfInRange());
  }


  public IEnumerator CheckIfInRange() {
      while (gameObject.activeInHierarchy) {

          Collider2D[] targetsInViewRange = Physics2D.OverlapCircleAll(transform.position, radius, 9);

          if (targetsInViewRange.Length > 0) {
              GameManager.Instance.CompleteLevel();
          }

          yield return new WaitForSeconds(0.3f);
      }


  }

  */
}
