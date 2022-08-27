namespace LawOfDemeter.After;

public class Carl2 : Person
{
    private readonly List<Asset> assets = new();
    private readonly List<Friend> friends = new();
    private readonly Garage garage;
    private readonly Gym gym;
    private readonly Home home = new();
    private readonly List<Membership> memberships = new();
    private readonly Piano piano;
    private readonly Shop shop;
    private readonly List<ISkill> skills = new();
    private readonly List<Tattoo> tattoos = new();
    private Hairstyle hairstyle;
    private Job? job;

    public Carl2(Garage garage, Gym gym, Shop shop, Piano piano)
    {
        this.garage = garage;
        this.gym = gym;
        this.shop = shop;
        this.piano = piano;
    }

    public bool Alive { get; }

    public bool CanHaveMidlifeCrisis => Age >= 40;

    //age in c# is a bit more complex then in the example.
    public int Age =>
        DateTime.UtcNow.Year - Dob.Year -
        DateTime.UtcNow.DayOfYear > Dob.DayOfYear
            ? 0
            : 1;

    public DateTime Dob { get; }

    private void Join(IOrganization organization)
    {
        if (organization is Gym { IsFancyEnough: false })
        {
            throw new InvalidOperationException("Gym is too boring to join.");
        }

        memberships.Add(organization.Join(this));
    }

    public void EnterMidlifeCrisis()
    {
        if (!CanHaveMidlifeCrisis)
        {
            throw new InvalidOperationException("At this moment Carl can't have midlife crisis");
        }

        Join(gym);
        job = null;

        assets.Add(garage.BuySportCar());
        assets.Add(shop.BuyDesignerWatch());
        tattoos.Add(new ScientologyTattoo());

        home.Renovate();

        hairstyle = Hairstyle.Goth;
        skills.Add(piano);

        friends.Clear();
    }
}