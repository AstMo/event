using PartyMaker.Common.Request;
using System;

namespace PartyMaker.Story.CommonStories
{
    public class DeleteEntityStoryContext : IRequest
    {
        public Guid Id { get; set; }
    }
}
