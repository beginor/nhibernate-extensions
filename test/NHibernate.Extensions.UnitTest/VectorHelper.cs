using System.Net.Http.Json;
using System.Text.Json.Serialization;

namespace NHibernate.Extensions.UnitTest;

public static class VectorHelper {

    public static async Task<float[]> EmbeddingAsync(string input, string model = "qwen3-embedding-0.6b") {
        var http = new HttpClient();
        var res = await http.PostAsJsonAsync(
            "http://172.21.11.160:10800/v1/embeddings",
            new EmpeddingRequest {
                Model = model,
                Input = [input]
            }
        );
        var result = await res.Content.ReadFromJsonAsync<EmbeddingResult>();
        return result.Data[0].Embedding;
    }

    public static async Task<float[][]> EmbeddingAsync(string[] input, string model = "qwen3-embedding-0.6b") {
        var http = new HttpClient();
        var res = await http.PostAsJsonAsync(
            "http://172.21.11.160:10800/v1/embeddings",
            new EmpeddingRequest {
                Model = model,
                Input = input
            }
        );
        var result = await res.Content.ReadFromJsonAsync<EmbeddingResult>();
        return result.Data.OrderBy(item => item.Index).Select(x => x.Embedding).ToArray();
    }

}


public class EmpeddingRequest {
    [JsonPropertyName("model")]
    public string Model { get; set; }
    [JsonPropertyName("input")]
    public string[] Input { get; set; }
}

public class EmbeddingResult {
    [JsonPropertyName("model")]
    public string Model { get; set; }
    [JsonPropertyName("data")]
    public EmbeddingResultItem[] Data { get; set; }
}

public class EmbeddingResultItem {
    [JsonPropertyName("embedding")]
    public float[] Embedding { get; set; }
    [JsonPropertyName("index")]
    public int Index { get; set; }
    [JsonPropertyName("object")]
    public string Object { get; set; }
}
