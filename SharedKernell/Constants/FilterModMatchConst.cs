namespace SharedKernell.Constants
{
    /// <summary>
    /// Opciones de modo de coincidencia de filtro (PrimeNg Table)
    /// </summary>
    public static class FilterModMatchConst
    {
        public const string StartsWith = "startsWith";
        public const string Contains = "contains";
        public const string NotContains = "notContains";
        public const string EndsWith = "endsWith";
        public new const string Equals = "equals";
        public const string NotEquals = "notEquals";
        public const string In = "in";
        public const string LessThan = "lt";
        public const string LessThanOrEqualTo = "lte";
        public const string GreaterThan = "gt";
        public const string GreaterThanOrEqualTo = "gte";
        public const string DateIs = "dateIs";
        public const string DateIsNot = "dateIsNot";
        public const string DateBefore = "dateBefore";
        public const string DateAfter = "dateAfter";
    }
}