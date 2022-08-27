namespace LawOfDemeter.Before;

public class Gym : IOrganization
{
    public bool IsFancyEnough { get; set; }

    public Membership Join(Person person)
    {
        return new Membership
        {
            Person = person,
            Organization = this
        };
    }
}