using UnityEditor;
using UnityEngine;
using WhaleTee.Reactive.Runtime.Input;
using WhaleTee.Runtime.Extensions;
using WhaleTee.Runtime.ServiceLocator;
using Random = UnityEngine.Random;

namespace Match3Project.Scripts.Merge3 {
  public class Match3LevelEditor : MonoBehaviour {
    [SerializeField] private LevelData levelData;
    [SerializeField] private Transform gemVisual;
    [SerializeField] private Transform cameraTransform;

    private UserInput userInput;
    private Grid2X2<CellVisual> grid;

    private void Awake() {
      ServiceLocator.Global.TryGet(out userInput);
    }

    private void Start() {
      grid = new Grid2X2<CellVisual>(
        levelData.Width,
        levelData.Height,
        1f,
        Vector3.zero,
        (grid, x, y) => new CellVisual(levelData, grid, x, y)
      );

      if (levelData.Cells.Count != levelData.Width * levelData.Height) {
        // Create new Level
        Debug.Log("Creating new level...");
        levelData.Cells.Clear();

        for (var x = 0; x < grid.Width; x++) {
          for (var y = 0; y < grid.Height; y++) {
            var cell = new LevelDataCell(x, y);
            cell.SetGem(new Gem(levelData.GemList[Random.Range(0, levelData.GemList.Count)]));
            levelData.Cells.Add(cell);
          }
        }
      }

      // Load Level
      Debug.Log("Loading level...");

      levelData.Cells.ForEach(cell => CreateVisual(grid.GetCell(cell.GetX(), cell.GetY()), cell));

      cameraTransform.position = new Vector3(grid.Width * .5f, grid.Height * .5f, cameraTransform.position.z);
    }

    private void Update() {
      var mouseWorldPosition = userInput.GetMousePositionWorld().With(z: 0f);

      grid.GetXY(mouseWorldPosition, out var x, out var y);

      if (grid.IsWithinBounds(x, y)) {
        var cell = grid.GetCell(x, y);

        if (cell != null) {
          if (cell.IsMouseInBounds(mouseWorldPosition)) {
            if (Input.GetKeyDown(KeyCode.Alpha1)) cell.SetGem(new Gem(levelData.GemList[0]));
            if (Input.GetKeyDown(KeyCode.Alpha2)) cell.SetGem(new Gem(levelData.GemList[1]));
            if (Input.GetKeyDown(KeyCode.Alpha3)) cell.SetGem(new Gem(levelData.GemList[2]));
          }
        }
      }
    }

    private void CreateVisual(CellVisual cellVisual, LevelDataCell levelCell) {
      if (levelCell.GetGem() == null) return;

      var gemVisualTransform = Instantiate(gemVisual, cellVisual.GetWorldPosition(), Quaternion.identity);

      cellVisual.SpriteRenderer = gemVisualTransform.GetComponent<SpriteRenderer>();
      cellVisual.Cell = levelCell;

      cellVisual.SetGem(levelCell.GetGem());
    }

    private class CellVisual : Cell {
      public SpriteRenderer SpriteRenderer { get; set; }
      public LevelDataCell Cell { get; set; }

      private readonly LevelData levelData;
      private readonly Grid2X2<CellVisual> grid;
      private int x, y;

      public CellVisual(LevelData levelData, Grid2X2<CellVisual> grid, int x, int y) {
        this.levelData = levelData;
        this.grid = grid;
        this.x = x;
        this.y = y;
      }

      public int GetX() => x;

      public int GetY() => y;
    
      public void SetXY(int x, int y) {
        this.x = x;
        this.y = y;
      }

      public void SetGem(Gem gem) {
        SpriteRenderer.sprite = gem.GetGemData().Sprite;
        Cell.SetGem(gem);
        #if UNITY_EDITOR
        EditorUtility.SetDirty(levelData);
        #endif
      }

      public Gem GetGem() => Cell.GetGem();

      public Vector3 GetWorldPosition() => grid.GetWorldPosition(x, y);

      public bool IsMouseInBounds(Vector2 mouseWorldPosition) {
        var worldPosition = GetWorldPosition();
        var bounds = new Bounds(worldPosition, Vector3.one * grid.CellSize);
        return bounds.Contains(mouseWorldPosition);
      }
    }
  }
}