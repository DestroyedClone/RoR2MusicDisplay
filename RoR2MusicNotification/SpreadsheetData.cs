using System;
using System.Collections.Generic;
using System.Text;

//DO NOT INCLUDE
//its not sensitive or anything just irrelevant to the enduser
//and file size
namespace RoR2MusicNotification
{
    internal class SpreadsheetDataHelper
    {
        //internal id to youtube link
        public static Dictionary<string, string> list = new Dictionary<string, string>()
        {
            {"muBossfightDLC1_10", "https://www.youtube.com/watch?v=gmgjuymq5kk"},
            {"muBossfightDLC1_12", "https://www.youtube.com/watch?v=hiRgFXGbVQU"},
            {"muEscape", "https://www.youtube.com/watch?v=yx1sSQQFkBQ"},
            {"muFULLSong02", "https://www.youtube.com/watch?v=McYWBsystGE"},
            {"muFULLSong06", "https://www.youtube.com/watch?v=Z1dxxRPP8Gc"},
            {"muFULLSong07", "https://www.youtube.com/watch?v=aGb-6wwDrpc"},
            {"muFULLSong18", "https://www.youtube.com/watch?v=MQXnhYcNz6w"},
            {"muFULLSong19", "https://www.youtube.com/watch?v=8FlsEBR6GD8"},
            {"muGameplayBase_09", "https://www.youtube.com/watch?v=VTJcLE_VVX8"},
            {"muGameplayDLC1_01", "https://www.youtube.com/watch?v=81QhAo3hLrM"},
            {"muGameplayDLC1_03", "https://www.youtube.com/watch?v=g89ozaj2SHo"},
            {"muGameplayDLC1_06", "https://www.youtube.com/watch?v=rdnpdJXgV0Q"},
            {"muGameplayDLC1_08", "https://www.youtube.com/watch?v=tw_nOq_t--I"},
            {"muGameplayDLC3_01_SH_Map", "https://www.youtube.com/watch?v=pVPTje9bxmQ"},
            {"muGameplayDLC3_02_P_Map", "https://www.youtube.com/watch?v=NjSzYe4j-Q4"},
            {"muGameplayDLC3_03_IA_Map", "https://www.youtube.com/watch?v=ZctSvryCdY0"},
            {"muGameplayDLC3_04_RC_Map", "https://www.youtube.com/watch?v=mSyIz3YcjVY"},
            {"muGameplayDLC3_05_CE_Map", "https://www.youtube.com/watch?v=5DGVzxPaM8c"},
            {"muGameplayDLC3_06_CC_Map", "https://www.youtube.com/watch?v=NOlfXpHX56A"},
            {"muGameplayDLC3_07_RC_Boss", "https://www.youtube.com/watch?v=s-7kgr1GP1I"},
            {"muGameplayDLC3_08_RMX_SolHeart_Boss", "https://www.youtube.com/watch?v=hCRzcphHQHA"},
            {"muGameplayDLC3_08_SH_Boss", "https://www.youtube.com/watch?v=FYnePfw4kEA"},
            {"muGameplayDLC4_01_M_Map", "https://www.youtube.com/watch?v=T08ZzNPKotM"},
            {"muGameplayDLC4_02_F_Map", "https://www.youtube.com/watch?v=wzub8etRFKQ"},
            {"muGameplayDLC4_03_SM_Map", "https://www.youtube.com/watch?v=mgp6leXGLJ0"},
            {"muGameplayDLC4_04_SOT_Map", "https://www.youtube.com/watch?v=0dzz1MGUzSw"},
            {"muGameplayDLC4_05_SAT_Map", "https://www.youtube.com/watch?v=0dzz1MGUzSw"},
            {"muGameplayDLC4_06_SUT_Map", "https://www.youtube.com/watch?v=0dzz1MGUzSw"},
            {"muGameplayDLC4_07_CR_Map", "https://www.youtube.com/watch?v=bx8_4bGB0VE"},
            {"muGameplayDLC4_08_M_Boss", "https://www.youtube.com/watch?v=cnhUa-n5K-4"},
            {"muGameplayDLC4_09_SM_Boss", "https://www.youtube.com/watch?v=I71NH2t1VlU"},
            {"muGameplayDLC4_BossAlive", "url"},
            {"muGameplayDLC4_BossDied", "url"},
            {"muGameplayDLC4_Ultima_Boss", "https://www.youtube.com/watch?v=fGEpIx3rzJc"},
            {"muIntroCutscene", "https://www.youtube.com/watch?v=6W1xkeufQTo"},
            {"muLogbook", "https://www.youtube.com/watch?v=pZi_Go8bZLI"},
            {"muMainEndingFull", "https://www.youtube.com/watch?v=AVjEPJdvIrg"},
            {"muMainEndingOutroA", "first part of Lacrimosum"},
            {"muMainEndingOutroB", "second part of Lacrimosum"},
            {"muMenu", "https://www.youtube.com/watch?v=RUN6Kqd9xgs"},
            {"muMenuDLC1", "https://www.youtube.com/watch?v=tkt4jWxs4Os"},
            {"muNone", "-"},
            {"muRaidfightDLC1_07", "https://www.youtube.com/watch?v=103F1YbbSv0"},
            {"muSong04", "https://www.youtube.com/watch?v=nTkPVYzMRoo"},
            {"muSong05", "https://www.youtube.com/watch?v=FXS1tFVd4Y4"},
            {"muSong08", "https://www.youtube.com/watch?v=RffBrCLOKv0"},
            {"muSong13", "https://www.youtube.com/watch?v=eQyP7qocptY"},
            {"muSong14", "https://www.youtube.com/watch?v=EGXPAoyP_cg"},
            {"muSong16", "https://www.youtube.com/watch?v=DDcRbrZWFno"},
            {"muSong21", "https://www.youtube.com/watch?v=kSKm8rTdRd0"},
            {"muSong22", "https://www.youtube.com/watch?v=KEqMGuTOB5Q"},
            {"muSong23", "https://www.youtube.com/watch?v=8KmiS1VyUgA"},
            {"muSong24", "https://www.youtube.com/watch?v=Nn9trJXUrp0"},
            {"muSong25", "https://www.youtube.com/watch?v=r2JeL1ibBI0"},
            {"muSong_Habitat", "https://www.youtube.com/watch?v=9HDsr1J9pho"},
            {"muSong_HelminthBoss", "https://www.youtube.com/watch?v=DypwpA9faIo"},
            {"muSong_HelminthRoost", "https://www.youtube.com/watch?v=qGfPPY-w_aY"},
            {"muSong_Lakes&HabitatBoss", "https://www.youtube.com/watch?v=gpf7hBVn8l0"},
            {"muSong_LakesGameplay", "https://www.youtube.com/watch?v=vqnMVdKvecg"},
            {"muSong_LemurianTemple", "https://www.youtube.com/watch?v=uxx4dTK8QzY"},
            {"muSong_LemurianTempleBoss", "https://www.youtube.com/watch?v=X06_-ja0p_w"},
            {"muSong_Meridian", "https://www.youtube.com/watch?v=VmauxZUG5UM"},
            {"muSong_MeridianFSB", "https://www.youtube.com/watch?v=VmauxZUG5UM"},
            {"muSong_Village", "https://www.youtube.com/watch?v=d9wx82K13QU"},
            {"muSong_VillageBoss", "https://www.youtube.com/watch?v=X06_-ja0p_w"},
        };
    }
}
