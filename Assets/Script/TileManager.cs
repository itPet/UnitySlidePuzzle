using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour {

    public List<TileController> tiles;

    public void TilePressed(TileController tile) {
        Debug.Log("Tile pressed: " + tile.gameObject.name);
        Debug.Log("Test. Remove this line");
        //More test stuff
    }



	
}
