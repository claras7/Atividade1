using System;
using UnityEngine;

public static class PlayerObserverManager
{
   //criar o canal
   public static event Action<int> OnMoedasChanged;
   
   // criar o "postar videos"
   public static void ChangedMoedas(int moedas)
   {
      // verifica se tem inscritos e manda notificação
      OnMoedasChanged?.Invoke(moedas);
   }
}
