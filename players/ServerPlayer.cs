using MenschADN.game;
using MenschADN.screens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MenschADN.players
{
    public class ServerPlayer : LocalPlayer
    {
        public network.NetworkReq specificClient;
        public ServerPlayer(GameScreen screen, int currentColor, string name) : base(screen, currentColor, name)
        {
        }
    }
}
