namespace Identity.Common
{
    public class Enums
    {
        public enum UserStatusEnum
        {
            Active = 1,
            Inactive,
            Pending,
            PendingSend,
            PendingConfirmation,
            Rejected
        }

        public enum UserRoleEnum
        {
            Administrator = 1,
            Basic,
            Visit
        }
    }
}