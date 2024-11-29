using UnityEngine;

public class DisableGameObject : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Gem") || other.gameObject.CompareTag("Obstacle"))
            other.gameObject.SetActive(false);
    }
}

using UnityEngine;

public class DisableRoad : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("MainRoad"))
            other.gameObject.SetActive(false);
    }
}

using UnityEngine;

public class Ground : MonoBehaviour
{
    [SerializeField] private GameObject _player;
    [SerializeField] private GameObject _box;

    private void Update()
    {
        _box.transform.position = new Vector3(_player.transform.position.x, -0.3f, _player.transform.position.z);
    }
}

using System;
using UnityEngine;

public class MapSettings : MonoBehaviour
{
    [Header("Speed Settings")]
    [SerializeField, Range(5f, 15f)] private float _speed;
    [SerializeField, Range(0.1f, 1f)] private float _speedIncrease;
    [SerializeField] private int _amountScoreToIncreaseSpeed;
    [Header("Score Settings")]
    [SerializeField] private float _scorePerSecond;
    [SerializeField] private float _scorePerSection;
    [SerializeField] private float _scoreForRedGem;
    [SerializeField] private float _scoreForGreenGem;
    [SerializeField] private float _scoreForBlueGem;
    [Header("Gem Settings")]
    [SerializeField, Range(0, 100)] private int _chanceRedGemSpawn;
    [SerializeField, Range(0, 100)] private int _chanceGreenGemSpawn;
    [SerializeField, Range(0, 100)] private int _chanceBlueGemSpawn;
    [SerializeField] private AnimationCurve _glowCurve;

    private bool _increaseSpeed = false;
    private int _localAmountScore;
    private float _intensity = 2f;
    private float _time;

    public static MapSettings mapSettings;
    public float Speed { get { return _speed; } set { _speed = value; } }
    public float ScorePerSecond { get { return _scorePerSecond; } set { _scorePerSecond = value; } }
    public float ScorePerSection { get { return _scorePerSection; } private set { } }
    public float ScoreForRedGem { get { return _scoreForRedGem; } private set { } }
    public float ScoreForGreenGem { get { return _scoreForGreenGem; } private set { } }
    public float ScoreForBlueGem { get { return _scoreForBlueGem; } private set { } }
    public float ChanceRedGemSpawn { get { return _chanceRedGemSpawn; } private set { } }
    public float ChanceGreenGemSpawn { get { return _chanceGreenGemSpawn; } private set { } }
    public float ChanceBlueGemSpawn { get { return _chanceBlueGemSpawn; } private set { } }
    public float Intensity { get { return _intensity; } private set { } }


    void Awake() => mapSettings = this;

    void Start()
    {
        _localAmountScore = _amountScoreToIncreaseSpeed;
    }

    void Update()
    {
        if (Score.score.Scor % _amountScoreToIncreaseSpeed <= 50f && _increaseSpeed == true)
        {
            _increaseSpeed = false;
            _speed += _speedIncrease;
            _localAmountScore += _amountScoreToIncreaseSpeed;
        }

        if (Score.score.Scor % _amountScoreToIncreaseSpeed > 50f)
            _increaseSpeed = true;

        if (_speed >= 14.8f)
            _speedIncrease = 0;

        _intensity = _glowCurve.Evaluate(_time);
        _time += Time.deltaTime;
    }
}

using UnityEngine;

public class MoveObjects : MonoBehaviour
{
    [SerializeField] private GameObject _object;

    void Update()
    {
        _object.transform.position += new Vector3(0, 0, MapSettings.mapSettings.Speed) * Time.deltaTime;
    }
}

using UnityEngine;

public class PlayerMoving : MonoBehaviour
{
    [SerializeField] private GameObject _player;
    [SerializeField] private float _smoothSlide;
    [SerializeField] private float _jumpForce;

    private float[] _listOfPosition = { 4.24f, 2.65f, 1.06f };
    private int _currentPosition = 1;

    private Rigidbody _playerRB;
    private bool _onGround = false;

