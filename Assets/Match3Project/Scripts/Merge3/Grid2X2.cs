using System;
using UnityEngine;

namespace Match3Project.Scripts.Merge3 {
  public class Grid2X2<TCell> where TCell : Cell {
    public event Action<GemDestroyedEvent> OnGemDestroyed;
    public int Width { get; }
    public int Height { get; }
    public float CellSize { get; }
    private readonly TCell[,] cells;
    private readonly Vector3 originPosition;

    public Grid2X2(
      int width, int height, float cellSize, Vector3 originPosition,
      Func<Grid2X2<TCell>, int, int, TCell> createCell
    ) {
      Width = width;
      Height = height;
      CellSize = cellSize;
      this.originPosition = originPosition;
      cells = new TCell[width, height];

      for (var x = 0; x < cells.GetLength(0); x++) {
        for (var y = 0; y < cells.GetLength(1); y++) {
          cells[x, y] = createCell(this, x, y);
        }
      }
    }

    public Vector3 GetWorldPosition(int x, int y) => new Vector3(x, y) * CellSize + originPosition;

    public void SwapCells(Vector2Int aPosition, Vector2Int bPosition) {
      var a = GetCell(aPosition.x, aPosition.y);
      var b = GetCell(bPosition.x, bPosition.y);

      if (a == null || b == null) return;

      cells[aPosition.x, aPosition.y] = b;
      cells[bPosition.x, bPosition.y] = a;

      a.SetXY(bPosition.x, bPosition.y);
      b.SetXY(aPosition.x, aPosition.y);
    }

    public void DestroyGemInCell(Vector2Int position) {
      if (IsWithinBounds(position.x, position.y)) {
        OnGemDestroyed?.Invoke(new GemDestroyedEvent { x = position.x, y = position.y });
        cells[position.x, position.y].SetGem(null);
      }
    }

    public void TryInsertCell(Vector2Int position, TCell cell) {
      if (IsWithinBounds(position.x, position.y) && cells[position.x, position.y] == null) {
        cells[position.x, position.y] = cell;
      }
    }

    public void DestroyCell(Vector2Int position) {
      if (IsWithinBounds(position.x, position.y)) {
        OnGemDestroyed?.Invoke(new GemDestroyedEvent { x = position.x, y = position.y });
        cells[position.x, position.y] = default;
      }
    }

    public void GetXY(Vector3 worldPosition, out int x, out int y) {
      // fix for crossing the origin position
      x = Mathf.RoundToInt((worldPosition - originPosition).x / CellSize);
      y = Mathf.RoundToInt((worldPosition - originPosition).y / CellSize);
    }

    public TCell GetCell(int x, int y) => IsWithinBounds(x, y) ? cells[x, y] : default;

    public TCell GetCell(Vector3 worldPosition) {
      GetXY(worldPosition, out var x, out var y);
      return GetCell(x, y);
    }

    public bool IsWithinBounds(int x, int y) => x >= 0 && y >= 0 && x < Width && y < Height;
  }
}