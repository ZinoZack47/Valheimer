using System;
using System.Collections.Generic;
using UnityEngine;

//using ItemType = ItemDrop.ItemData.ItemType;

namespace Alzheimer
{
    class Utilizer
    {
        /*private List<ItemDrop>[] arrItems =
            new List<ItemDrop>[Enum.GetNames(typeof(ItemType)).Length];
        */
        public void ExecuteCommande(string szCommande)
        {
            Player pLocal = Player.m_localPlayer;
            string[] arrCommande =
            {
                "Cheats - Check if cheats are enabled on the server",
                "Refresh - Reload variables",
                "Heal - Heal self to max hp",
                "God - Never take damage",
                "Ghost - Undetectable by mobs",
                "Pos - Get current position",
                "FreeFly - Toggle free camera",
                "Goto [x,z] - Teleport to the entered coordinates",
                "RaiseSkill [skill] [amount] - Raise a certain skill by a certain ammount",
                "TeleportTo [playername] - Teleport to a player on the server",
                "Items - List items of all types",
                "ItemsOfType [type] - List items of said type",
                "Spawn [amount] [level] - Spawn an entity",
                "Give [name, [ammount, [quality, [variant]]]] - Add an item to the inventory"
            };

            if (!pLocal)
            {
                Console.instance.Print("Local Player not in yet...");
                return;
            }
            /*
            if(arrItems.Length == 0)
            {
                GetItems();
                return;
            }
            */
            switch (szCommande)
            {
                case "Cheats":
                    {
                        if(Console.instance.IsCheatsEnabled())
                        {
                            Console.instance.Print("Cheats are on. Immersion Ruined DansGame");
                        }
                        else
                        {
                            Console.instance.Print("Cheats are off. Thank God forsenCD");
                        }
                        break;
                    }
                case "Help":
                    {
                        for(int i = 0; i < arrCommande.Length; i++)
                        {
                            Console.instance.Print(arrCommande[i]);
                        }
                        break;
                    }
                    /*
                case "Refresh":
                    {
                        ReLoad();
                        Console.instance.Print("Refreshed.");
                        break;
                    }
                    */
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
                    /*
                case "Items":
                    {
                        foreach (ItemType itemType in Enum.GetValues(typeof(ItemType)))
                        {
                            foreach (ItemDrop item in arrItems[(int)itemType])
                            {
                                Console.instance.Print(Enum.GetName(typeof(ItemType), itemType) + ": " + item.name);
                            }
                        }
                        break;
                    }*/
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

                        if (szCommande.StartsWith("Goto "))
                        {
                            string text5 = szCommande.Substring(5);
                            char[] separator = new char[]
                            {
                                    ',',
                                    ' '
                            };
                            string[] array3 = text5.Split(separator);
                            if (array3.Length < 2)
                            {
                                Console.instance.Print("Invalid Syntax Goto ");
                                break;
                            }
                            try
                            {
                                if(float.TryParse(array3[0], out float x) && float.TryParse(array3[1], out float z))
                                {
                                    Vector3 pos2 = new Vector3(x, pLocal.transform.position.y, z);
                                    pLocal.TeleportTo(pos2, pLocal.transform.rotation, true);
                                }
                                else
                                {
                                    Console.instance.Print("Invalid Syntax Goto [x,z]");
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.instance.Print("parse error:" + ex.ToString() + "  " + text5);
                            }
                        }
                        else if (szCommande.StartsWith("RaiseSkill "))
                        {
                            if (array.Length > 2)
                            {
                                string name = array[1];
                                if(int.TryParse(array[2], out int num4))
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
                        else if (szCommande.StartsWith("TeleportTo "))
                        {
                            if(array.Length > 2)
                            {
                                string name = array[1];
                                List<Player> PlayerList = Player.GetAllPlayers();
                                foreach (Player pPlayer in PlayerList)
                                {
                                    if (!pPlayer.GetPlayerName().Contains(name))
                                    {
                                        pLocal.TeleportTo(pPlayer.transform.position, pLocal.transform.rotation, true);
                                        break;
                                    }
                                }
                                Console.instance.Print("Couldn't find player with name " + name);
                                break;
                            }
                            Console.instance.Print("Invalid Syntax: TeleportTo [playername]");
                        }
                        /*
                        else if (szCommande.StartsWith("ItemsOfType "))
                        {
                            if(array.Length == 2)
                            {
                                string szType = array[1];
                                foreach (ItemType itemType in Enum.GetValues(typeof(ItemType)))
                                {
                                    if (Enum.GetName(typeof(ItemType), itemType).Contains(szType))
                                    {
                                        foreach(ItemDrop item in arrItems[(int)itemType])
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
                        }*/
                        else if (szCommande.StartsWith("Give "))
                        {
                            if(array.Length < 2)
                            {
                                Console.instance.Print("Invalid Syntax: Give [name, [ammount, [quality, [variant]]]]");
                                break;
                            }
                            string szItem = array[1];
                            int iAmmount = 1;
                            int iQuality = 1;
                            int iVariant = 1;
                            if (array.Length > 2)
                            {
                                if (!int.TryParse(array[2], out iAmmount))
                                {
                                    Console.instance.Print("Invalid Syntax ammount must be an integer");
                                    break;
                                }
                            }
                            else if (array.Length > 3)
                            {
                                if(!int.TryParse(array[3], out iQuality))
                                {
                                    Console.instance.Print("Invalid Syntax Quality must be an integer");
                                    break;
                                }
                            }
                            else if (array.Length > 4)
                            {
                                if(!int.TryParse(array[4], out iVariant))
                                {
                                    Console.instance.Print("Invalid Syntax Variant must be an integer");
                                    break;
                                }
                            }/*
                            foreach(ItemType itemType in Enum.GetValues(typeof(ItemType)))
                            {
                                foreach(ItemDrop item in arrItems[(int)itemType])
                                {
                                    if(item.name.Equals(szItem))
                                    {
                                        pLocal.GetInventory().AddItem(szItem, iAmmount, iQuality, iVariant, pLocal.GetPlayerID(), pLocal.GetPlayerName());
                                        break;
                                    }
                                }
                            }*/
                            Console.instance.Print("Couldn't find item with name " + szItem);
                        }
                        else if(szCommande.StartsWith("Spawn "))
                        {
                            if (array.Length <= 1)
                            {
                                Console.instance.Print("Invalid Syntax: Spawn [amount] [level]");
                                break;
                            }
                            string text4 = array[1];
                            int num8 = (array.Length >= 3) && int.TryParse(array[2], out num8) ? num8  : 1;
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
                                Character component2 = UnityEngine.Object.Instantiate<GameObject>(prefab, Player.m_localPlayer.transform.position + Player.m_localPlayer.transform.forward * 2f + Vector3.up, Quaternion.identity).GetComponent<Character>();
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
                                    Character component3 = UnityEngine.Object.Instantiate<GameObject>(prefab, Player.m_localPlayer.transform.position + Player.m_localPlayer.transform.forward * 2f + Vector3.up + b, Quaternion.identity).GetComponent<Character>();
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
        /*
        public void ReLoad()
        {
            GetItems();
        }
        
        private void GetItems()
        {
            foreach(ItemType itemType in Enum.GetValues(typeof(ItemType)))
                arrItems[(int)itemType] = ObjectDB.instance.GetAllItems(itemType, "");
        }
        */
        private static Utilizer singletonInstance = null;
        public static Utilizer instance
        {
            get
            {
                singletonInstance = singletonInstance ?? new Utilizer();
                return singletonInstance;
            }
        }
    }
}
