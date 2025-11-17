using UnityEngine;
using UnityEngine.U2D.IK;

public class enemyController : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] GameObject leftArm;
    [SerializeField] GameObject rightArm;
    [SerializeField] GameObject weapon;
    bool checkFront;

    LimbSolver2D armL;
    LimbSolver2D armR;
    Transform oldTargetL;
    Transform oldTargetR;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        armL = leftArm.GetComponent<LimbSolver2D>();
        armR = rightArm.GetComponent<LimbSolver2D>();
        //oldTargetL = armL.target;
        //oldTargetR = armR.target;
    }

    // Update is called once per frame
    void Update()
    {
        
        checkFront = Physics2D.OverlapBox(new Vector2(transform.position.x + (0.4f * transform.localScale.x), transform.position.y - 0.2f), new Vector2(0.5f, .5f), 0f);
        if (checkFront)
        {
            //armL.target = weapon;
            //armR.target = weapon;
        }
        else
        {
            //armL.target = oldTargetL;
            //armR.target = oldTargetR;
        }
    }
}
