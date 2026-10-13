using Application.PersonalMuseo.DataTrasnferObjets;
using Core.Application;

namespace Application.PersonalMuseo.UseCases.Commands.GetUserMuseoById
{
    public class GetUserMuseoByIdQuery : IRequestQuery<UserMuseoDto>
    {
        public string UserMuseoId { get; set; }

        public GetUserMuseoByIdQuery()
        {

        }
    }
}
