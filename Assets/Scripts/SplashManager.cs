using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SplashManager : MonoBehaviour
{
    public static SplashManager InstanceSplash;
    public AudioSource AudioSourc;
    public bool isMusic = true;
    // Start is called before the first frame update
    void Start()
    {
        if (InstanceSplash == null)
        {
            InstanceSplash = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        AudioSourc = GetComponent<AudioSource>();
        StartCoroutine(SplashOut());
    }
    public void musicPlay()// Handle For Background music
    {
        if (isMusic == true)
        {
            AudioSourc.Stop();
            MainMenuSc.instanceMainM.MusicText.text = "/";
            isMusic = false;
        }
        else
        {
            AudioSourc.Play();
            MainMenuSc.instanceMainM.MusicText.text = "";
            isMusic = true;
        }
    }

    IEnumerator SplashOut()//it handle the splash time
    {
        yield return new WaitForSeconds(4);
        SceneManager.LoadScene(1);

    }
}
