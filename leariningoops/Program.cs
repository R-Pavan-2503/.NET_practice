using System;
using System.Collections.Generic;
using System.Linq;

namespace LearningOOPs
{
    public interface IHasId
    {
        int Id { get; set; }
    }

    public interface IDisplayable
    {
        string GetDetails();
    }
    class Person : IHasId, IDisplayable
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public Person(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public virtual string GetDetails()
        {
            return $"ID: {Id}, Name: {Name}";
        }
    }
    class Doctor : Person
    {
        public string Specialty { get; set; }

        public Doctor(int id, string name, string specialty)
            : base(id, name)
        {
            Specialty = specialty;
        }

        public override string GetDetails()
        {
            return base.GetDetails() + $", Specialty: {Specialty}";
        }
    }

    class Patient : Person
    {
        public int Age { get; set; }

        public Patient(int id, string name, int age)
            : base(id, name)
        {
            Age = age;
        }

        public override string GetDetails()
        {
            return base.GetDetails() + $", Age: {Age}";
        }
    }




    class EntityManager<T> where T : IHasId, IDisplayable
    {
        private List<T> _items = new List<T>();

        public void Add(T item)
        {
            _items.Add(item);
        }

        public void ShowAll()
        {
            foreach (var item in _items)
            {
                Console.WriteLine(item.GetDetails());
            }
        }

        public T GetById(int id)
        {
            return _items.FirstOrDefault(item => item.Id == id);
        }
    }




    class Appointment : IDisplayable
    {
        public Doctor Doctor { get; set; }
        public Patient Patient { get; set; }
        public DateTime AppointmentDate { get; set; }

        public Appointment(Doctor doctor, Patient patient, DateTime appointmentDate)
        {
            Doctor = doctor;
            Patient = patient;
            AppointmentDate = appointmentDate;
        }

        public string GetDetails()
        {
            return $"Doctor: {Doctor.Name} ({Doctor.Specialty}), " +
                   $"Patient: {Patient.Name}, Date: {AppointmentDate}";
        }
    }




    class AppointmentManager
    {
        private List<Appointment> _appointments = new List<Appointment>();

        public void BookAppointment(
            EntityManager<Doctor> doctorManager,
            EntityManager<Patient> patientManager,
            int doctorId,
            int patientId,
            DateTime appointmentDate)
        {
            Doctor doctor = doctorManager.GetById(doctorId);
            Patient patient = patientManager.GetById(patientId);

            if (doctor == null)
            {
                Console.WriteLine($"❌ Doctor with ID {doctorId} not found.");
                return;
            }

            if (patient == null)
            {
                Console.WriteLine($"❌ Patient with ID {patientId} not found.");
                return;
            }

            Appointment appointment = new Appointment(doctor, patient, appointmentDate);
            _appointments.Add(appointment);

            Console.WriteLine($"✅ Appointment booked successfully for {patient.Name} with {doctor.Name} on {appointmentDate}");
        }

        public void ShowAllAppointments()
        {
            if (_appointments.Count == 0)
            {
                Console.WriteLine("No appointments booked yet.");
                return;
            }

            Console.WriteLine("\n📅 List of Appointments:");
            foreach (var appointment in _appointments)
            {
                Console.WriteLine(appointment.GetDetails());
            }
        }
    }




    class Program
    {
        static void Main(string[] args)
        {

            EntityManager<Doctor> doctorManager = new EntityManager<Doctor>();
            doctorManager.Add(new Doctor(1, "Dr. Smith", "Cardiology"));
            doctorManager.Add(new Doctor(2, "Dr. Johnson", "Neurology"));
            doctorManager.Add(new Doctor(3, "Dr. Williams", "Pediatrics"));

            Console.WriteLine("List of Doctors:");
            doctorManager.ShowAll();


            EntityManager<Patient> patientManager = new EntityManager<Patient>();
            patientManager.Add(new Patient(1, "Alice", 30));
            patientManager.Add(new Patient(2, "Bob", 45));
            patientManager.Add(new Patient(3, "Charlie", 25));

            Console.WriteLine("\nList of Patients:");
            patientManager.ShowAll();


            AppointmentManager appointmentManager = new AppointmentManager();

            Console.WriteLine("\nBooking Appointments...");
            appointmentManager.BookAppointment(doctorManager, patientManager, 2, 1, DateTime.Now.AddHours(2));
            appointmentManager.BookAppointment(doctorManager, patientManager, 1, 99, DateTime.Now.AddDays(1));

            appointmentManager.ShowAllAppointments();
        }
    }
}
