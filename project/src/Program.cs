using Elasticsearch.Net;
using Nest;
using System;

namespace project.src
{
    class Program
    {
        static void Main(string[] args)
        {
            Credentials cred = new("elastic", "vUoKXj9F_6uja0g-Mw4p", "81a9b2f168e8ca07ccff70fb90ec1630478635841286dc2c067a147724f7e351");
            var node = new Uri("https://localhost:9200");
            var settings = new ConnectionSettings(node)
                .DefaultIndex("songs")
                .CertificateFingerprint(cred.fingerprint)
                .BasicAuthentication(cred.username, cred.password)
                .EnableDebugMode();

            var client = new ElasticClient(settings);
            Console.WriteLine(client.Ping());

            Song s1 = new("Rule #4", "Fish in a Birdcage", "", new string[] { "Lyrics", "Sound", "Vibe" });
            Song s2 = new("Prairies", "BoyWithUke", "", new string[] { "Lyrics", "Duette"});
            Song s3 = new("Everything Black", "Unlike Pluto", "", new string[] { "Sound", "Vibes" });

            Console.WriteLine(s1.ToString());
            Console.WriteLine(s2.ToString());
            Console.WriteLine(s3.ToString());
            Console.WriteLine(client.ToString());

            Console.WriteLine("==========================================================\nAdding Indexes");

            var createIndexResponse = client.Indices.Create("myindex", c => c
                .Map<Song>(m => m.AutoMap()));
            Console.WriteLine("Index created: " + createIndexResponse.IsValid); 
            //Console.WriteLine("\n" + createIndexResponse.DebugInformation.ToString());


            var indexResponse = client.IndexDocument<Song>(s1);
            Console.WriteLine("song 1 added: " + indexResponse.IsValid);
            indexResponse = client.IndexDocument<Song>(s2);
            Console.WriteLine("song 2 added: " + indexResponse.IsValid);
            indexResponse = client.IndexDocument<Song>(s3);
            Console.WriteLine("song 3 added: " + indexResponse.IsValid);

            //Console.WriteLine("\n" + indexResponse.DebugInformation.ToString());


            Console.WriteLine("==========================================================\nQuerrying");
            var searchResponse = Search(client, "Rule #4", "name");
            if (searchResponse != null) { Console.WriteLine("serialized{name}: " + client.RequestResponseSerializer.SerializeToString(searchResponse.Documents)); }
            else
            {
                Console.WriteLine("serialized{name}: Null");
            }
            searchResponse = Search(client, "BoyWithUke", "author");
            if (searchResponse != null) { Console.WriteLine("serialized{author}: " + client.RequestResponseSerializer.SerializeToString(searchResponse.Documents)); }
            else
            {
                Console.WriteLine("serialized{author}: Null");
            }
            searchResponse = Search(client, "Lyrics", "tags");
            if (searchResponse != null) { Console.WriteLine("serialized{tags}: " + client.RequestResponseSerializer.SerializeToString(searchResponse.Documents)); }
            else
            {
                Console.WriteLine("serialized{tags}: Null");
            }
            searchResponse = Search(client, "Rule #3", "name");
            if (searchResponse != null) { Console.WriteLine("serialized{name}: " + client.RequestResponseSerializer.SerializeToString(searchResponse.Documents)); }
            else
            {
                Console.WriteLine("serialized{name}: Null");
            }
            searchResponse = Search(client, "Rule #3", "all");
            if (searchResponse != null) { Console.WriteLine("serialized{all}: " + client.RequestResponseSerializer.SerializeToString(searchResponse.Documents)); } 
            else
            {
                Console.WriteLine("serialized{all}: Null");
            }

            //Console.WriteLine("\n" + searchResponse.DebugInformation.ToString());
        }

        static ISearchResponse<Song>? Search(ElasticClient client, string query, string field)
        {
            if (field.Equals("name"))
            {
                return client.Search<Song>(s => s
                .AllIndices()
                .From(0)
                .Size(10)
                .Query(q => q
                    .Match(m => m
                        .Field(f => f.name)
                            .Query(query))));

            }
            else if (field.Equals("author"))
            {
                return client.Search<Song>(s => s
                .AllIndices()
                .From(0)
                .Size(10)
                .Query(q => q
                    .Match(m => m
                        .Field(f => f.author)
                            .Query(query))));

            }
            else if (field.Equals("description"))
            {
                return client.Search<Song>(s => s
                .AllIndices()
                .From(0)
                .Size(10)
                .Query(q => q
                    .Match(m => m
                        .Field(f => f.description)
                            .Query(query))));

            }
            else if (field.Equals("tags"))
            {
                return client.Search<Song>(s => s
                .AllIndices()
                .From(0)
                .Size(10)
                .Query(q => q
                    .Match(m => m
                        .Field(f => f.tagsList)
                            .Query(query))));

            }
            else if (field.Equals("all"))
            {
                Console.WriteLine("trying");
                return client.Search<Song>(s => s
                .AllIndices()
                .From(0)
                .Size(10)
                .Query(q => q.MatchAll())
            );
            }
            return null;
        }
    }
}
