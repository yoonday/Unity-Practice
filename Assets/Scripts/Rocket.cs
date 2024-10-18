using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI currentScoreTxt;
    [SerializeField] private TextMeshProUGUI highScoreTxt;

    public string sceneName = "RocketLauncher";

    private Rigidbody2D _rb2d;
    private float fuel = 100f;
    
    private readonly float SPEED = 5f;
    private readonly float FUELPERSHOOT = 10f;

    public Transform rocket;
    public Transform ground;

    public float maxDistance = 0;


    void Awake()
    {
        // TODO : Rigidbody2D 컴포넌트를 가져옴(캐싱)
        _rb2d = GetComponent<Rigidbody2D>(); // 요구사항 
        if (PlayerPrefs.HasKey("HighScore")) // 최고 점수가 있다면
        {
            float best = PlayerPrefs.GetFloat("HighScore"); // 저장되어있는 최고 점수
            highScoreTxt.text = $"HIGH : {best} M";
        }

    }

    private void Update()
    {   
        // 거리 계산
        float distance = Vector2.Distance(rocket.position, ground.position);
        
        if (distance > maxDistance) // 거리 갱신
        {
            maxDistance = distance;
        }

        int currentScore = (int)distance;
        int highScore = (int)maxDistance;
        
        // 현재 스코어 표시
        currentScoreTxt.text = $"{currentScore} M";

      
        // 최고 점수
        if (PlayerPrefs.HasKey("HighScore")) // 최고 점수가 있다면
        {
            float best = PlayerPrefs.GetFloat("HighScore"); // 저장되어있는 최고 점수

            if (best < highScore) // 최고 점수 < 현재 점수 → 현재 점수를 최고 점수에 저장
            {
                PlayerPrefs.SetFloat("HighScore", highScore);
                highScoreTxt.text = $"HIGH : {highScore} M";
            }
        }
        else // 최고 점수가 없다면
        {
            PlayerPrefs.SetFloat("HighScore", highScore);
            PlayerPrefs.Save();
            highScoreTxt.text = $"HIGH : {highScore} M";
        }
        
    }


    public void Shoot()
    {
        // TODO : fuel이 넉넉하면 윗 방향으로 SPEED만큼의 힘으로 점프, 모자라면 무시
        if ( 0f < fuel && fuel <= 100f)
        {
            _rb2d.AddForce(transform.up * SPEED, ForceMode2D.Impulse);
            fuel -= FUELPERSHOOT; // 요구사항 2
            Debug.Log("fuel" +  fuel); // 연료 확인용
        }

    }

    public void RestartGame()
    {
        SceneManager.LoadScene("RocketLauncher");
        Debug.Log("재시작");
    }
}
