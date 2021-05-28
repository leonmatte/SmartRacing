using System;

[System.Serializable]
public class PlayerData
{
    private String name;
    private int position;
    private String bestLapTime;

    public PlayerData(string name, int position, String bestLapTime)
    {
        this.name = name;
        this.position = position;
        this.bestLapTime = bestLapTime;
    }


    public String GetName()
    {
        return name;
    }

    public int GetPosition()
    {
        return position;
    }

    public String GetBestLapTime()
    {
        return bestLapTime;
    }
}
