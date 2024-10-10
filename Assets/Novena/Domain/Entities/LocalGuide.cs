namespace Novena.Domain.Entities
{
  public class LocalGuide
  {
    public int Id { get;}
    public int GuideId { get;}
    public string Json { get;}
    public bool Active { get;}

    public LocalGuide(int Id, int GuideId, string Json, bool Active)
    {
      this.Id = Id;
      this.GuideId = GuideId;
      this.Json = Json;
      this.Active = Active;
    }
  }
}