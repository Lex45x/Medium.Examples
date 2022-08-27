namespace LawOfDemeter.After;

public class Membership
{
    public Person Person { get; set; }
    public IOrganization Organization { get; set; }
}