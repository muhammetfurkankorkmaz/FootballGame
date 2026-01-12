//using System.Collections;
//using UnityEngine;

//public class CharacterAnimation : MonoBehaviour
//{
//    [SerializeField] GameObject visuals;
//    CharacterState currentState = CharacterState.normal;
//    [SerializeField] GameObject ballParentObject;

//    CharacterController chController;
//    void Start()
//    {
//        chController = GetComponent<CharacterController>();
//    }

//    // Update is called once per frame
//    void Update()
//    {

//    }

//    public void ChangeCharacterState(CharacterState newState, float chargePercent)
//    {
//        if (newState == CharacterState.normal)
//        {
//            NormalizeCharacter();
//        }
//        else if (newState == CharacterState.dashSetting)
//        {
//            StartDashSettingAnimation();
//        }
//        else if (newState == CharacterState.dashing)
//        {
//            DashingAnimation(chargePercent);
//        }
//        else if (newState == CharacterState.ballSetting)
//        {
//            StartBallShootingAnimation(chargePercent);
//        }
//        currentState = newState;
//    }

//    void NormalizeCharacter()
//    {
//        StopAllCoroutines();
//        StartCoroutine(NormalizeLerp());
//    }

//    void StartDashSettingAnimation()
//    {
//        StartCoroutine(DashSettingLerp());
//    }

//    void DashingAnimation(float chargePercent)
//    {
//        StartCoroutine(DashLerp(chController.DashDirection, chargePercent));
//    }
//    void StartBallShootingAnimation(float chargePercent)
//    {
//        Vector3 shootDir = ballParentObject.transform.right;
//        print("shoot dir is " + shootDir);
//        StartCoroutine(BallSetAnimation(shootDir, chargePercent));
//    }

//    IEnumerator NormalizeLerp()
//    {
//        Vector3 startSize = visuals.transform.localScale;
//        Vector3 endSize = new Vector3(1f, 1f, 1f);

//        float elapsed = 0;
//        float duration = 0.25f;

//        while (elapsed < duration)
//        {
//            float t = elapsed / duration;

//            visuals.transform.localScale = Vector3.Lerp(startSize, endSize, t);
//            elapsed += Time.deltaTime;
//            yield return null;
//        }
//        visuals.transform.localScale = endSize;
//    }

//    IEnumerator DashSettingLerp()//Size gets smaller
//    {
//        Vector3 startSize = visuals.transform.localScale;
//        Vector3 endSize = new Vector3(0.75f, 0.75f, 0.5f);

//        float elapsed = 0;
//        float duration = 0.25f;

//        while (elapsed < duration)
//        {
//            float t = elapsed / duration;

//            visuals.transform.localScale = Vector3.Lerp(startSize, endSize, t);
//            elapsed += Time.deltaTime;
//            yield return null;
//        }
//        visuals.transform.localScale = endSize;
//    }
//    IEnumerator DashLerp(Vector3 dashDirection, float chargePercent)//Size gets smaller
//    {
//        Vector3 startSize = visuals.transform.localScale;
//        //print(startSize + " startsize");
//        Vector3 endSize = new Vector3(1f, 1f, 1f);

//        float elapsed = 0;
//        float duration = 0.25f * chargePercent;

//        Vector3 stretchScale = new Vector3(
//        Mathf.Abs(dashDirection.x) > 0 ? 1.5f : 0.75f,
//        Mathf.Abs(dashDirection.y) > 0 ? 1.5f : 0.75f,
//        1f
//    );
//        //print(stretchScale + " stretchsize");


//        while (elapsed < duration)
//        {
//            float t = elapsed / duration;
//            visuals.transform.localScale = Vector3.Lerp(startSize, stretchScale, t);
//            elapsed += Time.deltaTime;
//            yield return null;
//        }
//        startSize = visuals.transform.localScale;

//        elapsed = 0;
//        duration = 0.1f;
//        while (elapsed < duration)
//        {
//            float t = elapsed / duration;

//            visuals.transform.localScale = Vector3.Lerp(startSize, endSize, t);
//            elapsed += Time.deltaTime;
//            yield return null;
//        }
//        visuals.transform.localScale = endSize;
//    }

//    IEnumerator BallSetAnimation(Vector3 dashDirection, float chargePercent)
//    {
//        Vector3 startSize = visuals.transform.localScale;
//        //print(startSize + " startsize");
//        Vector3 endSize = new Vector3(1f, 1f, 1f);

//        float elapsed = 0;
//        float duration = 0.25f;

//        dashDirection.Normalize();

//        bool horizontal = Mathf.Abs(dashDirection.x) > Mathf.Abs(dashDirection.y);

//        Vector3 stretchScale = horizontal
//            ? new Vector3(0.9f, 1.15f, 1f)
//            : new Vector3(1.15f, 0.9f, 1f);

//        print(stretchScale + " stretchsize");


//        while (elapsed < duration)
//        {
//            float t = elapsed / duration;
//            visuals.transform.localScale = Vector3.Lerp(startSize, stretchScale, t);
//            elapsed += Time.deltaTime;
//            yield return null;
//        }
//        //startSize = visuals.transform.localScale;

//        //elapsed = 0;
//        //duration = 0.1f;
//        //while (elapsed < duration)
//        //{
//        //    float t = elapsed / duration;

//        //    visuals.transform.localScale = Vector3.Lerp(startSize, endSize, t);
//        //    elapsed += Time.deltaTime;
//        //    yield return null;
//        //}
//        //visuals.transform.localScale = endSize;
//    }

//}//Class
