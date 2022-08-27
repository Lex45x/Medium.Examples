namespace LawOfDemeter.Before;

public class Shop
{
    public Asset BuyDesignerWatch()
    {
        return new DesignerWatch();
    }
}