﻿using GD2_MotionErrorCode.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GD2_MotionErrorCode
{
    public partial class Form1 : Form
    {
        private string RawData {  get; set; } = string.Empty;   
        private List<ErrorCodeModel> ErrorList { get; set; } = new List<ErrorCodeModel>();
        public Form1()
        {
            InitializeComponent();
            LoadCsv();
            AnalyzeFile();
        }
        /// <summary>
        /// 解析Csv檔案
        /// </summary>
        private void LoadCsv()
        {
            //string errCodePath = "H:\\C#\\GD2_MotionErrorCode\\GD2_MotionErrorCode\\ERR_QD75D4N.txt";
            //var allText = File.ReadAllText(errCodePath);

            var allText = @"<GROUP>,,
</GROUP>,,
<ERRCODE>,,
0001,""故障
·硬件異常"",""檢查是否有噪聲等影響。""
0002,""內部梯形圖異常
·硬件異常"",""檢查是否有噪聲等影響。""
0065,""運行中可編程控制器就緒OFF
·運行中可編程控制器就緒信號(Y0)為OFF狀態。"",""修改可編程控制器就緒信號(Y0)設置為ON/OFF時的程序。""
0066,""驅動器模塊就緒OFF
·在驅動器模塊就緒信號設置為OFF的狀態下執行了啟動請求。
·運行中驅動器模塊就緒信號為OFF狀態。"",""＜驅動器模塊就緒信號為OFF的狀態時執行了啟動請求時＞
·確認驅動器模塊的電源狀態、與驅動器模塊的布線狀態以及連接器的連接狀態。
·確認輸入信號邏輯選擇的設定值。
·使用不具有就緒輸出的驅動器模塊時，執行布線操作，使QD75N的驅動器模塊就緒信號輸入為始終ON狀態。
＜運行中驅動器模塊就緒信號為OFF狀態時＞
確認驅動器模塊的電源狀態、驅動器模塊的布線以及連接器的連接狀態。""
0067,""運行中測試模式異常
·無法在計算機與CPU模塊之間通信。"",""·確認連接了電纜的計算機側的I/F中是否存在異常。
·計算機與CPU模塊間的通信可能會花費較長時間，因此GX Configurator-QP或GX Works2的連接目標設置中將傳送速度設置為高速，將計算機與CPU模塊的通信I/F直接連接等，盡可能確保高速的線路狀態。
·停止從GX Developer或GX Woks2對CPU模塊的訪問(監視功能等)，降低通信線路的負載。""
0068,""硬件行程限位+
·在硬件行程限位(上限值信號FLS)為OFF的狀態下執行了啟動請求。
·運行中硬件行程限位(上限值信號FLS)為OFF狀態。"",""＜硬件行程限位(上限值信號FLS)為OFF狀態時執行了啟動請求時＞
·確認上限值信號(FLS)的布線狀態。
·確認限位開關的規格與輸入信號邏輯選擇的設置是否匹配。
·不需要設置硬件行程限位(限位開關)的系統中，執行布線操作，使QD75N的上限值信號(FLS)輸入為始終ON狀態。
＜運行中硬件行程限位(上限值信號FLS)為OFF狀態時＞
請執行軸錯誤覆位後，根據手動控制運行，移動到上限值信號(FLS)非OFF狀態的位置。""
0069,""硬件行程限位-
·硬件行程限位(下限值信號RLS)為OFF狀態時執行了啟動請求。
·運行中硬件行程限位(下限值信號RLS)為OFF狀態。"",""＜硬件行程限位(下限值信號RLS)為OFF狀態時執行了啟動請求時＞
·確認下限值信號(RLS)的布線狀態。
·確認限位開關的規格與輸入信號邏輯選擇的設置是否匹配。
·不需要設置硬件行程限位(限位開關)的系統中，執行布線操作，使QD75N的下限值信號(RLS)輸入為始終ON狀態。
＜運行中硬件行程限位(下限值信號RLS)為OFF狀態時＞
·請執行軸錯誤覆位後，根據手動控制運行，移動到下限值信號(RLS)非OFF狀態的位置。""
006A,""啟動時停止信號ON
·停止信號為ON狀態時執行了啟動請求。"",""解除停止指令後，重新設置時間進行啟動。
　向QD75N的輸出信號...軸1:Y4、軸2:Y5、軸3:Y6、軸4:Y7
　外部輸入...用於連接外部設備的連接器:停止信號(STOP)""
006B,""BUSY中可編程控制器就緒OFF→ON
·BUSY信號為ON的狀態下，對可編程控制器就緒信號執行了OFF→ON的操作。"",所有軸的BUSY信號為OFF的狀態下，可編程控制器就緒信號(Y0)設為ON。
00C9,""原點上啟動
·設置原點回歸重試禁用時，在原點回歸完成標志為ON的狀態下，啟動了近點DOG型的機械原點回歸。"",""·將原點回歸重試功能設為啟用(設定值:1)。
·根據手動控制運行，從當前位置(原點)開始移動後執行機械原點回歸。""
00CB,""DOG檢測出時間異常
·近點DOG型的機械原點回歸中，從原點回歸速度到爬行速度的減速期間，近點DOG信號為OFF狀態。"",""·降低原點回歸速度。
·延長DOG信號輸入時間。""
00CC,""零點檢測出時間異常
·制動器停止型②的機械原點回歸中，從原點回歸速度到爬行速度的減速期間，零點信號為OFF狀態。"",""·降低原點回歸速度。
·以爬行速度移動期間輸入外部零點信號。""
00CD,""停留時間異常
·制動器停止型①的機械原點回歸中，從原點回歸速度到爬行速度的減速期間經過停留時間。"",""·降低原點回歸速度。
·延長原點回歸的停留時間。""
00CE,""計數型移動量異常
·計數型①,②的機械原點回歸中，參數[近點DOG ON後的移動量設置]比原點回歸速度到減速停止的所需距離要小。"",""·根據速度限制值、原點回歸速度、減速時間計算移動距離，設置近點DOG ON後的移動量大於減速距離。
·降低原點回歸速度。
·為了延長近點DOG ON後的移動量，調整近點DOG的位置。""
00CF,""原點回歸請求ON
·高速原點回歸啟動(定位啟動No.9002)時，原點回歸請求標志為ON狀態。"",執行機械原點回歸(定位啟動No.9001)。
00D1,""無法重新啟動原點回歸
·通過停止信號停止機械原點回歸後，開啟了重新啟動指令。"",再次啟動機械原點回歸(定位啟動No.9001)。
00D5,""ABS傳送超時
·無法通過絕對位置恢覆指令與伺服放大器正常通信。"",""·重新檢查布線。
·修改程序。""
00D6,""ABS傳送SUM
·無法通過絕對位置恢覆指令與伺服放大器正常通信。"",""·重新檢查布線。
·修改程序。
·重新修改專用指令的參數。""
012C,""JOG速度超出範圍
·JOG啟動時，JOG速度超出設置範圍。"",在設置範圍內設置JOG速度。
012D,""寸動移動量錯誤
·寸動移動量不滿足設置條件(設定值過大)。
　設置條件:
　[寸動移動量×(A)　≦　JOG速度限制值]
　(A)…單位設置為pulse時:562.5
　　　　單位設置為pulse以外時:337.5"",縮小寸動移動量使之滿足設置條件。
01F4,""條件數據號不正確
·通過特殊啟動執行塊啟動時，啟動了使用條件數據(條件啟動、等待啟動、同時啟動、FOR(條件))的塊時，條件數據號超出設置範圍。(1≦條件數據號≦10)"",重新修改條件數據號。
01F5,""同時啟動前錯誤
＜塊啟動的同時啟動時＞
·執行同時啟動的對象軸為軸BUSY。
＜多個軸同時啟動控制時＞
·執行同時啟動的對象軸為軸BUSY。
·啟動軸的[同時啟動對象軸啟動數據號]為0或超出設置範圍。
·啟動軸以外的[同時啟動對象軸啟動數據號]超出設置範圍。"",""＜塊啟動的同時啟動時＞
將條件運算符設為正常。
＜多個軸同時啟動控制時＞
同時啟動對象軸啟動數據號設為正常。""
01F6,""數據號不正確
·要執行的定位數據號在1～600,7000～7004,9001～9004以外。
·正在執行JUMP目標的指定。
·將JUMP目標的指定在1～600以外。"",定位啟動號、定位啟動數據(塊啟動時)、定位數據(JUMP指令時)設為正常。
01F7,""無指令速度
·定位啟動時，最初執行的定位數據的指令速度已設置為當前速度(－1)。
·速度控制中已設置當前速度。
·速度·位置切換控制、位置·速度切換控制中已設置當前速度。"",將定位數據設為正常。
01F8,""直線移動量超出範圍
·參數或定位數據的[插補速度指定方法]在[合成速度]的設置中執行直線插補時，各定位數據中設置的各軸移動量超出1073741824(2^30)。
·單位為[degree]時，軟件行程限位上限≠軟件行程限位下限的設置中，INC指令中的定位地址在－360.00000以下或360.00000以上。"",重新修改定位地址。
01FA,""圓弧誤差偏差大
·執行中心點指定的圓弧插補、螺旋插補時，起點－中心點的半徑與終點－中心點的半徑之差超出參數[圓弧插補誤差允許範圍]。"",""·修改中心點地址(圓弧地址)。
·修改終點地址(定位地址)。""
01FB,""軟件行程限位+
·在超出軟件行程限位上限的位置上執行了定位。
·定位地址、當前值更改值超出軟件行程限位上限。
·輔助點指定的圓弧插補、螺旋插補中，輔助點超出軟件行程限位上限。"",""運行啟動時:
·通過手動控制使進給當前值在軟件行程限位範圍內。
·修改定位地址。(輔助點指定的圓弧插補或螺旋插補時也檢查圓弧地址。)
當前值更改:在軟件行程限位範圍內設置當前值更改值。
運行中:修改定位地址。""
01FC,""軟件行程限位-
·在超出軟件行程限位下限的位置上執行了定位。
·定位地址、當前值更改值超出軟件行程限位下限。
·輔助點指定的圓弧插補、螺旋插補中，輔助點超出軟件行程限位下限。"",""運行啟動時:
·通過手動控制使進給當前值在軟件行程限位範圍內。
·修改定位地址。(輔助點指定的圓弧插補或螺旋插補時也檢查圓弧地址。)
當前值更改:在軟件行程限位範圍內設置當前值更改值。
運行中:修改定位地址。""
0202,""當前值更改超出範圍
·單位為[degree]時，當前值更改的地址超出0～359.99999的範圍。"",在設置範圍內設置當前值更改值。
0203,""無法更改當前值
·控制方式通過當前值更改定位數據設置為運行模式[連續軌跡控制]。
·運行模式[連續軌跡控制]的定位數據的下一數據控制方式設置為[當前值更改]。"",""·指定當前值更改時，不指定連續軌跡控制。
·連續軌跡控制的下一個定位數據中不指定當前值更改。""
0204,""無法使用連續·連續軌跡控制
·速度控制、速度·位置切換控制、位置·速度切換控制、定長進給、當前值更改等連續軌跡控制無法執行的控制方式中指定了連續軌跡控制。
·速度控制、速度·位置切換控制、位置·速度切換控制、定長進給、當前值更改等之前的數據設置為連續軌跡控制。
·速度控制、位置·速度切換控制中指定了連續定位控制。"",""·根據連續軌跡控制的下一條定位數據不指定速度控制、定長進給、速度·位置切換控制、位置·速度切換控制、當前值更改。
·連續軌跡控制的運行模式下不執行定長進給、速度控制、速度·位置切換控制、位置·速度切換控制、當前值更改。
·連續定位控制的運行模式下不執行速度控制，位置·速度切換控制。""
0206,""運行模式超出範圍
·運行模式的設定值為2。"",修改運行模式。
0207,""對象軸BUSY插補
·對象軸運行中執行了插補啟動。"",修改控制方式。
0208,""單位組不一致
·參數或定位數據的[插補速度指定方法]的[合成速度]的設置中，基準軸和插補軸的單位不相同。"",修改定位數據或更改插補對象軸的參數[單位設置]。
0209,""插補記述指令不正確
·2軸插補、螺旋插補中插補對象軸的設置為本軸或不存在的軸。"",""·修改控制方式。
·修改插補對象軸。""
020A,""指令速度設置錯誤
·指令速度超出設置範圍。
　直線插補、圓弧插補、螺旋插補:基準軸超出設置範圍。
　速度控制插補:基準軸、插補軸的任意一個超出速度範圍。"",修改指令速度。
020B,""插補模式錯誤
·速度控制的插補控制、4軸直線插補控制下基準軸的參數或定位數據的[插補速度指定方法]中指定合成速度並執行了啟動。
·圓弧插補控制或螺旋插補控制下基準軸的參數或定位數據的[插補速度指定方法]中指定基準軸速度執行了啟動。"",正確設置[插補速度指定方法]。
020C,""控制方式設置錯誤
·控制方式的設定值超出範圍。
·連續定位控制、連續軌跡控制下連續執行時，控制軸數或插補對象軸與前一個數據不同。
·無線模式下執行了機械原點回歸、高速原點回歸、速度·位置以及位置·速度切換控制。
·數據No.600的控制方式中設置了NOP指令。
·緩沖存儲器地址1906(禁止使用區域)已設置為0以外。"",""·修改控制方式、插補對象軸或參數。
·不要對緩沖存儲器地址1906(禁止使用區域)進行設置。""
020D,""輔助點設置錯誤
輔助點指定的圓弧插補、螺旋插補中為以下之一。
·起點＝輔助點
·終點＝輔助點
·起點、終點、輔助點處於一條直線上
·輔助點地址、中心點地址超出－2147483648～2147483647的範圍"",修改輔助點地址(圓弧地址)。
020E,""終點設置錯誤
·輔助點指定的圓弧插補、螺旋插補中，起點＝終點。
·輔助點指定及中心點指定的圓弧插補、螺旋插補中，終點地址超出-2147483648～2147483647的範圍。"",修改終點地址(定位地址)。
020F,""中心點設置錯誤
中心點指定的圓弧插補、螺旋插補中為以下之一。
·起點＝中心點
·終點＝中心點
·中心點地址超出-2147483648～2147483647的範圍"",修改中心點地址(圓弧地址)。
0212,""地址超出範圍
·速度·位置、位置·速度切換控制中，定位地址的設定值為負值。
·1軸直線控制(ABS)、2～4軸直線插補控制(ABS)、螺旋插補控制(ABS)中，定位地址的設定值超出0～359.99999[degree]的範圍。"",修改定位地址。
0214,""無法同時啟動
·同時啟動的對象軸中存在發生了非本錯誤的軸。"",通過錯誤履歷確認發生了非本錯誤的軸並排除錯誤原因。修改塊啟動數據、定位數據。
0215,""條件數據錯誤
·條件對象的設定值未設置或超出範圍。
·條件運算符的設定值未設置或超出範圍。
·條件運算符是位運算符，參數1設置為32以上。
·對於已設置的條件對象，設置了無法使用的條件運算符。
·條件運算符在05H(P1≦**≦P2)中，參數1＞參數2。
·條件對象為[緩沖存儲器(1字/2字)]時，[地址]的設定值超出設置範圍。(1字：0～32767、2字:0～32766)"",將塊啟動數據設為正常。
0216,""特殊啟動指令錯誤
·相應特殊啟動指令不存在。"",修改特殊啟動指令代碼。
0217,""無法執行圓弧插補
·單位為[degree]的軸中執行了圓弧插補、螺旋插補。"",修改控制方式。
0218,""M代碼ON信號ON啟動
·M代碼ON信號(X4～X7)為ON狀態時執行了定位啟動。"",關閉M代碼ON信號後啟動。
0219,""可編程控制器就緒OFF啟動
·可編程控制器就緒信號(Y0)為OFF狀態時執行了定位啟動。"",確認可編程控制器就緒信號(Y0)設置為ON/OFF時的程序後，可編程控制器就緒ON後開始啟動。
021A,""準備就緒OFF啟動
·QD75準備就緒信號(X0)為OFF狀態時執行了定位啟動。"",確認QD75準備就緒ON後，開始啟動。
021F,""啟動號超出範圍
·定位啟動時，軸控制數據的[定位啟動號]的設定值在1～600,7000～7004,9001～9004以外。
·預讀啟動時，軸控制數據的[定位啟動號]的設定值在1～600以外。"",將定位啟動號設為正常。
0220,""半徑超出範圍
·圓弧的半徑超出536870912。"",修改定位數據。
0221,""控制方式LOOP設置錯誤
·控制方式[LOOP]的重覆次數設置為0。"",LOOP的重覆次數在1～65535中設置。
0222,""degree時ABS方向設置不正確
單位為[degree]時的ABS方向設定值
·設置為設置範圍外的值。
·軟件行程限位啟用時已設置為0以外。"",""·degree時，在設置範圍內設置ABS方向。
·軟件行程限位啟用時設置為[0]。
·將軟件行程限位設為禁用。(軟件行程限位上限值＝軟件行程限位下限值時為禁用。)""
0229,""M代碼ON時機錯誤
定位數據[M代碼ON信號輸出時機]的設定值超出範圍。"",將定位數據[M代碼ON信號輸出時機]修改為0～2。
022A,""插補速度指定方法錯誤
定位數據[插補速度指定方法]的設定值超出範圍。"",將定位數據[插補速度指定方法]修改為0～2。
022B,""螺距數超出範圍
螺旋插補中直線插補軸的定位數據[M代碼]中設置的螺距數超出範圍。"",將直線插補軸的定位數據[M代碼]中設置的螺距數修改為0～999。
0320,""保持錯誤
·CPU模塊的參數[錯誤停止時的輸出]中對於QD75N的設置為[保持]。"",""CPU模塊的參數[錯誤停止時的輸出]設置為[清除]。""
0321,""Flash ROM寫入錯誤
·無法寫入到Flash ROM。"",Flash ROM寫入壽命的評估。
0322,""Flash ROM和校驗錯誤
·Flash ROM寫入中途電源OFF。"",恢覆為出廠設置時的參數。
0323,""可編程控制器CPU錯誤
·CPU模塊發生了錯誤。"",確認CPU模塊中發生的錯誤代碼，並參照MELSEC-Q CPU模塊用戶手冊。
0324,""專用指令錯誤
·狀態為0以外時執行了Z.ABRST□指令。(與伺服放大器的通信開始時)
·絕對位置恢覆期間(與伺服放大器通信期間)，更改了Z.ABRST□指令的狀態。
·啟動號為1～600、7000～7004、9001～9004以外時，執行了ZP.PSTRT□指令。
·示教數據選擇為0，1以外時，執行了ZP.TEACH□指令。
·定位數據號為1～600以外時，執行了ZP.TEACH□指令。
·通過Z.ABRST□、ZP.PSTRT□、ZP.TEACH□指令指定了不存在的軸的指令。"",""·執行Z.ABRST□指令時，狀態設置為0。
·通過Z.ABRST□指令的絕對位置恢覆期間不更改狀態。
·執行ZP.PSTRT□指令時，啟動號在設置範圍內。
·執行ZP.TEACH□指令時，示教數據選擇以及定位數據號在設置範圍內。
·不通過Z.ABRST□、ZP.PSTRT□、ZP.TEACH□指令指定不存在的軸的指令。""
0325,""Flash ROM寫入次數錯誤
·程序連續超過25次向Flash ROM寫入。"",""修改程序後，不要連續向Flash ROM寫入。
(正常使用時發生該錯誤時，可以在錯誤覆位，或執行電源OFF→ON的操作/CPU模塊的覆位後寫入。)""
0326,""專用指令I/F錯誤
·CPU模塊與QD75N的I/F不匹配。"",故障
0384,""單位設置超出範圍
·基本參數1[單位設置]的設定值超出設置範圍。"",在設置範圍內進行設置後，對可編程控制器就緒信號(Y0)執行OFF→ON的操作。
0385,""每轉脈沖數超出範圍
·基本參數1[每轉的脈沖數]的設定值超出設置範圍。"",在設置範圍內進行設置後，對可編程控制器就緒信號(Y0)執行OFF→ON的操作。
0386,""每轉移動量超出範圍
·基本參數1[每轉的移動量]的設定值超出設置範圍。"",在設置範圍內進行設置後，對可編程控制器就緒信號(Y0)執行OFF→ON的操作。
0387,""單位倍率超出範圍
·基本參數1[單位倍率]的設定值超出設置範圍。"",在設置範圍內進行設置後，對可編程控制器就緒信號(Y0)執行OFF→ON的操作。
0388,""脈沖輸出模式錯誤
·基本參數1[脈沖輸出模式]的設定值超出設置範圍。"",在設置範圍內進行設置後，對可編程控制器就緒信號(Y0)執行OFF→ON的操作。
0389,""旋轉方向設置錯誤
·基本參數1[旋轉方向設置]的設定值超出設置範圍。"",在設置範圍內進行設置後，對可編程控制器就緒信號(Y0)執行OFF→ON的操作。
038A,""偏置速度超出範圍
·基本參數1[啟動時偏置速度]的設定值超出設置範圍。
·偏置速度超出速度限制值。"",""·將偏置速度設置在速度限制值以下。
·在設置範圍內進行設置後，對可編程控制器就緒信號(Y0)執行OFF→ON的操作。""
038E,""速度限制值超出範圍
·基本參數2[速度限制值]的設定值超出設置範圍。
·速度限制值換算成頻率的值超出模塊的最高輸出頻率。
·速度限制值比原點回歸速度小。"",""·頻率換算值不要超出模塊的最高輸出頻率。
　QD75P4N:200000[pulse/s]
　QD75D4N:4000000[pulse/s]
·設置原點回歸速度以上的值。
·在設置範圍內進行設置後，對可編程控制器就緒信號(Y0)執行OFF→ON的操作。""
038F,""加速時間0超出範圍
·基本參數2[加速時間0]的設定值超出設置範圍。"",在設置範圍內進行設置後，對可編程控制器就緒信號(Y0)執行OFF→ON的操作。
0390,""減速時間0超出範圍
·基本參數2[減速時間0]的設定值超出設置範圍。"",在設置範圍內進行設置後，對可編程控制器就緒信號(Y0)執行OFF→ON的操作。
0398,""反向間隙補償量錯誤
·每脈沖的移動量換算為脈沖數的值在256脈沖以上。"",將[每脈沖的移動量]換算為脈沖數的值設為256脈沖以內。
0399,""軟件行程限位上限
·單位為[degree]時，詳細參數1[軟件行程限位上限值]的設定值超出範圍。
·單位為[degree]以外時，軟件行程限位上限值＜軟件行程限位下限值。"",""·在設置範圍內進行設置。
·單位為[degree]以外時，設置為下限值＜上限值。""
039A,""軟件行程限位下限
·單位為[degree]時，詳細參數1[軟件行程限位下限值]的設定值超出範圍。
·單位為[degree]以外時，軟件行程限位上限值＜軟件行程限位下限值。"",""·在設置範圍內進行設置。
·單位為[degree]以外時，設置為下限值＜上限值。""
039B,""軟件行程限位選擇
·詳細參數1[軟件行程限位選擇]的設定值超出設置範圍。"",在設置範圍內進行設置後，對可編程控制器就緒信號(Y0)執行OFF→ON的操作。
039C,""軟件行程限位啟用/禁用設置
·詳細參數1[軟件行程限位啟用/禁用設置]的設定值超出設置範圍。"",在設置範圍內進行設置後，對可編程控制器就緒信號(Y0)執行OFF→ON的操作。
039D,""指令到位範圍
·詳細參數1[指令到位範圍]的設定值超出設置範圍。"",在設置範圍內進行設置後，對可編程控制器就緒信號(Y0)執行OFF→ON的操作。
039E,""轉矩限制設定值不正確
·詳細參數1[轉矩限制設定值]的設定值超出設置範圍。"",在設置範圍內進行設置後，對可編程控制器就緒信號(Y0)執行OFF→ON的操作。
039F,""M代碼ON時間錯誤
·詳細參數1[M代碼ON信號輸出時間]的設定值超出設置範圍。"",在設置範圍內進行設置後，對可編程控制器就緒信號(Y0)執行OFF→ON的操作。
03A0,""速度切換模式錯誤
·詳細參數1[速度切換模式]的設定值超出設置範圍。"",在設置範圍內進行設置後，對可編程控制器就緒信號(Y0)執行OFF→ON的操作。
03A1,""插補速度指定方法錯誤
·詳細參數1[插補速度指定方法]的設定值超出設置範圍。"",在設置範圍內進行設置後，對可編程控制器就緒信號(Y0)執行OFF→ON的操作。
03A2,""當前值更新請求錯誤
·詳細參數1[速度控制時的進給當前值]的設定值超出設置範圍。"",在設置範圍內進行設置後，對可編程控制器就緒信號(Y0)執行OFF→ON的操作。
03A4,""手動脈沖發生器輸入模式錯誤
·詳細參數1[手動脈沖發生器輸入選擇]的設定值超出設置範圍。"",在設置範圍內進行設置後，對可編程控制器就緒信號(Y0)執行OFF→ON的操作。
03A7,""速度·位置功能選擇錯誤
詳細參數1[速度·位置功能選擇]設置為2，未滿足以下3個條件。
①單位為[degree]
②軟件行程限位禁用
③進給當前值有更新"",""·使速度·位置切換控制(ABS模式)滿足上述①～③的條件。
·不執行速度·位置切換控制(ABS模式)時，速度·位置功能選擇設置為0後，對可編程控制器就緒信號(Y0)執行OFF→ON的操作。""
03B6,""加速時間1設置錯誤
·詳細參數2[加速時間1]的設定值超出設置範圍。"",在設置範圍內進行設置後，對可編程控制器就緒信號(Y0)執行OFF→ON的操作。
03B7,""加速時間2設置錯誤
·詳細參數2[加速時間2]的設定值超出設置範圍。"",在設置範圍內進行設置後，對可編程控制器就緒信號(Y0)執行OFF→ON的操作。
03B8,""加速時間3設置錯誤
·詳細參數2[加速時間3]的設定值超出設置範圍。"",在設置範圍內進行設置後，對可編程控制器就緒信號(Y0)執行OFF→ON的操作。
03B9,""減速時間1設置錯誤
·詳細參數2[減速時間1]的設定值超出設置範圍。"",在設置範圍內進行設置後，對可編程控制器就緒信號(Y0)執行OFF→ON的操作。
03BA,""減速時間2設置錯誤
·詳細參數2[減速時間2]的設定值超出設置範圍。"",在設置範圍內進行設置後，對可編程控制器就緒信號(Y0)執行OFF→ON的操作。
03BB,""減速時間3設置錯誤
·詳細參數2[減速時間3]的設定值超出設置範圍。"",在設置範圍內進行設置後，對可編程控制器就緒信號(Y0)執行OFF→ON的操作。
03BC,""JOG速度限制值錯誤
·詳細參數2[JOG速度限制值]的設定值超出設置範圍。
·詳細參數2[JOG速度限制值]的設定值超出速度限制值。"",""·在設置範圍內進行設置後，對可編程控制器就緒信號(Y0)執行OFF→ON的操作。
·設置為速度限制值以下的值。""
03BD,""JOG加速時間選擇設置錯誤
·詳細參數2[JOG加速時間選擇]的設定值超出設置範圍。"",在設置範圍內進行設置後，對可編程控制器就緒信號(Y0)執行OFF→ON的操作。
03BE,""JOG減速時間選擇設置錯誤
·詳細參數2[JOG減速時間選擇]的設定值超出設置範圍。"",在設置範圍內進行設置後，對可編程控制器就緒信號(Y0)執行OFF→ON的操作。
03BF,""加減速處理選擇設置錯誤
·詳細參數2[加減速處理選擇]的設定值超出設置範圍。"",在設置範圍內進行設置後，對可編程控制器就緒信號(Y0)執行OFF→ON的操作。
03C0,""S字形曲線比率設置錯誤
·詳細參數2[S字形曲線比率]的設定值超出設置範圍。"",在設置範圍內進行設置後，對可編程控制器就緒信號(Y0)執行OFF→ON的操作。
03C1,""急停止減速時間不正確
·詳細參數2[急停止減速時間]的設定值超出設置範圍。"",在設置範圍內進行設置後，對可編程控制器就緒信號(Y0)執行OFF→ON的操作。
03C2,""停止組1急停止選擇錯誤
·詳細參數2[停止組1急停止選擇]的設定值超出設置範圍。"",在設置範圍內進行設置後，對可編程控制器就緒信號(Y0)執行OFF→ON的操作。
03C3,""停止組2急停止選擇錯誤
·詳細參數2[停止組2急停止選擇]的設定值超出設置範圍。"",在設置範圍內進行設置後，對可編程控制器就緒信號(Y0)執行OFF→ON的操作。
03C4,""停止組3急停止選擇錯誤
·詳細參數2[停止組3急停止選擇]的設定值超出設置範圍。"",在設置範圍內進行設置後，對可編程控制器就緒信號(Y0)執行OFF→ON的操作。
03C6,""圓弧插補誤差超出允許範圍
·詳細參數2[圓弧插補誤差允許範圍]的設定值超出設置範圍。"",在設置範圍內進行設置後，對可編程控制器就緒信號(Y0)執行OFF→ON的操作。
03C7,""外部指令功能選擇錯誤
·詳細參數2[外部指令功能選擇]的設定值超出設置範圍。"",在設置範圍內進行設置後，對可編程控制器就緒信號(Y0)執行OFF→ON的操作。
03D4,""原點回歸方式錯誤
·原點回歸基本參數[原點回歸方式]的設定值超出設置範圍。"",在設置範圍內進行設置後，對可編程控制器就緒信號(Y0)執行OFF→ON的操作。
03D5,""原點回歸方向錯誤
·原點回歸基本參數[原點回歸方向]的設定值超出設置範圍。"",在設置範圍內進行設置後，對可編程控制器就緒信號(Y0)執行OFF→ON的操作。
03D6,""原點地址設置錯誤
·原點回歸基本參數[原點地址]的設定值超出設置範圍。"",在設置範圍內進行設置後，對可編程控制器就緒信號(Y0)執行OFF→ON的操作。
03D7,""原點回歸速度錯誤
·原點回歸基本參數[原點回歸速度]的設定值超出設置範圍。
·原點回歸基本參數[原點回歸速度]的設定值比啟動時的偏置速度小。"",""·在設置範圍內進行設置。
·啟動時設置為偏置速度以上的值。""
03D8,""爬行速度錯誤
·原點回歸基本參數[爬行速度]的設定值超出設置範圍。
·原點回歸基本參數[爬行速度]的設定值比原點回歸速度大。
·原點回歸基本參數[爬行速度]的設定值比啟動時的偏置速度小。"",""·在設置範圍內進行設置。
·設為原點回歸速度以下的值。
·啟動時設置為偏置速度以上的值。""
03D9,""原點回歸重試錯誤
·原點回歸基本參數[原點回歸重試]的設定值超出設置範圍。"",在設置範圍內進行設置後，對可編程控制器就緒信號(Y0)執行OFF→ON的操作。
03DF,""近點DOG ON後移動量設置錯誤
·原點回歸詳細參數[近點DOG ON後的移動量設置]的設定值超出設置範圍。"",在設置範圍內進行設置後，對可編程控制器就緒信號(Y0)執行OFF→ON的操作。
03E0,""原點回歸加速時間選擇錯誤
·原點回歸詳細參數[原點回歸加速時間選擇]的設定值超出設置範圍。"",在設置範圍內進行設置後，對可編程控制器就緒信號(Y0)執行OFF→ON的操作。
03E1,""原點回歸減速時間選擇錯誤
·原點回歸詳細參數[原點回歸減速時間選擇]的設定值超出設置範圍。"",在設置範圍內進行設置後，對可編程控制器就緒信號(Y0)執行OFF→ON的操作。
03E3,""原點回歸轉矩限制值錯誤
·原點回歸詳細參數[原點回歸轉矩限制值]的設定值超出設置範圍。
·原點回歸詳細參數[原點回歸轉矩限制值]超出詳細參數1[轉矩限制設定值]。"",在設置範圍內進行設置後，對可編程控制器就緒信號(Y0)執行OFF→ON的操作。
03E4,""偏差計數清除信號輸出時間設置錯誤
·原點回歸詳細參數[偏差計數清除信號輸出時間]的設定值超出設置範圍。"",在設置範圍內進行設置後，對可編程控制器就緒信號(Y0)執行OFF→ON的操作。
03E5,""原點移位時速度指定錯誤
·原點回歸詳細參數[原點移位時速度指定]的設定值超出設置範圍。"",在設置範圍內進行設置後，對可編程控制器就緒信號(Y0)執行OFF→ON的操作。
</ERRCODE>,,
";
            Console.WriteLine(allText);
            RawData = allText;
        }
        /// <summary>
        /// 解析說明代碼
        /// </summary>
        private void AnalyzeFile()
        {
            ErrorList.Clear();
            string pattern = @"<ERRCODE>,,\s*([\s\S]*?)\s*</ERRCODE>,,";
            Match match = Regex.Match(RawData, pattern);

            if (match.Success)
            {
                string result = match.Groups[1].Value.Trim();
                //(\w{4}),
                string pattern1 = @"(\w{4}),";
                string pattern2 = @"(?<=\w{4},)(.|\n)*?(?=(\w{4},|$))";
                MatchCollection matches1 = Regex.Matches(result, pattern1, RegexOptions.Singleline);
                MatchCollection matches2 = Regex.Matches(result, pattern2, RegexOptions.Singleline);

                if (matches1.Count == matches2.Count)
                {
                    
                    for (int i = 0; i < matches1.Count; i++)
                    {
                        ErrorCodeModel model = new ErrorCodeModel();
                        string[] spContext = matches2[i].ToString().Split(',');
                        model.HexCode = matches1[i].ToString().TrimEnd(',');
                        model.Context = spContext[0].ToString().TrimStart('"').TrimEnd('"').Replace("·", "-");
                        model.Description = spContext[1].ToString().Replace("\"", "").Replace("\r", "").Replace("\n", "").Replace("·", "\r\n").Replace("。", "。\n").Trim(); 
                        ErrorList.Add(model);
                        Console.WriteLine($"Code:{matches1[i]} 內容:{spContext[0]} 解決方法:{spContext[1]}");                        
                    }
                }
                

            }
            else
            {
                Console.WriteLine("未找到匹配的內容");
            }
        }
        /// <summary>
        /// 解析對應的代碼
        /// </summary>
        /// <param name="code"></param>
        private void AnalyzeCode(string code)
        {
            var res = ErrorList.Where(x => x.HexCode == code).FirstOrDefault();

            if (res != null)
            {
                richTextBox1.Clear();
                richTextBox2.Clear();
                richTextBox1.AppendText(res.Context);
                richTextBox2.AppendText(res.Description);
            }
            else
            {
                MessageBox.Show("查無代碼!");
            }
        }
        /// <summary>
        /// 載入RawData (測試用)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            AnalyzeFile();
        }
        /// <summary>
        /// 查詢按鈕
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            
            bool _code = int.TryParse(textBox1.Text.ToString(), out int code);
            if (_code)
            {
                AnalyzeCode(code.ToString("X4"));
            }            
        }
    }
}