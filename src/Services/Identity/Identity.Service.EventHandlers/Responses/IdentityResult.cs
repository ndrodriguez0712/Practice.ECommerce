namespace Identity.Service.EventHandlers.Responses
{
    public class IdentityResult
    {
        private static readonly IdentityResult _success = new IdentityResult
        {
            Succeeded = true
        };

        private List<IdentityError> _errors = new List<IdentityError>();

        public bool Succeeded { get; protected set; }

        public IEnumerable<IdentityError> Errors => _errors;

        public static IdentityResult Success => _success;

        public static IdentityResult Failed(params IdentityError[] errors)
        {
            IdentityResult identityResult = new IdentityResult
            {
                Succeeded = false
            };
            if (errors != null)
            {
                identityResult._errors.AddRange(errors);
            }

            return identityResult;
        }

        public override string ToString()
        {
            if (!Succeeded)
            {
                return string.Format("{0} : {1}", "Failed", string.Join(",", Errors.Select((IdentityError x) => x.Code).ToList()));
            }

            return "Succeeded";
        }
    }
}
