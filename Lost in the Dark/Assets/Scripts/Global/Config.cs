using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Config {

    private static float volume;
    private static Difficulty difficulty;

    public static float Volume {
        get {
            return volume;
        }
        set {
            volume = value;
        }
    }

    public static Difficulty LevelDifficulty {
        get {
            return difficulty;
        }
        set {
            difficulty = value;
        }
    }
}