    void Start()
    {
        _player.transform.position = new Vector3(_listOfPosition[_currentPosition], _player.transform.position.y, _player.transform.position.z);
        _playerRB = GetComponent<Rigidbody>();
    }

    void Update()
    {
        MoveLogic();
        Jump();
    }

    private void MoveLogic()
    {
        if (Input.GetKeyDown(KeyCode.A))
            if (_currentPosition != 0)
                _currentPosition--;

        if (Input.GetKeyDown(KeyCode.D))
            if (_currentPosition != 2)
                _currentPosition++;

        _player.transform.position = Vector3.Lerp(_player.transform.position,
                            new Vector3(_listOfPosition[_currentPosition], _player.transform.position.y, _player.transform.position.z), _smoothSlide * Time.deltaTime);
    }

    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && _onGround == true)
        {
            _playerRB.AddForce(Vector3.up * _jumpForce);
            _onGround = false;
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Ground"))
            _onGround = true;
    }


}
using System.Collections.Generic;
using UnityEngine;

public class PlayerRichEndOfPlatform : MonoBehaviour
{
    [SerializeField] private float _distance;

    private int _posIndex;
    private Vector3 _posGameObject;
    private List<int> _posIndexContainer = new List<int>(6);

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
            CreateNewSection();
    }

    private void CreateNewSection()
    {
        GameObject road = RoadInstance.instance.GetPooledObject();

        if (road != null)
        {
            road.transform.position = new Vector3(1.95f, 0, _distance * -1);
            road.SetActive(true);
            Score.score.PlayerPassSection();
        }

        if (_posIndexContainer.Count != 0)
            _posIndexContainer.Clear();

        for (int i = 0; i < 5; i++)
            _posIndexContainer.Add(i);

        while (_posIndexContainer.Count - 2 > 0)
        {
            _posIndex = Random.Range(0, 6);

            if (_posIndexContainer.Contains(_posIndex))
            {
                _posIndexContainer.Remove(_posIndex);
                _posGameObject = road.transform.GetChild(_posIndex).transform.position;

                int x = Random.Range(0, 2);
                if (x == 0)
                    GemLineGenerator.instance.Generate(_posGameObject);
                else
                    ObstacleGenerator.instance.Generate(_posGameObject);
            }
        }
    }

}

using System.Collections.Generic;
using UnityEngine;

public class RoadInstance : MonoBehaviour
{
    public static RoadInstance instance;

    private List<GameObject> pooledObjects = new List<GameObject>();
    private int amountToPool = 4;

    [SerializeField] private GameObject roadPreFab;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    void Start()
    {
        for (int i = 0; i < amountToPool; i++)
        {
            GameObject obj = Instantiate(roadPreFab);
            obj.SetActive(false);
            pooledObjects.Add(obj);
        }
    }

    public GameObject GetPooledObject()
    {
        for (int i = 0; i < pooledObjects.Count; i++)
            if (!pooledObjects[i].activeInHierarchy)
                return pooledObjects[i];
        return null;
    }
}

using UnityEngine;
using System;
using TMPro;

public class Score : MonoBehaviour
{
    private TextMeshProUGUI _scoreText;
    private TextMeshProUGUI _redGemCountText;
    private TextMeshProUGUI _greenGemCountText;
    private TextMeshProUGUI _blueGemCountText;

    private float _score = 0;

    private int _redGemCount = 0;
    private int _greenGemCount = 0;
    private int _blueGemCount = 0;

    public int RedGemCount { get { return _redGemCount; } private set { } }
    public int GreenGemCount { get { return _greenGemCount; } private set { } }
    public int BlueGemCount { get { return _blueGemCount; } private set { } }
    public float Scor { get { return _score; } private set { } }
    public static Score score;

    void Awake() => score = this;

