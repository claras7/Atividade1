using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class CircleController : MonoBehaviour
{
   private SpriteRenderer _spriteRenderer;
   [Header("Listening to...")] 
   public VoidEventChannel circleColorEvent;

   private void Awake()
   {
      _spriteRenderer = GetComponent<SpriteRenderer>();
   }

   private void OnEnable()
   {
      circleColorEvent.OnEventRaised += MudaCor;
   }

   private void OnDisable()
   {
      circleColorEvent.OnEventRaised -= MudaCor;
   }

   public void MudaCor()
   {
      _spriteRenderer.color = Random.ColorHSV();
   }
}
