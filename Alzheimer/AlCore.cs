using System;
using UnityEngine;

using ItemType = ItemDrop.ItemData.ItemType;

namespace Alzheimer
{
    public class AlCore : MonoBehaviour
    {
        private void Start()
        {
            if (!Console.instance.IsConsoleEnabled()) Console.SetConsoleEnabled(true);
            Console.instance.m_chatWindow.gameObject.SetActive(true);
            Console.instance.Print("You're in EZ Clap");
        }

        private void Update()
        {
            Player pLocal = Player.m_localPlayer;

            if (!pLocal)
                return;

            if (Input.GetKeyDown(KeyCode.Keypad1))
            {
                pLocal.Heal(pLocal.GetMaxHealth(), false);
            }
            else if (Input.GetKeyDown(KeyCode.Keypad2))
            {
                pLocal.AddStamina(pLocal.GetMaxStamina());
            }
            else if (Input.GetKeyDown(KeyCode.Keypad3))
            {
                if (szTargetName.Equals(""))
                {
                    Console.instance.Print("TelTarget not set");
                    return;
                }
                foreach (ZNet.PlayerInfo pInfo in ZNet.instance.GetPlayerList())
                {
                    if (pInfo.m_name.IndexOf(szTargetName, StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        pLocal.TeleportTo(pInfo.m_position, pLocal.transform.rotation, true);
                        return;
                    }
                }
                Console.instance.Print("TelTarget with name " + szTargetName + " not found");
                Console.instance.Print("Potential Targets: ");
                foreach (ZNet.PlayerInfo pInfo in ZNet.instance.GetPlayerList())
                {
                    Console.instance.Print($"{pInfo.m_name} : ({pInfo.m_position.x}, {pInfo.m_position.y}, {pInfo.m_position.z})");
                }
            }
            else if (Input.GetKeyDown(KeyCode.LeftAlt))
            {
                string szCommande = Console.instance.m_input.text;

                if (szCommande.Length < 1)
                    return;

                string[] arrCommande =
                {
                    "Kill - Suicide",
                    "Heal - Heal self to max hp",
                    "God - Never take damage",
                    "Ghost - Undetectable by mobs",
                    "Pos - Get current position",
                    "Seed - the seed of the current world",
                    "FreeFly - Toggle free camera",
                    "Tel [x [y] z] - Teleport to the entered coordinates",
                    "RaiseSkill [skill] [amount] - Raise a certain skill by a certain ammount",
                    "TeleportTo [playername] - Teleport to a player on the server",
                    "Items - List all types of items",
                    "ItemsOfType [type] - List items of said type",
                    "Spawn [amount] [level] - Spawn an entity",
                    "Give [name, [ammount, [quality, [variant]]]] - Add an item to the inventory",
                    "TelTarget [name] - Set the current teleportation target"
                    //"DeSpawn [name] [radius] - Despawns a target in radius"
                };

                switch (szCommande)
                {
                    case "Help":
                        {
                            for (int i = 0; i < arrCommande.Length; i++)
                            {
                                Console.instance.Print(arrCommande[i]);
                            }
                            break;
                        }
                    case "Seed":
                        {
                            Console.instance.Print("Seed: " + WorldGenerator.instance.GetSeed());
                            break;
                        }
                    case "Repair":
                        {
                            foreach(ItemDrop.ItemData item in pLocal.GetInventory().GetAllItems())
                            {
                                if (!item.IsWeapon())
                                    continue;

                                item.m_durability = item.GetMaxDurability();
                            }
                            break;
                        }
                    case "Kill":
                        {
                            pLocal.SetHealth(0);
                            break;
                        }
                    case "Heal":
                        {
                            pLocal.Heal(pLocal.GetMaxHealth(), false);
                            Console.instance.Print("Player Healed to Max.");
                            break;
                        }
                    case "God":
                        {
                            pLocal.SetGodMode(!pLocal.InGodMode());
                            Console.instance.Print("God Mode: " + (pLocal.InGodMode() ? "On." : "Off."));
                            break;
                        }
                    case "Ghost":
                        {
                            pLocal.SetGhostMode(!pLocal.InGhostMode());
                            Console.instance.Print("Ghost Mode: " + (pLocal.InGhostMode() ? "On." : "Off."));
                            break;
                        }
                    case "Items":
                        {
                            foreach (ItemType itemType in Enum.GetValues(typeof(ItemType)))
                            {
                                Console.instance.Print(Enum.GetName(typeof(ItemType), itemType));
                            }
                            break;
                        }
                    case "Pos":
                        {
                            Console.instance.Print("Player position (X,Y,Z): " + pLocal.transform.position.ToString("F0"));
                            break;
                        }
                    case "FreeFly":
                        {
                            GameCamera.instance.ToggleFreeFly();
                            break;
                        }
                    default:
                        {
                            string[] array = szCommande.Split(new char[]
                            {
                                ' '
                            });
                            if (szCommande.StartsWith("RaiseSkill "))
                            {
                                if (array.Length > 2)
                                {
                                    string name = array[1];
                                    if (int.TryParse(array[2], out int num4))
                                    {
                                        pLocal.GetSkills().CheatRaiseSkill(name, num4);
                                    }
                                    else
                                    {
                                        Console.instance.Print("Invalid Syntax: ammount must be an integer.");
                                    }
                                    break;
                                }
                                Console.instance.Print("Invalid Syntax: RaiseSkill [skill] [amount]");
                            }
                            else if (szCommande.StartsWith("TelTarget "))
                            {
                                if (array.Length == 2)
                                {
                                    szTargetName = array[1];
                                    Console.instance.Print("Target set to " + szTargetName);
                                    break;
                                }
                                Console.instance.Print("Invalid Syntax: TelTarget [name]");
                            }
                            else if (szCommande.StartsWith("Tel "))
                            {
                                if (array.Length > 2)
                                {
                                    if (!int.TryParse(array[1], out int x) || !int.TryParse(array[2], out int z))
                                    {
                                        Console.instance.Print("Invalid Syntax: x z must be ints");
                                        break;
                                    }
                                    int y = (int)pLocal.transform.position.y;
                                    if (array.Length > 3)
                                    {
                                        if (!int.TryParse(array[3], out y))
                                        {
                                            Console.instance.Print("Invalid Syntax: x y z must be ints");
                                            break;
                                        }
                                        (y, z) = (z, y);
                                    }
                                    pLocal.TeleportTo(new Vector3(x, y, z), pLocal.transform.rotation, true);
                                }

                                Console.instance.Print("Invalid Syntax : Tel [x [y] z]");
                            }
                            else if (szCommande.StartsWith("TeleportTo "))
                            {
                                if (array.Length == 2)
                                {
                                    string name = array[1];
                                    bool result = false;
                                    foreach (ZNet.PlayerInfo pInfo in ZNet.instance.GetPlayerList())
                                    {
                                        if (pInfo.m_name.IndexOf(name, StringComparison.OrdinalIgnoreCase) >= 0)
                                        {
                                            pLocal.TeleportTo(pInfo.m_position, pLocal.transform.rotation, true);
                                            result = true;
                                            break;
                                        }
                                    }
                                    if (!result) Console.instance.Print("Couldn't find player with name " + name);
                                    break;
                                }
                                Console.instance.Print("Invalid Syntax: TeleportTo [playername]");
                            }
                            else if (szCommande.StartsWith("ItemsOfType "))
                            {
                                if (array.Length == 2)
                                {
                                    string szType = array[1];
                                    foreach (ItemType itemType in Enum.GetValues(typeof(ItemType)))
                                    {
                                        if (Enum.GetName(typeof(ItemType), itemType).IndexOf(szType, StringComparison.OrdinalIgnoreCase) >= 0)
                                        {
                                            foreach (ItemDrop item in ObjectDB.instance.GetAllItems(itemType, ""))
                                            {
                                                Console.instance.Print(item.name);
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    Console.instance.Print("Invalid Syntax: ItemsOfType [type]");
                                }
                            }
                            else if (szCommande.StartsWith("Weight "))
                            {
                                if (array.Length < 2)
                                {
                                    Console.instance.Print("Invalid Syntax: Weight [new_weight]");
                                    break;
                                }

                                if (!int.TryParse(array[1], out int new_ammount) || new_ammount < 300)
                                {
                                    Console.instance.Print("Invalid Syntax: new_weight must be an int >= 300");
                                    break;
                                }

                                pLocal.m_maxCarryWeight = new_ammount;
                            }
                            else if (szCommande.StartsWith("Height "))
                            {
                                if (array.Length < 2)
                                {
                                    Console.instance.Print("Invalid Syntax: Height [new_height]");
                                    break;
                                }

                                if (!int.TryParse(array[1], out int new_height) || new_height < 4)
                                {
                                    Console.instance.Print("Invalid Syntax: new_height must be an int >= 4");
                                    break;
                                }

                                var prop = pLocal.GetInventory().GetType().GetField("m_height", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                                prop.SetValue(pLocal.GetInventory(), new_height);
                            }
                            else if (szCommande.StartsWith("Give "))
                            {
                                if (array.Length < 2)
                                {
                                    Console.instance.Print("Invalid Syntax: Give [name, [ammount, [quality, [variant]]]]");
                                    break;
                                }
                                string szItem = array[1];
                                int iAmmount = 1;
                                int iQuality = 1;
                                int iVariant = 1;

                                int result = 0;

                                if (array.Length > 4)
                                {
                                    if (!int.TryParse(array[4], out iVariant))
                                    {
                                        Console.instance.Print("Invalid Syntax Variant must be an integer");
                                        break;
                                    }
                                }
                                else if (array.Length > 3)
                                {
                                    if (!int.TryParse(array[3], out iQuality))
                                    {
                                        Console.instance.Print("Invalid Syntax Quality must be an integer");
                                        break;
                                    }
                                }
                                else if (array.Length > 2)
                                {
                                    if (!int.TryParse(array[2], out iAmmount))
                                    {
                                        Console.instance.Print("Invalid Syntax ammount must be an integer");
                                        break;
                                    }
                                }
                                foreach (ItemType itemType in Enum.GetValues(typeof(ItemType)))
                                {
                                    foreach (ItemDrop item in ObjectDB.instance.GetAllItems(itemType, ""))
                                    {
                                        if (item.name.Equals(szItem))
                                        {
                                            result = GiveItem(pLocal, item, iAmmount, iQuality, iVariant);
                                            break;
                                        }
                                    }
                                }
                                if (result == 0)
                                {
                                    Console.instance.Print("Couldn't find item with name " + szItem);
                                }
                            }
                            /*
                            else if (szCommande.StartsWith("DeSpawn "))
                            {
                                bool result = false;
                                if (array.Length == 3)
                                {
                                    if (!int.TryParse(array[2], out int maxRange))
                                    {
                                        Console.instance.Print("radius must be an integer");
                                        break;
                                    }
                                    
                                    int iMask = LayerMask.GetMask(new string[]
                                    {
                                        "item",
                                        "piece",
                                        "piece_nonsolid",
                                        "Default",
                                        "static_solid",
                                        "Default_small",
                                        "character",
                                        "character_net",
                                        "terrain",
                                        "vehicle"
                                    });

                                    Collider[] ColArray = Physics.OverlapSphere(pLocal.transform.position, maxRange, iMask);
                                    foreach (Collider collider in ColArray)
                                    {
                                        if (collider.attachedRigidbody)
                                        {
                                            ItemDrop component = collider.attachedRigidbody.GetComponent<ItemDrop>();
                                            if (!(component == null) && component.GetComponent<ZNetView>().IsValid())
                                            {
                                                Console.instance.Print(component.name);
                                                    
                                                if(component.name.IndexOf(array[1], StringComparison.OrdinalIgnoreCase) < 0)
                                                    continue;

                                                Destroy(component);
                                                result = true;
                                                break;
                                            }
                                        }
                                    }
                                    if (!result) Console.instance.Print("Couldn't find " + array[1]);
                                    break;
                                }
                                Console.instance.Print("Invalid Syntax : DeSpawn [name] [radius]");
                            }
                            */
                            else if (szCommande.StartsWith("Spawn "))
                            {
                                string text4 = array[1];
                                int num8 = (array.Length >= 3) && int.TryParse(array[2], out num8) ? num8 : 1;
                                int num9 = (array.Length >= 4) && int.TryParse(array[3], out num9) ? num9 : 1;

                                GameObject prefab = ZNetScene.instance.GetPrefab(text4);
                                if (!prefab)
                                {
                                    pLocal.Message(MessageHud.MessageType.TopLeft, "Missing object " + text4, 0, null);
                                    break;
                                }

                                if (num8 == 1)
                                {
                                    pLocal.Message(MessageHud.MessageType.TopLeft, "Spawning object " + text4, 0, null);
                                    Character component2 = Instantiate(prefab, Player.m_localPlayer.transform.position + Player.m_localPlayer.transform.forward * 2f + Vector3.up, Quaternion.identity).GetComponent<Character>();
                                    if (component2 & num9 > 1)
                                    {
                                        component2.SetLevel(num9);
                                    }
                                }
                                else
                                {
                                    for (int j = 0; j < num8; j++)
                                    {
                                        Vector3 b = UnityEngine.Random.insideUnitSphere * 0.5f;
                                        pLocal.Message(MessageHud.MessageType.TopLeft, "Spawning object " + text4, 0, null);
                                        Character component3 = Instantiate(prefab, Player.m_localPlayer.transform.position + Player.m_localPlayer.transform.forward * 2f + Vector3.up + b, Quaternion.identity).GetComponent<Character>();
                                        if (component3 & num9 > 1)
                                        {
                                            component3.SetLevel(num9);
                                        }
                                    }
                                }
                            }
                            break;
                        }
                }
            }
            else if (Input.GetKeyDown(KeyCode.Delete))
            {
                Initier.Unload();
            }
        }
        private int GiveItem(Player pLocal, ItemDrop Item, int amount, int quality, int variant)
        {
            Inventory Inv = pLocal.GetInventory();
            ItemDrop.ItemData item = Item.m_itemData;
            bool TopFirst = item.IsWeapon()
                || (item.m_shared.m_itemType == ItemType.Tool
                || item.m_shared.m_itemType == ItemType.Shield
                || item.m_shared.m_itemType == ItemType.Utility);
            
            Vector2i Pos = new Vector2i(-1, -1);

            if (TopFirst)
            {
                if (Inv.GetEmptySlots() <= 0)
                    return -1;

                for (int i = 0; i < Inv.GetHeight(); i++)
                {
                    for (int j = 0; j < Inv.GetWidth(); j++)
                    {
                        if (Inv.GetItemAt(j, i) == null)
                        {
                            Pos = new Vector2i(j, i);
                            break;
                        }
                    }
                }

                GameObject Go = Instantiate(ObjectDB.instance.GetItemPrefab(Item.name));
                ItemDrop NewItem = Go.GetComponent<ItemDrop>();
                NewItem.m_itemData.m_stack = Mathf.Min(amount, item.m_shared.m_maxStackSize);
                NewItem.m_itemData.m_quality = Mathf.Min(quality, item.m_shared.m_maxQuality);
                NewItem.m_itemData.m_variant = item.m_shared.m_variants > 1 ? variant : item.m_variant;
                NewItem.m_itemData.m_durability = item.GetMaxDurability();
                NewItem.m_itemData.m_crafterID = pLocal.GetPlayerID();
                NewItem.m_itemData.m_crafterName = pLocal.GetPlayerName();
                NewItem.m_itemData.m_gridPos = Pos;
                Inv.GetAllItems().Add(NewItem.m_itemData);
                Destroy(Go);
            }
            else
            {
                bool found = false;
                for (int k = Inv.GetHeight() - 1; k >= 0; k--)
                {
                    for (int l = 0; l < Inv.GetWidth(); l++)
                    {
                        if (Inv.GetItemAt(l, k) == null)
                        {
                            if (found)
                                continue;

                            Pos = new Vector2i(l, k);
                            found = true;
                            continue;
                        }
                        
                        else if (Inv.GetItemAt(l, k).m_shared.m_name == item.m_shared.m_name)
                        {
                            if (Inv.GetItemAt(l, k).m_stack == item.m_shared.m_maxStackSize)
                                continue;

                            Inv.GetItemAt(l, k).m_stack = Mathf.Min(amount + Inv.GetItemAt(l, k).m_stack, item.m_shared.m_maxStackSize);
                            return 1;
                        }
                    }
                }
                if (!found)
                {
                    Console.instance.Print("No space on Inventory");
                    return -1;
                }
                GameObject Go = Instantiate(ObjectDB.instance.GetItemPrefab(Item.name));
                ItemDrop NewItem = Go.GetComponent<ItemDrop>();
                NewItem.m_itemData.m_stack = Mathf.Min(amount, item.m_shared.m_maxStackSize);
                NewItem.m_itemData.m_gridPos = Pos;
                Inv.GetAllItems().Add(NewItem.m_itemData);
                Destroy(Go);
            }

            return 1;
        }

        private string szTargetName = "Zag";
    }
}
