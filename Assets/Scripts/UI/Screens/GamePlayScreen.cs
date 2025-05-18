using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePlayScreen : ScreenBase<GamePlayScreen, GamePlayScreenDataProvider>
{
    [SerializeField] private Button _closeButton;
    [SerializeField] private GridLayoutGroup _gridLayoutGroup;
    [SerializeField] private GameObject _stage;
    [SerializeField] private RectTransform _stageRect;

    [SerializeField] private Lamp _lampPrefab;
    [SerializeField] private Vector2 _lampDimensions;

    [SerializeField] private GameTimer _gameTimer;
    [SerializeField] private MovesCounter _movesCounter;

    private Lamp[,] _grid;
    private int _litCount;

    public override void Initialize(GamePlayScreenDataProvider provider)
    {
        _closeButton.SafeAddClickListener(ExitGamePlay);

        GenerateMap();
        StartGame();
    }

    private void StartGame()
    {
        RandomizeMap();

        _gameTimer.SafeResetTimer();
        _gameTimer.SafeStartTimer();

        _movesCounter.SafeReset();
    }

    private void EndGame()
    {
        _gameTimer.SafeStopTimer();
        ConfirmationPopupDataProvider dataProvider = new("You win!", "Replay", StartGame);
        UIManager.Instance.Open(dataProvider);
    }

    private void GenerateMap()
    {
        ClearMap();

        _grid = new Lamp[DataProvider.GridSize, DataProvider.GridSize];
        _gridLayoutGroup.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        _gridLayoutGroup.constraintCount = DataProvider.GridSize;
        _stageRect.sizeDelta = new Vector2(DataProvider.GridSize * _lampDimensions.x, DataProvider.GridSize * _lampDimensions.y);

        for (var row = 0; row < DataProvider.GridSize; row++)
        {
            for (var col = 0; col < DataProvider.GridSize; col++)
            {
                if (Instantiate(_lampPrefab, _stage.transform) is Lamp instance)
                {
                    instance.OnStateChanged += OnLampStateChanged;
                    instance.OnClicked += OnLampClicked;
                    _grid[row, col] = instance;
                }
            }
        }

        // adding neighbours
        for (int row = 0; row < DataProvider.GridSize; row++)
        {
            for (int col = 0; col < DataProvider.GridSize; col++)
            {
                if (_grid[row, col] is Lamp current)
                { 
                    if (col + 1 < DataProvider.GridSize)
                    {
                        current.AddNeighbour(_grid[row, col + 1]);
                    }

                    if (col - 1 >= 0)
                    {
                        current.AddNeighbour(_grid[row, col - 1]);
                    }

                    if (row - 1 >= 0)
                    {
                        current.AddNeighbour(_grid[row - 1, col]);
                    }

                    if (row + 1 < DataProvider.GridSize)
                    {
                        current.AddNeighbour(_grid[row + 1, col]);
                    }
                }
            }
        }
    }

    private void RandomizeMap()
    {
        if (_grid == null) return;

        // force all lamps to be off to ensure that it's solvable
        foreach (var lamp in _grid)
        {
            if (lamp == null) continue;
            lamp.SetState(false, false, false);
        }

        _litCount = 0;

        HashSet<Vector2Int> uniques = new();
        for (var i = 0; i < DataProvider.NumRandomizations; i++)
        {
            int randRow = Random.Range(0, DataProvider.GridSize - 1);
            int randCol = Random.Range(0, DataProvider.GridSize - 1);
            uniques.Add(new Vector2Int(randRow, randCol));
        }

        foreach (var position in uniques)
        {
            if (_grid[position.x, position.y] is Lamp valid)
            { 
                valid.Flip();
            }
        }
    }

    private void ClearMap()
    {
        if (_grid == null) return;

        foreach(var lamp in _grid)
        {
            if (lamp != null)
            {
                Destroy(lamp.gameObject);
            }
        }
        _grid = null;
        _litCount = 0;
    }

    private void ExitGamePlay()
    {
        SceneUtils.LoadScene(SceneUtils.GameScene.MainMenu);
    }

    private void OnLampClicked(Lamp lamp)
    {
        if (lamp == null) return;
        
        _movesCounter.SafeIncrement();
        lamp.Flip();

        // edge case ( though I don't think it's actually possible ) for if the lamp getting triggered is the last one
        // which causes lit count to be 0, but there are other changed events incoming that bumps it back to > 0
        StartCoroutine(VerifyGameState());
    }

    private void OnLampStateChanged(Lamp lamp)
    {
        if (lamp == null) return;

        _litCount += lamp.State ? 1 : -1;
    }

    private IEnumerator VerifyGameState()
    {
        yield return null;
        if (_litCount == 0)
        {
            EndGame();
        }
    }

}

public class GamePlayScreenDataProvider : ScreenDataProvider
{
    public int GridSize;
    public int NumRandomizations;

    public GamePlayScreenDataProvider(ScreenParams data) : base(data)
    {
    }
}
