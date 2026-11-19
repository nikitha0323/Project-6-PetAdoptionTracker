using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Nodes;

using var client = new HttpClient
{
    BaseAddress = new Uri("https://localhost:7140/"),
    Timeout = TimeSpan.FromSeconds(15)
};

var endpoints = new Dictionary<string, string>
{
    { "1", "Pets" },
    { "2", "Adopters" },
    { "3", "AdoptionApplications" },
    { "4", "Shelters" },
    { "5", "Breeds" }
};

while (true)
{
    Console.WriteLine();
    Console.WriteLine("PET ADOPTION TRACKER CLIENT");
    Console.WriteLine("1. View Pets");
    Console.WriteLine("2. View Adopters");
    Console.WriteLine("3. View Adoption Applications");
    Console.WriteLine("4. View Shelters");
    Console.WriteLine("5. View Breeds");
    Console.WriteLine("6. Create a Pet");
    Console.WriteLine("7. Update a Pet's Age");
    Console.WriteLine("8. Delete a Pet");
    Console.WriteLine("9. Calculate Adoption Fee");
    Console.WriteLine("10. Get Care Checklist");
    Console.WriteLine("0. Exit");
    Console.Write("Choose an option: ");

    string choice = Console.ReadLine()?.Trim() ?? "";

    if (choice == "0")
    {
        break;
    }

    try
    {
        if (endpoints.TryGetValue(choice, out var endpoint))
        {
            Console.Write(
                "Enter an ID, or press Enter to get all records: ");

            string id = Console.ReadLine()?.Trim() ?? "";

            if (id != "" &&
                (!int.TryParse(id, out int number) || number <= 0))
            {
                Console.WriteLine("Enter a positive whole-number ID.");
                continue;
            }

            string path = id == ""
                ? $"api/{endpoint}"
                : $"api/{endpoint}/{id}";

            using var response = await client.GetAsync(path);
            await ShowResponse(response);
        }
        else if (choice == "6")
        {
            string name = ReadText("Pet name: ");
            int age = ReadNumber("Age in years: ", 0);
            string description = ReadText("Description: ");

            Console.WriteLine(
                "Use existing shelter and breed IDs from options 4 and 5.");

            int shelterId = ReadNumber("Shelter ID: ", 1);
            int breedId = ReadNumber("Breed ID: ", 1);

            var pet = new
            {
                petId = 0,
                name,
                age,
                description,
                adoptionStatus = "Available",
                shelterId,
                breedId
            };

            using var response =
                await client.PostAsJsonAsync("api/Pets", pet);

            await ShowResponse(response);
        }
        else if (choice == "7")
        {
            int id = ReadNumber("Pet ID to update: ", 1);

            using var existing =
                await client.GetAsync($"api/Pets/{id}");

            if (!existing.IsSuccessStatusCode)
            {
                await ShowResponse(existing);
                continue;
            }

            string body = await existing.Content.ReadAsStringAsync();
            var pet = JsonNode.Parse(body)!.AsObject();

            Console.WriteLine($"Selected pet: {pet["name"]}");
            Console.WriteLine($"Current age: {pet["age"]}");

            pet["age"] = ReadNumber("New age: ", 0);

            using var response =
                await client.PostAsJsonAsync("api/Pets", pet);

            await ShowResponse(response);
        }
        else if (choice == "8")
        {
            int id = ReadNumber("Pet ID to delete: ", 1);

            Console.Write($"Delete pet {id}? Type yes to confirm: ");

            if (!string.Equals(
                Console.ReadLine()?.Trim(),
                "yes",
                StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("Deletion cancelled.");
                continue;
            }

            using var response =
                await client.DeleteAsync($"api/Pets/{id}");

            await ShowResponse(response);
        }
        else if (choice == "9")
        {
            int age = ReadNumber("Enter the pet's age: ", 0);

            using var response = await client.GetAsync(
                $"api/AdoptionTools/adoption-fee?age={age}");

            await ShowResponse(response);
        }
        else if (choice == "10")
        {
            string species = ReadText("Enter dog or cat: ");

            using var response = await client.GetAsync(
                "api/AdoptionTools/care-checklist?species="
                + Uri.EscapeDataString(species));

            await ShowResponse(response);
        }
        else
        {
            Console.WriteLine("Please choose a valid option.");
        }
    }
    catch (HttpRequestException ex)
    {
        Console.WriteLine("Could not connect to the API.");
        Console.WriteLine(ex.Message);
    }
    catch (TaskCanceledException)
    {
        Console.WriteLine("The request timed out.");
    }
    catch (JsonException ex)
    {
        Console.WriteLine("Could not read the API response.");
        Console.WriteLine(ex.Message);
    }
}

static string ReadText(string prompt)
{
    while (true)
    {
        Console.Write(prompt);
        string value = Console.ReadLine()?.Trim() ?? "";

        if (!string.IsNullOrWhiteSpace(value))
        {
            return value;
        }

        Console.WriteLine("This field is required.");
    }
}

static int ReadNumber(string prompt, int minimum)
{
    while (true)
    {
        Console.Write(prompt);

        if (int.TryParse(Console.ReadLine(), out int value) &&
            value >= minimum)
        {
            return value;
        }

        Console.WriteLine(
            $"Enter a whole number of at least {minimum}.");
    }
}

static async Task ShowResponse(HttpResponseMessage response)
{
    Console.WriteLine();
    Console.WriteLine(
        $"HTTP {(int)response.StatusCode} {response.StatusCode}");

    string body = await response.Content.ReadAsStringAsync();

    if (string.IsNullOrWhiteSpace(body))
    {
        if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
        {
            Console.WriteLine("Deleted successfully.");
        }

        return;
    }

    try
    {
        var json = JsonSerializer.Deserialize<JsonElement>(body);

        Console.WriteLine(JsonSerializer.Serialize(
            json,
            new JsonSerializerOptions { WriteIndented = true }));
    }
    catch (JsonException)
    {
        Console.WriteLine(body);
    }
}