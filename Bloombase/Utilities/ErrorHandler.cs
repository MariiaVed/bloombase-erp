namespace Bloombase;

public class ErrorHandler
{
    public async Task ShowErrorMessage(string message)
    {
        await MainThread.InvokeOnMainThreadAsync(async () =>
        {
            await Application.Current.MainPage.DisplayAlert("Error", message, "OK");
        });
    }

    public async Task ShowSuccessMessage(string message)
    {
        await MainThread.InvokeOnMainThreadAsync(async () =>
        {
            await Application.Current.MainPage.DisplayAlert("Success", message, "OK");
        });
    }
}