HOW TO INSTALL

In Package manager add by GitURL
https://github.com/nSavelev/TMP_RuntimeSpritesExtension.git

public class GameResult
{
	bool IsVictory { get; }
	RewardData[] Rewards { get; }
}

public interface Gameplay{
	UniTask<GameResult> PlayGame(int level);
}