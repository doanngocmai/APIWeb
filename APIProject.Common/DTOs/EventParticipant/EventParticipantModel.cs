using System;
using System.Collections.Generic;
using System.Text;

namespace APIProject.Common.DTOs.EventParticipant
{
    public class EventParticipantModel : CreateQRCodeModel
    {
        public List<int> ListGift { get; set; }
    }
}
