namespace TrafficSim.Roads
{
    public interface IRoad : ITile

    {
        int NumberOfLanes{get; set; }
        int TimeToTraverse {get; set;}
        double Cost { get; set; }
        int DirectionAOccupancy { get; set; }
        int DirectionBOccupancy { get; set; }

    }
}
