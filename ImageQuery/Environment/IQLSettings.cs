namespace ImageQuery.Environment
{
    public struct IQLSettings
    {
        public bool AllowParallel { get; set; }
        public uint ProcessCount { get; set; }

        public override string ToString()
        {
            return string.Format("IQLSettings {{ AllowParallel = {0}, ProcessCount = {1} }}", AllowParallel, ProcessCount);
        }
    }
}
