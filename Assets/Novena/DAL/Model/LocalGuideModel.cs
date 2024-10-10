using System.Data;
using Novena.Domain.Entities;

namespace Novena.DAL.Model
{
  public class LocalGuideModel : LocalGuide
  {
    public LocalGuideModel(int Id, int GuideId, string Json, bool Active) : base(Id, GuideId, Json, Active)
    {
    }
  }
}