namespace MoonGale.Runtime.Systems
{
    internal interface IScoreSystem
    {
        public void SaveScore(ScoreData data);

        public bool TryGetBestScore(out ScoreData data);
    }
}
