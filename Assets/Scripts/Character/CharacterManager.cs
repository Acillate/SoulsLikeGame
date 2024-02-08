using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
   protected virtual void Awake() 
   {
      DontDestroyOnLoad(this);
   }

   protected virtual void Update() {
      
   }
}