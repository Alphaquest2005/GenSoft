using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using SystemInterfaces;
using Common.DataEntites;
using CommonMessages;

namespace EventMessages.Commands
{
   

    public class GetMedia : ProcessSystemMessage
    {
        public List<int> MediaIdList { get; }
        
        public GetMedia(List<int> mediaIdList, IStateCommandInfo processInfo, ISystemProcess process, ISystemSource source) : base(new DynamicObject("GetMedia", new Dictionary<string, object>() { { "MediaIdList", mediaIdList } }), processInfo,process, source)
        {
            Contract.Requires(mediaIdList != null && mediaIdList.Any());
            MediaIdList = mediaIdList;
        }

    }
}
