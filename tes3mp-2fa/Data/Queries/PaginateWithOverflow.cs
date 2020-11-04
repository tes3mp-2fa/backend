namespace tes3mp_verifier.Data.Queries
{
  public class PaginateWithOverflow<T> : Paginate<T>
  {
    public PaginateWithOverflow(int page, int pageSize) : base(page, pageSize, 1) {}
  }
}
