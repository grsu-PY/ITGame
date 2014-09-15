using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace ITGame.CLI.Models
{
    public interface Identity
    {
        DataTable garments_equipment; /*  |  ID  |  Name  |  Type  { |        Characteristicks      |  or  | Str | Int | Agi | Def | }  
                                          |  1   |WolfRobe|  Body  { |  Str:3, Int:1, Agi:3, Def:8  |      |  3  |  1  |  3  |  8  | }  */

        DataTable weapons_equipment; /*  |  ID  |  Name  |  Type  |  Pdamage  |  Mdamage  | 
                                         |  1   | ArcBow |  Bow   |    10     |     0     |  */
        
        DataTable spells; /*  |  ID  |  Name  |  Type  | BaseDamage |  ManaCost  { |            Describe           | }  
                              |  1   |  Wrath | Shadow |    100     |    40      { | Deals shadow damage for target| }  */

        DataTable Chapter1; /*  |  SceneNumber  |  WetherText  |  EnvirDescribe  |  DialogText  |  AnswersText  |  */


        Guid Id
        {
            get;
            set;
        }

        string Name
        {
            get;
            set;
        }
    }
}
