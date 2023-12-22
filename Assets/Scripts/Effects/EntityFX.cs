using System.Collections;
using TMPro;
using UnityEngine;

public class EntityFX : MonoBehaviour {
  protected Player player;
  protected SpriteRenderer sr;

  [Header("Pop Up Text")]
  [SerializeField] private GameObject popUpTextPrefab;


  [Header("flash FX")]
  [SerializeField] private float flashDuration;
  [SerializeField] private Material hitMat;
  private Material originalMat;

  [Header("Aliment colors")]
  [SerializeField] private Color[] igniteColor;
  [SerializeField] private Color[] chillColor;
  [SerializeField] private Color[] shockColor;

  [Header("Aliment particles")]
  [SerializeField] private ParticleSystem igniteFx;
  [SerializeField] private ParticleSystem chillFx;
  [SerializeField] private ParticleSystem shockFx;


  [Header("Hit FX")]
  [SerializeField] private GameObject hitFx;
  [SerializeField] private GameObject criticalHitFx;


  protected virtual void Start() {
    sr = GetComponentInChildren<SpriteRenderer>();
    player = PlayerManager.instance.player;
    originalMat = sr.material;
  }


  public void CreatePopUpText(string _text) {
    float randomX = Random.Range(-1, 1);
    float randomY = Random.Range(1, 5);
    Vector3 positionOffset = new Vector3(randomX, randomY, 0);
    GameObject newText = Instantiate(popUpTextPrefab, transform.position + positionOffset, Quaternion.identity);

    newText.GetComponent<TextMeshPro>().text = _text;


  }

  public void MakeTransparent(bool _transparent) {
    if (_transparent) {
      sr.color = Color.clear;
    }
    else {
      sr.color = Color.white;
    }
  }

  private IEnumerator FlashFX() {
    sr.material = hitMat;
    Color currentColor = sr.color;
    sr.color = Color.white;
    yield return new WaitForSeconds(flashDuration);

    sr.color = currentColor;
    sr.material = originalMat;
  }

  private void RedColorBlink() {
    if (sr.color != Color.white)
      sr.color = Color.white;
    else
      sr.color = Color.red;
  }

  private void CancelColorChange() {
    CancelInvoke();
    sr.color = Color.white;

    igniteFx.Stop();
    chillFx.Stop();
    shockFx.Stop();
  }

  public void IgniteFxFor(float _seconds) {
    igniteFx.Play();
    InvokeRepeating("IgniteColorFx", 0, .3f);
    Invoke("CancelColorChange", _seconds);
  }
  public void ChillFxFor(float _seconds) {
    chillFx.Play();
    InvokeRepeating("ChillColorFx", 0, .3f);
    Invoke("CancelColorChange", _seconds);
  }
  public void ShockFxFor(float _seconds) {
    shockFx.Play();
    InvokeRepeating("ShockColorFx", 0, .3f);
    Invoke("CancelColorChange", _seconds);
  }

  private void IgniteColorFx() {
    if (sr.color != igniteColor[0]) sr.color = igniteColor[0];
    else sr.color = igniteColor[1];
  }

  private void ChillColorFx() {
    if (sr.color != chillColor[0]) sr.color = chillColor[0];
    else sr.color = chillColor[1];
  }

  private void ShockColorFx() {
    if (sr.color != shockColor[0]) sr.color = shockColor[0];
    else sr.color = shockColor[1];
  }

  public void CreateHitFx(Transform _target, bool _critical) {

    float zRotation = Random.Range(-90, 90);
    float xPosition = Random.Range(-.5f, .5f);
    float yPosition = Random.Range(-.5f, .5f);

    Vector3 hitFxRotation = new Vector3(0, 0, zRotation);
    GameObject hitPrefab = hitFx;
    if (_critical) {
      hitPrefab = criticalHitFx;
      float yRotaion = 0;
      zRotation = Random.Range(-45, 45);

      if (GetComponent<Entity>().facingDir == -1) {
        yRotaion = 180;
      }
      hitFxRotation = new Vector3(0, yRotaion, zRotation);
    }
    GameObject newHitFx = Instantiate(hitPrefab, _target.position + new Vector3(xPosition, yPosition), Quaternion.identity); //_target);

    newHitFx.transform.Rotate(hitFxRotation);

    Destroy(newHitFx, .5f);
  }
}
