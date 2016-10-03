using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class IChopable : MonoBehaviour {

    public int maxLife = 100;
    private int currentLife;

    private const float TIME_BETWEEN_DAMAGE = 1;
    private float timeFromLastHit = 0;

    public Text text;

    // Use this for initialization
    void Start () {
        currentLife = maxLife;
	}

    void Update ()
    {
        text.text = currentLife.ToString();
        if (timeFromLastHit > 0)
            timeFromLastHit -= Time.deltaTime;
    }

    //Currently, this return just a boolean, maybe return a list of item ?
    public bool Damage (int dam)
    {
        if (timeFromLastHit <= 0)
        {
            timeFromLastHit = TIME_BETWEEN_DAMAGE;
            currentLife -= dam;
        }

        return currentLife <= 0;
    }

    public bool isDead ()
    {
        return currentLife <= 0;
    }
}
