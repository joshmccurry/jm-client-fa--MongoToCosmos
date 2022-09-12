// See https://aka.ms/new-console-template for more information


using Microsoft.Extensions.Configuration;

var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

using (HttpClient client = new HttpClient()) {
    client.BaseAddress = new Uri(config.GetValue<string>("URL"));
    Random random = new Random();
    long time = DateTime.Now.Millisecond;
    while (true) {
        int v = random.Next(1, 10000);
        for (int i = 0; i < v; i++) {
            var load = "PutFromCosmosDB?name=test&category=" + random.Next(0, 7).ToString();
            time = DateTime.Now.Millisecond;
            HttpResponseMessage res = await client.GetAsync(load);
            res.EnsureSuccessStatusCode();
            Console.WriteLine($"Loading {i}\tof\t{v}: {load}\t{DateTime.Now.Millisecond-time}ms");
        }
        time = DateTime.Now.Millisecond;
        HttpResponseMessage response = await client.GetAsync("GetFromCosmosDB?name=test");
        response.EnsureSuccessStatusCode();
        string responseBody = await response.Content.ReadAsStringAsync();
        Console.WriteLine(responseBody);
        Console.WriteLine($"Completed in {DateTime.Now.Millisecond - time}ms");

        for (int i = 0; i < 8; i++) {
            time = DateTime.Now.Millisecond;
            var delete = "DeleteFromCosmosDB?category" + i.ToString();
            HttpResponseMessage res = await client.GetAsync(delete);
            res.EnsureSuccessStatusCode();
            Console.WriteLine($"Deleting: {delete}\t{DateTime.Now.Millisecond-time}ms\"");
        }

        Thread.Sleep(1000);
    }
}
