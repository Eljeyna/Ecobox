using UnityEngine;

public class Tutorial : MonoBehaviour
{
    public Animator[] animations = new Animator[4];

    private float waitTime;
    private int currentTask;
    private bool[] checks = new bool[2];
    private float moveVelocity;
    private float newMoveVelocity;
    private GameObject slime;
    private Addressables_Instantiate copy;
    private BaseCommon healthTest;

    private void LateUpdate()
    {
        if (StaticGameVariables.isPause)
        {
            return;
        }

        switch (currentTask)
        {
            case 0:
                moveVelocity = Player.Instance.rb.velocity.x;

                if (!checks[0] && newMoveVelocity < moveVelocity)
                {
                    checks[0] = true;
                    newMoveVelocity = moveVelocity;
                }
                else if (!checks[1] && newMoveVelocity > moveVelocity)
                {
                    checks[1] = true;
                    newMoveVelocity = moveVelocity;
                }

                if (checks[0] && checks[1])
                {
                    GameDirector.Instance.UpdateQuest("New Beginnings", 1);
                    currentTask = 1;
                }

#if UNITY_ANDROID || UNITY_IOS
                animations[0].enabled = true;
#endif

                break;
            case 1:
                if (Player.Instance.transform.position.y > 0.5f)
                {
                    GameDirector.Instance.UpdateQuest("New Beginnings", 2);
                    currentTask = 2;
                }

#if UNITY_ANDROID || UNITY_IOS
                animations[0].gameObject.SetActive(false);
                animations[1].enabled = true;
#endif

                break;
            case 2:
                if (Player.Instance.state == EntityState.Attack)
                {
                    GameDirector.Instance.UpdateQuest("New Beginnings", 3);
                    currentTask = 3;
                }

#if UNITY_ANDROID || UNITY_IOS
                animations[1].gameObject.SetActive(false);
                animations[2].enabled = true;
#endif

                break;
            case 3:
                slime = GameObject.Find("_SLIME_POINT");
                if (slime)
                {
                    copy = slime.GetComponent<Addressables_Instantiate>();
                    copy.SpawnEntities();
                    currentTask = 4;
                }

#if UNITY_ANDROID || UNITY_IOS
                animations[2].gameObject.SetActive(false);
#endif
                break;
            case 4:

                if (healthTest && healthTest.healthPercent <= 0)
                {
#if UNITY_ANDROID || UNITY_IOS

                    animations[3].gameObject.SetActive(false);
#endif
                    waitTime = Time.time + 4f;
                    currentTask = 5;
                }
                else if (copy.createdObjects != null)
                {
                    if (copy.createdObjects[0].IsValid() && copy.createdObjects[0].Result)
                    {
                        if (copy.createdObjects[0].Result.TryGetComponent(out BaseCommon baseCommon))
                        {
                            healthTest = baseCommon;
#if UNITY_ANDROID || UNITY_IOS
                            animations[3].enabled = true;
#endif
                        }
                    }
                }

                break;
            case 5:
                if (waitTime > Time.time)
                {
                    return;
                }

                SceneLoading.Instance.SwitchToScene("MainMenu", SceneLoading.startAnimationID);

                break;
            default:
                Destroy(this);
                break;
        }
    }
}
