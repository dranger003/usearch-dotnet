namespace usearch
{
    static class Program
    {
        static void Main()
        {
            var random = new Random();

            var vectors = Enumerable
                .Range(0, 10)
                .Select(x => Enumerable.Range(0, 1024).Select(y => random.NextSingle()).ToArray())
                .ToArray();

            var index = new USearch.Index<float>(vectors[0].Length);
            index.Reserve(vectors.Length);
            index.Add(vectors);

            var results = vectors
                .Select(x => index.Search(x))
                .ToList();

            Console.WriteLine();
            Console.WriteLine($"     |{String.Join("|", results.Select((x, i) => $" {i,-14}"))}|");
            Console.WriteLine($"-----|{String.Join("|", results.Select(x => new String('-', 15)))}|");

            //Console.WriteLine(String.Join("\n", results.Select((x, i) => $"{i,4} |{String.Join("|", x.Values.Select(x => $" {$"{(x >= 0 ? " " : "")}{x:F8}",-14}"))}|")));
            Console.WriteLine(String.Join("\n", results.Select((x, i) => $"{i,4} |{String.Join("|", results[i].Keys.Order().Select(key => $" {(key == results[i].Keys.Skip(1).First() ? "*" : " ")} {results[i][key],11:F8} "))}|")));
        }
    }
}
