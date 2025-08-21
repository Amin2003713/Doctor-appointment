namespace AppointmentPlanner.Shared.Models
{
    public class Hospital
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Disease { get; set; }
        public string DepartmentName { get; set; }
        public int DepartmentId { get; set; }
        public int DoctorId { get; set; }
        public int PatientId { get; set; }
        public string RecurrenceRule { get; set; }
        public string Symptoms { get; set; }
        public bool? IsAllDay { get; set; }
        public string ElementType { get; set; }
        public bool IsBlock { get; set; }
        public Nullable<int> RecurrenceID { get; set; }
        public string RecurrenceException { get; set; }
        public string StartTimezone { get; set; }
        public string EndTimezone { get; set; }
        public string Location { get; set; }

        public Hospital() { }

        public Hospital(int Id, string Name, DateTime StartTime, DateTime EndTime, string Disease, string DepartmentName, int DepartmentId, int DoctorId, int PatientId, string Symptoms)
        {
            this.Id = Id;
            this.Name = Name;
            this.StartTime = StartTime;
            this.EndTime = EndTime;
            this.Disease = Disease;
            this.DepartmentName = DepartmentName;
            this.DepartmentId = DepartmentId;
            this.DoctorId = DoctorId;
            this.PatientId = PatientId;
            this.Symptoms = Symptoms;
        }

        public List<Hospital> GetHospitalData()
        {
            List<Hospital> data = new List<Hospital>
            {
                new Hospital(1000, "Milka", new DateTime(2020, 2, 5, 10, 30, 0), new DateTime(2020, 2, 5, 11, 30, 0), "Bone Fracture", "ORTHOPEDICS", 4, 5, 2, "Swelling or bruising over a bone, Pain in the injured area"),
                new Hospital(1001, "Janet", new DateTime(2020, 2, 3, 11, 0, 0), new DateTime(2020, 2, 3, 12, 0, 0), "Biological Problems", "GENERAL", 1, 3, 4, "Physical aches or pain, Memory difficulties or personality changes"),
                new Hospital(1002, "Mercy", new DateTime(2020, 2, 2, 10, 0, 0), new DateTime(2020, 2, 2, 11, 0, 0), "Skin Problem", "DERMATOLOGY", 3, 4, 5, "outbreak of swollen, pale red bumps or plaques"),
                new Hospital(1003, "Laura", new DateTime(2020, 2, 9, 10, 0, 0), new DateTime(2020, 2, 9, 11, 0, 0), "Feeling very hungry - even though you are eating", "DIABETOLOGY", 5, 6, 1, "Urinating often, Extreme fatigue, Blurry vision"),
                new Hospital(1004, "Richa", new DateTime(2020, 2, 7, 10, 0, 0), new DateTime(2020, 2, 7, 11, 0, 0), "Skin care treatment", "DERMATOLOGIST", 3, 4, 2, "Scaly or rough skin, Peeling skin, open sores or lesions"),
                new Hospital(1005, "Adams", new DateTime(2020, 2, 7, 13, 30, 0), new DateTime(2020, 2, 7, 14, 0, 0), "General Checkup", "GENERAL", 1, 1, 3, "Decreased energy, Chronic fatigue, Difficulty concentrating"),
                new Hospital(1006, "Richa", new DateTime(2020, 2, 7, 16, 0, 0), new DateTime(2020, 2, 7, 17, 0, 0), "Left Arm Fracture", "ORTHOPEDICS", 4, 5, 6, "Swelling, warmth, or redness in the joint"),
                new Hospital(1007, "Adams", new DateTime(2020, 2, 13, 11, 0, 0), new DateTime(2020, 2, 13, 11, 30, 0), "Chest Pain", "CARDIOLOGY", 6, 7, 2, "Shortness of breath, Swollen feet or ankles"),
                new Hospital(1008, "Milka", new DateTime(2020, 2, 13, 9, 0, 0), new DateTime(2020, 2, 13, 10, 0, 0), "Skin Care Treatment", "DERMATOLOGIST", 3, 4, 2, "a rash, which might be painful or itchy"),
                new Hospital(1009, "Adams", new DateTime(2020, 2, 10, 14, 0, 0), new DateTime(2020, 2, 10, 16, 0, 0), "Surgery Treatment", "GENERAL", 1, 1, 3, "Pain at Site, Swelling/Hardening"),
                new Hospital(1010, "Adams", new DateTime(2020, 2, 11, 11, 0, 0), new DateTime(2020, 2, 11, 13, 0, 0), "Bone Problem", "ORTHOPEDICS", 4, 5, 3, "Recurring or constant joint pain or tenderness"),

                new Hospital(1011, "Adams", new DateTime(2020, 2, 1, 11, 0, 0), new DateTime(2020, 2, 1, 12, 0, 0), "General Checkup", "GENERAL", 1, 3, 1, "a pulsating feeling in the head, sensitivity to sound and light"),
                new Hospital(1012, "Janet", new DateTime(2020, 2, 1, 16, 0, 0), new DateTime(2020, 2, 1, 17, 0, 0), "Complete loss of sensation", "NEUROLOGY", 2, 2, 4, "Partial or complete paralysis, Muscle weakness"),
                new Hospital(1013, "Laura", new DateTime(2020, 2, 5, 13, 0, 0), new DateTime(2020, 2, 5, 14, 0, 0), "Health Checkup", "GENERAL", 1, 1, 1, "Sweating, Chills and Shivering"),
                new Hospital(1014, "Adams", new DateTime(2020, 2, 5, 16, 0, 0), new DateTime(2020, 2, 5, 17, 0, 0), "Eye and Spectacles Checkup", "GENERAL", 1, 3, 3, "Frequent squinting, Eye fatigue or strain"),
                new Hospital(1015, "Milka", new DateTime(2020, 2, 6, 12, 0, 0), new DateTime(2020, 2, 6, 13, 0, 0), "Feeling very hungry - even though you are eating", "DIABETOLOGY", 5, 6, 2, "Urinating often, Extreme fatigue, Blurry vision"),
                new Hospital(1016, "Adams", new DateTime(2020, 2, 6, 18, 0, 0), new DateTime(2020, 2, 6, 18, 30, 0), "Kidney disease", "DIABETOLOGY", 5, 6, 3, "Decreased urine output"),
                new Hospital(1017, "Janet", new DateTime(2020, 2, 4, 14, 0, 0), new DateTime(2020, 2, 4, 14, 30, 0), "Gastroparesis", "DIABETOLOGY", 5, 6, 4, "A feeling of fullness after eating just a few bites"),
                new Hospital(1018, "Laura", new DateTime(2020, 2, 4, 12, 0, 0), new DateTime(2020, 2, 4, 13, 0, 0), "Sleep apnea", "DIABETOLOGY", 5, 6, 1, "Gasping for air during sleep"),
                new Hospital(1019, "Milka", new DateTime(2020, 2, 4, 10, 0, 0), new DateTime(2020, 2, 4, 11, 0, 0), "Vision problems", "DIABETOLOGY", 5, 6, 2, "Severe, sudden eye pain"),
                new Hospital(1020, "Milka", new DateTime(2020, 2, 2, 12, 0, 0), new DateTime(2020, 2, 2, 13, 0, 0), "Feeling very hungry - even though you are eating", "DIABETOLOGY", 5, 6, 2, "Urinating often, Extreme fatigue, Blurry vision"),

                new Hospital(1021, "Adams", new DateTime(2020, 2, 6, 10, 0, 0), new DateTime(2020, 2, 6, 11, 0, 0), "Bone Fracture", "ORTHOPEDICS", 4, 5, 3, "Swelling or bruising over a bone, Pain in the injured area"),
                new Hospital(1022, "Mercy", new DateTime(2020, 2, 2, 15, 0, 0), new DateTime(2020, 2, 2, 15, 30, 0), "Left Arm Fracture", "ORTHOPEDICS", 4, 5, 5, "Deformity, such as a bent arm or wrist"),
                new Hospital(1023, "Milka", new DateTime(2020, 2, 6, 14, 0, 0), new DateTime(2020, 2, 6, 14, 30, 0), "Rapid heartbeat", "CARDIOLOGY", 6, 7, 2, "Fluttering sensation in the chest"),
                new Hospital(1024, "Adams", new DateTime(2020, 2, 4, 17, 30, 0), new DateTime(2020, 2, 4, 18, 30, 0), "Lightheadedness", "CARDIOLOGY", 6, 7, 3, "Sudden drop in blood pressure"),
                new Hospital(1025, "Janet", new DateTime(2020, 2, 4, 15, 0, 0), new DateTime(2020, 2, 4, 15, 30, 0), "Shortness of breath", "CARDIOLOGY", 6, 7, 4, "Nasal congestion, runny nose, itchy or watery eyes"),

                new Hospital(1026, "Milka", new DateTime(2020, 2, 3, 17, 0, 0), new DateTime(2020, 2, 3, 18, 30, 0), "Chest pain or discomfort", "CARDIOLOGY", 6, 7, 2, "Fast heart beat, and trouble breathing"),
                new Hospital(1027, "Milka", new DateTime(2020, 2, 6, 15, 30, 0), new DateTime(2020, 2, 6, 16, 0, 0), "Racing heartbeat", "CARDIOLOGY", 6, 7, 2, "A fluttering in your chest"),
                new Hospital(1028, "Milka", new DateTime(2020, 2, 3, 14, 0, 0), new DateTime(2020, 2, 3, 14, 30, 0), "Heart Problem", "CARDIOLOGY", 6, 7, 2, "Fluid buildup from being overweight"),
                new Hospital(1029, "Milka", new DateTime(2020, 2, 5, 19, 0, 0), new DateTime(2020, 2, 5, 19, 30, 0), "Dizziness", "DIABETOLOGY", 5, 6, 2, "Feeling of lightheadedness or nearly fainting")
            };
            return data;
        }
    }
}