using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;

namespace SK.Course;

public class MessagesReducer(int maxMessages = 10) : IChatHistoryReducer
{
    private readonly int _maxMessages = maxMessages;

    public Task<IEnumerable<ChatMessageContent>?> ReduceAsync(
        IReadOnlyList<ChatMessageContent> chatHistory,
        CancellationToken cancellationToken = default)
    {
        var reduced = chatHistory
            .Skip(Math.Max(0, chatHistory.Count - _maxMessages))
            .ToList();

        return Task.FromResult<IEnumerable<ChatMessageContent>?>(reduced);
    }
}