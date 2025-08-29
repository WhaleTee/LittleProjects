using System;
using System.Collections.Generic;
using System.Linq;

namespace Match3Project.Scripts.Merge3 {
  public static class MatchFinder {
    public static bool HasMatches<TCell>(Grid2X2<TCell> grid, int x, int y) where TCell : Cell => GetMatches(grid, x, y) is { Count: >= 3 };

    public static List<TCell> GetMatches<TCell>(Grid2X2<TCell> grid, int x, int y) where TCell : Cell {
      var gem = grid.GetCell(x, y).GetGem();

      if (gem == null) return null;

      return HorizontalMatch(grid, x, y, gem) ?? VerticalMatch(grid, x, y, gem);
    }

    public static List<List<TCell>> GetAllMatches<TCell>(Grid2X2<TCell> grid) where TCell : Cell {
      var allMatches = new List<List<TCell>>();

      for (var x = 0; x < grid.Width; x++) {
        for (var y = 0; y < grid.Height; y++) {
          if (HasMatches(grid, x, y)) {
            var matches = GetMatches(grid, x, y);

            if (allMatches.Count == 0) {
              allMatches.Add(matches);
            } else {
              var unique = allMatches.All(m => m.Count != matches.Count || !m.All(c => matches.Contains(c)));
              if (unique) allMatches.Add(matches);
            }
          }
        }
      }

      return allMatches;
    }

    public static List<TCell> GetMatchesTShape<TCell>(Grid2X2<TCell> grid, int x, int y) where TCell : Cell {
      throw new NotImplementedException();
    }

    private static int GetRightMatches<TCell>(Grid2X2<TCell> grid, int x, int y, Gem gem) where TCell : Cell {
      var matches = 0;

      for (var i = 1; grid.IsWithinBounds(x + i, y); i++) {
        if (grid.GetCell(x + i, y).GetGem() == gem) matches++;
        else break;
      }

      return matches;
    }

    private static int GetLeftMatches<TCell>(Grid2X2<TCell> grid, int x, int y, Gem gem) where TCell : Cell {
      var matches = 0;

      for (var i = 1; grid.IsWithinBounds(x - i, y); i++) {
        if (grid.GetCell(x - i, y).GetGem() == gem) matches++;
        else break;
      }

      return matches;
    }

    private static List<TCell> HorizontalMatch<TCell>(Grid2X2<TCell> grid, int x, int y, Gem gem) where TCell : Cell {
      var rightMatches = GetRightMatches(grid, x, y, gem);
      var leftMatches = GetLeftMatches(grid, x, y, gem);
      var horizontalMatches = 1 + leftMatches + rightMatches;

      if (horizontalMatches >= 3) {
        var horizontalMatch = new List<TCell>();
        var startX = x - leftMatches;

        for (var i = 0; i < horizontalMatches; i++) horizontalMatch.Add(grid.GetCell(startX + i, y));

        return horizontalMatch;
      }

      return null;
    }

    private static int GetUpMatches<TCell>(Grid2X2<TCell> grid, int x, int y, Gem gem) where TCell : Cell {
      var matches = 0;

      for (var i = 1; grid.IsWithinBounds(x, y + i); i++) {
        if (grid.GetCell(x, y + i).GetGem() == gem) matches++;
        else break;
      }

      return matches;
    }

    private static int GetDownMatches<TCell>(Grid2X2<TCell> grid, int x, int y, Gem gem) where TCell : Cell {
      var matches = 0;

      for (var i = 1; grid.IsWithinBounds(x, y - i); i++) {
        if (grid.GetCell(x, y - i).GetGem() == gem) matches++;
        else break;
      }

      return matches;
    }

    private static List<TCell> VerticalMatch<TCell>(Grid2X2<TCell> grid, int x, int y, Gem gem) where TCell : Cell {
      var upMatches = GetUpMatches(grid, x, y, gem);
      var downMatches = GetDownMatches(grid, x, y, gem);
      var verticalMatches = 1 + downMatches + upMatches;

      if (verticalMatches >= 3) {
        var horizontalMatch = new List<TCell>();
        var startY = y - downMatches;

        for (var i = 0; i < verticalMatches; i++) horizontalMatch.Add(grid.GetCell(x, startY + i));

        return horizontalMatch;
      }

      return null;
    }
  }
}