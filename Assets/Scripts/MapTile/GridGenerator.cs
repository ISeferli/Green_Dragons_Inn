using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GridGenerator : MonoBehaviour
{
    [SerializeField] float spacing = 1f;
    [SerializeField] GridTile tilePrefab;
    [SerializeField] List<GridTile> tiles;
    [SerializeField] private Terrain mapTerrain;

    [SerializeField] Color moveTileColor = Color.green;
    [SerializeField] Color selectTileColor = Color.red;
    public static GridGenerator instance;
    private int height;
    private int width;

    void Start(){
        instance = this;
        Vector3 terrainSize = mapTerrain.terrainData.size;
        this.width = (int)terrainSize.z;
        this.height = (int)terrainSize.x;
    }
    
    [ContextMenu("Generate")]
    public void GenerateMap(){
        ClearTiles();
        for (int h=1; h<this.height+1; h++) {
            for (int w=1; w<this.width+1; w++) {
                GenerateTile(h, w);
            }
        }
    }

    public void ClearTiles(){
        for(int i=transform.childCount-1; i>=0; i--){
            Destroy(transform.GetChild(i).gameObject);
        }
        this.tiles.Clear();
    }

    public Vector3 getPlacementNear(Vector3 clickPoint){
        var finalPosition = this.transform.GetComponent<Grid>().GetNearestPointOnGrid(clickPoint);
        return finalPosition;
    }

    void GenerateTile(int h, int w){
        var position = mapTerrain.transform.position + new Vector3(h*spacing, 0f, w*spacing);
        var tile = Instantiate(tilePrefab, getPlacementNear(position) + new Vector3(0f, .1f, 0f), Quaternion.Euler(-90f, 0f, 0f), transform);
        tiles.Add(tile);
    }

    public List<GridTile> TilesInRange(GridTile from, int range){
        float distance = range * spacing;
        return tiles.Where(t => Vector3.Distance(t.transform.position, from.transform.position) <= distance).ToList();
    }

    public void HighlightTile(GridTile highlight, int range){
        foreach(var tile in tiles){
            tile.removeHighlight();
        }

        if(highlight!=null){
            var tilesInRange = TilesInRange(highlight, range);
            highlight.Highlight(selectTileColor);
            foreach(var tile in tilesInRange){
                tile.Highlight(moveTileColor);
            }
        }
    }

    public void highlightCharacter(Transform player){
        if(player.tag.Equals("Character")){
            HighlightTile(player.GetComponent<CharacterPlayer>().currentTile, player.GetComponent<CharacterPlayer>().moveRemain);
        } 
    }

    public void showMovementPreview(Transform player){
        foreach(var tile in tiles){
            tile.removeHighlight();
        }

        player.GetComponent<CharacterPlayer>().currentTile.Highlight(player.GetComponent<CharacterPlayer>().moveColor);
        player.GetComponent<CharacterPlayer>().destinationTile.Highlight(player.GetComponent<CharacterPlayer>().moveColor);
   }

    public bool IsTileInRange(GridTile tile, GameObject whoPlays){
        var tilesInRange = TilesInRange(whoPlays.transform.GetComponent<CharacterPlayer>().currentTile, whoPlays.transform.GetComponent<CharacterPlayer>().moveRemain);
        return tilesInRange.Contains(tile);
    }

    public GridTile FindNearestTile(Vector3 point){
        return tiles.OrderBy(t => Vector3.Distance(t.transform.position, point)).FirstOrDefault();
    }

    public bool IsTileInRangeForEnemy(GridTile tile, GameObject whoPlays, int remain){
        var tilesInRange = TilesInRange(FindNearestTile(whoPlays.transform.position), remain);
        return tilesInRange.Contains(tile);
    }

    public GridTile FindBestMoveTile(GameObject target, int remainingMove, GameObject player){
        GridTile bestTile = null;
        int minDistance = int.MaxValue;
        List<GridTile> destinationTile = null;
        if(player.tag==("Character")){
            destinationTile = TilesInRange(target.GetComponent<EnemyInterface>().currentTile, player.GetComponent<CharacterStat>().attackRange);
        } else {
            destinationTile = TilesInRange(target.GetComponent<CharacterPlayer>().currentTile, player.GetComponent<EnemyInterface>().attackRange);
        }

        foreach (var tile in destinationTile){
            int distance;
            if(tile.isAnyoneOn!=null){
                continue;
            }

            if(player.tag==("Character")){
                distance = (int)Vector3.Distance(tile.transform.position, player.GetComponent<CharacterPlayer>().currentTile.transform.position);
            } else {
                distance = (int)Vector3.Distance(tile.transform.position, player.GetComponent<EnemyInterface>().currentTile.transform.position);
            }

            if (distance < minDistance){
                minDistance = distance;
                bestTile = tile;
            }
        }

        GridTile finalBestTile = null;
        int secMinDistance = int.MaxValue;
        if(minDistance > remainingMove){
            List<GridTile> playerTiles = null;
            if(player.tag==("Character")){
                playerTiles = TilesInRange(player.GetComponent<CharacterPlayer>().currentTile, remainingMove);
            } else {
                playerTiles = TilesInRange(player.GetComponent<EnemyInterface>().currentTile, remainingMove);
            }
            
            foreach (var tile in playerTiles){
                int distance = (int)Vector3.Distance(tile.transform.position, bestTile.transform.position);
                if (distance < secMinDistance){
                    secMinDistance = distance;
                    finalBestTile = tile;
                }
            }
            bestTile = finalBestTile;
        }
        return bestTile;
    }

}
