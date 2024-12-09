using UnityEngine;

public class PezSpawner : MonoBehaviour
{
    
    private float spawnTime;
    [SerializeField]private GameObject pezPrefab;

    //Mod
    public static float VelocidadPez { get; set; } = 3f;
    public static float SpawnRate { get; set; } = 1.5f;

    void Start()
    {
        
    }

    
    /* Mod
    void Update()
    {
        spawnTime = spawnTime - Time.deltaTime;

        if (spawnTime <= 0){
            Instantiate(pezPrefab, GetSpawnPosition(), Quaternion.identity);
            spawnTime = 1.5f; //Tiempo cada el que se crea un pez
        }
    }*/

    void Update()
{
    spawnTime -= Time.deltaTime;
    if (spawnTime <= 0)
    {
        GameObject pez = Instantiate(pezPrefab, GetSpawnPosition(), Quaternion.identity);
        pez.GetComponent<PezIA>().SetVelocidad(VelocidadPez); // Actualiza la velocidad del pez.
        spawnTime = SpawnRate;
    }
}

    private Vector3 GetSpawnPosition()
    {
        Vector2 limitesPantalla = Camera.main.ScreenToWorldPoint(new Vector3(UnityEngine.Screen.width, UnityEngine.Screen.height, 0));

        float aleatorioVertical = Random.Range(-limitesPantalla.y, limitesPantalla.y);
        float aleatorioHorizontal = 0;

        if(Random.Range(0, 2) == 0){
            aleatorioHorizontal = -limitesPantalla.x - 1;
        }
        else {
            aleatorioHorizontal = limitesPantalla.x + 1;
        }

        return new Vector3(aleatorioHorizontal, aleatorioVertical, 0);

    }
}
