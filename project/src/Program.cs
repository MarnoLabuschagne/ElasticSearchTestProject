using Elasticsearch.Net;
using Nest;
using System;

namespace project.src
{
    class Program
    {
        static void Main(string[] args)
        {
            Credentials cred = new("elastic", "18G62vOs=GJwv_EjT0ez", "966c64268e1ce0e971303ed7bf1caacbe2ef9a9fe4ed61b83874fc2802a28a0b");
            var node = new Uri("https://localhost:9200");
            var settings = new ConnectionSettings(node)
                .DefaultIndex("Songs")
                .CertificateFingerprint(cred.fingerprint)
                .BasicAuthentication(cred.username, cred.password)
                .EnableDebugMode();

            var client = new ElasticClient(settings);

            Song s1 = new("Rule #4", "Fish in a Birdcage", "", new string[] { "Lyrics", "Sound", "Vibe" });
            Song s2 = new("Prairies", "BoyWithUke", "", new string[] { "Lyrics", "Duette", "Love" });
            Song s3 = new("Everything Black", "Unlike Pluto", "", new string[] { "Sound", "Vibes" });

            Console.WriteLine(s1.ToString());
            Console.WriteLine(s2.ToString());
            Console.WriteLine(s3.ToString());
            Console.WriteLine(client.ToString());

            Console.WriteLine("==========================================================\nAdding Indexes");

            var createIndexResponse = client.Indices.Create("myindex", c => c
                .Map<Song>(m => m.AutoMap()));

            var indexResponse = client.IndexDocument(s1);
            Console.WriteLine(indexResponse.IsValid);
            indexResponse = client.IndexDocument(s2);
            Console.WriteLine(indexResponse.Id);
            indexResponse = client.IndexDocument(s3);
            Console.WriteLine(indexResponse.Id);


            Console.WriteLine("==========================================================\nQuerrying");

            /*var searchResponse = client.Search<Person>(s => s
                .From(0)
                .Size(10)
                .Query(q => q.MatchAll())
            );*/


            var searchResponse = Search(client, "Rule #4", "name");
            Console.WriteLine("serialized{name}: " + client.RequestResponseSerializer.SerializeToString(searchResponse.Documents));
            searchResponse = Search(client, "BoyWithUke", "author");
            Console.WriteLine("serialized{author}: " + client.RequestResponseSerializer.SerializeToString(searchResponse.Documents));
            searchResponse = Search(client, "Lyrics", "tags");
            Console.WriteLine("serialized{tags}: " + client.RequestResponseSerializer.SerializeToString(searchResponse.Documents));
            searchResponse = Search(client, "Rule #3", "name");
            Console.WriteLine("serialized{name}: " + client.RequestResponseSerializer.SerializeToString(searchResponse.Documents));
            searchResponse = Search(client, "Rule #3", "all");
            Console.WriteLine("serialized{all}: " + client.RequestResponseSerializer.SerializeToString(searchResponse.Documents));
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
