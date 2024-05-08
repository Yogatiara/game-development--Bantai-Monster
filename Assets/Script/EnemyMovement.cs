using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;


public class EnemyMovement : MonoBehaviour
{
  // Start is called before the first frame update
  private Animator animator;

  public Transform target;
  public float speed = 3f;
  // private Rigidbody2D rb;
  private bool isAnimatingDeath = false;

  public Movement movement;

  private EnemySpawner enemySpawner;

  public static int enemies = 0;

  void Start()
  {
    animator = GetComponent<Animator>();

    // rb = GetComponent<Rigidbody2D>();
  }

  // Update is called once per frame
  void Update()
  {
    if (!target)
    {
      target = GameObject.FindGameObjectWithTag("Player").transform;

    }

    else
    {
      movement = FindObjectOfType<Movement>();

      if (movement != null)
      {
        if (movement.playerDeath)
        {
          if (isAnimatingDeath)
          {
            speed = 0f;
          }
          else
          {
            speed = 2.5f;

          }
          target = GameObject.FindGameObjectWithTag("Respawn").transform;
        }

      }

      FollowTarget();
    }

    // Debug.Log(movement.playerDeath);


  }


  void FollowTarget()
  {

    Vector3 direction = (target.position - transform.position).normalized;

    if (direction.x < 0)
    {
      if (transform.rotation.eulerAngles.y != 180f)
      {
        transform.rotation = Quaternion.Euler(0f, 180f, 0f);
      }
    }
    else
    {
      if (transform.rotation.eulerAngles.y != 0f)
      {
        transform.rotation = Quaternion.Euler(0f, 0f, 0f);
      }
    }

    transform.position = Vector3.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);

  }


  void OnCollisionEnter2D(Collision2D other)
  {
    if (other.gameObject.CompareTag("Bullet") || other.gameObject.CompareTag("BulletIdle") && !isAnimatingDeath)
    {
      enemySpawner = FindObjectOfType<EnemySpawner>();


      enemies += 1;
      Debug.Log(enemies);
      if (enemies >= enemySpawner.maxEnemies)
      {

        StartCoroutine(ShowWinnerPopUp());
      }

      speed = 0;
      isAnimatingDeath = true;
      animator.SetBool("isDead", true);
      StartCoroutine(DestroyAfterAnimation());
    }


  }
  private IEnumerator ShowWinnerPopUp()
  {

    yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length + 0.3f);

    SceneManager.LoadSceneAsync(2);


  }

  private IEnumerator DestroyAfterAnimation()
  {
    float deathAnimationDuration = animator.GetCurrentAnimatorStateInfo(0).length + 0.3f;

    yield return new WaitForSeconds(deathAnimationDuration);
    Destroy(gameObject);


  }



}