    void Start()
    {
        _scoreText = GameObject.Find("Score Count").GetComponent<TextMeshProUGUI>();
        _redGemCountText = GameObject.Find("Red Gem Count").GetComponent<TextMeshProUGUI>();
        _redGemCountText.text = _redGemCount.ToString();
        _greenGemCountText = GameObject.Find("Green Gem Count").GetComponent<TextMeshProUGUI>();
        _greenGemCountText.text = _greenGemCount.ToString();
        _blueGemCountText = GameObject.Find("Blue Gem Count").GetComponent<TextMeshProUGUI>();
        _blueGemCountText.text = _blueGemCount.ToString();
    }

    void Update()
    {
        GiveScore();
    }

    void GiveScore()
    {
        _score += MapSettings.mapSettings.ScorePerSecond * Time.deltaTime;
        _scoreText.text = "Score: " + Math.Round(_score).ToString();
    }

    public void PlayerPassSection()
    {
        _score += MapSettings.mapSettings.ScorePerSection;
        _scoreText.text = "Score: " + Math.Round(_score).ToString();
    }

    public void PlayerPickUpGem(GameObject gem)
    {
        switch (gem.name)
        {
            case "Red Gem(Clone)":
                _score += MapSettings.mapSettings.ScoreForRedGem;
                _redGemCount++; _redGemCountText.text = _redGemCount.ToString(); break;
            case "Green Gem(Clone)":
                _score += MapSettings.mapSettings.ScoreForGreenGem;
                _greenGemCount++; _greenGemCountText.text = _greenGemCount.ToString(); break;
            case "Blue Gem(Clone)":
                _score += MapSettings.mapSettings.ScoreForBlueGem;
                _blueGemCount++; _blueGemCountText.text = _blueGemCount.ToString(); break;
        }
        _scoreText.text = "Score: " + Math.Round(_score).ToString();
    }
}

using UnityEngine;

public class Values : MonoBehaviour
{
    private float _highScore = 0;
    private int _redGemCount = 0;
    private int _greenGemCount = 0;
    private int _blueGemCount = 0;

    public float HighScore { get { return _highScore; } private set { } }
    public int RedGemCount { get { return _redGemCount; } private set { } }
    public int GreenGemCount { get { return _greenGemCount; } private set { } }
    public int BlueGemCount { get { return _blueGemCount; } private set { } }

    public static Values values;

    void Awake() => values = this;

    private void Start()
    {
        GetData();
    }

    public void GetData()
    {
        _highScore = PlayerPrefs.GetFloat("highScore");
        _redGemCount = PlayerPrefs.GetInt("redGemCount");
        _greenGemCount = PlayerPrefs.GetInt("greenGemCount");
        _blueGemCount = PlayerPrefs.GetInt("blueGemCount");
    }
}

using System.Collections.Generic;
using UnityEngine;


public class BlueGemInstance : MonoBehaviour
{
    public static BlueGemInstance instance;

    [SerializeField] private GameObject _gemPreFab;

    private List<GameObject> _pooledGems = new List<GameObject>();
    private int _gemsAmount = 20;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    void Start()
    {
        for (int i = 0; i < _gemsAmount; i++)
        {
            GameObject obj = Instantiate(_gemPreFab);
            obj.SetActive(false);
            _pooledGems.Add(obj);
        }
    }

    public GameObject GetPooledObject()
    {
        for (int i = 0; i < _pooledGems.Count; i++)
            if (!_pooledGems[i].activeInHierarchy)
                return _pooledGems[i];
        return null;
    }
}

using UnityEngine;

public class GemGlow : MonoBehaviour
{
    private Material _material;

    void Start()
    {
        _material = GetComponentInChildren<MeshRenderer>().material;
    }

    void Update()
    {
        _material.SetColor("_EmissionColor", _material.color * MapSettings.mapSettings.Intensity);
    }
}

using UnityEngine;

public class GemLineGenerator : MonoBehaviour
{
    private int _amountSpawnedGems;
    private int _rollChance;
    private float _spaceBetweenGems = 1f;
    private GameObject gem;


