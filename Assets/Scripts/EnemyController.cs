using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MovingObject {

    public Enemy enemy;

    #region Methods

    public void HitEvent()
    {
        /* go into the queque following the player
         * or explode
         * or fall from the cube
         * or leave the scene
         */
        Destroy(gameObject);
    }

    #endregion

}
