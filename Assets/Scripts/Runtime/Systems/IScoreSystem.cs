namespace MoonGale.Runtime.Systems
{
    internal interface IScoreSystem
    {
        public float SurvivedTimeSeconds { get; }

        public void StartTimer();

        public void StopTimer();

        public void SaveScore(ScoreData data);

        public bool TryGetBestScore(out ScoreData data);
    }
}
