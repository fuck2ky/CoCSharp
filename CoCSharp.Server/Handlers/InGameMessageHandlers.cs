﻿using CoCSharp.Data;
using CoCSharp.Data.Models;
using CoCSharp.Data.Slots;
using CoCSharp.Network;
using CoCSharp.Network.Messages;
using CoCSharp.Server.Core;

namespace CoCSharp.Server.Handlers
{
    public static class InGameMessageHandlers
    {
        private static KeepAliveResponseMessage s_keepAliveRespond = new KeepAliveResponseMessage();

        private static void HandleKeepAliveRequestMessage(CoCServer server, CoCRemoteClient client, Message message)
        {
            client.NetworkManager.SendMessage(s_keepAliveRespond);
        }

        private static void HandleAttackNpcMessage(CoCServer server, CoCRemoteClient client, Message message)
        {
            var anMessage = message as AttackNpcMessage;

            var npcVillage = server.NpcManager.LoadNpc(anMessage.NpcID);
            if (npcVillage == null)
            {
                client.NetworkManager.SendMessage(client.OwnHomeDataMessage);
                return;
            }

            var avatar = new AvatarMessageComponent(client)
            {
                AllianceCastleLevel = 1,
                AllianceCastleTotalCapacity = 10,
                AllianceCastleUsedCapacity = 0
            };

            var ndMessage = new NpcDataMessage();
            ndMessage.AvatarData = avatar;
            ndMessage.NpcVillage = npcVillage;
            ndMessage.NpcID = anMessage.NpcID;

            FancyConsole.WriteLine("[&(darkmagenta)Attack&(default)] Account &(darkcyan){0}&(default) attacked NPC &(darkcyan){1}&(default).",
                client.Token, anMessage.NpcID);

            client.NetworkManager.SendMessage(ndMessage);
        }

        private static void HandleAttackResultMessage(CoCServer server, CoCRemoteClient client, Message message)
        {
            var avatar = client;
            var ohdMessage = client.OwnHomeDataMessage;

            FancyConsole.WriteLine("[&(darkmagenta)Attack&(default)] Account &(darkcyan){0}&(default) returned home.",
                client.Token);

            client.NetworkManager.SendMessage(ohdMessage);
        }

        private static void HandleAvatarProfileRequestMessage(CoCServer server, CoCRemoteClient client, Message message)
        {
            var aprMessage = new AvatarProfileResponseMessage();
            aprMessage.Village = client.Home;
            aprMessage.AvatarData = new AvatarMessageComponent(client);

            //aprMessage.AvatarData.Unknown13 = 2; // League
            //aprMessage.AvatarData.Unknown14 = 13;
            //aprMessage.AvatarData.Unknown27 = 13;

            //aprMessage.AvatarData.AchievementProgress = new AchievementProgessSlot[]
            //{
            //    //new AchievementProgessSlot(23000021, 306),
            //    //new AchievementProgessSlot(23000022, 306),
            //    new AchievementProgessSlot(23000023, 306), // 23000021 to 23000023 -> all time best trophies.

            //    //new AchievementProgessSlot(23000060, 306), 
            //    //new AchievementProgessSlot(23000061, 306),
            //    new AchievementProgessSlot(23000062, 306) // 23000060 to 23000062 -> War Stars count.
            //};

            FancyConsole.WriteLine("[&(darkmagenta)Avatar&(default)] Profile &(darkcyan){0}&(default) was requested.",
                client.Token);

            client.NetworkManager.SendMessage(aprMessage);
        }

        private static void HandleCommandMessage(CoCServer server, CoCRemoteClient client, Message message)
        {
            var cmdMessage = message as CommandMessage;
            if (cmdMessage.Commands.Length > 0)
            {
                for (int i = 0; i < cmdMessage.Commands.Length; i++)
                {
                    var cmd = cmdMessage.Commands[i];

                    if (cmd == null)
                        break;

                    server.HandleCommand(client, cmd);
                }

                client.Save();
            }
        }

        private static void HandleChangeAvatarNameRequestMessage(CoCServer server, CoCRemoteClient client, Message message)
        {
            var careqMessage = (ChangeAvatarNameRequestMessage)message;
            client.Name = careqMessage.NewName;
            client.IsNamed = true;

            var count = client.TutorialProgess.Count;
            for (int i = count; i < count + 3; i++)
                client.TutorialProgess.Add(new TutorialProgressSlot(21000000 + i));

            var caresMessage = new ChangeAvatarNameResponseMessage();
            caresMessage.Unknown1 = 3; // CommandType
            caresMessage.NewName = careqMessage.NewName;
            caresMessage.Unknown3 = 1;
            caresMessage.Unknown4 = -1;
            client.NetworkManager.SendMessage(caresMessage);

            client.Save();
        }

