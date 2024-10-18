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
    private float fuel = 100f; // 연료바 초기 (최대값)
    private float currentFuel;
    private float addedFuel = 10f; // 프레임마다 더해줄 양
    
    private readonly float SPEED = 5f;
    private readonly float FUELPERSHOOT = 10f;

    public Transform rocket;
    public Transform ground;

    public float maxDistance = 0;

    public Image fuelBar; // 연료바 이미지 연결
    


    void Awake()
    {
        _rb2d = GetComponent<Rigidbody2D>(); // Q1. 요구사항 1 
        currentFuel = fuel; // 값 설정

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


        AddingFuel(); // 프레임별 연료 추가

    }


    public void Shoot()  
    {
        if ( 0f < fuel && fuel <= 100f) // Q1. 요구사항 2 : fuel이 넉넉하면 윗 방향으로 SPEED만큼의 힘으로 점프, 모자라면 무시
        {
            _rb2d.AddForce(transform.up * SPEED, ForceMode2D.Impulse);
            currentFuel -= FUELPERSHOOT;
            Debug.Log("fuel" + currentFuel); // 현재연료 확인용
            ConsumedFuel(); // 연료바에 적용
        }

    }

    public void RestartGame() // Q2. 요구사항 3 : 버튼 클릭시 재시작
    {
        SceneManager.LoadScene("RocketLauncher");
        Debug.Log("재시작");
    }

    public void ConsumedFuel() // Q3.요구사항 2 연료바 fillamount 변경
    {
        fuelBar.fillAmount = currentFuel / fuel; 
    }

    public void AddingFuel() // Q3.요구사항 3. 프레임마다 0.1씩 연료 추가
    {
        fuelBar.fillAmount += addedFuel * Time.deltaTime / fuel;
    }
}
