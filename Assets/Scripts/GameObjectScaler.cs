using UnityEngine;
using UnityEngine.SceneManagement;

public class GameObjectScaler : MonoBehaviour
{
    // Set this to the in-world distance between the left & right edges of your scene.
    public float sceneWidth = 10;

    Camera _camera;
    void Start()
    {
        _camera = GetComponent<Camera>();
    }

    void Update()
    {
        float unitsPerPixel = sceneWidth / Screen.width;

        float desiredHalfHeight = 0.3f * unitsPerPixel * Screen.height;

        _camera.orthographicSize = desiredHalfHeight;
    }

    public void GoToGameScene()
    {
        SceneManager.LoadScene("GameplayScene");
    }
}
