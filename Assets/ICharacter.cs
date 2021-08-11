using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public interface ICharacter

{
    

     void QCF(PlayerInput pi, bool EX);
     void QCB(PlayerInput pi, bool EX);
     void BDP(PlayerInput pi, bool EX);
     void FDP(PlayerInput pi, bool EX);
     void HCF(PlayerInput pi, bool EX);
     void HCB(PlayerInput pi, bool EX);
     void Block(PlayerInput pi );
    void resetStrings();
     void Attack(PlayerInput pi);
     void Jump(PlayerInput pi );
     void Dasher(PlayerInput pi );
     void WSkill(PlayerInput pi );
     void ASkill(PlayerInput pi );
     void HSkill(PlayerInput pi );
     void TSkill(PlayerInput pi );
    void Walk(PlayerInput pi);
      }


   



