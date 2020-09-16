using Microsoft.AspNetCore.Components;
using Something.UI.Secure.ViewModel;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Something.UI.Secure.Pages
{
    public partial class CreateData : ComponentBase
    {
        private readonly Random _random = new Random();
        [Inject]
        public HttpClient Http { get; set; }
        private int RandomNumber(int min, int max)
        {
            return _random.Next(min, max);
        }

        private string RandomString(int size, bool lowerCase = false)
        {
            var builder = new StringBuilder(size);

            char offset = lowerCase ? 'a' : 'A';
            const int lettersOffset = 26;

            for (var i = 0; i < size; i++)
            {
                var @char = (char)_random.Next(offset, offset + lettersOffset);
                builder.Append(@char);
            }

            return lowerCase ? builder.ToString().ToLower() : builder.ToString();
        }

        private SomethingVM[] somethingsVM;

        protected override async Task OnInitializedAsync()
        {
            somethingsVM = await Http.GetFromJsonAsync<SomethingVM[]>("https://localhost:44310/api/things");
        }

        protected async Task AddSomething()
        {
            var value = new Dictionary<string, string>
        {
        { "Name", RandomString(RandomNumber(10,20),true) }
        };

            var content = new FormUrlEncodedContent(value);
            var response = await Http.PostAsync(@"https://localhost:44310/api/things", content);
            somethingsVM = response.Content.ReadFromJsonAsync<SomethingVM[]>().Result;
        }
    }
}
