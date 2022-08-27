namespace LawOfDemeter.Before;

public class Garage
{
    public Asset BuySportCar()
    {
        return new SportCar();
    }
}