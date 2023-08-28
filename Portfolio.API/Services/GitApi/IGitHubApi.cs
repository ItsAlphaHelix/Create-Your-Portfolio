namespace Portfolio.API.Services.GitApi
{
    public interface IGitHubApi
    {
        Task<HttpResponseMessage> GetTotalUsesProgrammingLanguagesInGitHub();
    }
}
