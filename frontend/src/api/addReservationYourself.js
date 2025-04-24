import API_URL from "../appConfig";

export default async function addReservationYourself(formData, token){
    return fetch(`${API_URL}/Reservation/Create-single-reservation-yourself`, { 
        method: 'POST',
        headers: {
          'Content-Type': 'application/json'
        },
        body: JSON.stringify({
          CourtId: formData.courtId,
          StartTime: formData.startTime,
          EndTime: formData.endTime,
          CreationDate: formData.creationDate,
          TrainerId: formData.trainerId,
          ParticipantsCount: formData.participantsCount,
          IsEquipmentReserved: formData.isEquipmentReserved,
        }),
        credentials: 'include',
      });
}