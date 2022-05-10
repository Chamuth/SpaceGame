using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public enum OcclusionState
{
  Landed,
  Overhead,
  Distant
}

public struct PlanetTile
{
  public Vector3 center;
  public Renderer[] renderers;
}

public class DynamicOcclusion : MonoBehaviour
{
  [Header("Connections")]
  public Transform Player;

  [Header("Distances/Limits")]
  public float OverheadDistance = 2500f;
  public float LandedDistance = 750f;

  [Header("Settings")]
  public float UpdateStepDistance = 2f;

  [Header("Cull Counts")]
  public int OverheadCount = 160;
  public int LandedCount = 200;

  OcclusionState _State = OcclusionState.Distant;
  List<PlanetTile> _tiles = new List<PlanetTile>();
  Vector3 previousPosition = Vector3.zero;

  private void Start()
  {
    // process the tiles
    foreach (Transform child in transform)
    {
      if (child.name == "Atmosphere")
      {
        continue;
      }

      var renderers = child.gameObject.GetComponentsInChildren<Renderer>();
      _tiles.Add(new PlanetTile
      {
        center = renderers[0].bounds.center,
        renderers = renderers
      });
    }
  }

  void ProcessOcclusion(OcclusionState state)
  {
    if (_State == state && state == OcclusionState.Distant)
    {
      return;
    }

    _State = state;
    ResetTiles();

    if (state == OcclusionState.Overhead)
    {
      HidePercentage(OverheadCount);
    }

    if (state == OcclusionState.Landed)
    {
      HidePercentage(LandedCount);
    }
  }

  void ResetTiles()
  {
    _tiles.ToList().ForEach(t => ToggleTile(t, true));
  }

  void HidePercentage(int count)
  {
    var tiles = _tiles.OrderBy(x => Vector3.Distance(Player.position, x.center));
    var half = tiles.Skip(tiles.Count() - count);
    half.ToList().ForEach(t => ToggleTile(t, false));
  }

  void ToggleTile(PlanetTile tile, bool toggle)
  {
    foreach (Renderer r in tile.renderers)
      r.enabled = toggle;
  }

  void LateUpdate()
  {
    var distance = Vector3.Distance(transform.position, Player.position) - 10000;

    if (Vector3.Distance(Player.position, previousPosition) < UpdateStepDistance)
    {
      return;
    }

    previousPosition = transform.position;

    if (distance < LandedDistance)
    {
      ProcessOcclusion(OcclusionState.Landed);
      return;
    }

    if (distance < OverheadDistance)
    {
      ProcessOcclusion(OcclusionState.Overhead);
      return;
    }

    if (distance > OverheadDistance)
    {
      ProcessOcclusion(OcclusionState.Distant);
      return;
    }
  }
}