    public static GemLineGenerator instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    public void Generate(Vector3 position)
    {
        gem = null;
        _rollChance = Random.Range(0, 101);
        _amountSpawnedGems = Random.Range(2, 7);

        while (_amountSpawnedGems > 0)
        {
            if (_rollChance <= MapSettings.mapSettings.ChanceBlueGemSpawn)
                gem = BlueGemInstance.instance.GetPooledObject();
            else if (_rollChance <= MapSettings.mapSettings.ChanceGreenGemSpawn)
                gem = GreenGemInstance.instance.GetPooledObject();
            else if (_rollChance <= MapSettings.mapSettings.ChanceRedGemSpawn)
                gem = RedGemInstance.instance.GetPooledObject();

            if (gem != null)
            {
                gem.transform.position = new Vector3(position.x, position.y, position.z);
                gem.SetActive(true);
                _amountSpawnedGems--;
                position.z -= _spaceBetweenGems;
            }
            else break;
        }
    }
}

using UnityEngine;

public class GemPickUp : MonoBehaviour
{
    [SerializeField] private GameObject _player;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Gem"))
        {
            other.gameObject.SetActive(false);
            Score.score.PlayerPickUpGem(other.gameObject);
        }
    }
}

using System.Collections.Generic;
using UnityEngine;


public class GreenGemInstance : MonoBehaviour
{
    public static GreenGemInstance instance;

    [SerializeField] private GameObject _gemPreFab;

    private List<GameObject> _pooledGems = new List<GameObject>();
    private int _gemsAmount = 25;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    void Start()
    {
        for (int i = 0; i < _gemsAmount; i++)
        {
            GameObject obj = Instantiate(_gemPreFab);
            obj.SetActive(false);
            _pooledGems.Add(obj);
        }
    }

    public GameObject GetPooledObject()
    {
        for (int i = 0; i < _pooledGems.Count; i++)
            if (!_pooledGems[i].activeInHierarchy)
                return _pooledGems[i];
        return null;
    }
}

using System.Collections.Generic;
using UnityEngine;


public class RedGemInstance : MonoBehaviour
{
    public static RedGemInstance instance;

    [SerializeField] private GameObject _gemPreFab;

    private List<GameObject> _pooledGems = new List<GameObject>();
    private int _gemsAmount = 30;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    void Start()
    {
        for (int i = 0; i < _gemsAmount; i++)
        {
            GameObject obj = Instantiate(_gemPreFab);
            obj.SetActive(false);
            _pooledGems.Add(obj);
        }
    }

    public GameObject GetPooledObject()
    {
        for (int i = 0; i < _pooledGems.Count; i++)
            if (!_pooledGems[i].activeInHierarchy)
                return _pooledGems[i];
        return null;
    }
}

using System;
using TMPro;
using UnityEngine;

public class GetData : MonoBehaviour
{
    private TextMeshProUGUI _highScoreText;
    private TextMeshProUGUI _redGemCountText;
    private TextMeshProUGUI _greenGemCountText;
    private TextMeshProUGUI _blueGemCountText;

    void Start()
    {
        _highScoreText = GameObject.Find("Score Count").GetComponent<TextMeshProUGUI>();
        _highScoreText.text = "High Score: " + Math.Round(PlayerPrefs.GetFloat("highScore")).ToString();
        _redGemCountText = GameObject.Find("Red Gem Count").GetComponent<TextMeshProUGUI>();
        _redGemCountText.text = PlayerPrefs.GetInt("redGemCount").ToString();
        _greenGemCountText = GameObject.Find("Green Gem Count").GetComponent<TextMeshProUGUI>();
        _greenGemCountText.text = PlayerPrefs.GetInt("greenGemCount").ToString();
        _blueGemCountText = GameObject.Find("Blue Gem Count").GetComponent<TextMeshProUGUI>();
        _blueGemCountText.text = PlayerPrefs.GetInt("blueGemCount").ToString();
    }


}


using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    public void ChangeScene(string sceneName)
    {
        switch (sceneName)
        {
            case "Gameplay": SceneManager.LoadScene("GamePlay"); break;
            case "MainMenu": SceneManager.LoadScene("MainMenu"); break;
        }
    }
    public void Exit()
    {
        Application.Quit();
    }
}

