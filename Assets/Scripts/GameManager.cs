using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum GameState { Active, Gameover }

public class GameManager : MonoBehaviour {

    public static GameManager Instance { get; private set; }

    public int level = 0;
    public List<Transform> levelSunPositions;
    public List<Color> levelColors;

    public Transform sunTransform;
    public SpriteRenderer sunSprite;
    public SpriteRenderer innerSunSprite;

    public float levelTime = 30;
    public float timeLeft;

    public Image timeImage;

    public GameState state;

    public RectTransform scorePanel;
    public RectTransform timePanel;
    public RectTransform startButton;

    public TextMeshProUGUI dayText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI bestText;
    public Color completeColor;

    public AudioClip levelCompleteSFX;
    public AudioClip victorySFX;
    public AudioClip gameOverSFX;

    public List<float> cameraYPosition;

    public GameObject rain;

    public Transform victoryPanel;

    public bool isLoading = false;

    public GameObject rootBranch;

    public int score = 0;
    public int highscore;


    public void Awake() {
        Time.timeScale = 0f;
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(this);
        }
    }

    private void Start() {
        highscore = PlayerPrefs.GetInt("highscore", 0);
        bestText.text = highscore.ToString();
    }

    public void AddScore() {
        score++;
        scoreText.text = score.ToString();
    }

    public void StartGame() {
        Camera.main.DOColor(levelColors[level], 0.3f);
        Time.timeScale = 1f;
        
        // Use DOTween sequence for cleaner UI animation flow
        Sequence uiSequence = DOTween.Sequence();
        uiSequence.Append(scorePanel.DOScale(1, 0.3f));
        uiSequence.Join(timePanel.DOScale(1, 0.3f));
        uiSequence.Join(startButton.DOScale(0, 0.3f));
        
        state = GameState.Active;
        SoundManager.Instance.musicSource.Play();
        rain.SetActive(true);
        
        // Start timer only once (removed duplicate call)
        StartCoroutine(Timer());
    }


    public IEnumerator Timer() {
        while (state == GameState.Active) {
            yield return new WaitForSeconds(1f);
            timeLeft--;
            timeImage.DOFillAmount(timeLeft / levelTime, 0.3f);
            if (timeLeft <= 0 || !rootBranch.activeInHierarchy) {
                state = GameState.Gameover;
                StartCoroutine(GameOver());
            }
        }
    }

    public void GameOverClick() {
        StartCoroutine(GameOver());
    }

    public IEnumerator GameOver() {
        state = GameState.Gameover;
        
        // Combine multiple DOTween animations into a sequence for better performance
        Sequence gameOverSequence = DOTween.Sequence();
        gameOverSequence.Append(sunTransform.DOMove(new Vector3(0f, 3f, 0f), 0.5f));
        gameOverSequence.Join(Camera.main.DOColor(levelColors[6], 1f));
        
        Camera.main.transform.position = new Vector3(0f, 3f, -20f);
        Camera.main.orthographicSize = 3;
        Camera.main.transform.DOShakePosition(3f);
        SoundManager.PlayRandomSfx(gameOverSFX);
        
        if (score > highscore) {
            PlayerPrefs.SetInt("highscore", score);
        }
        
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void CompleteLevel() {
        if (!isLoading && state != GameState.Gameover) {
            StartCoroutine(SetUpLevel());
        }
    }

    public IEnumerator SetUpLevel() {
        if (level >= 6) {
            isLoading = true;
            
            // Combine DOTween animations into a sequence for better performance
            Sequence victorySequence = DOTween.Sequence();
            victorySequence.Append(timeImage.DOColor(completeColor, 0.3f));
            victorySequence.Join(timeImage.DOFillAmount(1, 0.3f));
            victorySequence.Join(sunSprite.DOColor(completeColor, 0.3f));
            victorySequence.Join(innerSunSprite.DOColor(completeColor, 0.3f));
            victorySequence.Join(Camera.main.DOColor(levelColors[2], 0.3f));
            victorySequence.Append(victoryPanel.DOScale(1f, 0.3f));
            
            dayText.text = "7";
            SoundManager.PlayRandomSfx(victorySFX);

            state = GameState.Gameover;
            StopAllCoroutines();
        } else {
            isLoading = true;
            levelTime += 20;
            timeLeft = levelTime;
            
            // Combine DOTween animations for level progression
            Sequence levelSequence = DOTween.Sequence();
            levelSequence.Append(timeImage.DOFillAmount(timeLeft / levelTime, 0.3f));
            levelSequence.Join(timeImage.rectTransform.DOPunchScale(new Vector3(1.1f, 1.1f), 0.3f, 1, 1));
            levelSequence.Join(sunTransform.DOShakeScale(0.5f));
            levelSequence.Append(sunTransform.DOMove(levelSunPositions[level + 1].position, 0.5f));
            levelSequence.Join(Camera.main.transform.DOMove(new Vector3(0f, cameraYPosition[level + 1], -20f), 0.3f));
            levelSequence.Join(Camera.main.DOColor(levelColors[level + 1], 0.3f));
            
            level++;
            dayText.text = (level + 1).ToString();
            SoundManager.PlayRandomSfx(levelCompleteSFX);
            Camera.main.orthographicSize = Camera.main.orthographicSize <= 13 ? Camera.main.orthographicSize += 2 : 13;
            
            yield return new WaitForSeconds(1f);
            isLoading = false;
        }
    }

}
