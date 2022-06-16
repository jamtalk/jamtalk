using System.Linq;
using System.Collections;
using UnityEngine.UI;
public class BingoBoard : BaseBingoBoard<AlphabetData,Image, BingoButton>
{
    public override bool CheckOn(int index) => buttons[index].isOn;
}
