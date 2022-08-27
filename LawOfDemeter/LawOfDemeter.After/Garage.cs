namespace LawOfDemeter.After;

public class Garage
{
    public Asset BuySportCar()
    {
        return new SportCar();
    }
}