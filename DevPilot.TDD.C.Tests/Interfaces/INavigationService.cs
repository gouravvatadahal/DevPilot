namespace DevPilot.TDD.C.Tests.Interfaces;

public interface INavigationService
{
    string CurrentPath { get; }
    void NavigateTo(string path);
} 