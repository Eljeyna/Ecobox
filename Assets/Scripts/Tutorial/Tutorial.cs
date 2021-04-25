using UnityEngine;

public class Tutorial : MonoBehaviour
{
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
        switch (currentTask)
        {
            case 0:
                moveVelocity = Player.Instance.moveVelocity;

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

                break;
            case 1:
                if (Player.Instance.transform.position.y > 0.5f)
                {
                    GameDirector.Instance.UpdateQuest("New Beginnings", 2);
                    currentTask = 2;
                }

                break;
            case 2:
                if (Player.Instance.state == EntityState.Attack)
                {
                    GameDirector.Instance.UpdateQuest("New Beginnings", 3);
                    currentTask = 3;
                }

                break;
            case 3:
                slime = GameObject.Find("_SLIME_POINT");
                if (slime)
                {
                    copy = slime.GetComponent<Addressables_Instantiate>();
                    copy.SpawnEntities();
                    currentTask = 4;
                }

                break;
            case 4:
                if (healthTest && healthTest.healthPercent <= 0)
                {
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
                        }
                    }
                }

                break;
            case 5:
                if (waitTime > Time.time)
                {
                    return;
                }

                SceneLoading.Instance.LoadLevel("MainMenu");

                break;
            default:
                Destroy(this);
                break;
        }
    }
}
