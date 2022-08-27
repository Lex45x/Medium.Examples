namespace LawOfDemeter.After;

public class Life
{
    public void LoopOption1()
    {
        var hasMidlifeCrisis = false;
        var carl = new Carl1();
        var gym = new Gym();
        var garage = new Garage();
        var shop = new Shop();
        var piano = new Piano();

        while (carl.Alive)
        {
            if (!hasMidlifeCrisis && carl.Age >= 40)
            {
                hasMidlifeCrisis = true;

                carl.Join(gym);
                carl.QuitJob();

                carl.AddAsset(garage.BuySportCar());
                carl.AddAsset(shop.BuyDesignerWatch());
                carl.AddTattoo(new ScientologyTattoo());

                carl.RenovateHome();

                carl.ChangeHairstyle(Hairstyle.Goth);
                carl.Learn(piano);

                carl.ClearFriends();
            }
        }
    }

    public void LoopOption2()
    {
        var gym = new Gym();
        var garage = new Garage();
        var shop = new Shop();
        var piano = new Piano();

        var carl = new Carl2(garage, gym, shop, piano);

        while (carl.Alive)
        {
            if (carl.CanHaveMidlifeCrisis)
            {
                carl.EnterMidlifeCrisis();
            }
        }
    }
}