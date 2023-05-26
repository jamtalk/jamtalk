using System;

[Serializable]
public abstract class ContentsTestSetting
{
    public abstract void Apply();
}

[Serializable]
public class AlphabetContentsSetting : ContentsTestSetting
{
    public eAlphabet currentAlhpabet;
    public override void Apply()
    {
        GameManager.Instance.currentAlphabet = currentAlhpabet;
    }
}
[Serializable]
public class DigraphsContentsSetting : ContentsTestSetting
{
    public eDigraphs currentDigrpahs;
    public override void Apply()
    {
        GameManager.Instance.currentDigrpahs = currentDigrpahs;
    }
}
[Serializable]
public class VowelContentsSetting : ContentsTestSetting
{
    public eVowelType currentVowel;
    public override void Apply()
    {
        GameManager.Instance.currentVowel = currentVowel;
    }
}
[Serializable]
public class BookContentsSetting : ContentsTestSetting
{
    public eBookType book;
    public int bookNumber;
    public int page;
    public override void Apply()
    {
        GameManager.Instance.currentBook = book;
        GameManager.Instance.currentBookNumber = bookNumber;
        GameManager.Instance.currentPage = page;
    }
}