//Roxane Seibel 74527 - Jon Gouspy 74538 - Sylvain Gross 74525 - Leith Chakroun 74529
//------------------------------------------------------------------------------
// <auto-generated>
//     Ce code a été généré à partir d'un modèle.
//
//     Des modifications manuelles apportées à ce fichier peuvent conduire à un comportement inattendu de votre application.
//     Les modifications manuelles apportées à ce fichier sont remplacées si le code est régénéré.
// </auto-generated>
//------------------------------------------------------------------------------


namespace OOP_Final_Project.Models
{

    using System;
    using System.Collections.Generic;
    using System.ComponentModel;

    public partial class Cohort
    {

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Cohort()
        {

            this.Account = new HashSet<Account>();

            this.Course = new HashSet<Course>();

        }


        public int Id { get; set; }
        [DisplayName("Cohort Name")]
        public string CohortName { get; set; }



        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

        public virtual ICollection<Account> Account { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

        public virtual ICollection<Course> Course { get; set; }

    }
}
