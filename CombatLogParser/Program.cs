    namespace CombatLogParser
    {
        using Discord;
        using Discord.Webhook;
        using System.Threading.Tasks;

        using EventInfo;

        class Program
        {
            static void Main(string[] args)
            {
                new Program().MainAsync().GetAwaiter().GetResult();
            }

            public async Task MainAsync()
            {
                var parse = new Examples.LiveParsing(@"C:\Program Files (x86)\World of Warcraft\_classic_\Logs\WoWCombatLog.txt");

                System.Console.WriteLine("Register Event: Encounter Start");
                parse.LogParser.RegisterEvent(async (s, e) =>
                {
                    string name = e.Get(ENCOUNTER_START.EncounterName);
                    System.Console.WriteLine($"Encounter Started -> {name}.");
                    using (var client = new DiscordWebhookClient("https://discordapp.com/api/webhooks/671392095876939806/NY7ozlkwYr6ICF62BDGGmxS5XX4XFI6hLgcseqjhRulsIlMhECygzVqkeB7NborpeSLB"))
                    {
                        // // var embed = new EmbedBuilder
                        // // {
                        // //     Title = "Encounter Started",
                        // //     Description = name
                        // // };

                        await client.SendMessageAsync(text: $"Encounter Started -> {name}");
                    }

                }, Events.ENCOUNTER_START);

                System.Console.WriteLine("Register Event: Party Kill");
                parse.LogParser.RegisterEvent(async (s, e) =>
                {
                    string name = e.Get(PARTY_KILL.EnemyName);
                    System.Console.WriteLine($"Killed Target -> {name}.");

                    using (var client = new DiscordWebhookClient("https://discordapp.com/api/webhooks/671392770220621864/kGxx_IgyhR0Lk2osd5I2fx54V7tdVxkVPEDNCKS54Wek5ms5Fmbcmj0JncOBmcEqIQ7t"))
                    {
                        await client.SendMessageAsync(text: $"Killed enemy -> {name}");
                    }
                }, Events.PARTY_KILL);

                // Block the program until it is closed.
                await Task.Delay(-1);
            }
        }
    }