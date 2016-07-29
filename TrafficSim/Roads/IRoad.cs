namespace TrafficSim.Roads
{
    public interface IRoad : ITile

    {
        int NumberOfLanes{get; set; }
        int SpeedLimit {get; set;}
        double Cost { get; set; }
        ushort DirectionAOccupancy { get; set; }
        ushort DirectionBOccupancy { get; set; }

    }
}
