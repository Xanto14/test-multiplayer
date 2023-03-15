using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TileManager : MonoBehaviour {

    [SerializeField]
    private GameObject tuilePrefab = null;
    [SerializeField]
    private Vector3 terrainTaille = new Vector3(20, 1, 20);
    [SerializeField]
    private int renderDistance = 5;
    [SerializeField]
    private Transform[] gameTransforms;
    [SerializeField]
    private Transform joueurTransform;

    private Dictionary<Vector2, GameObject> tuilesEnsemble = new Dictionary<Vector2, GameObject>();
    private Vector2[] tuilesTerrainPrécédentes;
    private List<GameObject> tuilesObjetsPrécédents = new List<GameObject>();

    private void Update() {
        //sauvegarder la tuile du joueur
        Vector2 tuileJoueur = TuileDepuisPosition(joueurTransform.position);
        //sauvegarder la tuile de tous les objets dans gameTransforms
        // sela sert a charger des objets autre que le player afin qu'il continue de (par exemple) intéragir avec le terrain même lorsqu'ils sont hors du render distance.
        List<Vector2> tuilesTerrain = new List<Vector2>();
        tuilesTerrain.Add(tuileJoueur);
        foreach (Transform t in gameTransforms)
            tuilesTerrain.Add(TuileDepuisPosition(t.position));

        //si aucune tuile existe ou si les tuiles ont changé
        if (tuilesTerrainPrécédentes == null || SiTuilesDiff(tuilesTerrain)) {
            List<GameObject> tuileObjets = new List<GameObject>();
            //activer les nouvelles tuiles
            foreach (Vector2 t in tuilesTerrain) {
                bool EstTuileJoueur = t == tuileJoueur;
                int radius = EstTuileJoueur ? renderDistance : 1;
                for (int i = -radius; i <= radius; i++)
                    for (int j = -radius; j <= radius; j++)
                        ActiverOuCréerTuiles((int)t.x + i, (int)t.y + j, tuileObjets);
            }
            //désactiver les tuiles non utilisées
            foreach (GameObject g in tuilesObjetsPrécédents)
                if (!tuileObjets.Contains(g))
                g.SetActive(false);
                  
            tuilesObjetsPrécédents = new List<GameObject>(tuileObjets);
        }

        tuilesTerrainPrécédentes = tuilesTerrain.ToArray();
        
    }

    
    //sert a activer ou créer les tuiles
    private void ActiverOuCréerTuiles(int xIndex, int yIndex, List<GameObject> tileObjects) {
        if (!tuilesEnsemble.ContainsKey(new Vector2(xIndex, yIndex))) {
            tileObjects.Add(CréerTuile(xIndex, yIndex));
        } else {
            GameObject t = tuilesEnsemble[new Vector2(xIndex, yIndex)];
            tileObjects.Add(t);
            if (!t.activeSelf)
                t.SetActive(true);
        }
    }

    //instantie les tuiles selon le prefab donné
    private GameObject CréerTuile(int xIndex, int yIndex) {
        GameObject terrain = Instantiate(
            tuilePrefab,
            new Vector3(terrainTaille.x * xIndex, terrainTaille.y, terrainTaille.z * yIndex),
            Quaternion.identity
        );
        terrain.name = terrain.name + " [" + xIndex + " , " + yIndex + "]";

        tuilesEnsemble.Add(new Vector2(xIndex, yIndex), terrain);

        return terrain;
    }

    //determine la tuile présente à une position xyz donnée 
    private Vector2 TuileDepuisPosition(Vector3 position) {
        return new Vector2(Mathf.FloorToInt(position.x / terrainTaille.x + .5f), Mathf.FloorToInt(position.z / terrainTaille.z + .5f));
    }

    //vérifie si les tuiles ont changé
    private bool SiTuilesDiff(List<Vector2> centerTiles) {
        if (tuilesTerrainPrécédentes.Length != centerTiles.Count)
            return true;
        for (int i = 0; i < tuilesTerrainPrécédentes.Length; i++)
            if (tuilesTerrainPrécédentes[i] != centerTiles[i])
                return true;
        return false;
    }

    //fonction pour détruire l'entièreté du terrain
    public void DétruireTerrain() {
        foreach (KeyValuePair<Vector2, GameObject> kv in tuilesEnsemble)
            Destroy(kv.Value);
        tuilesEnsemble.Clear();
    }

}