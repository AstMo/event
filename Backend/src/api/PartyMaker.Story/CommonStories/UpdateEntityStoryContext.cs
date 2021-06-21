using PartyMaker.Common.Request;
using System;

namespace PartyMaker.Story.CommonStories
{
    public class UpdateEntityStoryContext : IRequest
    {
        public Guid Id { get; set; }
    }
}
