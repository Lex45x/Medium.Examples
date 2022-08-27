namespace LawOfDemeter.Before;

public class Life
{
    public void Loop()
    {
        var hasMidlifeCrisis = false;
        var carl = new Carl();
        var gym = new Gym();
        var garage = new Garage();
        var shop = new Shop();
        var piano = new Piano();

        while (carl.Alive)
        {
            //age in c# is a bit more complex then in the example.
            var carlAge = DateTime.UtcNow.Year - carl.Dob.Year -
                DateTime.UtcNow.DayOfYear > carl.Dob.DayOfYear
                    ? 0
                    : 1;

            if (carl.IsSleeping)
            {
                carl.Home.Demolish();
            }

            if (!hasMidlifeCrisis && carlAge >= 40)
            {
                hasMidlifeCrisis = true;

                if (gym.IsFancyEnough)
                {
                    carl.Memberships.Add(gym.Join(carl));
                }

                carl.Quit(carl.Job);

                carl.Assets.Add(garage.BuySportCar());
                carl.Assets.Add(shop.BuyDesignerWatch());
                carl.Assets.Clear(); //kinda sad

                carl.Tattoos.Add(new ScientologyTattoo());
                carl.Home.Renovate();
                carl.Hairstyle = Hairstyle.Goth;
                carl.Learn(piano);
                carl.Friends.Clear();
            }
        }
    }
}