namespace HaoKao.OpenPlatformService.Application.ViewModels.ClientViewModel;


    public class SecureTextAttribute(string dangerChars) : ValidationAttribute
    {
        public string DangerChars { get; set; } = dangerChars;

        public SecureTextAttribute() : this("</>|':") { }

        public override string FormatErrorMessage(string name)
        {
            return string.Format("{0} can't include these special chars \"{1}\"", name, DangerChars);
        }

        public override bool IsValid(object value)
        {
            if (value == null) return true;

            var str = value.ToString();
            foreach (var c in DangerChars)
            {
                if (str.IndexOf(c) >= 0)
                {
                    return false;
                }
            }

            return true;
        }
    }