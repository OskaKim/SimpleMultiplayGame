using System;
using System.Threading.Tasks;

public abstract class AsyncRequest {
    public async void DoRequest(Task task, Action onComplete) {
        string currentTrace = System.Environment.StackTrace;
        try {
            await task;
        }
        catch (Exception e) {
            ParseServiceException(e);
            Exception eFull = new Exception($"Call stack before async call:\n{currentTrace}\n", e);
            throw eFull;
        }
        finally {
            onComplete?.Invoke();
        }
    }
    public async void DoRequest<T>(Task<T> task, Action<T> onComplete) {
        T result = default;
        string currentTrace = System.Environment.StackTrace;
        try {
            result = await task;
            return;
        }
        catch (Exception e) {
            ParseServiceException(e);
            Exception eFull = new Exception($"Call stack before async call:\n{currentTrace}\n", e);
            throw eFull;
        }
        finally {
            onComplete?.Invoke(result);
        }
    }

    protected abstract void ParseServiceException(Exception e);
}
