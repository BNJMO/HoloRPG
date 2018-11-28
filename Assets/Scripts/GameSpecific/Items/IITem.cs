using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IITem {
    /// <summary>
    /// Unique ID that identifies this item.
    /// </summary>
    string ItemID { get; }
    /// <summary>
    /// A short description for the item
    /// </summary>
    string ItemName { get; }
    /// <summary>
    /// Sprite used to represent the item in the UI.
    /// </summary>
    Sprite Icon { get; }
    /// <summary>
    /// Defines the category of the item (i.g. enemy item drop)
    /// </summary>
    int Level { get; }

	
}
