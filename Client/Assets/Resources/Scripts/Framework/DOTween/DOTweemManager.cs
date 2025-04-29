using DG.Tweening;

public class DOTweemManager{
    public static DOTweemManager Instance => _instance ??= new DOTweemManager();

    private static DOTweemManager _instance;

    private DOTweemManager(){
        DOTween.Init();
    }
}