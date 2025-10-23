using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using AgriculturalLandManagement.Models;

public class CornerUploadMiddleware :IMiddleware
{
    
    private readonly ICornerRepository _cornerRepository;
    private readonly ILogger<CornerUploadMiddleware> _logger;

    private static readonly Dictionary<int, List<byte[]>> _imageChunks = new();
    public static List<(int cornerIndex, byte[] imageData)> PendingImages = new();

    public CornerUploadMiddleware( ICornerRepository cornerRepository,ILogger<CornerUploadMiddleware> logger)
    {
        _cornerRepository = cornerRepository;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context,RequestDelegate next)
    {
        if (!IsWebSocketRequest(context))
        {
            await next(context);
            return;
        }

        using var webSocket = await context.WebSockets.AcceptWebSocketAsync();
        await HandleWebSocketSession(webSocket);
    }

    private bool IsWebSocketRequest(HttpContext context)
    {
        return context.WebSockets.IsWebSocketRequest && context.Request.Path.Equals("/ws");
    }

    private async Task HandleWebSocketSession(WebSocket webSocket)
    {
        var buffer = new byte[4096];
        int? expectedCornerCount = null;
        int currentCornerIndex = -1;

        while (webSocket.State == WebSocketState.Open)
        {
            var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

            if (result.MessageType == WebSocketMessageType.Close)
            {
                await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Done", CancellationToken.None);
                break;
            }

            if (result.MessageType == WebSocketMessageType.Text)
            {
                (expectedCornerCount, currentCornerIndex) = await HandleTextMessage(buffer, result.Count, expectedCornerCount, currentCornerIndex);
                
            }
            else if (result.MessageType == WebSocketMessageType.Binary && currentCornerIndex >= 0)
            {
                HandleBinaryChunk(buffer, result.Count, currentCornerIndex);
                _logger.LogInformation("Handle Image based request");
            }
        }

        FinalizeImages();
    }

    private async Task<(int? updatedCornerCount, int updatedCornerIndex)> HandleTextMessage(
        byte[] buffer, int count, int? cornerCount, int currentCornerIndex)
    {
        var json = Encoding.UTF8.GetString(buffer, 0, count);
        var obj = JsonSerializer.Deserialize<JsonElement>(json);
    
        // 1️⃣ Corner count
        if (obj.TryGetProperty("cornerCount", out var countProp))
        {
            _logger.LogInformation("corner count request received");
            return (countProp.GetInt32(), currentCornerIndex);
        }
    
        // 2️⃣ Image start / index (no landId means it's just an index)
        if (obj.TryGetProperty("cornerIndex", out var indexProp) && !obj.TryGetProperty("landId", out _))
        {
            _logger.LogInformation("corner index (image start) request received");
            int index = indexProp.GetInt32();
            _imageChunks[index] = new List<byte[]>(); // prepare to store image chunks
            return (cornerCount, index);
        }
    
        // 3️⃣ Otherwise, treat as corner metadata
        try
        {
            await CreateCornerFromMetadata(obj);
            _logger.LogInformation("Corner created from received request");
        }
        catch (Exception ex)
        {
            _logger.LogError("Failed to create corner from metadata: " + json);
            _logger.LogError(ex.ToString());
        }
    
        return (cornerCount, currentCornerIndex);
    }


    private async Task CreateCornerFromMetadata(JsonElement obj)
    {
        try
        {
            var corner = new Corner
            {
                LandId = obj.GetProperty("landId").GetInt32(),
                Latitude = obj.GetProperty("latitude").GetDouble(),
                Longitude = obj.GetProperty("longitude").GetDouble(),
                Index = obj.GetProperty("cornerIndex").GetInt32()
            };

            _logger.LogInformation("Creating corner: {@corner}", corner);
            await _cornerRepository.CreateAsync(corner);
            _logger.LogInformation("Corner saved to DB");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create corner from metadata: {json}", obj.ToString());
        }
    }


    private void HandleBinaryChunk(byte[] buffer, int count, int currentCornerIndex)
    {
        var chunk = buffer.Take(count).ToArray();
        _imageChunks[currentCornerIndex].Add(chunk);
    }

    private void FinalizeImages()
    {
        foreach (var kvp in _imageChunks)
        {

            var fullImage = kvp.Value.SelectMany(c => c).ToArray();
            PendingImages.Add((kvp.Key, fullImage));
            _logger.LogInformation("Image saved in the in memory storage");
        }

    }

    
}
