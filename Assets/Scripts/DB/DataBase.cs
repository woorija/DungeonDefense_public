using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueData
{
    public int namepath { get; private set; }
    public string text { get; private set; }
    public DialogueData() { }
    public DialogueData(int _namepath,string _text)
    {
        namepath = _namepath;
        text = _text;
    }
}

public class DataBase : MonoBehaviour
{
    public static DataBase Instance;
    public  UnitDB unitDB;
    public  MonsterDB monsterDB;
    public Material[] MonsterMaterials; // 몬스터 타입별 테두리 색상 구분
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        unitDB.Init();
        monsterDB.Init();
    }
    public static readonly Dictionary<int, string> MonsterPathDB = new Dictionary<int, string> // 몬스터 애니메이션 경로
    {
        {11, "Cleopatra_mob1" },
        {12, "Cleopatra_mob2" },
        {13, "Cleopatra_mob3" },
        {14, "Cleopatra" },

        {21, "Cadmus_mob1" },
        {22, "Cadmus_mob2" },
        {23, "Cadmus_mob3" },
        {24, "Cadmus" },

        {31, "Seadanda_mob1" },
        {32, "Seadanda_mob2" },
        {33, "Seadanda_mob3" },
        {34, "Seadanda" },

        {994, "Gilgamesh" }
    };

    public static readonly Dictionary<int, DialogueData> Dialogue_textDB = new Dictionary<int, DialogueData>
    { // 0아크리치 1클레오파트라 2카드모스 3셰어단더 4길가메시
        {101, new DialogueData(1, "이젠 이 방법 뿐이야. 던전의 힘을 얻어서 이집트를 다시 살려낼거야") },
        {102, new DialogueData(0, "...죽어....") },
        {103, new DialogueData(1, "던전의 주인인가! 이 엄청난 힘! 내 것으로 만들어버리겠다!") },
        {104, new DialogueData(0, "... 나 죽어...") },
        {105, new DialogueData(1, "??") },
        {106, new DialogueData(0, "누나! 나 죽어!!!!") },

        {201, new DialogueData(2, "이곳은 뭐지..? 세상 온갖 악한 저주들의 냄새가 나는군.") },
        {202, new DialogueData(0, "내 던전에 들어오다니.. 겁을 상실한 것인가? 어 그런데 그… 누구세요?") },
        {203, new DialogueData(2, "나는 테베의 왕 카드모스! 악을 무찌르기 위해서 왔다!") },
        {204, new DialogueData(0, "어… 음… 그 죄송한데 모르는 분이라서요;") },
        {205, new DialogueData(2, "그리스 최초의 영웅인 나를 모른다니!") },
        {206, new DialogueData(0, "그리스 영웅하면 페르세우스라던가 헤라클레스라던가… 카드모스는 모르는데요;") },
        {207, new DialogueData(2, "용서 할 수 없다!") },

        {301, new DialogueData(3, "와 여긴 어디? 신기하게 생겼네") },
        {302, new DialogueData(0, "넌 또 누군데 남의 던전에 들어오는거니..?") },
        {303, new DialogueData(3, "난 셰어단더! 아저씨는 누구..?") },
        {304, new DialogueData(0, "어.. 그냥 평범한 아크리치란다? 어린애들이 놀기엔 위험하니까 얼른 돌아가렴") },
        {305, new DialogueData(3, "와 재밋겠다! 여기서 놀아야지!") },
        {306, new DialogueData(0, "아니… 돌아가라니까!") },

        {9901, new DialogueData(4, "아아 느껴진다. 소문대로 이곳에는 죽음을 거부한 자가 있구나. 신의 섭리를 무시하고 살아가는 존재에게서 느껴지는 소름끼치도록 수많은 저주.") },
        {9902, new DialogueData(0, "이미 반신의 몸으로 죽음이 두려워서 나를 찾아오다니 참으로 우습구나. 길가메시!") },
        {9903, new DialogueData(4, "어떻게 나를 알고 있는것이지?") },
        {9904, new DialogueData(0, "어... 그 딱 봐도 길가메시인데요. 금발이며 잘난척하는 표정이며 누가봐도 길가메시인걸?") },
        {9905, new DialogueData(4, "나를 알고 있다면 이야기가 빠르겠군. 영생의 비밀을 내놓아라!") }
    };

    public static readonly Dictionary<int, string> ArtifactNameDB = new Dictionary<int, string> // 유물이름정보
    {
        {0,"Ca₁₀(PO4)₆(OH)₂" },
        {1,"환영의 돌" },
        {2,"뾰족한 송곳니" },
        {3,"스팀팩" },
        {4,"단단한 대퇴골" },
        {5,"부러진 직검" },
        {6,"서리함" },
        {7,"고급 식탁보" },
        {8,"딜루미네이터" },
        {9,"존의 모래시계" },
        {10,"박쥐 가면" },
        {11,"UV선글라스" },
        {12,"1959년산 로마네 콩티" },
        {13,"타이런트 바이러스" },
        {14,"변종 동충하초" },
        {15,"누군가의 세번째 팔" },
        {16,"왠지 Wa! 라고 말하고 싶어지는 두개골" },
        {17,"여신의 슬픔" },
        {18,"비상용 탈출장치" }
    };

    public static readonly Dictionary<int, string> ArtifactTextDB = new Dictionary<int, string> // 유물 툴팁정보
    {
        {0,"Ca₁₀(PO4)₆(OH)₂ 수산화인회석, 즉 뼈를 이루는 칼슘의 보충제\n───────────\n스켈레톤 종족의 공격력 + 5" },
        {1,"겉보기엔 그냥 돌맹이지만 엄청난 힘이 숨겨져 있다\n───────────\n고스트 종족의 공격력 + 5" },
        {2,"선천적으로 뾰족한 송곳니를 가진 뱀파이어들도 이를 보호하기 위해 사용한다고 한다.\n───────────\n흡혈귀 종족의 공격력 + 3" },
        {3,"전투 효율을 높이는 약물. 부작용이 심하지만 어차피 좀비에게 부작용은 별 의미가 없다.\n───────────\n좀비 종족의 공격력 + 5" },
        {4,"하체 운동에 진심이었던 망자의 대퇴골\n───────────\n스켈레톤 공격력 + 10" },
        {5,"날이 중간부터 부러져 없어진 직검.\n───────────\n스켈레톤 나이트 공격력 + 20" },
        {6,"“여기 힘이 있었노라. 그리고 여기 절망 또한 있었노라.”\n───────────\n데스나이트 공격력 + 30\n공격 거리 + 1" },
        {7,"좋은 원단으로 만들어진 고급진 식탁보\n───────────\n고스트 공격 속도 + 0.5" },
        {8,"빛을 없애는 라이터. 어둠속에서 스펙터는 그 힘이 증폭된다.\n───────────\n스펙터 공격 거리 + 0.5\n공격력 + 5" },
        {9,"뒤집히는 순간 시간이 잠시 멈춘다는 전설의 모래시계\n───────────\n팬텀 특수능력 공격력 증가량 +1" },
        {10,"어느 슈퍼히어로가 쓴다고 알려진 가면\n───────────\n흡혈박쥐 공격력 + 5" },
        {11,"고급 선글라스는 태양광의 자외선을 100% 차단해준다.\n───────────\n뱀파이어 공격력 + 10" },
        {12,"한 병에 2천만원이 훌쩍 넘는 이런 고급 와인은 아무리 밤의 귀족이라도 쉽게 마실 수 없다고 한다.\n───────────\n뱀파이어 로드 공격력 + 20\n특수능력이 3명을 처치할 때 마다 발동된다." },
        {13,"좀비로 인해 멸망한 다른 세계에서 넘어온 좀비 바이러스\n───────────\n좀비의 공격 속도 +0.5" },
        {14,"사람을 숙주로 하는 변종 동충하초로 다른 세계를 멸망시킨 곰팡이형 바이러스\n───────────\n구울의 공격속도 + 0.5\n공격력 + 10" },
        {15,"어보미네이션 중 가장 유명했다는 누군가의 세번째 팔\n───────────\n어보미네이션의 공격속도 + 0.5\n특수능력 데미지 2배" },
        {16,"언더테일 아시는구나! 혹시 모르는분들에 대해 설명드립니다 샌즈랑 언더테일의 세가지 엔딩루트중 몰살엔딩의 최종보스로 진.짜.겁.나.어.렵.습.니.다\n───────────\n플레이어 마나 회복량 1.5배" },
        {17,"왠지 모르게 마나 최대치가 증가 할것 같이 생긴 보석\n───────────\n플레이어 최대마나 2배" },
        {18,"비상! 비상! 탈출을 시도합니다!\n───────────\n플레이어에게 도달한 적을 시작점으로 돌려보낸다. (1회용)" },
    };
}
