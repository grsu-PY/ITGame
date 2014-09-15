﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ITGame.CLI.Models.Creature.Actions
{
    public interface ICanAttack : ITGame.CLI.IAction
    {
        void WeaponAttack();
        void SpellAttack();
    }
}
