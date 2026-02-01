using Dubox.Domain.Entities;
using Dubox.Domain.Enums;
using Dubox.Domain.Specification;

namespace Dubox.Application.Specifications
{
    public class GetBoxWithIncludesSpecification : Specification<Box>
    {
        public GetBoxWithIncludesSpecification(Guid boxId)
        {
            AddCriteria(box => box.BoxId == boxId);
            AddInclude(nameof(Box.Project));;
            AddInclude(nameof(Box.BoxActivities));
            AddInclude(nameof(Box.CurrentLocation));
            AddInclude(nameof(Box.BoxPanels));
            AddInclude($"{nameof(Box.BoxPanels)}.{nameof(BoxPanel.PanelType)}");
            
            AddInclude(nameof(Box.Factory));
        }

        public GetBoxWithIncludesSpecification(Guid boxId , Guid factoryId, string bay , string row , string position)
        {
            AddCriteria(b => b.FactoryId == factoryId &&
                      b.Status != BoxStatusEnum.Dispatched &&
                       b.Status != BoxStatusEnum.OnHold &&
                       b.Project.Status != ProjectStatusEnum.OnHold &&
                        b.Project.Status != ProjectStatusEnum.Closed &&
                      b.BoxId != boxId && 
                      b.Bay == bay &&
                      b.Row == row &&
                      b.Position == position &&
                      !string.IsNullOrWhiteSpace(b.Bay) &&
                      !string.IsNullOrWhiteSpace(b.Row));
         
        }
    }
}
