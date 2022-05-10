using System.Collections.Generic;
using UnityEngine;

public class Atmosphere : MonoBehaviour
{
  public Transform target;
  public Transform atmos;
  public Material atmosMat;
  public Transform dl;

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

    // Day/Night cycle processing
    var up = Vector3.Normalize(transform.position - target.position);
    var dla = dl.forward;
    var angle = (Vector3.Angle(up, dla) - 60f) / 160f; // 0 to 1, 1 is sun up

    RenderSettings.skybox.SetFloat("_SunAngle", angle);

    // Ambient color
    RenderSettings.ambientLight = Color.Lerp(c, Color.black, 
      Mathf.Clamp((distance - 10000f) / 1000f, 0, 1)
    );
  }
}
