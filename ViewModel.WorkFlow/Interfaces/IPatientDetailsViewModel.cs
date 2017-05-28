using System.Collections.Generic;
using Interfaces;
using ViewModelInterfaces;

namespace ViewModel.Interfaces
{
    public interface IPatientDetailsViewModel : IEntityViewModel<IPatientDetailsInfo>
    {
        IPatientInfo CurrentPatient { get; set; }
        IList<IPersonAddressInfo> Addresses { get; set; }
        IList<IPersonPhoneNumberInfo> PhoneNumbers { get; set; }
        IList<INextOfKinInfo> NextOfKins { get; set; }
        INonResidentInfo NonResidentInfo { get; set; }
    }
}