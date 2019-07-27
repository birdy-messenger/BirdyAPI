﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BirdyAPI.Types;

namespace BirdyAPI.Dto
{
    public class ChatInfoDto
    {
        public List<Tuple<string, ChatStatus>> Users { get; set; }
        public List<MessageDto> Messages { get; set; }
    }
}
