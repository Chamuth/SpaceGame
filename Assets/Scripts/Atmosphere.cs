using UnityEngine;

public class Atmosphere : MonoBehaviour
{
  public Transform target;
  public Transform atmos;
  public Material atmosMat;

  private Color c = new Color(0.416f, 0.446f, 0.528f);

  void Update()
  {
    var distance = Vector3.Distance(transform.position, target.position);

    RenderSettings.skybox.SetFloat("_SpaceOffset",
      Mathf.Clamp((distance - 10000f) / 1000f, 0, 1)
    );

    atmosMat.SetFloat("_SpaceOffset", Mathf.Clamp(
      (Mathf.Abs(Vector3.Distance(atmos.position, transform.position)) - (atmos.localScale.x * 5)) / 300f
    , 0, 1));

    RenderSettings.ambientLight = Color.Lerp(c, Color.black, Mathf.Clamp((distance - 10000f) / 1000f, 0, 1));
  }
}
