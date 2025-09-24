using System.Collections.Concurrent;
using AccessControl.Backend.Models;

namespace AccessControl.Backend.Services;

public interface IAccessLogService
{
    IReadOnlyList<AccessLogResponse> GetRecentLogs(int limit);
    void AddLog(AccessLogResponse log);
}

public sealed class InMemoryAccessLogService : IAccessLogService
{
    private readonly ConcurrentQueue<AccessLogResponse> _logs = new();

    public IReadOnlyList<AccessLogResponse> GetRecentLogs(int limit)
    {
        return _logs
            .Reverse()
            .Take(limit)
            .ToList();
    }

    public void AddLog(AccessLogResponse log)
    {
        _logs.Enqueue(log);
        while (_logs.Count > 1000)
        {
            _logs.TryDequeue(out _);
        }
    }
}
