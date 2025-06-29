using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class hand : MonoBehaviour
{
 [Serializable]
 public class Par
 {
  public PowerEnum _powerEnum;
  public Sprite _sprite;
 }
 [SerializeField] private SpriteRenderer _renderer;
 [SerializeField] private SpriteRenderer _spriteRenderer;
 public List< Par> pars;
 private Dictionary<PowerEnum, Sprite> paDic = new Dictionary<PowerEnum, Sprite>();
 private void Start()
 {
  PowerManager.Instance.OnChangePower += Change;
  foreach (var lPar in pars)
  {
   paDic.Add(lPar._powerEnum, lPar._sprite);
  }
 }
 private void Change(PowerEnum obj)
 {
  Debug.Log( obj);
  _spriteRenderer.sprite = paDic[obj];
  switch (obj)
  {

   case PowerEnum.Null:
    break;
   case PowerEnum.Water:
    DOTween.To(() => _spriteRenderer.color, x => _spriteRenderer.color = x, Color.cyan, 1);
    break;
   case PowerEnum.Fire:
    DOTween.To(() => _spriteRenderer.color, x => _spriteRenderer.color = x, Color.red, 1);
    break;

  }
 }
}
