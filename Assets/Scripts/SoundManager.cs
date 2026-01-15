using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private static SoundManager _instance;

    public static SoundManager Instance
    {
        get
        {
            if (_instance == null)
                Debug.LogError("Sound Manager is empty!!!");

            return _instance;
        }
    }
    private void Awake()
    {
        _instance = this;
    }
    [SerializeField] AudioSource blueDashAS;
    [SerializeField] AudioSource redDashAS;
    //[SerializeField] AudioSource blueDashSettingAS;
    //[SerializeField] AudioSource redDashSettingAS;
    [SerializeField] AudioSource collideAS;
    [SerializeField] AudioSource ballCaptureAS;
    [SerializeField] AudioSource ballStealAS;
    [SerializeField] AudioSource ballKickAS;
    [SerializeField] AudioSource goalAS;
    [SerializeField] AudioSource ballWallHitAS;

    [SerializeField] AudioClip dashSFX;
    //[SerializeField] AudioClip dashSettingSFX;
    [SerializeField] AudioClip collideSFX;
    [SerializeField] AudioClip ballCaptureSFX;
    [SerializeField] AudioClip ballStealSFX;
    [SerializeField] AudioClip ballKickSFX;
    [SerializeField] AudioClip goalSFX;
    [SerializeField] AudioClip ballWallHitSFX;

    public void PlayDashSound(TeamType _teamType)
    {
        if (blueDashAS == null || redDashAS == null || dashSFX == null) return;
        if (_teamType == TeamType.left)
        {
            //if(blueDashSettingAS.isPlaying)
            //{
            //    blueDashSettingAS.Stop();
            //}
            blueDashAS.PlayOneShot(dashSFX);
        }
        else
        {
            //if (redDashSettingAS.isPlaying)
            //{
            //    redDashSettingAS.Stop();
            //}
            redDashAS.PlayOneShot(dashSFX);
        }
    }
    //public void PlayDashSettingSound(TeamType _teamType)
    //{
    //    if (blueDashSettingAS == null || redDashSettingAS == null || dashSettingSFX == null) return;
    //    if (_teamType == TeamType.left)
    //    {
    //        blueDashSettingAS.PlayOneShot(dashSettingSFX);
    //    }
    //    else
    //    {
    //        redDashSettingAS.PlayOneShot(dashSettingSFX);
    //    }
    //}
    public void PlayCollideSound()
    {
        if (collideSFX == null || collideAS == null) return;
        if (blueDashAS.isPlaying)
        {
            blueDashAS.Stop();
        }
        if (redDashAS.isPlaying)
        {
            redDashAS.Stop();
        }
        collideAS.PlayOneShot(collideSFX);
    }
    public void PlayCaptureSound()
    {
        if (ballCaptureAS == null || ballCaptureSFX == null) return;
        ballCaptureAS.PlayOneShot(ballCaptureSFX);
    }
    public void PlayBallStealSound()
    {
        if (ballStealAS == null || ballStealSFX == null) return;
        ballStealAS.PlayOneShot(ballStealSFX);
    }
    public void PlayBallKickSound()
    {
        if (ballKickAS == null || ballKickSFX == null) return;
        ballKickAS.PlayOneShot(ballKickSFX);
    }
    public void PlayGoalSound()
    {
        if (goalAS == null || goalSFX == null) return;
        goalAS.PlayOneShot(goalSFX);
    }
    public void PLayBallWallHitSound()
    {
        if (ballWallHitAS == null || ballWallHitSFX == null) return;
        ballWallHitAS.PlayOneShot(ballWallHitSFX);
    }
}//Class
