using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour {

    int rowSize; //same as private
    public float shuffleSpeed = 0.05f;
    public float moveSpeed = 0.2f;
    public int shuffleCount = 1;
    public List<TileController> tiles;
    bool inputLock = false;

    private void Start() {
        rowSize = (int) Mathf.Sqrt(tiles.Count);
        StartCoroutine(Shuffle());
    }

    IEnumerator Shuffle() {
        for (int i = 0; i < shuffleCount;) {
            if (TilePressed(tiles[Random.Range(0, tiles.Count)], true)){
                i++;
                yield return new WaitForSeconds(shuffleSpeed);
            }
        }
    }

    public bool TilePressed(TileController tile, bool shuffle) {
        if (inputLock)
            return false;

        TileController emptyNeighbour = EmptyNeighbour(tile);
        if (emptyNeighbour == null)
            return false;

        float speed = shuffle ? shuffleSpeed : moveSpeed;
        SwitchTiles(tile, emptyNeighbour, speed);
        return true;
    }

    void SwitchTiles(TileController tile1, TileController tile2, float speed) {
        inputLock = true;

        //Switch fysical objects and animate
        Vector3 pos = tile1.gameObject.transform.position;

        LeanTween.move(tile1.gameObject, tile2.gameObject.transform.position, speed)
                 .setEase(LeanTweenType.easeInCubic)
                 .setOnComplete(() => { 
                    inputLock = false; 
        });
        LeanTween.move(tile2.gameObject, pos, speed).
                 setEase(LeanTweenType.easeInCubic);
        //tile1.gameObject.transform.position = tile2.gameObject.transform.position;
        //tile2.gameObject.transform.position = pos;

        //Switch in list
        int index1 = TileIndex(tile1);
        int index2 = TileIndex(tile2);

        tiles[index1] = tile2;
        tiles[index2] = tile1;
    }

    TileController EmptyNeighbour(TileController tile) {
        foreach( TileController t in Neighbours(tile)) {
            if (t.empty)
                return t;
        }

        return null;
    }

    List<TileController> Neighbours(TileController tile) {
        int index = TileIndex(tile);
        List<TileController> neighbours = new List<TileController>();

        int over = index - rowSize;
        if(InRange(over))
            neighbours.Add(tiles[over]);

        int under = index + rowSize;
        if (InRange(under))
            neighbours.Add(tiles[under]);

        int right = index + 1;
        if (InRange(right) && (index + 1) % rowSize != 0)
            neighbours.Add(tiles[right]);

        int left = index - 1;
        if (InRange(left) && index % rowSize != 0)
            neighbours.Add(tiles[left]);

        return neighbours;
    }

    bool InRange(int index) {
        return (index >= 0 && index < tiles.Count);
    }

    int TileIndex(TileController tile) {
        return tiles.IndexOf(tile);
    }
	
}
