using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using R3;
using Unity.VisualScripting;
using UnityEngine;
using WhaleTee.Reactive.Runtime.Input;
using WhaleTee.Runtime;
using WhaleTee.Runtime.ServiceLocator;
using WhaleTee.Runtime.Extensions;

namespace Match3Project.Scripts.Merge3 {
  public class Match3Level : MonoBehaviour {
    [SerializeField] private LevelData levelData;
    [SerializeField] private Transform gemVisual;
    [SerializeField] private Transform cameraTransform;

    private UserInput userInput;
    private Grid2X2<CellVisual> grid;
    private CellVisual pickupCell;
    private bool trySwapGems;

    private void Awake() {
      ServiceLocator.Global.TryGet(out userInput);
    }

    private void Start() {
      grid = new Grid2X2<CellVisual>(
        levelData.Width,
        levelData.Height,
        1f,
        Vector3.zero,
        (grid, x, y) => new CellVisual(grid, x, y)
      );

      if (levelData.Cells.Count != levelData.Width * levelData.Height) {
        // Create new Level
        Debug.LogError("Level incomplete. Create level data first.");
      } else {
        // Load Level
        Debug.Log("Loading level...");

        levelData.Cells.ForEach(cell => CreateVisual(grid.GetCell(cell.GetX(), cell.GetY()), cell));
      }

      cameraTransform.position = new Vector3(grid.Width * .5f, grid.Height * .5f, cameraTransform.position.z);

      userInput.Click.Subscribe(
        click => {
          if (trySwapGems) return;
      
          if (!click) {
            pickupCell = null;
            return;
          }
      
          var mouseWorldPosition = userInput.GetMousePositionWorld().With(z: 0f);
      
          grid.GetXY(mouseWorldPosition, out var x, out var y);
          var cell = grid.GetCell(x, y);
      
          if (cell == null) return;
      
          if (cell.IsMouseInBounds(mouseWorldPosition)) pickupCell = cell;
        }
      );
    }

    private void Update() {
      if (pickupCell == null) return;

      var mouseWorldPosition = userInput.GetMousePositionWorld().With(z: 0f);

      grid.GetXY(mouseWorldPosition, out var x, out var y);
      var cell = grid.GetCell(x, y);

      if (cell == null) return;

      if (cell.IsMouseInBounds(mouseWorldPosition)) {
        if (pickupCell != cell) {
          if ((pickupCell.GetWorldPosition() - cell.GetWorldPosition()).magnitude <= 1f) {
            Debug.Log("Swapping gems...");
            StartCoroutine(TrySwapGems(cell, pickupCell));
            pickupCell = null;
          }
        }
      }
    }

    private IEnumerator TrySwapGems(CellVisual a, CellVisual b) {
      trySwapGems = true;
      yield return SwapGemsInternal(a, b);

      if (MatchFinder.HasMatches(grid, a.GetX(), a.GetY()) || MatchFinder.HasMatches(grid, b.GetX(), b.GetY())) {
        Debug.Log("Match found!");
        DestroyAndInstantiateRandomGemsAtMatches(MatchFinder.GetMatches(grid, a.GetX(), a.GetY())); 
        DestroyAndInstantiateRandomGemsAtMatches(MatchFinder.GetMatches(grid, b.GetX(), b.GetY()));
      } else {
        Debug.Log("No match found!");
        yield return SwapGemsInternal(a, b);
      }

      trySwapGems = false;
    }

    private void DestroyAndInstantiateRandomGemsAtMatches(List<CellVisual> matches) {
      if (matches == null || matches.Count == 0) return;
      
      foreach (var match in matches) {
        ReCreateVisual(grid.GetCell(match.GetX(), match.GetY()));
      }
    }

    private IEnumerator SwapGemsInternal(CellVisual a, CellVisual b) {
      a.GetXY(out var ax, out var ay);
      b.GetXY(out var bx, out var by);

      var aPosition = new Vector2Int(ax, ay);
      var bPosition = new Vector2Int(bx, by);

      grid.SwapCells(aPosition, bPosition);

      a.Transform.DOLocalMove(grid.GetWorldPosition(bx, by), 0.35f);
      b.Transform.DOLocalMove(grid.GetWorldPosition(ax, ay), 0.35f);

      yield return new WaitForSeconds(0.35f);
    }

    private void CreateVisual(CellVisual cellVisual, Cell levelCell) {
      if (levelCell == null) return;

      var gemVisualTransform = Instantiate(gemVisual, cellVisual.GetWorldPosition(), Quaternion.identity);

      cellVisual.Transform = gemVisualTransform;
      cellVisual.MonoBehaviour = gemVisualTransform.AddComponent<EmptyMonoBehaviour>();
      cellVisual.SpriteRenderer = gemVisualTransform.GetComponent<SpriteRenderer>();

      cellVisual.SetGem(levelCell.GetGem());
    }

    private void ReCreateVisual(CellVisual cellVisual) {
      if (cellVisual == null) return;

      Destroy(cellVisual.Transform.gameObject);
      
      var gemVisualTransform = Instantiate(gemVisual, cellVisual.GetWorldPosition(), Quaternion.identity);

      cellVisual.Transform = gemVisualTransform;
      cellVisual.MonoBehaviour = gemVisualTransform.AddComponent<EmptyMonoBehaviour>();
      cellVisual.SpriteRenderer = gemVisualTransform.GetComponent<SpriteRenderer>();

      cellVisual.SetGem(new Gem(levelData.GemList[Random.Range(0, levelData.GemList.Count)]));
    }

    private class CellVisual : Cell {
      public MonoBehaviour MonoBehaviour { get; set; }
      public Transform Transform { get; set; }
      public SpriteRenderer SpriteRenderer { get; set; }
      private Gem gem;

      private readonly Grid2X2<CellVisual> grid;
      private int x, y;

      public CellVisual(Grid2X2<CellVisual> grid, int x, int y) {
        this.grid = grid;
        this.x = x;
        this.y = y;
      }

      public int GetX() => x;

      public int GetY() => y;

      public void GetXY(out int x, out int y) {
        x = this.x;
        y = this.y;
      }

      public void SetXY(int x, int y) {
        this.x = x;
        this.y = y;
      }

      public void SetGem(Gem gem) {
        SpriteRenderer.sprite = gem.GetGemData().Sprite;
        this.gem = gem;
      }

      public Gem GetGem() => gem;

      public Vector3 GetWorldPosition() => grid.GetWorldPosition(x, y);

      public bool IsMouseInBounds(Vector2 mouseWorldPosition) {
        var worldPosition = GetWorldPosition();
        var bounds = new Bounds(worldPosition, Vector3.one * grid.CellSize);
        return bounds.Contains(mouseWorldPosition);
      }
    }
  }
}