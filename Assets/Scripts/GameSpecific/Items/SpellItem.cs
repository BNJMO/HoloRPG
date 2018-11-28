using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellItem : AbstractItem {

    public ISpell SpellToLearn { get { return spellToLean; } }

    [Header("Spell Item")]
    [SerializeField] private AbstractSpell spellToLean; 
	
}
