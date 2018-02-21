﻿// disable this for auto collect
#define COLLECT_ON_KEYPRESS

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using Database = ItemDatabase;
using Item = ItemDatabase.Item;

/* ItemController
 *  Attach this script to every collectible item.
 *  The item must have a BoxCollider attached
 */
public class ItemController : MonoBehaviour {

#if COLLECT_ON_KEYPRESS
	[Tooltip("Key binding to collect this item")]
	public /*const*/ KeyCode actionKey = KeyCode.E;
#endif
	[Tooltip("Fetch the object data by it's database id")]
	public int itemID = Item.INVALID_ID;
	[Tooltip("Fetch the object data by it's database name or handle identifier")]
	public string itemName;


	private Item item;


	// Use this for initialization
	void Start () {
		BoxCollider colliderInfo;

		// get the corresponding item stats from database
		item = FetchItem();

		// collect collision information
		colliderInfo = GetComponent<BoxCollider>();
		colliderInfo.isTrigger = true;
	}


	// Gives player chance to collect items, when in touch
	// with it
	void OnTriggerStay(Collider other) {
		Debug.Log(other.gameObject.name);
#if COLLECT_ON_KEYPRESS
		if(Input.GetKeyDown(actionKey) && other.gameObject.name == "Player")
#else
		if(other.gameObject.name == "Player")
#endif
		{
			// equip item
			Destroy(gameObject);
		}		
	}


	// Fetches the item corresponding to itemID or itemName
	// Hence itemID has higher priority, the resulting Item
	// might not have the name specified in itemName.
	// If neither itemId, nor itemName are found, the
	// function returns the defaulf Item
	private Item FetchItem() {
		// first try fetching by set id
		try {
			item = ItemDatabase.FetchItemById(itemID);
		} catch (KeyNotFoundException) {
			// if ID is not valid, try fetching by set name
			try {
				item = ItemDatabase.FetchItemByName(itemName);
			} catch (KeyNotFoundException) {
				// if name is invalid, create empy
				item = new Item();

				// Log exeption
				Debug.LogFormat("ItemController: Invalid item identifiers '{0}'!", gameObject.name); 
				Debug.LogFormat("Item ID {0} not found in Database.", itemID);
				Debug.LogFormat("Item name '{0}' not found in Database.", itemName);
			}
		}

		return item;
	}
}