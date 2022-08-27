namespace LawOfDemeter.After;

public class Carl1 : Person
{
    private readonly List<Asset> assets = new();
    private readonly List<Friend> friends = new();
    private readonly Home home = new();
    private readonly List<Membership> memberships = new();

    private readonly List<ISkill> skills = new();
    private readonly List<Tattoo> tattoos = new();
    private Hairstyle hairstyle;

    private Job? job;

    //age in c# is a bit more complex then in the example.
    public int Age =>
        DateTime.UtcNow.Year - Dob.Year -
        DateTime.UtcNow.DayOfYear > Dob.DayOfYear
            ? 0
            : 1;

    public DateTime Dob { get; }
    public bool Alive { get; }

    public void Learn(ISkill skill)
    {
        skills.Add(skill);
    }

    public void RenovateHome()
    {
        //todo: we can add any business rules here.
        home.Renovate();
    }

    public void AddAsset(Asset asset)
    {
        assets.Add(asset);
    }

    public void QuitJob()
    {
        //null is bad, but I will allow it 
        //learn more here: https://www.lucidchart.com/techblog/2015/08/31/the-worst-mistake-of-computer-science/
        job = null;
    }

    public void AddTattoo(Tattoo scientologyTattoo)
    {
        tattoos.Add(scientologyTattoo);
    }

    public void ClearFriends()
    {
        friends.Clear();
    }

    public void ChangeHairstyle(Hairstyle hairstyle)
    {
        if (hairstyle == Hairstyle.Bald)
        {
            throw new InvalidOperationException("This is how additional business logic can be implemented.");
        }

        this.hairstyle = hairstyle;
    }

    public void Join(IOrganization organization)
    {
        if (organization is Gym { IsFancyEnough: false })
        {
            throw new InvalidOperationException("Gym is too boring to join.");
        }

        memberships.Add(organization.Join(this));
    }
}