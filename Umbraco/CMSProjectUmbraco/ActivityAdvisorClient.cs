namespace CMSProjectUmbraco.ActivityAdvisor;

public class ActivityAdvisor
{
    public string activity { get; set; }
    public string type { get; set; }
    public int participants { get; set; }
    public double price { get; set; }
    public string link { get; set; }
    public string key { get; set; }
    public double accessibility { get; set; }
}

public class ActivityAdvisorClient
{
    public static async Task<ActivityAdvisor?> GetActivityAdvisorAsync()
    {
        var client = new HttpClient();

        var response = await client.GetFromJsonAsync<ActivityAdvisor?>("https://www.boredapi.com/api/activity");

        return response;
    }
}