using UnityEngine;
using TMPro;

public class TitleOpenVFX : MonoBehaviour
{
    [SerializeField, Range(0.1f, 1f)] private float _speedAppear;
    private UnityEngine.UI.Button _button;

    private float _startValue = -0.8f;
    private float _localValue = 0f;
    private TextMeshProUGUI _titleText;
    private TextMeshProUGUI _playText;
    private bool _continue = false;

    void Start()
    {
        _titleText = GameObject.Find("Title").GetComponent<TextMeshProUGUI>();
        _playText = GameObject.Find("Play").GetComponent<TextMeshProUGUI>();
        _button = GameObject.Find("ButtonPlay").GetComponent<UnityEngine.UI.Button>();

        _titleText.fontMaterial.SetFloat(ShaderUtilities.ID_FaceDilate, _startValue);
        _playText.fontMaterial.SetFloat(ShaderUtilities.ID_FaceDilate, _startValue);

        _localValue = _startValue;

        _button.interactable = false;
    }

    void Update()
    {
        if (_continue == false)
            AppearTitle();
        else
            AppearOther();
    }

    void AppearTitle()
    {
        if (_localValue < -0.2f)
        {
            _titleText.fontMaterial.SetFloat(ShaderUtilities.ID_FaceDilate, _localValue);
            _localValue += _speedAppear * Time.deltaTime;
        }
        else
        {
            _continue = true;
            _localValue = _startValue;
        }
    }

    void AppearOther()
    {
        if (_localValue < -0.2f)
        {
            _playText.fontMaterial.SetFloat(ShaderUtilities.ID_FaceDilate, _localValue);
            _localValue += _speedAppear * Time.deltaTime;
        }
        else
            _button.interactable = true;
    }
}

using UnityEngine;
using UnityEngine.SceneManagement;

public class CollideWithObstacle : MonoBehaviour
{
    [SerializeField] private GameObject _player;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Obstacle"))
        {
            MapSettings.mapSettings.Speed = 0f;
            MapSettings.mapSettings.ScorePerSecond = 0f;

            if (Values.values.HighScore < Score.score.Scor)
                PlayerPrefs.SetFloat("highScore", Score.score.Scor);

            PlayerPrefs.SetInt("redGemCount", Values.values.RedGemCount + Score.score.RedGemCount);
            PlayerPrefs.SetInt("greenGemCount", Values.values.GreenGemCount + Score.score.GreenGemCount);
            PlayerPrefs.SetInt("blueGemCount", Values.values.BlueGemCount + Score.score.BlueGemCount);

            SceneManager.LoadScene("GameOver");
        }
    }
}

using UnityEngine;

public class ObstacleGenerator : MonoBehaviour
{
    private GameObject _obstacle;

    public static ObstacleGenerator instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    public void Generate(Vector3 position)
    {
        _obstacle = null;
        _obstacle = ObstacleInstance.instance.GetPooledObject();

        if (_obstacle != null)
        {
            _obstacle.transform.position = new Vector3(position.x, position.y, position.z);
            _obstacle.SetActive(true);
        }
    }
}

using System.Collections.Generic;
using UnityEngine;


public class ObstacleInstance : MonoBehaviour
{
    public static ObstacleInstance instance;

    [SerializeField] private GameObject _obstaclePreFab;

    private List<GameObject> _pooledObstacle = new List<GameObject>();
    private int _obstacleAmount = 20;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    void Start()
    {
        for (int i = 0; i < _obstacleAmount; i++)
        {
            GameObject obj = Instantiate(_obstaclePreFab);
            obj.SetActive(false);
            _pooledObstacle.Add(obj);
        }
    }

    public GameObject GetPooledObject()
    {
        for (int i = 0; i < _pooledObstacle.Count; i++)
            if (!_pooledObstacle[i].activeInHierarchy)
                return _pooledObstacle[i];
        return null;
    }
}
