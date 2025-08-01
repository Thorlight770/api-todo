namespace api.todo.Model
{
    public class TrackStep
    {
        public long? ID { get; set; }
        public long? RequestMasterID { get; set; }
        public string Track { get; set; }
        public string SubTrack { get; set; }
        public string Detail1 { get; set; }
        public string Detail2 { get; set; }
        public string Status { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string UserID { get; set; }
        public string SupervisorID { get; set; }
    }
}
