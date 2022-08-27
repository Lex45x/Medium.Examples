namespace LawOfDemeter.After;

public class Shop
{
    public Asset BuyDesignerWatch()
    {
        return new DesignerWatch();
    }
}