using System;

namespace SnsGatewayLambda.Models
{
    public class EventMessage
    {
        public string? Content { get; set; }
        public string? Source { get; set; }
        public DateTime Timestamp { get; set; }
        public string? EventType { get; set; }
    }
}