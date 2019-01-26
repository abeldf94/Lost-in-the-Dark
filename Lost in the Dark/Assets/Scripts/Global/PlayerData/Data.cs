using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Used for keep the data between scenes
public static class Data {
    private static int kills;
    private static string timeAlive;
    private static int shootedBullets;
    private static int damageReceived;
    private static float damageGiven;
    private static int items;

    public static int Kills {
        get {
            return kills;
        }
        set {
            kills = value;
        }
    }

    public static string Time {
        get {
            return timeAlive;
        }
        set {
            timeAlive = value;
        }
    }

    public static int Bullets {
        get {
            return shootedBullets;
        }
        set {
            shootedBullets = value;
        }
    }

    public static int DamageReceived {
        get {
            return damageReceived;
        }
        set {
            damageReceived = value;
        }
    }

    public static float DamageGiven {
        get {
            return damageGiven;
        }
        set {
            damageGiven = value;
        }
    }

    public static int Items {
        get {
            return items;
        }
        set {
            items = value;
        }
    }

    public static void Clear() {
        kills = 0;
        timeAlive = "";
        shootedBullets = 0;
        damageReceived = 0;
        damageGiven = 0f;
        items = 0;
    }
}

