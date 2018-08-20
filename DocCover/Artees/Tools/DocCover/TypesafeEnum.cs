namespace Artees.Tools.DocCover
{
    /// <summary>
    /// <see href="http://www.javacamp.org/designPattern/enum.html"/>
    /// </summary>
    public abstract class TypesafeEnum
    {
        private static int _nextId;
        
        // ReSharper disable once UnusedMember.Global
        public readonly int Id = _nextId++;
        
        // ReSharper disable once MemberCanBePrivate.Global
        public readonly string Name;

        protected TypesafeEnum(string name)
        {
            Name = name;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}