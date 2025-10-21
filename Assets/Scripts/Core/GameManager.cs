using UnityEngine;
using UnityEngine.SceneManagement;


public enum GameState { Menu, Playing, Paused, GameOver }


public class GameManager : MonoBehaviour
{
public static GameManager Instance { get; private set; }


[Header("Panels")] public GameObject panelStart; public GameObject panelHUD; public GameObject panelPause; public GameObject panelGameOver;
[Header("Game Over UI")] public TMPro.TMP_Text finalScoreText;


GameState state;


void Awake()
{
if (Instance && Instance != this) { Destroy(gameObject); return; }
Instance = this;
SetState(GameState.Menu);
Time.timeScale = 1f; // ensure normal speed
ScoreManager.Reset();
}


void Update()
{
if (state == GameState.Playing && Input.GetKeyDown(KeyCode.Escape)) Pause();
else if (state == GameState.Paused && Input.GetKeyDown(KeyCode.Escape)) Continue();
}


public void StartGame()
{
// Reset score & player health for a fresh run
ScoreManager.Reset();
var p = GameObject.FindGameObjectWithTag("Player");
var h = p ? p.GetComponent<Health>() : null;
if (h) h.SetMaxAndFill(100);


// Clear any leftover zombies
foreach (var z in GameObject.FindGameObjectsWithTag("Enemy")) z.SetActive(false);


SetState(GameState.Playing);
}


public void Pause() { if (state != GameState.Playing) return; SetState(GameState.Paused); }
public void Continue() { if (state != GameState.Paused) return; SetState(GameState.Playing); }


public void Restart()
{
Time.timeScale = 1f;
SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
}


public void BackToMenu()
{
Time.timeScale = 1f;
SetState(GameState.Menu);
}


public void OnPlayerDied()
{
SetState(GameState.GameOver);
if (finalScoreText) finalScoreText.text = ScoreManager.Current.ToString();
}


void SetState(GameState s)
{
state = s;
Time.timeScale = (s == GameState.Paused) ? 0f : 1f;
if (panelStart) panelStart.SetActive(s == GameState.Menu);
if (panelHUD) panelHUD.SetActive(s == GameState.Playing);
if (panelPause) panelPause.SetActive(s == GameState.Paused);
if (panelGameOver) panelGameOver.SetActive(s == GameState.GameOver);
}
}