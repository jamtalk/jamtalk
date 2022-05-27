public enum eSceneName
{
    Test,
    JT_PL1_102,
    JT_PL1_103,
    JT_PL1_104,
    JT_PL1_105,
    JT_PL1_106,
    JT_PL1_107,
    JT_PL1_108,
    JT_PL1_109,
    JT_PL1_110,
    JT_PL1_111,
    JT_PL1_112,
    JT_PL1_113,
    JT_PL1_114,
    JT_PL1_115,
    JT_PL1_116,
    JT_PL1_117_3,
    JT_PL1_117_4,
    JT_PL1_118,
    JT_PL1_119,
    JT_PL1_120,
    JT_PL1_121,
}

public enum eContents
{
    JT_PL1_102,
    JT_PL1_103,
    JT_PL1_104,
    JT_PL1_105,
    JT_PL1_106,
    JT_PL1_107,
    JT_PL1_108,
    JT_PL1_109,
    JT_PL1_110,
    JT_PL1_111,
    JT_PL1_112,
    JT_PL1_113,
    JT_PL1_114,
    JT_PL1_115,
    JT_PL1_116,
    JT_PL1_117,
    JT_PL1_118,
    JT_PL1_119,
    JT_PL1_120,
    JT_PL1_121,
}

public enum eAlphabet
{
    A,
    B,
    C,
    D,
    E,
    F,
    G,
    H,
    I,
    J,
    K,
    L,
    M,
    N,
    O,
    P,
    Q,
    R,
    S,
    T,
    U,
    V,
    W,
    X,
    Y,
    Z
}

public enum eAlphabetStyle
{
    /// <summary>
    /// 102
    /// </summary>
    Card,
    /// <summary>
    /// 106, 116
    /// </summary>
    Brown,
    /// <summary>
    /// 109
    /// </summary>
    NeonYellow,
    /// <summary>
    /// 109
    /// </summary>
    NeonRed,
    /// <summary>
    /// 110
    /// </summary>
    FullColor,
    /// <summary>
    /// 112
    /// </summary>
    Dino,
    /// <summary>
    /// 115
    /// </summary>
    FullColorCard,
    /// <summary>
    /// 117
    /// </summary>
    BingoRed,
    /// <summary>
    /// 117
    /// </summary>
    BingoBlue,
    /// <summary>
    /// 119
    /// </summary>
    Yellow,
    /// <summary>
    /// 120
    /// </summary>
    White,
    /// <summary>
    /// 104
    /// </summary>
    Gray,
    /// <summary>
    /// 109
    /// </summary>
    NeonFulcolor,
}

public enum eAlphbetType
{
    Upper,
    Lower
}

public enum eGameResult
{
    Perfect,
    Greate,
    Fail
}
public enum eRocketDirection
{
    Vertical,
    Horizontal
}
public enum eCharactorDirection
{
    ToLeft,
    ToRight,
}
public enum eAPIAct
{
    register,       //회원가입
    memberinfo,     //회원정보수정
    exists,         //아이디중복검사
    member,         //회원정보조회
    login,          //로그인
    memberout,      //회원탈퇴
    board,          //게시판조회
    couponreg,      //쿠폰생성
    edulog,         //학습기록
    edulog_view,    //학습기록 조회
}