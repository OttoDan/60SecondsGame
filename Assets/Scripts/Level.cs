using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level { //TODO: make this a  : MonoBehaviour { //to let us be able to export unity scenes into a transform that carries a levelManager & obstacleManager

	public string Name { get; private set; }
    public AudioClip Song { get; private set; }

    public Level(string name, AudioClip song)
    {
        Name = name;
        Song = song; 
    }
}
