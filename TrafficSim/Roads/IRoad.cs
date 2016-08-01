namespace TrafficSim.Roads
{
    public interface IRoad : ITile

    {
        int NumberOfLanes{get; set; }
        float TimeToTraverse {get; set;}
        double Cost { get; set; }
        ushort DirectionAOccupancy { get; set; }
        ushort DirectionBOccupancy { get; set; }

    }
}
