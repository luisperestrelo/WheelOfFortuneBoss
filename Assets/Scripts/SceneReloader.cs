using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneReloader : MonoBehaviour
{
    public string initialSceneName;

    void Start()
    {
        // UNBELIEAVABLY lame solution, but just wanted something fast for the MVP

        //destroy all DontDestroyOnLoad objects...
        Destroy(FindObjectOfType<Player>().gameObject);
        Destroy(FindObjectOfType<SceneLoader>().gameObject);
        Destroy(FindObjectOfType<SFXPool>().gameObject);
        Destroy(FindObjectOfType<MusicPlayer>().gameObject);
        Destroy(FindObjectOfType<CardPool>().gameObject);
        Destroy(FindObjectOfType<CardManager>().gameObject);
        Destroy(FindObjectOfType<CircularPath>().gameObject);
        Destroy(FindObjectOfType<WheelManager>().gameObject);
        Destroy(FindObjectOfType<RunManager>().gameObject);

        SceneManager.LoadScene(initialSceneName, LoadSceneMode.Single);





    }
}