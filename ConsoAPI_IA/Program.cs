using ConsoAPI_IA.Models;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

string url = "https://localhost:7186/api/";

LoginForm login = new LoginForm
{
    Idenfifiant = "test@test.com",
    Password = "Test1234="
};

ConnectedUser cu = new ConnectedUser();

HttpClient client = new HttpClient();
client.BaseAddress = new Uri(url);

//Envoi d'info
string jsonToSend = JsonConvert.SerializeObject(login);
HttpContent content = new StringContent(jsonToSend, Encoding.UTF8, "application/json");

using (HttpResponseMessage response = client.PostAsync("auth/login", content).Result)
{
    if (response.IsSuccessStatusCode)
    {
        string recievedJson = response.Content.ReadAsStringAsync().Result;
        cu = JsonConvert.DeserializeObject<ConnectedUser>(recievedJson);
    }
    else
    {
        Console.WriteLine(response.StatusCode);
    }
}

Console.WriteLine("Pseudo : "+ cu.Pseudo);
Console.WriteLine("Token : "+ cu.Token);
Console.WriteLine();
Console.WriteLine("---------------------------");
Console.WriteLine();



//Envoi avec token
NewMessage newmessage = new NewMessage { Content = "salut marcel"};
string jsonmessage = JsonConvert.SerializeObject(newmessage);
HttpContent messageContent = new StringContent(jsonmessage, Encoding.UTF8, "application/json");
client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", cu.Token);
using(HttpResponseMessage response = client.PostAsync("message", messageContent).Result)
{
    try
    {
        if (response.IsSuccessStatusCode) Console.WriteLine("Ok");
        else { Console.WriteLine(response.ReasonPhrase); }
    }
    catch(Exception e)
    {
        Console.WriteLine(e.Message);
    }
    
}

//Récupération
List<Message> messages = new List<Message>();
using (HttpResponseMessage response = client.GetAsync("message").Result)
{
    if (response.IsSuccessStatusCode)
    {
        string json = response.Content.ReadAsStringAsync().Result;
        messages = JsonConvert.DeserializeObject<List<Message>>(json);
    }
}

foreach (Message item in messages)
{
    Console.WriteLine($"- {item.CreatedAt.ToShortDateString()} : {item.Content}");
}