        private static void HandleChatMessageClientMessageMessage(CoCServer server, CoCRemoteClient client, Message message)
        {
            var cmcMessage = message as ChatMessageClientMessage;
            var cmsMessage = new ChatMessageServerMessage();
            if (cmcMessage.Message[0] == '/')
            {
                var cmd = cmcMessage.Message.Substring(1);
                cmsMessage.Name = "Server";
                switch (cmd)
                {
                    case "help":
                        cmsMessage.Message = "Crappy Command Implementation: Available commands -> /help, /addgems, /clearobstacles, /max";
                        client.NetworkManager.SendMessage(cmsMessage);
                        break;

                    case "addgems":
                        client.Gems += 500;
                        client.FreeGems += 500;

                        cmsMessage.Message = "Added 500 gems.";
                        client.NetworkManager.SendMessage(cmsMessage);

                        //var ohdMessage = client.Avatar.OwnHomeDataMessage;
                        client.NetworkManager.SendMessage(client.OwnHomeDataMessage);
                        return;

                    case "clearobstacles":
                        var count = client.Home.Obstacles.Count;
                        client.Home.Obstacles.Clear();

                        //var ohdMessage = client.Avatar.OwnHomeDataMessage;
                        client.NetworkManager.SendMessage(client.OwnHomeDataMessage);

                        cmsMessage.Message = "Cleared " + count + " obstacles.";
                        client.NetworkManager.SendMessage(cmsMessage);
                        return;

                    case "max":
                        var countBuilding = client.Home.Buildings.Count;
                        for (int i = 0; i < countBuilding; i++)
                        {
                            var building = client.Home.Buildings[i];
                            var collection = AssetManager.DefaultInstance.SearchCsv<BuildingData>(building.Data.ID);
                            var data = collection[collection.Count - 1];
                            if (building.IsConstructing)
                                building.CancelConstruction();

                            building.Data = data;
                        }

                        var countTraps = client.Home.Traps.Count;
                        for (int i = 0; i < countTraps; i++)
                        {
                            var trap = client.Home.Traps[i];
                            var collection = AssetManager.DefaultInstance.SearchCsv<TrapData>(trap.Data.ID);
                            var data = collection[collection.Count - 1];
                            if (trap.IsConstructing)
                                trap.CancelConstruction();

                            trap.Data = data;
                        }

                        cmsMessage.Message = "Maxed " + countBuilding + " buildings and " + countTraps + " traps.";
                        client.NetworkManager.SendMessage(cmsMessage);

                        client.NetworkManager.SendMessage(client.OwnHomeDataMessage);
                        return;

#if DEBUG
                        // Add this feature only in the DEBUG build
                    case "populatedb":
                        for (int i = 0; i < 50; i++)
                            server.AvatarManager.CreateNewAvatar();

                        cmsMessage.Message = "Created 50 new avatar.";
                        client.NetworkManager.SendMessage(cmsMessage);
                        return;
#endif

                    default:
                        cmsMessage.Message = "Unknown command.";
                        client.NetworkManager.SendMessage(cmsMessage);
                        goto case "help";
                }
            }

            //TODO: Set alliance and all that jazz.

            cmsMessage.Level = client.Level;
            cmsMessage.CurrentUserID = client.ID;
            cmsMessage.UserID = client.ID;
            cmsMessage.Name = client.Name;
            cmsMessage.Message = cmcMessage.Message;

            server.SendMessageAll(cmsMessage);
        }

        public static void RegisterInGameMessageHandlers(CoCServer server)
        {
            server.RegisterMessageHandler(new CommandMessage(), HandleCommandMessage);
            server.RegisterMessageHandler(new KeepAliveRequestMessage(), HandleKeepAliveRequestMessage);
            server.RegisterMessageHandler(new AttackNpcMessage(), HandleAttackNpcMessage);
            server.RegisterMessageHandler(new AttackResultMessage(), HandleAttackResultMessage);
            server.RegisterMessageHandler(new ChatMessageClientMessage(), HandleChatMessageClientMessageMessage);
            server.RegisterMessageHandler(new AvatarProfileRequestMessage(), HandleAvatarProfileRequestMessage);
            server.RegisterMessageHandler(new ChangeAvatarNameRequestMessage(), HandleChangeAvatarNameRequestMessage);
        }
    }
}
