using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelLoading : MonoBehaviour
{
    public static LevelLoading levelLoad = null;
    private LevelGen gen;
    public bool setup;
    public PlayerController play;
    // Start is called before the first frame update
    private void Awake()
    {
        if (levelLoad == null) levelLoad = this;
        else if (levelLoad != this) Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
        gen = GetComponent<LevelGen>();
        play = FindObjectOfType<PlayerController>();
        LoadLevel();
    }
    public void LoadLevel()
    {
        setup = true;
        //Keep track of time it takes to generate levels
        var watch = System.Diagnostics.Stopwatch.StartNew();

        //Generate level
        gen.Generation();

        //Spawn player in
        play.transform.position = gen.spawnPos;

        //Stop timer and print time elapsed
        watch.Stop();
        var elapsedMs = watch.ElapsedMilliseconds;
        Debug.Log("Generation Time: " + elapsedMs + "ms");
        setup = false;
    }
}
